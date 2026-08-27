using UnityEngine;

public class EnvironmentalStateManager : MonoBehaviour
{
    [SerializeField]
    GameObject FridgeDoorController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (FridgeDoorController == null)
        {
            FridgeDoorController = GameObject.Find("SM_Fridge/Cube");
        }
    }

    // Open Fridge Door
    void OpenFridge()
    {
        FridgeDoorController.SendMessage("SetBottomDoorOpen");
    }

    void CloseFridge()
    {
        FridgeDoorController.SendMessage("SetBottomDoorClosed");
    }

}
