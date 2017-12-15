using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveGenerator : MonoBehaviour
{
    public int mapWidth;
    public int mapHeight;
    public float noiseScale;

    public Noise noise = new Noise();

    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;
    public Mesh mesh;
    public float[,] noiseMap;
    public float heightMultiplier;
    public Vector2 windVector;
    // Use this for initialization
    void Start()
    {
        //mesh = GetComponent<MeshFilter>().sharedMesh;
       


    }

    // Update is called once per frame
    void Update()
    {
        offset += windVector * Time.deltaTime;
        DrawMesh(GenerateWaveMesh((noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset))));
    }

    void OnValidate()
    {
        if (mapWidth < 1)
        {
            mapWidth = 1;
        }
        if (mapHeight < 1)
        {
            mapHeight = 1;
        }
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (octaves < 0)
        {
            octaves = 0;
        }
       
    }

    public MeshData GenerateWaveMesh(float [,] heightMap)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        MeshData meshData = new MeshData(width, height);
        int vertexIndex = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {

                meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, transform.position.y+heightMap[x, y] * heightMultiplier, topLeftZ - y);
                meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                if (x < width - 1 && y < height - 1)
                {
                    meshData.AddTriangle(vertexIndex, vertexIndex + width + 1, vertexIndex + width);
                    meshData.AddTriangle(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }

        return meshData;

    }

    public void DrawMesh(MeshData meshData)
    {
        //  GetComponent<MeshFilter>().mesh.Clear();
        GetComponent<MeshFilter>().mesh = meshData.CreateMesh();
        GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().mesh;
    }


    public class MeshData
    {
        public int[] triangles;
        public Vector3[] vertices;
        public Vector2[] uvs;

        int triangleIndex;

        public MeshData(int meshWidth, int meshLength)
        {
            vertices = new Vector3[meshWidth * meshLength];
            uvs = new Vector2[meshWidth * meshLength];
            triangles = new int[(meshWidth - 1) * (meshLength - 1) * 6];
        }

        public void AddTriangle(int a, int b, int c)
        {
            triangles[triangleIndex] = a;
            triangles[triangleIndex + 1] = b;
            triangles[triangleIndex + 2] = c;
            triangleIndex += 3;
        }

        public Mesh CreateMesh()
        {
            Mesh mesh = new Mesh();
            mesh.name = "wave";
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.RecalculateNormals();
            return mesh;
        }
    }
}
