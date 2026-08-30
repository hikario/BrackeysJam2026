using UnityEngine;
using UnityEngine.EventSystems;

public class BedScript : MonoBehaviour, IPointerClickHandler
{
    EnvironmentalStateManager ESM;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ESM = GameObject.Find("EnvironmentalStateManager").GetComponent<EnvironmentalStateManager>();
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        ESM.AdvanceTime();
    }
}
