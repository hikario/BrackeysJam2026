using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using TMPro;

public class EvidenceScreenController : ComputerScreen
{
    private List<Evidence> evidenceToDisplay = new List<Evidence>();
    [SerializeField] private Button evidenceThumbnailPrefab;
    [SerializeField] private Image photoEvidencePrefab;
    [SerializeField] private AudioEvidence audioEvidencePrefab;
    [SerializeField] private RectTransform evidenceThumbnailParentRectTransform;

    [Header("Evidence Viewer")]
    [SerializeField] private GameObject evidenceViewerGameObject;
    [SerializeField] private RectTransform evidenceParentRectTransform;
    [SerializeField] private ScrollSnap evidenceScrollSnap;
    [SerializeField] private Animator evidenceViewerAnimator;

    private void OnEnable()
    {
        evidenceViewerGameObject.SetActive(false);

        foreach (Evidence evidence in ComputerScreenManager.instance.evidenceDefinitionReference.evidence)
        {
            if (ComputerScreenManager.currentSequence == evidence.evidencePhase && evidence.CheckForRequiredPurchasable() && !evidenceToDisplay.Contains(evidence))
            {
                //Debug.Log($"{evidence.evidenceName} is collected, instantiate");
                Button thumbnail = Instantiate(evidenceThumbnailPrefab, evidenceThumbnailParentRectTransform);
                thumbnail.GetComponentInChildren<Image>().sprite = evidence.evidenceSprite;
                thumbnail.onClick.AddListener(() => OpenEvidenceViewer(evidence));
                thumbnail.name = evidence.evidenceName + " Thumbnail";

                if (evidence.evidenceType == EvidenceType.Photo)
                {
                    thumbnail.GetComponentInChildren<TextMeshProUGUI>().text = evidence.evidenceName + ".png";
                    Image photoEvidence = Instantiate(photoEvidencePrefab, evidenceParentRectTransform);
                    photoEvidence.sprite = evidence.evidenceSprite;
                    photoEvidence.gameObject.name = evidence.evidenceName + " Evidence";
                }
                else if (evidence.evidenceType == EvidenceType.Audio)
                {
                    thumbnail.GetComponentInChildren<TextMeshProUGUI>().text = evidence.evidenceName + ".wav";
                    AudioEvidence audioEvidence = Instantiate(audioEvidencePrefab, evidenceParentRectTransform);
                    audioEvidence.InitAudioEvidence(evidence);
                }
                evidenceToDisplay.Add(evidence);
            }
            //else
            //{
            //    Debug.Log($"{evidence.evidenceName} is NOT collected; Phase is correct is {ComputerScreenManager.currentSequence == evidence.evidencePhase} and purchasable has been placed is {evidence.CheckForRequiredPurchasable()}");
            //}
        }
    }

    int newPage = 0;
    public void OpenEvidenceViewer(Evidence _evidence)
    {
        //Debug.Log($"Display {_evidence.evidenceName}");
        evidenceViewerGameObject.SetActive(true);

        foreach (Evidence evidence in evidenceToDisplay)
        {
            if (evidence.evidenceName == _evidence.evidenceName)
            {
                newPage = evidenceToDisplay.IndexOf(evidence);
            }
        }

        Invoke("ChangePage", .1f);
    }
    private void ChangePage()
    {
        evidenceScrollSnap.ChangePage(newPage);
    }

    public void CloseEvidenceViewer()
    {
        evidenceViewerAnimator.SetBool("expand", false);
        Invoke("DisableEvidenceViewer", .5f);
    }
    private void DisableEvidenceViewer()
    {
        evidenceViewerGameObject.SetActive(false);
    }
}
