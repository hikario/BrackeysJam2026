using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FMODUnity;

public class AudioEvidence : MonoBehaviour
{
    private Evidence evidenceData;
    [SerializeField] private float clipDuration = 1;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Button playButton;
    [SerializeField] private Button stopButton;
    [SerializeField] private Image progressBar;

    public void InitAudioEvidence(Evidence _evidence)
    {
        evidenceData = _evidence;

        clipDuration = _evidence.audioLength;

        playButton.onClick.AddListener(() => TogglePlay(true));
        stopButton.onClick.AddListener(() => TogglePlay(false));
        titleText.text = evidenceData.evidenceName + ".wav";

        this.name = evidenceData.evidenceName + " Evidence";
    }

    public void TogglePlay(bool play)
    {
        playButton.interactable = !play;
        stopButton.interactable = play;

        if (play)
        {
            evidenceData.evidenceAudioEvent.PlayOrResume(Camera.main.gameObject);
            coProgressBar = StartCoroutine(CoProgressBar());
        }
        else
        {
            evidenceData.evidenceAudioEvent.Stop();
            StopCoroutine(coProgressBar);
            coProgressBar = null;
        }
    }

    Coroutine coProgressBar;
    public IEnumerator CoProgressBar()
    {
        if(clipDuration == 0f)
        {
            yield return null;
        }

        float elapsed = 0;
        Debug.Log($"START {Time.frameCount}, {clipDuration}");
        while (elapsed < clipDuration)
        {
            progressBar.fillAmount = elapsed / clipDuration;
            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Debug.Log($"STOP {Time.frameCount}");

        TogglePlay(false);
    }
}
