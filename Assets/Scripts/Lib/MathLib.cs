using NUnit.Framework;
using UnityEngine;

public class MathLib : MonoBehaviour
{
    public static float DotProduct(Vector3 a, Vector3 b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }

    public static Quaternion RotationBetween(Vector3 lastJoint, Vector3 pivot, Vector3 target)
    {
        // Vectors
        // -------
        Vector3 _pivotToEE = lastJoint - pivot;
        Vector3 _pivotToTarget = target - pivot;

        // Rotation
        // --------
        QuaternionLib fromToRot = QuaternionLib.FromToRotation(_pivotToEE, _pivotToTarget);

        return new Quaternion(fromToRot.x, fromToRot.y, fromToRot.z, fromToRot.w);
    }

    public static float NormalizeAngle(float angle)
    {
        angle = angle % 360f;
        if (angle > 180f) angle -= 360f;
        return angle;
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

    public static QuaternionLib Identity => new QuaternionLib(0.0f, 0.0f, 0.0f, 1.0f);

    public static QuaternionLib Normalize(QuaternionLib q)
    {
        float magnitude = Mathf.Sqrt(q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w);
        return new QuaternionLib(q.x / magnitude,
                                   q.y / magnitude,
                                   q.z / magnitude,
                                q.w / magnitude);
    }

    public static Quaternion ClampRotation(Transform joint, Vector3 minEuler, Vector3 maxEuler)
    {
        Vector3 euler = joint.localEulerAngles;

        // Normalize to -180..180 for consistent clamping
        euler.x = MathLib.NormalizeAngle(euler.x);
        euler.y = MathLib.NormalizeAngle(euler.y);
        euler.z = MathLib.NormalizeAngle(euler.z);

        // Conditionally clamp each axis
        euler.x = Mathf.Clamp(euler.x, minEuler.x, maxEuler.x);
        euler.y = Mathf.Clamp(euler.y, minEuler.y, maxEuler.y);
        euler.z = Mathf.Clamp(euler.z, minEuler.z, maxEuler.z);

        return Quaternion.Euler(euler);
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
        float s = Mathf.Sqrt((1 + dot) * 2);
        float invS = 1 / s;

        return Normalize(new QuaternionLib(
            crossAxis.x * invS,
            crossAxis.y * invS,
            crossAxis.z * invS,
            s * 0.5f));
    }

    public static Quaternion ApplyRotation(Quaternion a, Quaternion b)
    {
        Quaternion result = new Quaternion(
               a.w * b.x + a.x * b.w + a.y * b.z - a.z * b.y,
               a.w * b.y - a.x * b.z + a.y * b.w + a.z * b.x,
               a.w * b.z + a.x * b.y - a.y * b.x + a.z * b.w,
            a.w * b.w - a.x * b.x - a.y * b.y - a.z * b.z);
        return result;
    }
}