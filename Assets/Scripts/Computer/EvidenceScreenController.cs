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

    [Header("Conspiracy Board ;P")]
    public UILineRenderer lineRenderer;
    private bool firstPointSet = false;


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
                    photoEvidence.relevantToggle.onValueChanged.AddListener((value) => UpdateLineRenderer(value, GetThumbnailForEvidence(evidence).GetComponent<RectTransform>(),
                        ComputerScreenManager.instance.evidenceDefinitionReference.evidence.IndexOf(evidence)));
                }
                else if (evidence.evidenceType == EvidenceType.Audio)
                {
                    AudioEvidence audioEvidence = Instantiate(audioEvidencePrefab, evidenceParentRectTransform);
                    audioEvidence.InitAudioEvidence(evidence);
                    audioEvidence.relevantToggle.onValueChanged.AddListener((value) => UpdateLineRenderer(value, GetThumbnailForEvidence(evidence).GetComponent<RectTransform>(), 
                        ComputerScreenManager.instance.evidenceDefinitionReference.evidence.IndexOf(evidence)));
                }
                evidenceToDisplay.Add(evidence);
            }
            //else
            //{
            //    Debug.Log($"{evidence.evidenceName} is NOT collected; Phase is correct is {ComputerScreenManager.currentSequence == evidence.evidencePhase} and purchasable has been placed is {evidence.CheckForRequiredPurchasable()}");
            //}
        }

        for (int i = 0; i < evidenceToDisplay.Count; i++)
        {
            if (evidenceToDisplay[i].flaggedAsRelevant)
            {
                UpdateLineRenderer(true, GetThumbnailForEvidence(evidenceToDisplay[i]).GetComponent<RectTransform>(), 
                    ComputerScreenManager.instance.evidenceDefinitionReference.evidence.IndexOf(evidenceToDisplay[i]));
            }
        }
    }

    private void UpdateLineRenderer(bool addVertex, RectTransform rectTransform, int iD)
    {
        Vector2[] positionsArray = new Vector2[lineRenderer.Points.Length];
        positionsArray = lineRenderer.Points;
        List<Vector2> positionsList = new List<Vector2>();
        positionsList.AddRange(positionsArray);
        float verticalVariation = iD;
        if (iD%2 == 0)
        {
            verticalVariation = verticalVariation * 25;
        }
        else
        {
            verticalVariation = verticalVariation * -25;
        }

        Debug.Log($"ID is {iD}, vertical variation is {verticalVariation}");

        if (addVertex)
        {
            if (!firstPointSet)
            {
                positionsList[0] = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + verticalVariation);
                firstPointSet = true;
            }
            else
            {
                positionsList.Add(new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + verticalVariation));
            }
        }
        else
        {
            for (int i = 0; i < positionsList.Count - 1; i++)
            {
                if (positionsList[i] == new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + verticalVariation))
                {
                    positionsList.RemoveAt(i);
                }
            }
        }

        positionsArray = positionsList.ToArray();
        lineRenderer.Points = positionsArray;
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
