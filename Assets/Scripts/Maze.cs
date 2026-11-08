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
    Matrix4x4 ms;
    int idx = 0;
    bool turning = false;
    float angle = 0f;
    float turnSide = 0f;

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
            new Vector3(1, 0, 9),
            new Vector3(1, 0, 10)
        };

        memory = MathLib.TranslateM(new Vector3(0, 0, -10));
        ms = MathLib.ScaleM(new Vector3(10, 10, 10));
        Matrix4x4 m = ms * memory;
        TheCar.GetComponent<MeshFilter>().mesh.vertices = MathLib.ApplyTransform(m, geometry).ToArray();           
    }

    void Update()
    {
        // Color logic
        if (turning)
        {
            TheCar.GetComponent<MeshRenderer>().material.color = Color.magenta;
        }
        else if (idx == points.Count - 1) 
        {
            TheCar.GetComponent<MeshRenderer>().material.color = Color.green;
        }
        else
        {
            TheCar.GetComponent<MeshRenderer>().material.color = Color.blue;
        }

        float dz = 0f;
        float rotY = 0f;

        if (idx < points.Count)
        {
            if (!turning)
            {
                dz = 0.1f;
                // current position
                Vector3 pos = new Vector3(memory[0, 3], memory[1, 3], memory[2, 3]);
                Vector3 goal = points[idx];
                goal.x = -goal.x; 
                // stop moving
                if (Vector3.Distance(pos, goal) <= 0.101f)
                {
                    // Where is the cube now?
                    // | a  b  c  x |
                    // | d  e  f  y |
                    // | g  h  i  z |
                    // | 0  0  0  1 |
                    memory[0, 3] = goal.x;
                    memory[1, 3] = goal.y;
                    memory[2, 3] = goal.z;
                    dz = 0f; 

                    if (idx + 1 < points.Count)
                    {
                        Vector3 nextGoal = points[idx + 1];
                        nextGoal.x = -nextGoal.x;
                        // current forward vector
                        Vector3 fwd = new Vector3(memory[0, 2], memory[1, 2], memory[2, 2]);
                        Vector3 toNext = (nextGoal - goal).normalized;

                        // Cross Product
                        float cross = fwd.z * toNext.x - fwd.x * toNext.z;
                        if (cross > 0.1f) turnSide = 1f;            //left       
                        else if (cross < -0.1f) turnSide = -1f;     //right
                        else turnSide = 0f;                         //straight

                        if (turnSide != 0f)
                        {
                            turning = true;
                            angle = 0f;
                        }
                        else
                        {
                            idx++;
                            dz = 0.1f;
                        }
                    }
                    else
                    {
                        idx++;
                    }
                }
            }
            else
            {
                dz = 0f;
                rotY = turnSide * 1f; 
                angle += Mathf.Abs(rotY);
                if (angle >= 90f)
                {
                    turning = false;
                    idx++;
                }
            }
        }

        // homogeneous transformations 
        Matrix4x4 mt = MathLib.TranslateM(new Vector3(0, 0, dz));
        Matrix4x4 mr = MathLib.RotateYM(rotY);
        Matrix4x4 matrix = ms * memory * mt * mr;
        TheCar.GetComponent<MeshFilter>().mesh.vertices = MathLib.ApplyTransform(matrix, geometry).ToArray();
        memory = memory * mt * mr;
    }
}
