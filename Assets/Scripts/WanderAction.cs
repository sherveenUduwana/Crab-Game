using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Wander")]
public class WanderAction : Action
{
    public override void Act(StateController controller)
    {
        Wander(controller);
    }

    private void Wander(StateController controller)
    {
        
        controller.navMeshAgent.isStopped = false;

        if (controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance && !controller.navMeshAgent.pathPending)
        {
            controller.navMeshAgent.destination = NewWanderTarget(controller);
        }
    }

    private Vector3 NewWanderTarget(StateController controller)
    {
        Bounds territory = controller.gameObject.GetComponent<Crab>().territory;
        float wanderThreshold = 8;
        if(controller.GetComponent<Crab>().isTrained)
        {
            wanderThreshold = 2;
        }
        float wanderStep = 5;
        // Randomly select an angle between 45 and 135 degrees (but in radians)
        float angle = Random.Range(Mathf.PI / 4, Mathf.PI * 3 / 4);
        Vector3 tempTarget = new Vector3(controller.transform.position.x + Mathf.Cos(angle) * wanderStep, controller.transform.position.y, controller.transform.position.z + Mathf.Sin(angle) * wanderStep);

        int i = Random.Range(1,4);
        if (!territory.Contains(tempTarget))
        {
           
            tempTarget = new Vector3(controller.transform.position.x + Mathf.Cos(angle + (Mathf.PI*i / 2)) * wanderStep, controller.transform.position.y, controller.transform.position.z + Mathf.Sin(angle + (Mathf.PI*i / 2)) * wanderStep);
          if (Vector3.Distance(tempTarget, territory.ClosestPoint(tempTarget)) > wanderThreshold)
            {
                tempTarget = territory.ClosestPoint(tempTarget);
            }
          
        }

        controller.target = tempTarget;
        return controller.target;
    }
}