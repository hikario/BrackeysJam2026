using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PurchasableObjectDefinition", menuName = "Scriptable Objects/PurchasableObjectDefinition")]
public class PurchasableObjectDefinition : ScriptableObject
{
    [SerializeField] public List<PurchasableObject> purchasableObjects = new List<PurchasableObject>();

    public void MarkObjectAsPurchased(string purchasableObjectName)
    {
        foreach (PurchasableObject purchasableObject in purchasableObjects)
        {
            if (purchasableObject.objectName == purchasableObjectName)
            {
                purchasableObject.isPurchased = true;
            }
        }
    }
    public void MarkObjectAsPlaced(string purchasableObjectName)
    {
        foreach (PurchasableObject purchasableObject in purchasableObjects)
        {
            if (purchasableObject.objectName == purchasableObjectName)
            {
                purchasableObject.isPlaced = true;
            }
        }
    }
}
