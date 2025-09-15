
using System;
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
    
    // Other scripts
    // -------------
    private SpawnManager _spawnManager;
    private QuaternionLib quat;
    
    void Start()
    {
        if (_spawnManager == null)
        {
            _spawnManager = gameObject.GetComponent<SpawnManager>();
        }
        _lastJoint = _spawnManager.joints.Last();
    }
    
    public void CCDAlgorithm()
    {
        for (int j = 0; j < nbrIteration; j++)
        {
            for (int i = _spawnManager.segments - 1; i >= 0; i--)
            {
                _pivot = _spawnManager.joints[i];
                
                // Directions
                _pivotToEE = _lastJoint.transform.position - _pivot.transform.position;
                _pivotToTarget = target.transform.position - _pivot.transform.position;
                
                // Rotation calc
                QuaternionLib test = QuaternionLib.FromToRotation(_pivotToEE, _pivotToTarget);
                Debug.Log("MY OWN FROM TO ROTATION values : (" + test.x + ", " +test.y+ ", " + test.z+ ", "+ test.w + ") and index : " + i);
                
                QuaternionLib testRot = QuaternionLib.Lerp(QuaternionLib.Identity, test, 0.1f);
                Debug.Log("MY OWN LERP values : (" + testRot.x + ", " +testRot.y+ ", " + testRot.z+ ", "+ testRot.w + ") and index : " + i);
                
                Quaternion testRotation = QuaternionLib.ApplyRotation(testRot,  _pivot.transform.rotation);
                _pivot.transform.rotation = testRotation;
            }
        }
    }

    void Update()
    {
        CCDAlgorithm();
    }
}
