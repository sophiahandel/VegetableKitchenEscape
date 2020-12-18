using UnityEngine;
using System.Collections;

//------------------------------------------
public class AI_LookOut : MonoBehaviour {  
    //------------------------------------------  
    public enum LOOK_OUT_STATE {
        IDLE,
        PATROL,
        LOOK_AROUND,
        RUN_AWAY,
        CALL_FOR_HELP,
        WATCH,
        DEATH
    };  
    
    //------------------------------------------  
    public LOOK_OUT_STATE CurrentState  {    
        get {
            return currentstate;
        }    
        
        set {      
            //Update current state      
            currentstate = value;

            //Stop all running coroutines      
            StopAllCoroutines();
            switch(currentstate) {
                case LOOK_OUT_STATE.IDLE:
                    StartCoroutine(AIIdle());
                    break;
                case LOOK_OUT_STATE.PATROL:       
                    StartCoroutine(AIPatrol());        
                    break;
                case LOOK_OUT_STATE.LOOK_AROUND:
                    StartCoroutine(AILook_Around());
                    break;
                case LOOK_OUT_STATE.RUN_AWAY:
                    StartCoroutine(AIRunAway());
                    break;     
                case LOOK_OUT_STATE.CALL_FOR_HELP:  
                    animator.SetTrigger("CallForHelp");  
                    numGuardMice = 3;   
                    StartCoroutine(AICallForHelp());
                    break;      
                case LOOK_OUT_STATE.WATCH:       
                    StartCoroutine(AIWatch());
                    break;      
                case LOOK_OUT_STATE.DEATH:         
                    StartCoroutine(AIDeath());        
                    break;      
            }    
        }  
    }  

    //stopping distance
    public float stopDistance;

    // reference to guard mice prefab
    public GameObject guardMicePrefab;
    public GameObject miceSpawnPoint;

    //------------------------------------------  
    [SerializeField] private LOOK_OUT_STATE currentstate = LOOK_OUT_STATE.IDLE;  // the backing enemy state to CurrentState

    //does AI Patrol?
    public bool useWaypoints = false;
    
    //Reference to line of sight component  
    private LineSight ThisLineSight = null;  
    
    //Reference to nav mesh agent  
    private UnityEngine.AI.NavMeshAgent ThisAgent = null;  
    
    //Reference to player transform  
    private Transform PlayerTransform = null;  

    //patrol destination
    Transform PatrolDestination;


    //list of destinations
    public GameObject[] waypoints;

    //list of run away points
    public GameObject[] safetyPoints;

    //current patrol destination
    private int currWaypoint;

    //mouse NPC
    private MouseNPC mouseNPC;

    //count of guard mice
    private int numGuardMice;
    private Animator animator;
    private AudioSource soundSource;
    
    //------------------------------------------  
    void Awake()  {    
        ThisLineSight = GetComponent<LineSight>();    
        ThisAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();    
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        mouseNPC = GetComponent<MouseNPC>();
        animator = GetComponent<Animator>();

        ThisAgent.stoppingDistance = stopDistance;
    }  
    
    //------------------------------------------  
    void Start()  {
        //Configure starting state  
        if (!useWaypoints)
        {
            CurrentState = LOOK_OUT_STATE.IDLE;
        }
        else
        {
            //set first destination  
            //waypoints = GameObject.FindGameObjectsWithTag("Destination");
            currWaypoint = -1;
            setNextWaypoint();

            CurrentState = LOOK_OUT_STATE.PATROL;
        }
        soundSource = GetComponent<AudioSource>();
    }

        //------------------------------------------  
        public IEnumerator AIIdle()
        {
            while (currentstate == LOOK_OUT_STATE.IDLE)
            {
                //Set strict search      
                ThisLineSight.Sensitity = LineSight.SightSensitivity.STRICT;

                ThisAgent.isStopped = true;

                //If we can see the target then start chasing      
                if (ThisLineSight.CanSeeTarget)
                {
                    CurrentState = LOOK_OUT_STATE.CALL_FOR_HELP;
                    yield break;
                }

                //Wait until next frame      
                yield return null;
            }
        }

        //------------------------------------------  
        public IEnumerator AIPatrol()  {    
        //Loop while patrolling    
        while (currentstate == LOOK_OUT_STATE.PATROL) {      
            //Set strict search      
            ThisLineSight.Sensitity = LineSight.SightSensitivity.STRICT;      
            
            //Chase to patrol position      
            ThisAgent.isStopped = false;      
            ThisAgent.SetDestination(PatrolDestination.position);      
            
            //Check to see if reached destination
            if (ThisAgent.remainingDistance <= ThisAgent.stoppingDistance)
            {
                setNextWaypoint();
            }

            //Wait until path is computed      
            while (ThisAgent.pathPending) {  
                yield return null;      
            }
            
            //set speed
            this.setSpeed();


            //If we can see the target then start chasing      
            if (ThisLineSight.CanSeeTarget) {        
                ThisAgent.isStopped = true;        
                CurrentState = LOOK_OUT_STATE.CALL_FOR_HELP;        
                yield break;      
            }      
            
            //Wait until next frame      
            yield return null;    
        }  
    }

    //------------------------------------------
    public IEnumerator AILook_Around()
    {
        // specifiy local variables
        ThisLineSight.Sensitity = LineSight.SightSensitivity.STRICT;
        float angle = 360.0f; // Degree per time unit
        float time = 5.0f; // Time unit in sec
        Vector3 axis = Vector3.up; // Rotation axis, here it the yaw axis
        float timer = 0;

        // look around
        while (timer < time)
        {
            transform.RotateAround(transform.position, axis, angle * Time.deltaTime / time);
            timer += Time.deltaTime;
            if (ThisLineSight.CanSeeTarget)
            {
                CurrentState = LOOK_OUT_STATE.CALL_FOR_HELP;
                yield break;
            }
            yield return null;
        }

        // still could not locate player, return to idle/patrol state
        if (!useWaypoints)
        {
            CurrentState = LOOK_OUT_STATE.IDLE;
        }
        else
        {
            CurrentState = LOOK_OUT_STATE.PATROL;
        }

        yield break;
    }

    //------------------------------------------  
    public IEnumerator AIRunAway() {   
        //Loop while chasing    
        while(currentstate == LOOK_OUT_STATE.RUN_AWAY) {      
            //Set loose search      
            ThisLineSight.Sensitity = LineSight.SightSensitivity.LOOSE;  

            RunAwayFromPlayer();    
            
            //Wait until path is computed      
            while(ThisAgent.pathPending)   {     
                yield return null;      
            }

            //set speed
            this.setSpeed();
            
            //Have we reached destination?      
            if (ThisAgent.remainingDistance <= ThisAgent.stoppingDistance) {
                //Stop agent        
                ThisAgent.isStopped = true; 
                
                //Reached destination but cannot see player        
                if(!ThisLineSight.CanSeeTarget) {
                    //look around
                    CurrentState = LOOK_OUT_STATE.WATCH;
                } else { 
                    //Reached destination and can see player         
                    //CurrentState = LOOK_OUT_STATE.CALL_FOR_HELP;       
                } 
                
                yield break;      
            }   

            //Wait until next frame      
            yield return null;    
        }  
    }
    
    //------------------------------------------  
    public IEnumerator AICallForHelp()  {   
        //Loop while chasing and attacking    
        while (currentstate == LOOK_OUT_STATE.CALL_FOR_HELP) {          
            ThisAgent.isStopped = true;
            setSpeed(0);


            // play the sound
            soundSource.Play();
            
            //wait for mice to hear call
            yield return new WaitForSeconds(1f);

            //spawn 3 guard mice 
            Instantiate(guardMicePrefab, miceSpawnPoint.GetComponent<Transform>().position + Vector3.forward, Quaternion.identity);
            yield return new WaitForSeconds(3.5f);
            Instantiate(guardMicePrefab, miceSpawnPoint.GetComponent<Transform>().position, Quaternion.identity);
            yield return new WaitForSeconds(3.5f);
            Instantiate(guardMicePrefab, miceSpawnPoint.GetComponent<Transform>().position + Vector3.back, Quaternion.identity);

            CurrentState = LOOK_OUT_STATE.WATCH;

            //Wait until next frame      
            yield return null;    
        }    
        yield break;  
    } 

    //------------------------------------------  
    public IEnumerator AIWatch()
        {
            while (currentstate == LOOK_OUT_STATE.WATCH)
            {
                ThisAgent.isStopped = true;
                setSpeed(0);

                if (numGuardMice <= 0) {
                    CurrentState = LOOK_OUT_STATE.LOOK_AROUND;
                    yield break;
                }

                // if(!ThisLineSight.CanSeeTarget && Vector3.Distance(PlayerTransform.position, this.PlayerTransform.position) < 3) {
                //     CurrentState = LOOK_OUT_STATE.RUN_AWAY;
                // }

                //Wait until next frame      
                yield return null;
            }
        }
    //------------------------------------------  
    public IEnumerator AIDeath()  {
        //stop agent   
        ThisAgent.isStopped = true;

        yield break;  
    }  

    public void deathHasOccured() {
        CurrentState = LOOK_OUT_STATE.DEATH;
    }

    //-------------------------------------------
    private void setNextWaypoint() {
        if (waypoints.Length == 0)
        {
            Debug.Log("There are no waypoints assigned to " + this.ToString());
        }
        else
        {
            // get index of next destination
            currWaypoint++;
            if (currWaypoint >= waypoints.Length)
            {
                currWaypoint = 0;
            }

            PatrolDestination = waypoints[currWaypoint].GetComponent<Transform>();
            ThisAgent.SetDestination(PatrolDestination.position);
        }
    }

    private void setSpeed() {
        mouseNPC.setSpeedForwardAnimations(ThisAgent.velocity.magnitude, ThisAgent.speed);
    }

    private void setSpeed(float magn) {
        mouseNPC.setSpeedForwardAnimations(magn, ThisAgent.speed);
    }

    void RunAwayFromPlayer()
     {
         float furthestDistanceSoFar = 0;
         Vector3 runPosition = Vector3.zero;
 
         //Check each point
         foreach (GameObject point in safetyPoints)
         {
             //print(Vector3.Distance(player.position, point.position));
             float currentCheckDistance = Vector3.Distance(PlayerTransform.position, point.GetComponent<Transform>().position);
             if (currentCheckDistance > furthestDistanceSoFar)
             {
                 furthestDistanceSoFar = currentCheckDistance;
                 runPosition = point.GetComponent<Transform>().position;
             }
         }
         //Set the right destination for the furthest spot
         ThisAgent.SetDestination(runPosition);
     }

     public void guardMouseDown() {
         numGuardMice -= 1;
         print("num guard mice " + numGuardMice);
     }
}

