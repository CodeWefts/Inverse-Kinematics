
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
    private float CalculAngleDeg(Vector3 vectorA, Vector3 vectorB, Vector3 vectorC)
    {
        /*
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
        
        return result;*/
        
        Vector3 aNorm = vectorA.normalized;
        Vector3 bNorm = vectorB.normalized;
    
        float dot = DotProduct(aNorm, bNorm);
        dot = Mathf.Clamp(dot, -1f, 1f);

        float angle = Mathf.Acos(dot);

        Vector3 cross = Vector3.Cross(aNorm, bNorm);
        float sign = Mathf.Sign(Vector3.Dot(cross, vectorC));

        float angleDeg = angle * Mathf.Rad2Deg * sign;

        return angleDeg;
    }
    
    // TODO : ADAPT THIS FUNCTION BY MY OWN CALC
    private float SignOfAngle(Vector3 from, Vector3 to, Vector3 axis)
    {
        Vector3 cross = Vector3.Cross(from, to);
        float sign = Vector3.Dot(cross, axis);
        return sign >= 0 ? 1f : -1f;
    }
    
    #endregion
    
    public void CCDAlgorithm()
    {
        // TEST
        if (i < 0)
            i = spawnManager.segments - 1;
        
        /*for (int j = 0; j < nbrIteration; j++)
        {
            for (int i = spawnManager.segments - 1; i > 0; i--)
            {*/
                _pivot = spawnManager.joints[i];
            
                _pivotToTarget = CalculVector(_pivot.transform.position, target.transform.position);
                _pivotToLastJoint = CalculVector(_pivot.transform.position, _lastJoint.transform.position);

                float angle = CalculAngleDeg(_pivotToTarget, _pivotToLastJoint, Vector3.forward);
                Debug.Log(angle);
            
                _pivot.transform.rotation = Quaternion.Euler(0,0,angle);
                    
                    
                // TODO : Check this func
                //float sign = SignOfAngle(_pivotToTarget, _pivotToLastJoint, Vector3.forward);
                
                //angle *= sign;
                
                //spawnManager.joints[i].transform.rotation = Quaternion.Euler(0, 0, angle);
            /*}
        }*/
        
        
        i--;
    }
    
    //TEST
    private int i = -1;
    [SerializeField] bool iteration =  false;
    
    private void IterationFunc()
    {
        if (iteration)
            CCDAlgorithm();
        
        iteration = false;
    }

    void Update()
    {
        IterationFunc();
    }
}
