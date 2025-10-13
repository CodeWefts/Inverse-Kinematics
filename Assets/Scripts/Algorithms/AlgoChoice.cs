using UnityEngine;
using System.Linq;

public class AlgoChoice : MonoBehaviour
{
    public enum Algorithm
    {
        CCD = 0,
        Jacobian = 1,
        FABRIK = 2
    }

    public enum TypeOfIteration
    {
        AUTO = 0,
        CLICK_TO_ACTIVATE = 1
    }

    public bool reset = false;
        
    [SerializeField] public Algorithm algorithm;
    [SerializeField] public TypeOfIteration typeOfIteration;
    
    [SerializeField] public CyclicCoordinateDescentAlgorithm ccd;
    [SerializeField] public JacobianAlgorithm jacobian;
    [SerializeField] public FabrikAlgorithm fabrik;
    
    [SerializeField] public SpawnManager spawn;

	[SerializeField] public float threshold = 0.01f;
    
    void Start()
    {
        Init();
    }

    void Update()
    {
        Solvers();
    }
    
    private void Init()
    {
        if(spawn == null)
            spawn = gameObject.GetComponent<SpawnManager>();

        switch (algorithm)
        {
            case Algorithm.CCD:
                if (ccd == null)
                {
                    gameObject.AddComponent<CyclicCoordinateDescentAlgorithm>();
                    ccd = gameObject.GetComponent<CyclicCoordinateDescentAlgorithm>();
                }
                break;

            case Algorithm.Jacobian:
                if (jacobian == null)
                {
                    gameObject.AddComponent<JacobianAlgorithm>();
                    jacobian = gameObject.GetComponent<JacobianAlgorithm>();
                }
                break;
            
            case Algorithm.FABRIK:
                if (fabrik == null)
                {
                    gameObject.AddComponent<FabrikAlgorithm>();
                    fabrik = gameObject.GetComponent<FabrikAlgorithm>();
                }
                break;
        }
    }

    private void Solvers()
    {
        switch (typeOfIteration)
        {
            case TypeOfIteration.AUTO:
                
                if (algorithm == Algorithm.CCD)
                {
                    if(!IsCloseToTarget())
                    {
                        ccd.CCDAlgorithm();
                    }
                }

                else if (algorithm == Algorithm.FABRIK)
                {
                    fabrik.Solve();
                }
                
                else
                {
                    jacobian.JacobianAlgorithmFunc();
                }
                
                break;
            case TypeOfIteration.CLICK_TO_ACTIVATE:
                
                if(algorithm == Algorithm.CCD)
                    ccd.Iteration();
                
                else if (algorithm == Algorithm.FABRIK)
                {
                    
                }
                
                else{}
                break;
        }

        if (reset)
            Reset();
    }

    private void Reset()
    {
        for (int j = 0; j < spawn.joints.Count - 1; j++)
        {
            spawn.joints[j].transform.rotation = Quaternion.identity;
        }

        ccd.i = -1;
        reset = false;
    }

	 public bool IsCloseToTarget()
    {
		GameObject _lastJoint = spawn.joints.Last();
		GameObject target = spawn.target;
        float distance = Vector3.Distance(_lastJoint.transform.position, target.transform.position);
        return distance <= threshold;
    }
}
