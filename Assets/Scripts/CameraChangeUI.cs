using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CameraChangeUI : MonoBehaviour
{
    public Button prevCameraButton;
    public Button nextCameraButton;

    public UnityEvent onPrevCameraButtonPressed;
    public UnityEvent onNextCameraButtonPressed;

    public void OnEnable()
    {
        prevCameraButton.onClick.AddListener(OnPrevCameraButtonPressed);
        nextCameraButton.onClick.AddListener(OnNextCameraButtonPressed);
    }

    private void OnPrevCameraButtonPressed()
    {
        onPrevCameraButtonPressed.Invoke();
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void OnNextCameraButtonPressed()
    {
        onNextCameraButtonPressed.Invoke();
        EventSystem.current.SetSelectedGameObject(null);
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