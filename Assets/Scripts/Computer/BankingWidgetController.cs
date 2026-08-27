using UnityEngine;
using TMPro;

public class BankingWidgetController : ComputerScreen
{
    [SerializeField] private BankingLineItem bankingLineItemPrefab;
    [SerializeField] public TextMeshProUGUI currentBalanceTextDisplay;
    [SerializeField] public Color positiveValueColor = Color.darkSlateGray;
    [SerializeField] public Color negativeValueColor = Color.darkRed;
    [SerializeField] private RectTransform lineItemContainer;

    private void OnEnable()
    {
        UpdateCurrentMoneyTextDisplay();
    }

    public void UpdateCurrentMoneyTextDisplay()
    {
        currentBalanceTextDisplay.text = string.Format("{0:C}", ComputerScreenManager.instance.GetCurrentMoney());

        if (ComputerScreenManager.instance.GetCurrentMoney() > 0)
        {
            currentBalanceTextDisplay.color = positiveValueColor;
        }
        else
        {
            currentBalanceTextDisplay.color = negativeValueColor;
        }
    }

    public void AddNewLineItem(string name, float amount)
    {
        BankingLineItem newLineItem = Instantiate(bankingLineItemPrefab, lineItemContainer);
        newLineItem.InitLineItem(name, amount, positiveValueColor);
    }

    public void AddNewLineItem(PurchasableObject purchasableObject)
    {
        BankingLineItem newLineItem = Instantiate(bankingLineItemPrefab, lineItemContainer);
        newLineItem.InitLineItem(purchasableObject, negativeValueColor);
    }
}