using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PurchasableListing : MonoBehaviour
{
    private PurchasableObject purchasableData;
    [SerializeField] public Button purchaseButton;
    [SerializeField] private Image objectImage;
    [SerializeField] private TextMeshProUGUI objectName;
    [SerializeField] private TextMeshProUGUI objectPrice;
    [SerializeField] private TextMeshProUGUI objectDescription;

    public void InitListing(PurchasableObject purchasable)
    {
        purchasableData = purchasable;

        this.name = purchasableData.objectName + " Listing";

        objectName.text = purchasableData.objectName;
        objectPrice.text = "$" + purchasableData.objectPrice.ToString();
        objectDescription.text = purchasableData.objectDescription;
    }
}
