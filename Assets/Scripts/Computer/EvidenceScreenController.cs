using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EvidenceScreenController : ComputerScreen
{
    // TODO Add scroll snap for evidence viewer
    [SerializeField] private Button evidenceThumbnailPrefab;
    [SerializeField] private Image photoEvidencePrefab;
    [SerializeField] private AudioEvidence audioEvidencePrefab;
    [SerializeField] private RectTransform evidenceThumbnailParentRectTransform;
    [SerializeField] private RectTransform evidenceParentRectTransform;

    private void OnEnable()
    {
        foreach (Evidence evidence in ComputerScreenManager.instance.evidenceDefinitionReference.evidence)
        {
            if (ComputerScreenManager.currentSequence == evidence.evidencePhase && evidence.CheckForRequiredPurchasable())
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
            }
            else
            {
                //Debug.Log($"{evidence.evidenceName} is NOT collected; Phase is correct is {ComputerScreenManager.currentSequence == evidence.evidencePhase} and purchasable has been placed is {evidence.CheckForRequiredPurchasable()}");
            }
        }
    }

    public void OpenEvidenceViewer(Evidence _evidence)
    {
        Debug.Log($"Display {_evidence.evidenceName}");
    }
}
