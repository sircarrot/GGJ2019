using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtension
{
    public static Vector2 ToVector2(this Vector3 v)
    {
        return new Vector2(v.x, v.y);
    }

    public static Vector3 WithX(this Vector3 v, float x)
    {
        return new Vector3(x, v.y, v.z);
    }

    public static Vector3 WithY(this Vector3 v, float y)
    {
        return new Vector3(v.x, y, v.z);
    }

    public static Vector3 WithZ(this Vector3 v, float z)
    {
        return new Vector3(v.x, v.y, z);
    }

    public static Vector2 WithX(this Vector2 v, float x)
    {
        return new Vector2(x, v.y);
    }

    public static Vector2 WithY(this Vector2 v, float y)
    {
        return new Vector2(v.x, y);
    }

    public static Vector3 WithZ(this Vector2 v, float z)
    {
        return new Vector3(v.x, v.y, z);
    }
    
    public static Vector3 Mask(this Vector3 a, Vector3 mask)
    {
        return new Vector3(a.x * mask.x, a.y * mask.y, a.z * mask.z);
    }

    public static Vector3 Abs(this Vector3 a)
    {
        return new Vector3(Mathf.Abs(a.x), Mathf.Abs(a.y), Mathf.Abs(a.z));
    }

    public static Vector3 Inverse(this Vector3 a)
    {
        return new Vector3(1 / a.x, 1 / a.y, 1 / a.z);
    }
}