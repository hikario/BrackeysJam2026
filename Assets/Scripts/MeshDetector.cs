using UnityEngine;
using UnityEngine.EventSystems;

public class MeshDetector : MonoBehaviour
{
    void Start()
    {
        addPhysicsRaycaster();
    }

    void addPhysicsRaycaster()
    {
        PhysicsRaycaster physicsRaycaster = GameObject.FindFirstObjectByType<PhysicsRaycaster>();
        if (physicsRaycaster == null)
        {
            Camera.main.gameObject.AddComponent<PhysicsRaycaster>();
        }
    }
}
