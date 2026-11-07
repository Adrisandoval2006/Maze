using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Maze : MonoBehaviour
{
    List<Vector3> geometry;
    List<int> topology;
    List<Vector3> points;
    GameObject TheCar;
    Matrix4x4 memory;

    void Start()
    {
        geometry = new List<Vector3>()
        {
            new Vector3(-0.5f, -0.5f,  0.5f),
            new Vector3( 0.5f, -0.5f,  0.5f),
            new Vector3( 0.5f,  0.5f,  0.5f),
            new Vector3(-0.5f,  0.5f,  0.5f),
            new Vector3( 0.5f, -0.5f, -0.5f),
            new Vector3( 0.5f,  0.5f, -0.5f),
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(-0.5f,  0.5f, -0.5f)
        };

        topology = new List<int>()
        {
            // FRONT FACE
            0, 1, 2,
            0, 2, 3,
            // RIGHT FACE
            1, 4, 5,
            1, 5, 2,
            // BACK FACE
            4, 6, 7,
            4, 7, 5,
            // LEFT FACE
            6, 0, 3,
            6, 3, 7,
            // BOTTOM FACE
            1, 0, 6,
            1, 6, 4,
            // TOP FACE
            3, 2, 5,
            3, 5, 7
        };

        Mesh myMesh = new Mesh();
        myMesh.vertices = geometry.ToArray();
        myMesh.triangles = topology.ToArray();

        TheCar = new GameObject("TheCar");
        MeshFilter mf = TheCar.AddComponent<MeshFilter>();
        mf.mesh = myMesh;

        MeshRenderer mr = TheCar.AddComponent<MeshRenderer>();
        mr.material.color = Color.blue;

        points = new List<Vector3>()
        {
            new Vector3(0, 0, -9),
            new Vector3(3, 0, -9),
            new Vector3(3, 0, -8),
            new Vector3(5, 0, -8),
            new Vector3(5, 0, -9),
            new Vector3(9, 0, -9),
            new Vector3(9, 0, -7),
            new Vector3(8, 0, -7),
            new Vector3(8, 0, -8),
            new Vector3(7, 0, -8),
            new Vector3(7, 0, -6),
            new Vector3(4, 0, -6),
            new Vector3(4, 0, -7),
            new Vector3(3, 0, -7),
            new Vector3(3, 0, -4),
            new Vector3(2, 0, -4),
            new Vector3(2, 0, -3),
            new Vector3(1, 0, -3),
            new Vector3(1, 0, 0),
            new Vector3(5, 0, 0),
            new Vector3(5, 0, -1),
            new Vector3(6, 0, -1),

            new Vector3(6, 0, 1),
            new Vector3(7, 0, 1),
            new Vector3(7, 0, 2),
            new Vector3(5, 0, 2),
            new Vector3(5, 0, 1),
            new Vector3(4, 0, 1),
            new Vector3(4, 0, 3),
            new Vector3(2, 0, 3),
            new Vector3(2, 0, 4),
            new Vector3(0, 0, 4),
            new Vector3(0, 0, 5),
            new Vector3(1, 0, 5),
            new Vector3(1, 0, 6),
            new Vector3(2, 0, 6),
            new Vector3(2, 0, 7),
            new Vector3(3, 0, 7),
            new Vector3(3, 0, 5),
            new Vector3(4, 0, 5),
            new Vector3(4, 0, 8),
            new Vector3(3, 0, 8),
            new Vector3(3, 0, 9),
            new Vector3(1, 0, 9)
        };           
    }

    void Update()
    {
       // Starting position
        Matrix4x4 scale = MathLib.ScaleM(new Vector3(10, 10, 10));
        Matrix4x4 position = MathLib.TranslateM(new Vector3 (0, 0, -10));
        Matrix4x4 m = scale * position;
        TheCar.GetComponent<MeshFilter>().mesh.vertices = MathLib.ApplyTransform(m, geometry).ToArray(); 

        // Where is the cube now?
        // | a  b  c  x |
        // | d  e  f  y |
        // | g  h  i  z |
        // | 0  0  0  1 |
        float x = memory[0, 3];
        float y = memory[1, 3];
        float z = memory[2, 3];

        
               
    }
}
