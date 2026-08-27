using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    // Cameras
    GameObject CurrentCamera;
    GameObject TargetCamera;
    [SerializeField]
    GameObject FridgeInsideCamera;
    [SerializeField]
    GameObject KitchenCamera;
    [SerializeField]
    GameObject BedroomDoorCamera;
    [SerializeField]
    GameObject BedroomCamera;
    [SerializeField]
    GameObject ComputerCamera;

    // Cinemachine Brain variable, so we don't have to keep looking it up
    CinemachineBrain activeBrain;

    // Input actions, only needed for testing/debugging
    InputAction first;
    InputAction second;
    InputAction third;
    InputAction fourth;
    InputAction fifth;

    bool UpdatedCurrent = false;
    [SerializeField]
    GameObject EnvironmentalStateManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        first = InputSystem.actions.FindAction("1");
        second = InputSystem.actions.FindAction("2");
        third = InputSystem.actions.FindAction("3");
        fourth = InputSystem.actions.FindAction("4");
        fifth = InputSystem.actions.FindAction("5");

        activeBrain = CinemachineBrain.GetActiveBrain(0);
        CinemachineCamera CineCam = activeBrain.ActiveVirtualCamera as CinemachineCamera;
        CurrentCamera = CineCam.gameObject;

        if (EnvironmentalStateManager == null)
        {
            EnvironmentalStateManager = GameObject.Find("/EnvironmentalStateManager");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!activeBrain.IsBlending)
        {
            if (first.IsPressed())
            {
                MoveToFridge();
            }
            else if (second.IsPressed())
            {
                MoveToKitchen();
            }
            else if (third.IsPressed())
            {
                MoveToBedroomDoor();
            }
            else if (fourth.IsPressed())
            {
                MoveToBedroom();
            }
            else if (fifth.IsPressed())
            {
                MoveToComputer();
            }
        }
        else
        {
            if (!UpdatedCurrent)
            {
                CurrentCamera.SetActive(false);
                CurrentCamera = TargetCamera;
                UpdatedCurrent = true;
            }
        }
    }

    void MoveToFridge()
    {
        FridgeInsideCamera.SetActive(true);
        TargetCamera = FridgeInsideCamera;
        UpdatedCurrent = false;

        if(CurrentCamera != FridgeInsideCamera)
        {
            Debug.Log("Opening Fridge");
            EnvironmentalStateManager.SendMessage("OpenFridge");
        }
    }

    void MoveToKitchen()
    {
        KitchenCamera.SetActive(true);
        TargetCamera = KitchenCamera;
        UpdatedCurrent = false;

        if(CurrentCamera == FridgeInsideCamera)
        {
            EnvironmentalStateManager.SendMessage("CloseFridge");
        }
    }

    void MoveToBedroomDoor()
    {
        BedroomDoorCamera.SetActive(true);
        TargetCamera = BedroomDoorCamera;
        UpdatedCurrent = false;

        if(CurrentCamera == FridgeInsideCamera)
        {
            EnvironmentalStateManager.SendMessage("CloseFridge");
        }
    }

    void MoveToBedroom()
    {
        BedroomCamera.SetActive(true);
        TargetCamera = BedroomCamera;
        UpdatedCurrent = false;

        if(CurrentCamera == FridgeInsideCamera)
        {
            EnvironmentalStateManager.SendMessage("CloseFridge");
        }
    }

    void MoveToComputer()
    {
        ComputerCamera.SetActive(true);
        TargetCamera = ComputerCamera;
        UpdatedCurrent = false;

        if(CurrentCamera == FridgeInsideCamera)
        {
            EnvironmentalStateManager.SendMessage("CloseFridge");
        }
    }
}
