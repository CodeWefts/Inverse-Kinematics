using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JointManager : MonoBehaviour
{

    [SerializeField] public int _segments = 3;

    [SerializeField] public GameObject joint; // TODO : Add joint at each bone's start and end.
    [SerializeField] public GameObject bone;

    private Vector3 _initialJointPosition = new Vector3(0,0,0);
    private Vector3 _initialBonePosition = new Vector3(0,0,0);
    
    private List<GameObject> _bones = new List<GameObject>(); // TEST 
    private List<GameObject> _joints = new List<GameObject>(); // TEST 

    private void SpawnBones()
    {
        for (int j = 0; j < _segments; j++)
        {
            GameObject newJoint = Instantiate(joint, _initialJointPosition, Quaternion.identity);
            _joints.Add(newJoint);
            
            _initialBonePosition.y = _initialJointPosition.y + 1f; // TODO : Change int raw value to the scale of joint's gameobject
            
            GameObject newBone = Instantiate(bone, _initialBonePosition,  Quaternion.identity);
            _bones.Add(newBone);
            
            if (j != 0)
            {
                _joints.Last().transform.SetParent(_joints[j-1].transform);
            }
            
            _bones.Last().transform.SetParent(_joints.Last().transform);
            
            _initialJointPosition = _initialBonePosition + new Vector3(0, 1, 0); // TODO : Change int raw value.
        }
    }
    
    void Start()
    {
        SpawnBones();
        
    }
    
}
