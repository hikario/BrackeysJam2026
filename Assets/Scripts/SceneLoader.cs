using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader: MonoBehaviour
{
    public float delay;

    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene("00_MainMenu");
    }

    public void LoadMainMenuSceneWithDelay()
    {
        Invoke("LoadMainMenuScene", delay);
    }

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void NextSceneWithDelay()
    {
        Invoke("NextScene", delay);
    }

    public void ApartmentScene()
    {
        SceneManager.LoadScene("ApartmentLevel");
    }

    public void ApartmentSceneWithDelay()
    {
        Invoke("ApartmentScene", delay);
    }

    public void RollCredits()
    {
        SceneManager.LoadScene("CreditRoll");
    }

    public void RollCreditsWithDelay()
    {
        Invoke("RollCredits", delay);
    }

    public void ReloadSceneWithDelay()
    {
        Invoke("ReloadScene", delay);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
