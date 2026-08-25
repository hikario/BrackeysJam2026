using UnityEngine;

public class OpenFridgeDoor : MonoBehaviour
{
    public GameObject doorTop;
    public GameObject doorBottom;
    private float doorRotationTop;
    private float doorRotationBottom;
    private bool openDoorTop;
    private bool openDoorBottom;

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
        if (doorRotationTop > -60)
        {
            doorTop.transform.RotateAround(doorTop.transform.position, Vector3.up, -20 * Time.deltaTime);
            doorRotationTop += Vector3.up.y * -20 * Time.deltaTime;
        }
    }

    void CloseDoorTop()
    {
        if (doorRotationTop < 0)
        {
            doorTop.transform.RotateAround(doorTop.transform.position, Vector3.up, +20 * Time.deltaTime);
            doorRotationTop += Vector3.up.y * +20 * Time.deltaTime;
        }
        if (doorRotationTop > 0)
        {
            doorTop.transform.Rotate(0.0f, 0.0f, 0.0f);
            doorRotationTop = 0;
        }
    }

    void OpenDoorBottom()
    {
        if (doorRotationBottom > -60)
        {
            doorBottom.transform.RotateAround(doorBottom.transform.position, Vector3.up, -20 * Time.deltaTime);
            doorRotationBottom += Vector3.up.y * -20 * Time.deltaTime;
        }
    }

    void CloseDoorBottom()
    {
        if (doorRotationBottom < 0)
        {
            doorBottom.transform.RotateAround(doorBottom.transform.position, Vector3.up, +20 * Time.deltaTime);
            doorRotationBottom += Vector3.up.y * +20 * Time.deltaTime;
        }
        if (doorRotationBottom > 0)
        {
            doorBottom.transform.Rotate(0.0f, 0.0f, 0.0f);
            doorRotationBottom = 0;
        }
    }
}
