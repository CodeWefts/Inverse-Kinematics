
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
    
    
    private SpawnManager _spawnManager;
    void Start()
    {
        if (_spawnManager == null)
        {
            _spawnManager = gameObject.GetComponent<SpawnManager>();
        }

        _lastJoint = _spawnManager.joints.Last();

    }
    
    #region Math calculations

    private float DotProduct(Vector2 a, Vector2 b)
    {
        /*
        if (axis == Vector3.right) // Axis X
        {
            return a.y * b.y + a.z * b.z;
        }
        else if (axis == Vector3.forward) // Axis Z
        {
            return  a.x * b.x + a.y * b.y;
        }
        else // Axis Y
        {
            return a.x * b.x + a.z * b.z;
        }*/
        
        return a.x * b.x + a.y * b.y;
    }
    
    private float DotProduct(Vector3 a, Vector3 b, Vector3 axis)
    {
        return a.x * b.x + a.y * b.y +  a.z * b.z;
    }

    private float NormVector(Vector3 a)
    {
        return Mathf.Sqrt(a.x * a.x + a.y * a.y + a.z * a.z);
    }
    private float CalculAngleDeg(Vector3 vectorA, Vector3 vectorB, Vector3 axis)
    {
        // SETUP 
        // ----
        
        // TEST FROM SCRATCH
        /*
        Vector2 vectorFrom;
        Vector2 vectorTo;
        
        if (axis == Vector3.right) // Axis X
        {
            vectorFrom = new Vector2(vectorA.y, vectorA.z);
            vectorTo = new Vector2(vectorB.y, vectorB.z);
        }
        else if (axis == Vector3.forward) // Axis Z
        {
            vectorFrom = new Vector2(vectorA.y, vectorA.z);
            vectorTo = new Vector2(vectorB.y, vectorB.z);
        }
        else // Axis Y
        {
            vectorFrom = new Vector2(vectorA.y, vectorA.z);
            vectorTo = new Vector2(vectorB.y, vectorB.z);
        }
        
        float dotProduct = DotProduct(vectorFrom, vectorTo);
        float normFrom =  NormVector(vectorFrom);
        float normTo =  NormVector(vectorTo);

        float angle = dotProduct / (normFrom * normTo);
        angle = Mathf.Acos(angle);
        return angle;*/
        
        // CALCULATIONS
        // ------------
        
        // Dot product
        float dot = DotProduct(vectorA, vectorB);

        // Norm Vector
        float normVectorA =  NormVector(vectorA);
        float normVectorB = NormVector(vectorB);
        
        // --------------OK--------------------------------
        
        // test
        float angle = dot / (normVectorA * normVectorB);
        angle = Mathf.Acos(angle);
        
        return angle *  Mathf.Rad2Deg;
    }
    
    #endregion
    
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
                // TODO : Build my own "FromToRotation" and "Lerp" Functions
                Quaternion pivotToEE = Quaternion.FromToRotation(_pivotToEE, _pivotToTarget);
                Quaternion rotRestrained = Quaternion.Lerp(Quaternion.identity, pivotToEE, 0.1f);
                
                
                _pivot.transform.rotation = rotRestrained * _pivot.transform.rotation;
            }
        }
    }

    void Update()
    {
        CCDAlgorithm();
    }
}
