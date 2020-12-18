using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootstepEmitter : MonoBehaviour
{
    public void ExecuteFootstep()
    {
        EventManager.TriggerEvent<playerStepEvent, Vector3>(transform.position);
    }
}
