using System.Collections.Generic;
using System.Linq; // For list parameter
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] public int segments = 3;
    
    // GAME OBJECT TODO : Create gameobject directly in code and not on the scene
    [SerializeField] public GameObject joint; // TODO : Add joint to the end of the skeleton.
    [SerializeField] public GameObject bone;
    [SerializeField] public GameObject target;
    
    private Vector3 _initialJointPosition = new Vector3(0,0,0);
    private Vector3 _initialBonePosition = new Vector3(0,0,0);

    public List<GameObject> bones = new List<GameObject>();
    public List<GameObject> joints = new List<GameObject>();
    
    
    private void SpawnBones()
    {
        float blue_nuances = 0f;
        float blue_incrementation = 1f/segments;
        
        for (int j = 0; j < segments; j++)
        {
            // ------------------- JOINTS  -------------------------
            // _____________________________________________________
            
            GameObject newJoint = Instantiate(joint, _initialJointPosition, Quaternion.identity);
            joints.Add(newJoint);
            
            newJoint.gameObject.GetComponent<Renderer>().material.color = new Color(0,0,blue_nuances);
            
            // -------------------  BONES  -------------------------
            // _____________________________________________________

            _initialBonePosition.y = _initialJointPosition.y + 1f; // TODO : Change int raw value to the scale of joint's gameobject
            
            GameObject newBone = Instantiate(bone, _initialBonePosition,  Quaternion.identity);
            bones.Add(newBone);

            newBone.gameObject.GetComponent<Renderer>().material.color = new Color(0,0,blue_nuances);

            _initialJointPosition = _initialBonePosition + new Vector3(0, 1, 0); // TODO : Change int raw value.
            
            // -------------------  RELATION  ----------------------
            // _____________________________________________________
            if (j != 0)
            {
                joints.Last().transform.SetParent(joints[j-1].transform);
            }
            
            bones.Last().transform.SetParent(joints.Last().transform);
            
            blue_nuances += blue_incrementation;
        }
        
        GameObject lastJoint = Instantiate(joint, _initialJointPosition, Quaternion.identity);
        joints.Add(lastJoint);
        int index = joints.Count - 1;
        joints.Last().transform.SetParent(joints[index-1].transform);
        
        
    }
    
    void Start()
    {
        SpawnBones();
    }
}
