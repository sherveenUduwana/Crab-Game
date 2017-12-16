using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateController : MonoBehaviour
{

    public State currentState;
    public Transform eyes;
    public State remainState;
    [HideInInspector]
    public Bounds territory;


    [HideInInspector]
    public NavMeshAgent navMeshAgent;
    //[HideInInspector]
    public Vector3 target;
    [HideInInspector]
    public float stateTimeElapsed;

    private bool aiActive;




    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = false;

    }

    void Update()
    {
        currentState.UpdateState(this);
    }

    /*public void SetupAI(bool aiActivationFromTankManager)
    {
        aiActive = aiActivationFromTankManager;
        if (aiActive)
        {
            navMeshAgent.enabled = true;
        }
        else
        {
            //navMeshAgent.enabled = false;
        }
    }*/

    public void TransitionToState(State nextState)
    {
        if (nextState != remainState)
        {
            currentState = nextState;
            if (GetComponentInChildren<StateUI>())
            {
                GetComponentInChildren<StateUI>().ShowState();
            }
            OnExitState();
        }
        
    }

    public bool CheckIfCountDownElapsed(float duration)
    {
        stateTimeElapsed += Time.deltaTime;
        return (stateTimeElapsed >= duration);
    }

    private void OnExitState()
    {
        stateTimeElapsed = 0;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = currentState.sceneGizmoColor;
        Gizmos.DrawLine(transform.position, target);
        Gizmos.DrawWireSphere(target, 0.1f);
        
    }

    
}