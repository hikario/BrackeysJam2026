using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
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
    CinemachineBrain activeBrain;
    InputAction first;
    InputAction second;
    InputAction third;
    InputAction fourth;
    InputAction fifth;

    bool UpdatedCurrent = false;

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
        // CurrentCamera.SetActive(false);
        TargetCamera = FridgeInsideCamera;
        UpdatedCurrent = false;
    }

    void MoveToKitchen()
    {
        KitchenCamera.SetActive(true);
        // CurrentCamera.SetActive(false);
        TargetCamera = KitchenCamera;
        UpdatedCurrent = false;
    }

    void MoveToBedroomDoor()
    {
        BedroomDoorCamera.SetActive(true);
        // CurrentCamera.SetActive(false);
        TargetCamera = BedroomDoorCamera;
        UpdatedCurrent = false;
    }

    void MoveToBedroom()
    {
        BedroomCamera.SetActive(true);
        // CurrentCamera.SetActive(false);
        TargetCamera = BedroomCamera;
        UpdatedCurrent = false;
    }

    void MoveToComputer()
    {
        ComputerCamera.SetActive(true);
        // CurrentCamera.SetActive(false);
        TargetCamera = ComputerCamera;
        UpdatedCurrent = false;
    }
}
