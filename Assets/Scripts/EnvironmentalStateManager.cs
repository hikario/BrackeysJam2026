using UnityEngine;

public class EnvironmentalStateManager : MonoBehaviour
{
    [SerializeField]
    GameObject FridgeDoorController;
    int GamePhase;
    GameObject compScreenManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (FridgeDoorController == null)
        {
            FridgeDoorController = GameObject.Find("SM_Fridge/Cube");
        }

        if (compScreenManager == null)
        {
            compScreenManager = GameObject.Find("ComputerScreenManager");
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

    void AdvanceTime()
    {
        GamePhase++;
        compScreenManager.SendMessage("ProgressToNextSequence",GamePhase);
    }

    int GetTime()
    {
        return GamePhase;
    }

}
