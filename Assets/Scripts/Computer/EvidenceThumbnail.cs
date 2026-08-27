using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EvidenceThumbnail : MonoBehaviour
{
    private Evidence evidenceData;
    [SerializeField] public Button button;
    [SerializeField] private Image thumbnailImage;
    [SerializeField] public Image relevantImage;
    [SerializeField] private TextMeshProUGUI titleText;

    public void InitEvidenceThumbnail(Evidence _evidence)
    {
        evidenceData = _evidence;

        thumbnailImage.sprite = evidenceData.evidenceSprite;
        if (evidenceData.evidenceType == EvidenceType.Photo)
        {
            titleText.text = evidenceData.evidenceName + ".png";
        }
        else if (evidenceData.evidenceType == EvidenceType.Audio)
        {
            titleText.text = evidenceData.evidenceName + ".wav";
        }

        relevantImage.enabled = evidenceData.flaggedAsRelevant;
        
        this.name = evidenceData.evidenceName + " Thumbnail";
    }

    public Evidence EvidenceDataFromThumbnail()
    {
        return evidenceData;
    }
}
