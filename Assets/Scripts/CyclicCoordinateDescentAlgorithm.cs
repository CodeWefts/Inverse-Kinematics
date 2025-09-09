
using System.Linq;
using UnityEngine;

public class CyclicCoordinateDescentAlgorithm : MonoBehaviour
{
    [SerializeField] private int nbrIteration;
    [SerializeField] private GameObject _target;
    
    private GameObject _lastJoint; // The end-effector
    private GameObject _pivot;
    

    private Vector3 _pivotToTarget;
    private Vector3 _pivotToLastJoint;
    
    
    public SpawnManager spawnManager;
    void Start()
    {
        _pivotToTarget = new Vector3(0, 0, 0);
        _pivotToLastJoint = new  Vector3(0, 0, 0);
        
        if (spawnManager == null)
        {
            spawnManager = gameObject.GetComponent<SpawnManager>();
        }

        _lastJoint = spawnManager.joints.Last();

    }

    private Vector3 CalculVector(Vector3 positionA, Vector3 positionB)
    {
        Vector3 vector = new Vector3(positionB.x - positionA.x, positionB.y - positionA.y, positionB.z - positionA.z);
        return vector;
    }
    
    
    public void CCDAlgorithm()
    {
        for (int r = 0; r < nbrIteration; r++)
        {
            for (int i = spawnManager.segments; i > 0; i--)
            {
                _pivotToTarget = CalculVector(_pivot.transform.position, _target.transform.position);
                _pivotToLastJoint = CalculVector(_pivot.transform.position, _lastJoint.transform.position);
                
                
            }
        }
    }
}
