using UnityEngine;

public class JointManager : MonoBehaviour
{

    [SerializeField] public int _segments = 3;

    [SerializeField] public GameObject joint; // TODO : Add joint at each bone's start and end.
    [SerializeField] public GameObject bone;

    private Vector3 _initialPosition = new Vector3(0,0,0);
    void Start()
    {
        for (int j = 0; j < _segments; j++)
        {
            Instantiate(bone, _initialPosition, Quaternion.identity);
            _initialPosition = _initialPosition + new Vector3(0, 2, 0); // TODO : Change int raw value.
        }
    }
    
}
