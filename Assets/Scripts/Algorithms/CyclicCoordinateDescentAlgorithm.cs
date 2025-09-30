using System.Linq;
using UnityEngine;

public class CyclicCoordinateDescentAlgorithm : MonoBehaviour
{
    [SerializeField] private int nbrIteration;
    [SerializeField] private GameObject target;
    
    private GameObject _lastJoint; // The end-effector
    private GameObject _pivot; // The joint used for rotation
    

    [Header("Pivot to Target")] private Vector3 _pivotToTarget = new Vector3(0, 0, 0);
    [Header("Pivot to LastJoint")]private Vector3 _pivotToEE = new  Vector3(0, 0, 0);
    
    public int i = -1;
    [SerializeField] public bool ite = false;

    // Other scripts
    // -------------
    private SpawnManager _spawnManager;
    
    /*
    void Start()
    {
        if (_spawnManager == null)
        {
            _spawnManager = gameObject.GetComponent<SpawnManager>();
        }
        _lastJoint = _spawnManager.joints.Last();
    }*/

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
    
    public void CCDAlgorithm()
    {
        if (i < 0)
            i = _spawnManager.segments;
        
        _pivot = _spawnManager.joints[i];

        QuaternionLib test = RotationBetween();

        // TODO : Rework those lines
        Quaternion testRotation = QuaternionLib.ApplyRotation(test,  _pivot.transform.rotation);
        Quaternion finalRotation = QuaternionLib.ClampRotationHinge(testRotation, -90f, 90f, _pivot.transform.forward,_pivot.transform.up);
        
        _pivot.transform.rotation = finalRotation;
        //_pivot.transform.rotation = testRotation;

        i--;
    }
    
    public void Iteration()
    {
        if (ite)
        {
            CCDAlgorithm();
            ite = false;
        }
            
    }
    
    public bool IsCloseToTarget(float tolerance = 0.1f)
    {
        //test
        float distance = Vector3.Distance(_lastJoint.transform.position, target.transform.position);
        return distance <= tolerance;
    }
}
