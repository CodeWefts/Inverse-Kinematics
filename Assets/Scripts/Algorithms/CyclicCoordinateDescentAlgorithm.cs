using System.Linq;
using UnityEngine;

public class CyclicCoordinateDescentAlgorithm : MonoBehaviour
{
    private int nbrIteration;
    private GameObject target;
    
    private GameObject _lastJoint; // The end-effector
    private GameObject _pivot; // The joint used for rotation
    

    [Header("Pivot to Target")] private Vector3 _pivotToTarget = new Vector3(0, 0, 0);
    [Header("Pivot to LastJoint")]private Vector3 _pivotToEE = new  Vector3(0, 0, 0);
    
    public int i = -1;

    // Other scripts
    // -------------
    private SpawnManager _spawnManager;
    public float threshold = 0.01f;
    
    void Start()
    {
        if (_spawnManager == null)
        {
            _spawnManager = GameObject.Find("IKManager").GetComponent<SpawnManager>();
        }
        
        if (target == null)
        {
            target = _spawnManager.target;
        }
        
        _lastJoint = _spawnManager.joints.Last();
    }

    public QuaternionLib RotationBetween()
    {
        // Vectors
        // -------
        _pivotToEE = _lastJoint.transform.position - _pivot.transform.position;
        _pivotToTarget = target.transform.position - _pivot.transform.position;
                
        // Rotation
        // --------
        QuaternionLib fromToRot = QuaternionLib.FromToRotation(_pivotToEE, _pivotToTarget);
        
        return fromToRot;
    }

    private int iter = 10;
    public void CCDAlgorithm()
    {
        /*
        int iterations = 10; // nombre d’itérations
        for (int it = 0; it < iterations; it++)
        {
            for (int j = _spawnManager.joints.Count - 2; j >= 0; j--) // -2 pour sauter l’effector
            {
                Transform joint = _spawnManager.joints[j].transform;
                Vector3 toEnd = _lastJoint.transform.position - joint.position;
                Vector3 toTarget = target.transform.position - joint.position;

                Vector3 axis = Vector3.Cross(toEnd, toTarget); // l'axe de rotation minimal pour aligner
                float angle = Vector3.SignedAngle(toEnd, toTarget, axis);

                // Appliquer en RELATIF pour éviter les sur-rotations globales
                if (axis.sqrMagnitude > 1e-4f) // éviter axes nuls
                    joint.rotation = Quaternion.AngleAxis(angle, axis.normalized) * joint.rotation;

                if (Vector3.Distance(_lastJoint.transform.position, target.transform.position) < threshold)
                    return;
            }
        }*/
        
        if (i < 0)
            i = _spawnManager.joints.Count - 2;

        _pivot = _spawnManager.joints[i];
        JointManager jointt = _pivot.GetComponent<JointManager>();
        
        QuaternionLib fromToRot  = RotationBetween();
        Quaternion testRotation = QuaternionLib.ApplyRotation(fromToRot ,  _pivot.transform.rotation);

        Vector3 axisX = _pivot.transform.right;
        Vector3 axisY = _pivot.transform.up;
        Vector3 axisZ = _pivot.transform.forward;
        // TODO : Rework those lines
        Quaternion finalRotation = QuaternionLib.ClampRotationHinge(
            testRotation,
            jointt.clampMin,
            jointt.clampMax,
            axisX,
            axisY,
            axisZ,
            _pivot.transform.forward 
            );

        _pivot.transform.rotation = finalRotation;
        i--;
    }
    
    public void Iteration()
    {
        if (_spawnManager.ite)
        {
            CCDAlgorithm();
            _spawnManager.ite = false;
        }
    }
    
    public bool IsCloseToTarget()
    {
        //test
        float distance = Vector3.Distance(_lastJoint.transform.position, target.transform.position);
        return distance <= threshold;
    }
}