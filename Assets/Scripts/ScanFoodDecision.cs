using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Scan Food")]
public class ScanFoodDecision : Decision {

    public override bool Decide(StateController controller)
    {
        bool foundFood = ScanFood(controller);
        return foundFood;
    }

    private bool ScanFood(StateController controller)
    {
        GameObject noticedFood = controller.gameObject.GetComponent<Crab>().noticedFood;
        if (noticedFood != null)
        {
            return true;
        }
        RaycastHit hit;
        
        Debug.DrawRay(controller.eyes.position, controller.eyes.forward.normalized * 3, Color.green);

        if (Physics.SphereCast(controller.eyes.position, 3, controller.eyes.forward, out hit, 5)
            && hit.collider.CompareTag("CrabFood"))
        {
            
            controller.gameObject.GetComponent<Crab>().noticedFood = hit.transform.gameObject;
            return true;
        }
        else
        {
            return false;
        }
    }
}
