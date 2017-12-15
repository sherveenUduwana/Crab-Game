using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Eat")]
public class EatAction : Action
{
    public override void Act(StateController controller)
    {
        Eat(controller);
    }

    private void Eat(StateController controller)
    {
        GameObject noticedFood = controller.GetComponent<Crab>().noticedFood;

        if (noticedFood != null)
        {
           

            controller.navMeshAgent.ResetPath();
            controller.target = noticedFood.transform.position;
            controller.navMeshAgent.destination = controller.target;


            RaycastHit hit;

            Debug.DrawRay(controller.eyes.position, controller.eyes.forward.normalized * 3, Color.green);

            if (Physics.SphereCast(controller.eyes.position, 1, controller.eyes.forward, out hit, controller.navMeshAgent.stoppingDistance)
                && hit.collider.CompareTag("CrabFood"))
            {
                controller.navMeshAgent.isStopped = true;
                Destroy(noticedFood);
                //play an eating animation
            }
            else
            {
                //Eat(controller);
            }
        }
           
    }
}