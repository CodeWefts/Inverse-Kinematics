using UnityEngine;

public class AlgoChoice : MonoBehaviour
{
    public enum Algorithm
    {
        CCD = 0,
        Jacobian = 1,
        FABRIK = 2
    }
        
    [SerializeField] public Algorithm algorithm;
    [SerializeField] public CyclicCoordinateDescentAlgorithm ccd;
    [SerializeField] public JacobianAlgorithm jacobian;

    void Start()
    {
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
                break;
        }
    }
}
