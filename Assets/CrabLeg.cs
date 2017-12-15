using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabLeg : MonoBehaviour {
    public float flailForce;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    }

    public void Flail()
    {
        GetComponent<Rigidbody>().AddForce(new Vector3(0,1,1)*flailForce);
    }
}
