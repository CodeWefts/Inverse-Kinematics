using UnityEngine;
using UnityEngine.InputSystem;  // nouveau Input System

public class TargetSpawner : MonoBehaviour
{/* // TODO : Spawn the target when left clicked 
    public GameObject targetPrefab;
    private GameObject currentTarget;
    private float fixedZ = 0f;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector3 mousePos = Mouse.current.position.ReadValue();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane));
            worldPos.z = fixedZ;

            if (currentTarget == null)
            {
                currentTarget = Instantiate(targetPrefab, worldPos, Quaternion.identity);
            }
            else
            {
                currentTarget.transform.position = worldPos;
            }
        }
    }*/
}