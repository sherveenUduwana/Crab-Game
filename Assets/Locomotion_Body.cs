using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Locomotion_Body : MonoBehaviour {
    public int numberOfLegs;
    public int legsOnGround;

    private StateController controller;
    
    public NavMeshAgent agent;
    private Vector3 oldDestination;
    private new Rigidbody rigidbody;
    private float g;
    private int currentWaypoint = 0;
    public bool destinationChanged = false;
    public Locomotion_Leg[] legs;

    // Use this for initialization
    void Start () {
        rigidbody = GetComponent<Rigidbody>();
        g = Mathf.Abs(Physics.gravity.y);
        controller = GetComponent<StateController>();
        agent = controller.navMeshAgent;
        legs = transform.root.GetComponentsInChildren<Locomotion_Leg>();
        numberOfLegs = legs.Length;
    }
	
	// Update is called once per frame
	void Update () {
	}

    void FixedUpdate()
    {
        GetCurrentWaypoint();
        CheckLegsOnGround();
        rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, rigidbody.velocity*(legsOnGround / numberOfLegs),Time.fixedDeltaTime*2); //if no legs are on ground, crab cannot move
        rigidbody.AddForce(Vector3.up * g*g*0.5f * (legsOnGround / numberOfLegs) * rigidbody.mass);  //if all legs are on ground, gravity has no effect on body.
        MoveTowardsPoint(controller.target);
    }

    void CheckLegsOnGround()
    {
        legsOnGround = 0;
        for(int i = 0; i < legs.Length; i++)
        {
            if (legs[i].isGripping)
            {
                legsOnGround++;
            }
        }
    }

    void MoveTowardsPoint(Vector3 target)
    {
        Vector3 targetDirection = target - rigidbody.position;
        int nextWaypoint = currentWaypoint + 1;
        Vector3 correctionVector = correctionVector = ClosestPointOnLine(agent.path.corners[0], agent.path.corners[agent.path.corners.Length - 1], rigidbody.position);

        if (Vector3.Distance(rigidbody.position, correctionVector) < agent.stoppingDistance)
        {
            rigidbody.AddForce(targetDirection.normalized * agent.speed);
        }
        else
        {

            rigidbody.AddForce(correctionVector - rigidbody.position);
        }

        rigidbody.rotation = Quaternion.Lerp(rigidbody.rotation, Quaternion.LookRotation(targetDirection), Time.fixedDeltaTime * agent.speed);
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
    }

    void GetCurrentWaypoint()
    {
        destinationChanged = false;
        if(currentWaypoint < agent.path.corners.Length-1)
        {
            if (Vector3.Distance(rigidbody.position, controller.target) < Vector3.Distance(agent.path.corners[currentWaypoint], controller.target))
            {
                currentWaypoint++;
            }
        }
        if(oldDestination != agent.destination)
        {
            destinationChanged = true;
            oldDestination = agent.destination;
            currentWaypoint = 0;
        }
    }



    void OnValidate()
    {
        if(numberOfLegs <= 0)
        {
            numberOfLegs = 1;
        }

        if(legsOnGround > numberOfLegs)
        {
            legsOnGround = numberOfLegs;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.forward * 2);
        for (int i = 0; i < agent.path.corners.Length; i++)
        {
            Gizmos.DrawWireSphere(agent.path.corners[i], 0.2f);
        }

       
    }
}
