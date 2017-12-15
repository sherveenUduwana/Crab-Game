using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObject : MonoBehaviour
{
    public GameObject[] ThrowableObjects;
    public Vector3 overHandPos;
    public Vector3 underHandPos;
    GameObject objectToThrow;
    int currentObject = 0;
    Vector3 handPos;
    float windBack;
    public float maxWindBack;
    Vector3 initialMousePos;
    private Trainer trainer;
    public bool isOverhand = false;

    private MeshRenderer mesh;

    //TODO: Add Boards and Bits Radial Menu to select throwable object type, and overhand or underhand;
    void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
        trainer = transform.parent.GetComponentInChildren<Trainer>();
    }

    void Start()
    {
        handPos = underHandPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetTimeScale(0.2f);
            objectToThrow = Instantiate(ThrowableObjects[currentObject], transform.position, Quaternion.identity);
            ShowArc();
            initialMousePos = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X")*-5, 0));
            if (objectToThrow != null)
            {
                objectToThrow.transform.position = transform.position;
                objectToThrow.transform.rotation = transform.rotation;
                if (objectToThrow.CompareTag("CrabFood")) //player should have a whistle sound to communicate getting the crabs attention
                {
                    trainer.currentCrab.noticedFood = objectToThrow;
                    if (trainer.currentCrab.GetComponentInChildren<StateUI>())
                    {
                        trainer.currentCrab.GetComponentInChildren<StateUI>().ShowState();
                    }
                    
                }
                
                
            }
            
            windBack = (initialMousePos.y - Input.mousePosition.y)/10;
            if (windBack >= maxWindBack) {
                windBack = maxWindBack;
            }
            GetComponent<ArcLaunch3D>().velocity = windBack;
            
            if (Input.GetMouseButtonDown(1))
            {
                Destroy(objectToThrow);
                SetTimeScale(1);
                hideArc();
            }
        }
        else
        {
            SetTimeScale(1);
            if (objectToThrow != null && Input.GetMouseButtonUp(0))
            {
                SetTimeScale(1);
                GetComponent<ArcLaunch3D>().LaunchObject(objectToThrow);
                hideArc();
            }
            if (Input.GetMouseButtonDown(1))
            {
                InverseGrip();
            }
        }

    }

    void ShowArc()
    {
        if (mesh.enabled == false)
        {
            mesh.enabled = true;
        }
    }
    void hideArc()
    {
        mesh.enabled = false;
    }

    void SetTimeScale(float t)
    {
        Time.timeScale = t;
        Time.fixedDeltaTime = 0.02f * t;
    }

    void InverseGrip()
    {
        isOverhand = !isOverhand;
        if (isOverhand)
        {
            handPos = overHandPos;
            GetComponent<ArcLaunch3D>().angle = 22.5f;
            maxWindBack *= 2f;
        } else
        {
            handPos = underHandPos;
            GetComponent<ArcLaunch3D>().angle = 45f;
            maxWindBack *= 0.5f;
        }
        transform.localPosition = handPos;
    }
}