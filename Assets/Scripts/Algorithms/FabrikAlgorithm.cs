using UnityEngine;

public class FabrikAlgorithm : MonoBehaviour
{
    private float[] lengths;
    private float totalLength;
    private Vector3[] positions; // Keep each joint positions during algorithm
    private float threshold = 0.01f;

    // Other scripts
    // -------------
    private SpawnManager _spawnManager;
    
    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (_spawnManager == null)
        {
            _spawnManager = GameObject.Find("IKManager").GetComponent<SpawnManager>();
        }
        
        int nbrJoints = _spawnManager.joints.Count;
        lengths = new float[nbrJoints - 1];
        positions = new Vector3[nbrJoints];
        totalLength = 0;

        for (int i = 0; i < nbrJoints - 1; i++)
        {
            lengths[i] = Vector3.Distance(_spawnManager.joints[i].transform.position, _spawnManager.joints[i + 1].transform.position);
            totalLength += lengths[i];
        }
    }

    public void Solve()
    {
        if (_spawnManager.target == null || _spawnManager.joints.Count == 0)
            return;

        int n = _spawnManager.joints.Count;
        
        for (int i = 0; i < n; i++)
            positions[i] = _spawnManager.joints[i].transform.position;

        if ((_spawnManager.target.transform.position - _spawnManager.joints[0].transform.position).sqrMagnitude > totalLength * totalLength)
        {
            for (int i = 0; i < n - 1; i++)
            {
                float r = Vector3.Distance(positions[i], _spawnManager.target.transform.position);
                float lambda = lengths[i] / r;
                positions[i + 1] = (1 - lambda) * positions[i] + lambda * _spawnManager.target.transform.position;
            }
        }
        else
        {
            Vector3 basePosition = positions[0];
            float distToTarget = Vector3.Distance(positions[n - 1], _spawnManager.target.transform.position);
            int iter = 0;

            while (distToTarget > threshold && iter < 10) // TODO: REWORK THIS LINE
            {
                // BACKWARD REACHING
                // -----------------
                positions[n - 1] = _spawnManager.target.transform.position;
                for (int i = n - 2; i >= 0; i--)
                {
                    float r = Vector3.Distance(positions[i + 1], positions[i]);
                    float lambda = lengths[i] / r;
                    positions[i] = (1 - lambda) * positions[i + 1] + lambda * positions[i];
                }

                // FORWARD REACHING
                // ----------------
                positions[0] = basePosition;
                for (int i = 0; i < n - 1; i++)
                {
                    float r = Vector3.Distance(positions[i + 1], positions[i]);
                    float lambda = lengths[i] / r;
                    positions[i + 1] = (1 - lambda) * positions[i] + lambda * positions[i + 1];
                }

                distToTarget = Vector3.Distance(positions[n - 1], _spawnManager.target.transform.position);
                iter++;
            }
        }

        // Rotations
        for (int i = 0; i < n - 1; i++)
        {
            Vector3 dir = positions[i + 1] - positions[i];
            _spawnManager.joints[i].transform.rotation = Quaternion.LookRotation(dir) * Quaternion.Euler(90, 0, 0);
        }

        _spawnManager.joints[n - 1].transform.position = positions[n - 1];
    }
}
