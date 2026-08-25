using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 30f; // Set the countdown duration
    public bool timerIsRunning = false;
    public TextMeshProUGUI timerText; // Optional: Assign a UI Text element in the Inspector
    private string timerString;
    public float timerDelay = 3.5f;
    public GameObject buttonList;
    public GameObject fadeScreenCanvas;

    [SerializeField]
    private GameObject nextObject;

    void Start()
    {
        UpdateTimerDisplay();
        if (buttonList == null)
        {
            buttonList = GameObject.Find("ActionButtons");
        }
        buttonList.SendMessage("DisableButtons");
    }

    public void TimerInvoke()
    {
        Invoke("StartTimer",timerDelay);
    }

    public void StartTimer()
    {
        timerIsRunning = true;
        buttonList.SendMessage("EnableButtons");
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime; // Decrease time
                
                UpdateTimerDisplay(); // Update UI
            }
            else
            {
                TimeUp();
            }
        }
    }

    void UpdateTimerDisplay()
    {
        int seconds = (int)(timeRemaining % 60);
        int minutes = (int)(timeRemaining / 60);

        timerString = string.Format("{0:00}:{1:00}", minutes, seconds);
        timerText.text = timerString;
    }

    public void TimeUp()
    {
        Debug.Log("Time's up!");
        timeRemaining = 0;
        UpdateTimerDisplay();
        timerIsRunning = false;
        nextObject.GetComponent<ObjectTimerFlipper>().StartFlipper();
        if (fadeScreenCanvas != null)
        {
            fadeScreenCanvas.GetComponent<SceneFader>().RunFade();
        }
    }
}
