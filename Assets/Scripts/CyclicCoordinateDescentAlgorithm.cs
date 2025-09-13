
using System.Linq;
using UnityEngine;

public class CyclicCoordinateDescentAlgorithm : MonoBehaviour
{
    [SerializeField] private int nbrIteration;
    [SerializeField] private GameObject target;
    
    private GameObject _lastJoint; // The end-effector
    private GameObject _pivot; // The joint used for rotation
    

    [Header("Pivot to Target")] private Vector3 _pivotToTarget = new Vector3(0, 0, 0);
    [Header("Pivot to LastJoint")]private Vector3 _pivotToLastJoint = new  Vector3(0, 0, 0);
    
    
    private SpawnManager spawnManager;
    void Start()
    {
        if (spawnManager == null)
        {
            spawnManager = gameObject.GetComponent<SpawnManager>();
        }

        _lastJoint = spawnManager.joints.Last();

    }
    
    #region Math calculations
    
    private Vector3 CalculVector(Vector3 positionA, Vector3 positionB)
    {
        return positionB - positionA;
    }

    private float DotProduct(Vector3 a, Vector3 b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }

    private float NormVector(Vector3 a)
    {
        return Mathf.Sqrt(a.x * a.x + a.y * a.y + a.z * a.z);
    }
    private float CalculAngleDeg(Vector3 vectorA, Vector3 vectorB)
    {
        // Dot product
        float dot = DotProduct(vectorA, vectorB);
        
        // Norm Vector
        float normVectorA =  NormVector(vectorA);
        float normVectorB = NormVector(vectorB);
        
        // Clamp 
        float div = dot / (normVectorA * normVectorB);
        if (div > 1f) div = 1f;
        else if (div < -1f) div = -1f;

        // Arc-cosine for angle in rad
        float result = Mathf.Acos(div) * Mathf.Rad2Deg;
        
        return result;
    }
    #endregion
    
    public void CCDAlgorithm()
    {
        for (int i = spawnManager.segments; i > 0; i--)
        {
            _pivotToTarget = CalculVector(_pivot.transform.position, target.transform.position);
            _pivotToLastJoint = CalculVector(_pivot.transform.position, _lastJoint.transform.position);

            float angle = CalculAngleDeg(_pivotToTarget, _pivotToLastJoint);
        }
        
    }
}
