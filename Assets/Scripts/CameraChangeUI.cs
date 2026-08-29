using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CameraChangeUI : MonoBehaviour
{
    public Button prevCameraButton;
    public Button nextCameraButton;

    public UnityEvent onPrevCameraButtonPressed;
    public UnityEvent onNextCameraButtonPressed;

    public void OnEnable()
    {
        prevCameraButton.onClick.AddListener(OnPrevCameraButtonPressed);
        prevCameraButton.onClick.AddListener(OnNextCameraButtonPressed);
    }

    private void OnPrevCameraButtonPressed()
    {
        onPrevCameraButtonPressed.Invoke();
    }

    private void OnNextCameraButtonPressed()
    {
        onNextCameraButtonPressed.Invoke();
    }

    public void TogglePrevCameraButtonInteraction(bool interaction)
    {
        prevCameraButton.interactable = interaction;
    }

    public void ToggleNextCameraButtonInteraction(bool interaction)
    {
        nextCameraButton.interactable = interaction;
    }
}