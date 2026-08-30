using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;
using FMODUnity;

public class CameraController : MonoBehaviour
{
    // Cameras
    GameObject CurrentCamera;
    GameObject TargetCamera;
    [SerializeField]
    GameObject LivingRoomCamera;
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
    [SerializeField]
    GameObject UICanvas;

    [SerializeField]
    public EventReference playerFS;

    // Cinemachine Brain variable, so we don't have to keep looking it up
    CinemachineBrain activeBrain;

    // Input actions, only needed for testing/debugging
    InputAction first;
    InputAction second;
    InputAction third;
    InputAction fourth;
    InputAction fifth;

    List<GameObject> cameraList;
    int cameraListIndex;
    int cameraListLength;

    bool UpdatedCurrent = false;
    bool playingAudio = false;

    float elapsedTime = 0.0f;
    float timeBetweenPlays = 0.8f;
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

        cameraList = new List<GameObject>();
        cameraList.Add(LivingRoomCamera);
        cameraList.Add(FridgeInsideCamera);
        cameraList.Add(KitchenCamera);
        cameraList.Add(BedroomDoorCamera);
        cameraList.Add(BedroomCamera);
        cameraList.Add(ComputerCamera);

        cameraListLength = cameraList.Count;
        cameraListIndex = 2;
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
            if(!playingAudio)
            {
                playerFS.PlayOneShot();
                playingAudio = true;
            }
            else
            {
                elapsedTime += Time.deltaTime;
                if(elapsedTime >= timeBetweenPlays)
                {
                    playingAudio = false;
                    elapsedTime = 0.0f;
                }
            }

            if (!UpdatedCurrent)
            {
                CurrentCamera.SetActive(false);
                CurrentCamera = TargetCamera;
                UpdatedCurrent = true;
            }
        }
    }

    void MoveToLivingRoom()
    {
        LivingRoomCamera.SetActive(true);
        TargetCamera = LivingRoomCamera;
        UpdatedCurrent = false;

        if(CurrentCamera == FridgeInsideCamera)
        {
            EnvironmentalStateManager.SendMessage("CloseFridge");
        }
    }

    void MoveToFridge()
    {
        FridgeInsideCamera.SetActive(true);
        TargetCamera = FridgeInsideCamera;
        UpdatedCurrent = false;

        if(CurrentCamera != FridgeInsideCamera)
        {
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

    public void MoveToPrevious()
    {
        // cameraNextSFX.PlayOneShot();
        int previous = cameraListIndex;
        cameraListIndex = Math.Min(++cameraListIndex, cameraListLength-1);

        if(previous != cameraListIndex)
        {
            switch(cameraListIndex)
            {
                case 0:
                    MoveToLivingRoom();
                    break;
                case 1:
                    MoveToFridge();
                    break;
                case 2:
                    MoveToKitchen();
                    break;
                case 3:
                    MoveToBedroomDoor();
                    break;
                case 4:
                    MoveToBedroom();
                    break;
                case 5:
                    MoveToComputer();
                    break;
                default:
                    Debug.Log("Oh hell.");
                    break;
            }
            if(cameraListIndex == cameraListLength - 1)
            {
                UICanvas.SendMessage("TogglePrevCameraButtonInteraction", false);
            }
            if (previous == 0)
            {
                UICanvas.SendMessage("ToggleNextCameraButtonInteraction", true);
            }
        }


    }

    public void MoveToNext()
    {
        // cameraNextSFX.PlayOneShot();
        int previous = cameraListIndex;
        cameraListIndex = Math.Max(--cameraListIndex, 0);

        if(previous != cameraListIndex)
        {
            switch(cameraListIndex)
            {
                case 0:
                    MoveToLivingRoom();
                    break;
                case 1:
                    MoveToFridge();
                    break;
                case 2:
                    MoveToKitchen();
                    break;
                case 3:
                    MoveToBedroomDoor();
                    break;
                case 4:
                    MoveToBedroom();
                    break;
                case 5:
                    MoveToComputer();
                    break;
                default:
                    Debug.Log("Oh shit.");
                    break;
            }
            if(cameraListIndex == 0)
            {
                UICanvas.SendMessage("ToggleNextCameraButtonInteraction", false);
            }
            if(previous == cameraListLength - 1)
            {
                UICanvas.SendMessage("TogglePrevCameraButtonInteraction", true);
            }
        }
    }
}
