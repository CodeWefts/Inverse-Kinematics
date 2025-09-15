using System;
using UnityEngine;

public class MathLib : MonoBehaviour
{
    public static float DotProduct(Vector3 a, Vector3 b)
    {
        return a.x * b.x + a.y * b.y +  a.z * b.z;
    }
}

public class QuaternionLib
{
    public float x, y, z, w;

    public QuaternionLib(float x, float y, float z, float w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public static QuaternionLib Identity => new QuaternionLib(0.0f,0.0f, 0.0f, 1.0f);
    
    public static QuaternionLib Normalize(QuaternionLib q)
    {
        float magnitude = Mathf.Sqrt(q.x*q.x + q.y*q.y + q.z*q.z + q.w*q.w );
        return new QuaternionLib(q.x/magnitude,  
                                q.y/magnitude, 
                                q.z/magnitude, 
                                q.w/magnitude);
    }

    public static QuaternionLib FromToRotation(Vector3 from, Vector3 to)
    {
        Vector3 fromVec = from.normalized;
        Vector3 toVec = to.normalized;

        float dot = MathLib.DotProduct(fromVec, toVec);
        if (dot > 0.9999f) return Identity;
        if (dot < -0.9999f)
        {
            Vector3 axis = Vector3.Cross(Vector3.right, fromVec);
            
            if (axis.sqrMagnitude < 0.01f)
                axis = Vector3.Cross(Vector3.up, fromVec);
            
            axis.Normalize();
            return new QuaternionLib(axis.x, axis.y, axis.z, 0);
        }

        Vector3 crossAxis = Vector3.Cross(fromVec, toVec);
        float s =  Mathf.Sqrt((1+dot)*2);
        float invS = 1 / s;
        
        return Normalize(new QuaternionLib(
            crossAxis.x * invS,
            crossAxis.y * invS,
            crossAxis.z * invS,
            s * 0.5f)); // TODO : change this line
    }
    
}

