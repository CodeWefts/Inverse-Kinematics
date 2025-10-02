using NUnit.Framework;
using UnityEngine;

public class MathLib : MonoBehaviour
{
    public static float DotProduct(Vector3 a, Vector3 b)
    {
        return a.x * b.x + a.y * b.y +  a.z * b.z;
    }
    
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle > 180f)
            angle -= 360f;
        return Mathf.Clamp(angle, min, max);
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
        return new QuaternionLib(  q.x/magnitude,  
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
            s * 0.5f));
    }
    
    public static Quaternion ClampRotationHinge(Quaternion rotation, Vector3 clampMin, Vector3 clampMax,Vector3 axisX, Vector3 axisY, Vector3 axisZ, Vector3 initialForward)
    {
        Vector3 rotatedForward = rotation * initialForward;

        float angleX = Vector3.SignedAngle(initialForward, rotatedForward, axisX);
        float angleY = Vector3.SignedAngle(initialForward, rotatedForward, axisY);
        float angleZ = Vector3.SignedAngle(initialForward, rotatedForward, axisZ);

        angleX = Mathf.Clamp(angleX, clampMin.x, clampMax.x);
        angleY = Mathf.Clamp(angleY, clampMin.y, clampMax.y);
        angleZ = Mathf.Clamp(angleZ, clampMin.z, clampMax.z);

        Quaternion rotX = Quaternion.AngleAxis(angleX, axisX);
        Quaternion rotY = Quaternion.AngleAxis(angleY, axisY);
        Quaternion rotZ = Quaternion.AngleAxis(angleZ, axisZ);

        return rotX * rotY * rotZ;
    }

    public static QuaternionLib Lerp(QuaternionLib a, QuaternionLib b, float t)
    {
        QuaternionLib result = new QuaternionLib(a.x + (b.x - a.x ) * t, 
                                                 a.y + (b.y - a.y ) * t, 
                                                 a.z + (b.z - a.z ) * t, 
                                              a.w + (b.w - a.w ) * t);
        return Normalize(result);
    }

    public static Quaternion ApplyRotation(QuaternionLib a, Quaternion b)
    {
        Quaternion result = new Quaternion(
               a.w * b.x + a.x * b.w + a.y * b.z - a.z * b.y,
               a.w * b.y - a.x * b.z + a.y * b.w + a.z * b.x,
               a.w * b.z + a.x * b.y - a.y * b.x + a.z * b.w,
            a.w * b.w - a.x * b.x - a.y * b.y - a.z * b.z);
        return result;
    }
    
}

