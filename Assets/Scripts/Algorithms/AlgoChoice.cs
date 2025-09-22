using UnityEngine;

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

    void Start()
    {
        if(spawn == null)
            spawn = gameObject.GetComponent<SpawnManager>();
        
        switch (algorithm)
        {
            case Algorithm.CCD:
                if (ccd == null)
                {
                    ccd = gameObject.GetComponent<CyclicCoordinateDescentAlgorithm>();
                    ccd.CCDAlgorithm();
                }
                
                break;
            case Algorithm.Jacobian:
                if (jacobian == null)
                {
                    jacobian = gameObject.GetComponent<JacobianAlgorithm>();
                    jacobian.JacobianAlgorithmFunc();
                }
                break;
            
            case Algorithm.FABRIK:
                if (fabrik == null)
                {
                    fabrik = gameObject.GetComponent<FabrikAlgorithm>();
                    fabrik.FabrikAlgorithmFunc();
                }
                break;
        }
    }

    private void Reset()
    {
        for (int j = 0; j < spawn.segments; j++)
        {
            spawn.joints[j].transform.rotation = Quaternion.identity;
        }

        ccd.i = -1;
        reset = false;
    }

    void Update()
    {
        /*
        switch (typeOfIteration)
        {
            case TypeOfIteration.AUTO:
                
                if (algorithm == Algorithm.CCD)
                {
                    if(!ccd.IsCloseToTarget())
                    {
                        ccd.CCDAlgorithm();
                    }
                }

                else if (algorithm == Algorithm.FABRIK)
                {
                    
                }
                
                else{}
                
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
            Reset();*/
    }
}
