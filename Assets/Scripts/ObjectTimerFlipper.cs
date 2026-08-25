using UnityEngine;
using System.Collections;
using TMPro;

public class ObjectTimerFlipper : MonoBehaviour
{
    [SerializeField]
    public GameObject nextObject;
    public bool turnOff;
    public bool startFlipper;
    public bool finalFlip;
    [SerializeField] private TextMeshProUGUI textToUse;
    [SerializeField] private bool fadeIn = false;
    private float timeMultiplier;
    public float textHangTime;

    [SerializeField]
    private GameObject sceneLoader;
    [SerializeField]
    private GameObject fadeScreenCanvas;

    public float delay;


    void Start()
    {
        if (startFlipper)
        {
            StartFlipper();
        }
    }

    public void StartFlipper()
    {
        if (fadeIn)
        {
            StartCoroutine(IntroFade(textToUse));
        }
        else
        {
            Invoke("FlipObjects", delay);
        }
    }

    public void FlipObjects()
    {
        if(nextObject != null)
        {
            nextObject.SetActive(true);
            nextObject.GetComponent<ObjectTimerFlipper>().StartFlipper();
            // Debug.Log("Fwip!!!!");
        }
        if (turnOff)
        {
            gameObject.SetActive(false);
        }
        if (finalFlip)
        {
            Debug.Log("Final fwip!!!!");
            if (fadeScreenCanvas != null && sceneLoader != null)
            {
                Debug.Log("Loading next scene!!!!");
                sceneLoader.GetComponent<SceneLoader>().NextSceneWithDelay();
                fadeScreenCanvas.GetComponent<SceneFader>().RunFade();
            }
        }
    }

    private IEnumerator IntroFade(TextMeshProUGUI textToUse)
    {
        yield return StartCoroutine(FadeInText(1f, textToUse));
        if (turnOff)
        {
            yield return new WaitForSeconds(textHangTime);
            yield return StartCoroutine(FadeOutText(1f, textToUse));
        }
        if (finalFlip)
        {
            Debug.Log("Final fwip!!!!");
            if (fadeScreenCanvas != null && sceneLoader != null)
            {
                Debug.Log("Loading next scene!!!!");
                sceneLoader.GetComponent<SceneLoader>().NextSceneWithDelay();
                fadeScreenCanvas.GetComponent<SceneFader>().RunFade();
            }
        }
        else
        {
            Invoke("FlipObjects", delay);
        }
        //End of transition, do some extra stuff!!
    }

    private IEnumerator FadeInText(float timeSpeed, TextMeshProUGUI text)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        while (text.color.a < 1.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime * timeSpeed));
            yield return null;
        }
    }
    private IEnumerator FadeOutText(float timeSpeed, TextMeshProUGUI text)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        while (text.color.a > 0.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime * timeSpeed));
            yield return null;
        }
    }
    public void FadeInText(float timeSpeed = -1.0f)
    {
        if (timeSpeed <= 0.0f)
        {
            timeSpeed = timeMultiplier;
        }
        StartCoroutine(FadeInText(timeSpeed, textToUse));
    }
    public void FadeOutText(float timeSpeed = -1.0f)
    {
        if (timeSpeed <= 0.0f)
        {
            timeSpeed = timeMultiplier;
        }
        StartCoroutine(FadeOutText(timeSpeed, textToUse));
    }

    public void RemoveNext()
    {
        nextObject = null;
    }

    public void AddNext(GameObject nextGO)
    {
        nextObject = nextGO;
    }

    public void MakeFinalFlip()
    {
        finalFlip = true;
    }
}
