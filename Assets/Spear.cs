using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour {
    bool isStuck;
    Vector3 stickPoint;
    Quaternion stickAngle;
    Vector3 target;
    Collision stickCollision;
	// Use this for initialization
	void Start () {
      //  GetComponent<Collider>().isTrigger = true;
	}
    void Update()
    {
        Debug.Log(Vector3.Angle(transform.position, transform.position + transform.forward * GetComponent<Collider>().bounds.extents.z-transform.position));
    }

    // Update is called once per frame
    void OnCollisionEnter(Collision col)
    {
        
        if (!col.transform.root.GetComponent<Crab>()) 
        {
            //Stop movement
            transform.GetComponent<Rigidbody>().useGravity = false;
            transform.GetComponent<Rigidbody>().isKinematic = true;
            //prevent collisions between the surface the spear is tuck in and the spear
            Physics.IgnoreCollision(GetComponent<Collider>(), col.collider);

            //move the spear deeper into the target
            transform.position += transform.forward * GetComponent<Collider>().bounds.extents.z*0.75f;
            transform.parent = col.transform;
            //Disable so that future collisions don't cause the spear to get stuck again
            StartCoroutine(DisableAfteryield(2f));
        } else
        {
            this.enabled = false;
        }
    }


    IEnumerator DisableAfteryield(float t)
    {
        yield return new WaitForSeconds(t);
        this.enabled = false;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 0.1f);
        Gizmos.DrawWireSphere(transform.position + transform.forward * GetComponent<Collider>().bounds.extents.z, 0.1f);
    }
}
