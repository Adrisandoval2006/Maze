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
    int currentTargetIndex = 0;
    bool isTurning = false;
    float turnedDegrees = 0f;
    float currentTurnDirection = 0f;

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

        memory = MathLib.TranslateM(new Vector3(0, 0, -10));
        ms = MathLib.ScaleM(new Vector3(10, 10, 10));
        Matrix4x4 m = ms * memory;
        TheCar.GetComponent<MeshFilter>().mesh.vertices = MathLib.ApplyTransform(m, geometry).ToArray();           
    }

    void Update()
    {
        float dz = 0f;
        float rotY = 0f;

        if (currentTargetIndex < points.Count)
        {
            if (!isTurning)
            {
                // State: Moving forward
                dz = 0.1f;

                // Where is the cube now?
                Vector3 currentPos = new Vector3(memory[0, 3], memory[1, 3], memory[2, 3]);
                Vector3 targetPos = points[currentTargetIndex];

                // If we reached the target point (within step tolerance)
                if (Vector3.Distance(currentPos, targetPos) <= 0.101f)
                {
                    // Where is the cube now?
                    // | a  b  c  x |
                    // | d  e  f  y |
                    // | g  h  i  z |
                    // | 0  0  0  1 |
                    memory[0, 3] = targetPos.x;
                    memory[1, 3] = targetPos.y;
                    memory[2, 3] = targetPos.z;

                    dz = 0f; // Stop moving, prepare to turn

                    if (currentTargetIndex + 1 < points.Count)
                    {
                        Vector3 nextPos = points[currentTargetIndex + 1];
                        
                        // Extract current forward vector from memory's Z axis
                        Vector3 currentForward = new Vector3(memory[0, 2], memory[1, 2], memory[2, 2]);
                        Vector3 dirToNext = (nextPos - targetPos).normalized;

                        // Use 2D cross product to dynamically determine turn direction
                        // Matches the table in image_3eecbe.png exactly
                        float cross = currentForward.z * dirToNext.x - currentForward.x * dirToNext.z;

                        if (cross > 0.1f) currentTurnDirection = 1f;       // Left Turn
                        else if (cross < -0.1f) currentTurnDirection = -1f; // Right Turn
                        else currentTurnDirection = 0f;                    // Straight

                        if (currentTurnDirection != 0f)
                        {
                            isTurning = true;
                            turnedDegrees = 0f;
                        }
                        else
                        {
                            // Skip straight ahead to next point
                            currentTargetIndex++;
                            dz = 0.1f;
                        }
                    }
                    else
                    {
                        // Maze finished
                        currentTargetIndex++;
                    }
                }
            }
            else
            {
                // State: Rotating at corner
                dz = 0f;
                rotY = currentTurnDirection * 1f; // 1 degree per frame
                turnedDegrees += Mathf.Abs(rotY);

                if (turnedDegrees >= 90f)
                {
                    isTurning = false;
                    currentTargetIndex++; // Move to next point objective
                }
            }
        }

        // Apply transformations using purely homogeneous matrices as instructed
        Matrix4x4 mt = MathLib.TranslateM(new Vector3(0, 0, dz));
        Matrix4x4 mr = MathLib.RotateYM(rotY);
        
        Matrix4x4 matrix = ms * memory * mt * mr;
        
        TheCar.GetComponent<MeshFilter>().mesh.vertices = MathLib.ApplyTransform(matrix, geometry).ToArray();
        
        // Memory "remembers" every change in position/rotation
        memory = memory * mt * mr;
    }

}
