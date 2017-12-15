using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : MonoBehaviour {
    public bool isTrained;
    public GameObject trainer;
   // [HideInInspector]
    public GameObject noticedFood;
    // Use this for initialization
    CrabLeg[] legs;
    public bool walkTest;
    public float timer;
    public int hungerLevel;
    public Bounds territory;

    //TODO: use BOunds.expand, to expand territory, or Bounds.encapsulate
    void Awake()
    {
       
    }

    void Start () {
        legs = GetComponentsInChildren<CrabLeg>();
        territory = new Bounds(transform.position, new Vector3(10, 10, 10));
    }
	
	// Update is called once per frame
	void Update () {
        if (isTrained)
        {
            territory.center = trainer.transform.position;
        }
    }

    IEnumerator Walk()
    {
        for (int i = 0; i < legs.Length; i++)
        { 
            legs[i].Flail();
            yield return new WaitForSeconds(0.1f);
        }
        timer = 0;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(territory.center, territory.size);
    }
}
