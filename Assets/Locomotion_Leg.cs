using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locomotion_Leg : MonoBehaviour
{
    private Vector3 startPos;
    Vector3 idealTarget;
    Vector3 currentTarget;
    public Transform body;
    private StateController controller;
    float currentTargetScore = Mathf.Infinity;
    Locomotion_Body locomotionBody;
    private Vector3 correctionStart;
    public bool isGripping = false;
    private new Rigidbody rigidbody;
    private float g;
    public float range;
    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        g = Mathf.Abs(Physics.gravity.y);
        controller = body.GetComponent<StateController>();
        locomotionBody = body.GetComponent<Locomotion_Body>();
        correctionStart = rigidbody.position;
        startPos = body.position - rigidbody.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {

        UpdateIdealTarget();
        ChooseTarget();

        if (isGripping == false)
        {
            rigidbody.position = Vector3.MoveTowards(rigidbody.position, currentTarget, Time.fixedDeltaTime * 5);
            CheckGroundCollision();
        }
        else
        {
            if (Vector3.Distance(body.position, rigidbody.position) > range)
            {
                isGripping = false;
                currentTargetScore = Mathf.Infinity;
                rigidbody.AddForce(Vector3.up);
                rigidbody.position = Vector3.MoveTowards(rigidbody.position, body.position + startPos, Time.fixedDeltaTime * 10);
            }
        }
        if (Vector3.Distance(body.position, rigidbody.position) > range + body.GetComponent<Collider>().bounds.extents.z)
        {
            isGripping = false;
            currentTargetScore = Mathf.Infinity;
            rigidbody.AddForce(Vector3.up);
            rigidbody.position = body.position + ((rigidbody.position - body.position).normalized * range);
        }


    }

    void UpdateIdealTarget()
    {
        idealTarget = (controller.target - rigidbody.position).normalized * range;
        idealTarget += body.position + startPos;
        //idealTarget = idealTarget.normalized;
    }

    void ChooseTarget()
    {
        Vector3 potentialTarget = ConvertToGroundLevel(rigidbody.position + (Random.insideUnitSphere * range));

        float score = Vector3.Distance(potentialTarget, idealTarget);
        if (score < currentTargetScore)
        {
            currentTarget = potentialTarget;
            currentTargetScore = score;
        }

    }

    private void CheckGroundCollision()
    {
        if (Vector3.Distance(rigidbody.position, currentTarget) < 0.05f && isGripping == false)
        {
            rigidbody.position = currentTarget;
            rigidbody.velocity = Vector3.zero;
            isGripping = true;
            correctionStart = rigidbody.position;
        }
    }

    private Vector3 ConvertToGroundLevel(Vector3 position)
    {
        RaycastHit hit;
        position = new Vector3(position.x, position.y + 100, position.z);
        int layerMask = LayerMask.GetMask("Water");
        if (Physics.Raycast(new Ray(position, Vector3.down), out hit, 1000f, layerMask))
        {
            return hit.point;
        }
        else
        {
            return new Vector3(position.x, 999, position.z);
        }

    }

    /* void MoveTowardsPoint(Vector3 target)
     {
         Vector3 targetDirection = target - rigidbody.position;
         Vector3 correctionVector = correctionVector = ClosestPointOnLine(correctionStart, target, rigidbody.position);

         if (Vector3.Distance(rigidbody.position, correctionVector) < 0.1f)
         {
             rigidbody.AddForce(targetDirection.normalized *5);
         }
         else
         {

             rigidbody.AddForce(correctionVector - rigidbody.position);
         }

         rigidbody.rotation = Quaternion.LookRotation(targetDirection);
     }

     Vector3 ClosestPointOnLine(Vector3 vA, Vector3 vB, Vector3 vPoint)
     {
         Vector3 vVector1 = vPoint - vA;
         Vector3 vVector2 = (vB - vA).normalized;

         float d = Vector3.Distance(vA, vB);
         float t = Vector3.Dot(vVector2, vVector1);

         if (t <= 0)
             return vA;

         if (t >= d)
             return vB;

         Vector3 vVector3 = vVector2 * t;

         Vector3 vClosestPoint = vA + vVector3;

         return vClosestPoint;
     }*/

    void OnDrawGizmos()
    {
        if (!isGripping)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.white;
        }


        Gizmos.DrawCube(transform.position, Vector3.one * 0.1f);
        Gizmos.DrawLine(transform.position, body.GetComponent<Collider>().ClosestPoint(transform.position));
        

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(idealTarget, 0.1f);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(ConvertToGroundLevel(rigidbody.position + (Random.onUnitSphere * range)), 0.1f);
        Gizmos.DrawWireSphere(currentTarget, 0.1f);
    }
}
