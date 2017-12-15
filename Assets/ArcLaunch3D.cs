using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcLaunch3D : MonoBehaviour {

    private GameObject arcMesh;
    private Mesh mesh;
    private MeshRenderer mr;
    public float velocity;
    public float angle;
    public int resolution = 10;
    public float meshWidth;
    float g; //force of gravity on the y axis
    float radianAngle;
    public Vector3[] launchTrajectory;

    void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        g = Mathf.Abs(Physics.gravity.y);
    }

    void Update()
    {
        if(mesh != null && Application.isPlaying)
        {
            MakeArcMesh(CalcArcArray());
        }
    }

    void Start()
    {
        MakeArcMesh(CalcArcArray());
        GetComponent<MeshRenderer>().enabled = false;
    }

    void MakeArcMesh(Vector3[] arcVerts)
    {
        mesh.Clear();
        Vector3[] vertices = new Vector3[(resolution + 1) * 2];//not the same as arc verts. This is the 3d information used to create the mesh
        int[] triangles = new int[resolution * 6 * 2];// six vertices for the top of the arc, and six for the bottom. Total of twelve.

        for (int i = 0; i <= resolution; i++)
        {
            //set vertices
            vertices[i * 2] = new Vector3(meshWidth * 0.5f, arcVerts[i].y, arcVerts[i].x); // even vertices on right
            vertices[(i * 2) + 1] = new Vector3(meshWidth * -0.5f, arcVerts[i].y, arcVerts[i].x); // odd vertices on left

            //set triangles
            if (i != resolution)
            {
                //top side
                triangles[i * 12] = i * 2;
                triangles[i * 12 + 1] = triangles[i * 12 + 4] = i * 2 + 1;
                triangles[i * 12 + 2] = triangles[i * 12 + 3] = (i + 1) * 2;
                triangles[i * 12 + 5] = (i + 1) * 2 + 1;

                //bottom side
                triangles[i * 12 + 6] = i * 2;
                triangles[i * 12 + 7] = triangles[i * 12 + 10] = (i + 1) * 2;
                triangles[i * 12 + 8] = triangles[i * 12 + 9] = i * 2 + 1;
                triangles[i * 12 + 11] = (i + 1) * 2 + 1;
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
        }
    }

    Vector3[] CalcArcArray()
    {
        Vector3[] arcArray = new Vector3[resolution + 1];
        radianAngle = Mathf.Deg2Rad * angle;
        float maxDistance;
        if(velocity < 0)
        {
            maxDistance = 0;
        }
        else
        {
            maxDistance = (velocity * velocity * (Mathf.Sin(2 * radianAngle))) / g;
        }

        for (int i = 0; i <= resolution; i++)
        {
            float t = (float)i / (float)resolution; // ratio from 0 - 1 of where we are from point A to point B
            arcArray[i] = CalcArcPoint(t, maxDistance);
        }
        return arcArray;
    }

    Vector3 CalcArcPoint(float t, float maxDistance)
    {
        float x = t * maxDistance;
        float y = (x * Mathf.Tan(radianAngle)) - ((g * x * x) / (2 * (velocity * velocity) * (Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle))));
        return new Vector3(x, y);
    }

    public void LaunchObject(GameObject objectToThrow)
    {
        Vector3[] launchWayPoints = new Vector3[mesh.vertices.Length / 2];
        bool containsNan = false;
        for (int i = 0; i < launchWayPoints.Length; i++)
        {
            launchWayPoints[i] = mesh.vertices[i * 2];
            launchWayPoints[i].x -= meshWidth / 2;
            launchWayPoints[i] = transform.TransformPoint(launchWayPoints[i]);
            if(float.IsNaN(launchWayPoints[i].x) || float.IsNaN(launchWayPoints[i].y) || float.IsNaN(launchWayPoints[i].z))
            {
                containsNan = true;
            }
        }
        if(containsNan == false)
        {
            LaunchObject launcher = objectToThrow.AddComponent<LaunchObject>();
            launcher.target = launchWayPoints[launchWayPoints.Length-1];
            launcher.firingAngle = angle;

        }
        
    }


}
