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
    
    void Start()
    {
        if (_spawnManager == null)
        {
            _spawnManager = gameObject.GetComponent<SpawnManager>();
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
        
        //Debug.Log("FromToRotation : ("+ fromToRot.x + ", " +  fromToRot.y + ", " + fromToRot.z + ", " +  fromToRot.w + ")");
        
        // TODO : Clamp rotation by axis
        
        return fromToRot;
    }
    
    public void CCDAlgorithm()
    {
        if (i < 0)
            i = _spawnManager.segments;
        
        _pivot = _spawnManager.joints[i];
        
        QuaternionLib test = RotationBetween();
        
        Quaternion testRotation = QuaternionLib.ApplyRotation(test,  _pivot.transform.rotation);
        _pivot.transform.rotation = testRotation;

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
    
    public bool IsCloseToTarget()
    {
        if ((int)_lastJoint.transform.position.x == (int)target.transform.position.x 
            && 
            (int)_lastJoint.transform.position.y == (int)target.transform.position.y 
            &&
            (int)_lastJoint.transform.position.z == (int)target.transform.position.z )
            return true;
        return false;
    }
}
