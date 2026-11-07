using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using System.Collections.Generic;

public static class MathLib
{
    public static Matrix4x4 TranslateM(Vector3 offset)
    {
        Matrix4x4 result = Matrix4x4.identity;
        result[0, 3] = offset.x;
        result[1, 3] = offset.y;
        result[2, 3] = offset.z;
        return result;
    }

    public static Matrix4x4 ScaleM(Vector3 scale)
    {
        Matrix4x4 result = Matrix4x4.identity;
        result[0, 0] = scale.x;
        result[1, 1] = scale.y;
        result[2, 2] = scale.z;
        return result;
    }

    public static Matrix4x4 RotateXM(float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);
        Matrix4x4 result = Matrix4x4.identity;
        result[1, 1] = cos;
        result[1, 2] = -sin;
        result[2, 1] = sin;
        result[2, 2] = cos;
        return result;
    }

    public static Matrix4x4 RotateYM(float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);
        Matrix4x4 result = Matrix4x4.identity;
        result[0, 0] = cos;
        result[0, 2] = sin;
        result[2, 0] = -sin;
        result[2, 2] = cos;
        return result;
    }

    public static Matrix4x4 RotateZM(float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);
        Matrix4x4 result = Matrix4x4.identity;
        result[0, 0] = cos;
        result[0, 1] = -sin;
        result[1, 0] = sin;
        result[1, 1] = cos;
        return result;
    }
    
    public static List<Vector3> ApplyTransform(Matrix4x4 m, List<Vector3> originals)
    {
        List<Vector3> result = new List<Vector3>();
        foreach(Vector3 v in originals)
        {
            Vector4 temp = new Vector4(v.x, v.y, v.z, 1);
            result.Add(m * temp); //Vector4 to Vector3
        }
        return result;
    }
}
