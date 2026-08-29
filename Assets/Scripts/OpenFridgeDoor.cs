using UnityEngine;
using FMODUnity;

public class OpenFridgeDoor : MonoBehaviour
{
    public GameObject doorTop;
    public GameObject doorBottom;
    private float doorRotationTop;
    private float doorRotationBottom;
    private bool openDoorTop;
    private bool openDoorBottom;
    public float doorSpeed;

    public EventReference doorOpenAudioEvent;
    public EventReference doorCloseAudioEvent;
    public EventReference fridgeAmbientAudioEvent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        doorRotationTop = 0.0f;
        doorRotationBottom = 0.0f;
        openDoorTop = false;
        openDoorBottom = false;
    }

    void Update()
    {
        if (openDoorTop == true)
        {
            OpenDoorTop();
        }
        if (openDoorTop == false)
        {
            CloseDoorTop();
        }
        if (UnityEngine.InputSystem.Keyboard.current.bKey.isPressed)
        {
            openDoorBottom = true;
        }
        if (openDoorBottom == true)
        {
            OpenDoorBottom();
        }
        if (UnityEngine.InputSystem.Keyboard.current.cKey.isPressed)
        {
            openDoorBottom = false;
        }
        if (openDoorBottom == false)
        {
            CloseDoorBottom();
        }
    }
    
    void OpenDoorTop()
    {
        doorOpenAudioEvent.PlayOneShot();
        fridgeAmbientAudioEvent.Play();
        if (doorRotationTop > -60)
        {
            doorTop.transform.RotateAround(doorTop.transform.position, Vector3.up, (-1 * doorSpeed) * Time.deltaTime);
            doorRotationTop += Vector3.up.y * (-1 * doorSpeed) * Time.deltaTime;
        }
    }

    void CloseDoorTop()
    {
        doorCloseAudioEvent.PlayOneShot();
        fridgeAmbientAudioEvent.Stop();
        if (doorRotationTop < 0)
        {
            doorTop.transform.RotateAround(doorTop.transform.position, Vector3.up, doorSpeed * Time.deltaTime);
            doorRotationTop += Vector3.up.y * doorSpeed * Time.deltaTime;
        }
        if (doorRotationTop > 0)
        {
            doorTop.transform.Rotate(0.0f, 0.0f, 0.0f);
            doorRotationTop = 0;
        }
    }

    void OpenDoorBottom()
    {
        doorOpenAudioEvent.PlayOneShot();
        fridgeAmbientAudioEvent.Play();
        if (doorRotationBottom > -60)
        {
            doorBottom.transform.RotateAround(doorBottom.transform.position, Vector3.up, (-1 * doorSpeed) * Time.deltaTime);
            doorRotationBottom += Vector3.up.y * (-1 * doorSpeed) * Time.deltaTime;
        }
    }

    void CloseDoorBottom()
    {
        doorCloseAudioEvent.PlayOneShot();
        fridgeAmbientAudioEvent.Stop();
        if (doorRotationBottom < 0)
        {
            doorBottom.transform.RotateAround(doorBottom.transform.position, Vector3.up, doorSpeed * Time.deltaTime);
            doorRotationBottom += Vector3.up.y * doorSpeed * Time.deltaTime;
        }
        if (doorRotationBottom > 0)
        {
            doorBottom.transform.Rotate(0.0f, 0.0f, 0.0f);
            doorRotationBottom = 0;
        }
    }

    void SetBottomDoorOpen()
    {
        openDoorBottom = true;
    }

    void SetBottomDoorClosed()
    {
        openDoorBottom = false;
    }
}
