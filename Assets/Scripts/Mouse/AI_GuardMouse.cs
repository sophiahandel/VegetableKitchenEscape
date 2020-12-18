using UnityEngine;
using System.Collections;

//------------------------------------------
public class AI_GuardMouse : MonoBehaviour {  
    //------------------------------------------  
    public enum GUARD_MOUSE_STATE {
        IDLE,
        PATROL,
        LOOK_AROUND,
        CHASE, 
        ATTACK,
        HELP_LOOK_OUT,
        DEATH
    };  
    
    //------------------------------------------  
    public GUARD_MOUSE_STATE CurrentState  {    
        get {
            return currentstate;
        }    
        
        set {      
            //Update current state      
            currentstate = value;

            //Stop all running coroutines      
            StopAllCoroutines();
            switch(currentstate) {
                case GUARD_MOUSE_STATE.IDLE:
                    mouseNPC.attack(false);
                    StartCoroutine(AIIdle());
                    break;
                case GUARD_MOUSE_STATE.PATROL:    
                    mouseNPC.attack(false);       
                    StartCoroutine(AIPatrol());        
                    break;
                case GUARD_MOUSE_STATE.LOOK_AROUND:
                    mouseNPC.attack(false);
                    StartCoroutine(AILook_Around());
                    break;
                case GUARD_MOUSE_STATE.CHASE:  
                    mouseNPC.attack(false);
                    StartCoroutine(AIChase());
                    break;     
                case GUARD_MOUSE_STATE.ATTACK:    
                    mouseNPC.attack(true);        
                    StartCoroutine(AIAttack());
                    break;   
                case GUARD_MOUSE_STATE.HELP_LOOK_OUT:  
                    mouseNPC.attack(false);
                    ThisAgent.SetDestination(lookOutMouse.GetComponent<Transform>().position);  
                    StartCoroutine(AIHelpLookout());
                    break;     
                case GUARD_MOUSE_STATE.DEATH:  
                    mouseNPC.attack(false);   
                    lookOutMouse.GetComponent<AI_LookOut>().guardMouseDown();
                    StartCoroutine(AIDeath());        
                    break;      
            }    
        }  
    }  

    //stopping distance
    public float stopDistance;

    //------------------------------------------  
    [SerializeField] private GUARD_MOUSE_STATE currentstate = GUARD_MOUSE_STATE.IDLE;  // the backing enemy state to CurrentState

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

    //current patrol destination
    private int currWaypoint;

    private GameObject lookOutMouse;

    //mouse NPC
    private MouseNPC mouseNPC;
    
    //------------------------------------------  
    void Awake()  {    
        ThisLineSight = GetComponent<LineSight>();    
        ThisAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();    
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        mouseNPC = GetComponent<MouseNPC>();
        lookOutMouse = GameObject.FindGameObjectWithTag("LookOutMouse");

        ThisAgent.stoppingDistance = stopDistance;
    }  
    
    //------------------------------------------  
    void Start()  {
        CurrentState = GUARD_MOUSE_STATE.CHASE;
    }

        //------------------------------------------  
        public IEnumerator AIIdle()
        {
            while (currentstate == GUARD_MOUSE_STATE.IDLE)
            {
                //Set strict search      
                ThisLineSight.Sensitity = LineSight.SightSensitivity.STRICT;

                ThisAgent.isStopped = true;

                //If we can see the target then start chasing      
                if (ThisLineSight.CanSeeTarget)
                {
                    CurrentState = GUARD_MOUSE_STATE.CHASE;
                    yield break;
                }

                //Wait until next frame      
                yield return null;
            }
        }

        //------------------------------------------  
        public IEnumerator AIPatrol()  {    
        //Loop while patrolling    
        while (currentstate == GUARD_MOUSE_STATE.PATROL) {      
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
                CurrentState = GUARD_MOUSE_STATE.CHASE;        
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
                CurrentState = GUARD_MOUSE_STATE.CHASE;
                yield break;
            }
            yield return null;
        }

        // still could not locate player, return to idle/patrol state
        if (!useWaypoints)
        {
            CurrentState = GUARD_MOUSE_STATE.IDLE;
        }
        else
        {
            CurrentState = GUARD_MOUSE_STATE.PATROL;
        }

        yield break;
    }

    //------------------------------------------  
    public IEnumerator AIChase() {   
        //Loop while chasing    
        while(currentstate == GUARD_MOUSE_STATE.CHASE) {      
            //Set loose search      
            ThisLineSight.Sensitity = LineSight.SightSensitivity.LOOSE;  

            //Chase to last known position      
            ThisAgent.isStopped = false;  
            // ThisAgent.SetDestination(ThisLineSight.LastKnowSighting);  
            ThisAgent.SetDestination(PlayerTransform.position);      
            
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
                    // CurrentState = GUARD_MOUSE_STATE.LOOK_AROUND;
                } else { 
                    //Reached destination and can see player. Reached attacking distance          
                    CurrentState = GUARD_MOUSE_STATE.ATTACK;       
                } 
                
                yield break;      
            }   

            //Wait until next frame      
            yield return null;    
        }  
    }
    
    //------------------------------------------  
    public IEnumerator AIAttack()  {   
        //Loop while chasing and attacking    
        while (currentstate == GUARD_MOUSE_STATE.ATTACK) {      
            //Chase to player position      
            ThisAgent.isStopped = false;      
            ThisAgent.SetDestination(PlayerTransform.position);   
            
            //Wait until path is computed      
            while (ThisAgent.pathPending) {
                print("path pending");
                yield return null;
            }      

            //set speed
            this.setSpeed();
            
            //Has player run away?      
            if (ThisAgent.remainingDistance > ThisAgent.stoppingDistance + 2) {
                //Change back to chase    
                CurrentState = GUARD_MOUSE_STATE.CHASE;
                yield break;
            } else {
                // hit the player (player takes care of own code)
                // print("attacking");
            }      
            
            //Wait until next frame      
            yield return null;    
        }    
        yield break;  
    } 
    
    //------------------------------------------  
    public IEnumerator AIHelpLookout()  {   
        //Loop while chasing and attacking    
        while (currentstate == GUARD_MOUSE_STATE.HELP_LOOK_OUT) {      
            // //Chase to player position      
            // ThisAgent.isStopped = false;      
            // ThisAgent.SetDestination(PlayerTransform.position);   
            
            // //Wait until path is computed      
            // while (ThisAgent.pathPending) {
            //     print("path pending");
            //     yield return null;
            // }      

            // //set speed
            // this.setSpeed();
                 
            if (ThisAgent.remainingDistance > ThisAgent.stoppingDistance + 2) {
                //Change back to chase    
                CurrentState = GUARD_MOUSE_STATE.LOOK_AROUND;
                yield break;
            }    
            
            //Wait until next frame      
            yield return null;    
        }    
        yield break;  
    } 
    //------------------------------------------  
    public IEnumerator AIDeath()  {
        //stop agent   
        ThisAgent.isStopped = true;

        yield break;  
    }  

    public void deathHasOccured() {
        CurrentState = GUARD_MOUSE_STATE.DEATH;
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
}

