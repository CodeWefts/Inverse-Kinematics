using System.Linq;
using UnityEngine;

public class CyclicCoordinateDescentAlgorithm : MonoBehaviour
{
    private GameObject _lastJoint; // The end-effector
    private GameObject _pivot; // The joint used for rotation
    
    public int i = -1;

    // Other scripts
    // -------------
    private SpawnManager _spawnManager;
    
    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (_spawnManager == null)
        {
            _spawnManager = GameObject.Find("IKManager").GetComponent<SpawnManager>();
        }
        
        _lastJoint = _spawnManager.joints.Last();
    }

    private QuaternionLib RotationBetween()
    {
        // Vectors
        // -------
        Vector3 _pivotToEE = _lastJoint.transform.position - _pivot.transform.position;
        Vector3 _pivotToTarget = _spawnManager.target.transform.position - _pivot.transform.position;
                
        // Rotation
        // --------
        QuaternionLib fromToRot = QuaternionLib.FromToRotation(_pivotToEE, _pivotToTarget);
        
        return fromToRot;
    }
    
    public void CCDAlgorithm()
    {
        if (i < 0)
            i = _spawnManager.joints.Count - 2;

        Debug.Log("Joint . " + _spawnManager.joints[i].name);
        _pivot = _spawnManager.joints[i];
        JointManager jointt = _pivot.GetComponent<JointManager>();
        
        QuaternionLib fromToRot  = RotationBetween();
        Quaternion testRotation = QuaternionLib.ApplyRotation(fromToRot ,  _pivot.transform.rotation);
        
        if (jointt.clampMin != Vector3.zero && jointt.clampMax != Vector3.zero)
        {
            Vector3 axisX = _pivot.transform.right;
            Vector3 axisY = _pivot.transform.up;
            Vector3 axisZ = _pivot.transform.forward;
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
        }
        
        _pivot.transform.rotation = testRotation;
        
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
}