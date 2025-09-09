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
                break;
            case Algorithm.FABRIK:
                break;
        }
    }
}
