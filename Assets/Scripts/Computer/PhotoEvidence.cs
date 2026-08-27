using UnityEngine;
using UnityEngine.UI;

public class PhotoEvidence : MonoBehaviour
{
    private Evidence evidenceData;
    [SerializeField] public Toggle relevantToggle;
    [SerializeField] public Image photoImage;

    public void InitPhotoEvidence(Evidence _evidence)
    {
        evidenceData = _evidence;

        relevantToggle.isOn = evidenceData.flaggedAsRelevant;
        relevantToggle.onValueChanged.AddListener((value) => 
        {
            ComputerScreenManager.instance.evidenceDefinitionReference.MarkEvidenceAsRelevant(evidenceData, value);
            ComputerScreenManager.instance.evidenceScreenController.GetThumbnailForEvidence(_evidence).relevantImage.enabled = value;
        });
        photoImage.sprite = evidenceData.evidenceSprite;

        this.name = evidenceData.evidenceName + " Evidence";
    }
}
