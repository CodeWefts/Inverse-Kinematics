using System.Linq;
using UnityEngine;

public class CyclicCoordinateDescentAlgorithm : MonoBehaviour
{
    private GameObject _lastJoint; // The end-effector
    private GameObject _pivot; // The joint used for rotation

    public int i = -1;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        // Setup
        // -----
        if (_spawnManager == null)
        {
            _spawnManager = GameObject.Find("IKManager").GetComponent<SpawnManager>();
        }

        _lastJoint = _spawnManager.joints.Last();
    }

    // Other scripts
    // -------------
    private SpawnManager _spawnManager;

    public void CCDAlgorithm()
    {
        if (i < 0)
            i = _spawnManager.joints.Count - 2;

        _pivot = _spawnManager.joints[i];
        JointManager jointt = _pivot.GetComponent<JointManager>();

        Quaternion fromToRot = MathLib.RotationBetween(_lastJoint.transform.position, _pivot.transform.position, _spawnManager.target.transform.position);

        Quaternion testRotation = QuaternionLib.ApplyRotation(fromToRot, _pivot.transform.rotation);
        _pivot.transform.rotation = testRotation;

        Quaternion test = QuaternionLib.ClampRotation(_pivot.transform, jointt.clampMin, jointt.clampMax);
        _pivot.transform.localRotation = test;
        i--;
    }

    public void Iteration()
    {
        // For debugging
        // -------------
        if (_spawnManager.ite)
        {
            CCDAlgorithm();
            _spawnManager.ite = false;
        }
    }
}