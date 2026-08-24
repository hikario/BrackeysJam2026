using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PurchasableObjectDefinition", menuName = "Scriptable Objects/PurchasableObjectDefinition")]
public class PurchasableObjectDefinition : ScriptableObject
{
    [SerializeField] public List<PurchasableObject> purchasableObjects = new List<PurchasableObject>();
}
