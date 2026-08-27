using UnityEngine;
using TMPro;

public class BankingLineItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemCostText;

    public void InitLineItem(PurchasableObject purchasableObject, Color color)
    {
        itemNameText.text = purchasableObject.objectName;
        itemCostText.text = string.Format("{0:C}", -purchasableObject.objectPrice);
        itemCostText.color = color; 
    }
    public void InitLineItem(string name, float amount, Color color)
    {
        itemNameText.text = name;
        itemCostText.text = string.Format("{0:C}", amount);
        itemCostText.color = color; 
    }
}
