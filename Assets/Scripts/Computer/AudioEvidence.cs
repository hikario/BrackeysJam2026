using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AudioEvidence : MonoBehaviour
{
    private Evidence evidenceData;
    // TODO replace Unity Audio setup with FMOD event emitters
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float clipDuration;
    [SerializeField] private Button playButton;
    [SerializeField] private Button stopButton;
    [SerializeField] private Image progressBar;

    public void InitAudioEvidence(Evidence _evidence)
    {
        evidenceData = _evidence;

        audioSource.clip = _evidence.evidenceAudioClip;
        clipDuration = _evidence.evidenceAudioClip.length;
        playButton.onClick.AddListener(() => TogglePlay(true));
        stopButton.onClick.AddListener(() => TogglePlay(false));

        this.name = evidenceData.evidenceName + " Evidence";
    }

    public void TogglePlay(bool play)
    {
        playButton.interactable = !play;
        stopButton.interactable = play;

        if (play)
        {
            audioSource.Play();
            coProgressBar = StartCoroutine(CoProgressBar());
        }
        else
        {
            StopCoroutine(coProgressBar);
            coProgressBar = null;
        }
    }

    Coroutine coProgressBar;
    public IEnumerator CoProgressBar()
    {
        float elapsed = 0;

        while (elapsed < clipDuration)
        {
            progressBar.fillAmount = elapsed / clipDuration;
            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        TogglePlay(false);
    }
}
