using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchObject : MonoBehaviour {
    //[HideInInspector]
    public Vector3 target;
    int currentVert = 0;
    float horizontalSpeed;
    float verticalSpeed;
    float g;
    public float firingAngle = 45;
    float Vx;
    float Vy;
    float flightDuration;
    float elapseTime;
    bool rotate = true;

    // Use this for initialization
    //TODO: use Physics.IgnoreCollision instead of physics layers to prevent projectile colliding with projectile launcher
    void Awake()
    {
         g = Mathf.Abs(Physics.gravity.y);
    }
    void Start()
    {
        if(target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target);
            // Calculate the velocity needed to throw the object to the target at specified angle.
            float velocity = distanceToTarget / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / g);
            // Extract the X  Y componenent of the velocity
            Vx = Mathf.Sqrt(velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
            Vy = Mathf.Sqrt(velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);


            transform.LookAt(target);
            Vector3 localVelocity = new Vector3(0f, Vy, Vx);
            
            // transform it to global vector
            Vector3 globalVelocity = transform.TransformDirection(localVelocity);

            // launch projectile by setting its initial velocity
            GetComponent<Rigidbody>().velocity = globalVelocity;
            //Destroy(this);
        }
    }

	// Update is called once per frame
	void Update ()
    {
        // Rotate projectile to face the target.
        if(Vector3.Distance(target, transform.position) < 0.5)
        {
            rotate = false;
        }
        if (rotate)
        {
            transform.rotation = Quaternion.LookRotation(target - transform.position);
        }
                
    }

    void OnCollisionEnter(Collision col)
    {
        Destroy(this);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(target, 0.1f);
    }
    
}
