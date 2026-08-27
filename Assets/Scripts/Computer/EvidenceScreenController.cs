using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using TMPro;

public class EvidenceScreenController : ComputerScreen
{
    private List<Evidence> evidenceToDisplay = new List<Evidence>();
    private List<EvidenceThumbnail> evidenceThumbnails = new List<EvidenceThumbnail>();
    [SerializeField] private EvidenceThumbnail evidenceThumbnailPrefab;
    [SerializeField] private PhotoEvidence photoEvidencePrefab;
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
                EvidenceThumbnail thumbnail = Instantiate(evidenceThumbnailPrefab, evidenceThumbnailParentRectTransform);
                thumbnail.InitEvidenceThumbnail(evidence);
                thumbnail.button.onClick.AddListener(() => OpenEvidenceViewer(evidence));
                evidenceThumbnails.Add(thumbnail);

                if (evidence.evidenceType == EvidenceType.Photo)
                {
                    PhotoEvidence photoEvidence = Instantiate(photoEvidencePrefab, evidenceParentRectTransform);
                    photoEvidence.InitPhotoEvidence(evidence);
                }
                else if (evidence.evidenceType == EvidenceType.Audio)
                {
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

    public EvidenceThumbnail GetThumbnailForEvidence(Evidence _evidence)
    {
        foreach (EvidenceThumbnail evidenceThumbnail in evidenceThumbnails)
        {
            if (evidenceThumbnail.EvidenceDataFromThumbnail() == _evidence)
            {
                return evidenceThumbnail;
            }
        }

        Debug.LogError($"NO THUMBNAIL FOUND FOR {_evidence.evidenceName}");
        return null;
    }
}
