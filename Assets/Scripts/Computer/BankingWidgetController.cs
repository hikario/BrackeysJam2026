using UnityEngine;
using TMPro;

public class BankingWidgetController : ComputerScreen
{
    [SerializeField] private BankingLineItem bankingLineItemPrefab;
    [SerializeField] private TextMeshProUGUI currentBalanceTextDisplay;
    [SerializeField] private Color positiveValueColor = Color.darkSlateGray;
    [SerializeField] private Color negativeValueColor = Color.darkRed;
    [SerializeField] private GameObject[] finalDayLockoutObjects;
    [SerializeField] private RectTransform lineItemContainer;

    private void OnEnable()
    {
        UpdateCurrentMoneyTextDisplay();
    }

    public void UpdateCurrentMoneyTextDisplay()
    {
        if (ComputerScreenManager.currentSequence == 3)
        {
            foreach (GameObject obj in finalDayLockoutObjects)
            {
                obj.SetActive(true);
            }
        }

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