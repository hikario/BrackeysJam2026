using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class PurchasableListing : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public PurchasableObject purchasableData;
    [SerializeField] public RectTransform containerRectTransform;
    [SerializeField] public Button purchaseButton;
    [SerializeField] private Image objectImage;
    [SerializeField] private TextMeshProUGUI objectName;
    [SerializeField] private TextMeshProUGUI objectPrice;
    [SerializeField] private TextMeshProUGUI objectDescription;
    [SerializeField] public UnityEvent onPointerEnter;
    [SerializeField] public UnityEvent onPointerExit;

    public void InitListing(PurchasableObject purchasable)
    {
        purchasableData = purchasable;

        this.name = purchasableData.objectName + " Listing";
        objectImage.material = purchasableData.objectRTMaterial;
        objectName.text = purchasableData.objectName;
        objectPrice.text = string.Format("{0:C}", purchasableData.objectPrice);
        objectDescription.text = purchasableData.objectDescription;

        if (purchasableData.isPurchased)
        {
            purchaseButton.interactable = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointerEnter.Invoke();
        LayoutRebuilder.ForceRebuildLayoutImmediate(this.GetComponent<RectTransform>());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onPointerExit.Invoke();
        LayoutRebuilder.ForceRebuildLayoutImmediate(this.GetComponent<RectTransform>());
    }
}
