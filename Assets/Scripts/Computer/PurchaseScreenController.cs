using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseScreenController : ComputerScreen
{
    [SerializeField] public bool resetOnPlay = true;
    [SerializeField] public PurchasableListing listingPrefab;
    [SerializeField] public PurchasableObjectViewer rtCameraPrefab;
    private List<PurchasableObject> instantiatedObjects = new List<PurchasableObject>();
    private List<PurchasableObjectViewer> viewers = new List<PurchasableObjectViewer>();
    [SerializeField] public RectTransform listingParentRectTransform;
    [SerializeField] public Transform listingObjectsTransform;

    private void OnEnable()
    {
        foreach (PurchasableObject purchasableObject in ComputerScreenManager.instance.purchasableObjectDefinitionReference.purchasableObjects)
        {
            if (ComputerScreenManager.currentSequence >= purchasableObject.objectTier && !instantiatedObjects.Contains(purchasableObject))
            {
                //Debug.Log($"Instantiate listing for {purchasableObject.objectName} at {Time.frameCount}");
                PurchasableListing listing = Instantiate(listingPrefab, listingParentRectTransform);
                listing.InitListing(purchasableObject);
                listing.purchaseButton.onClick.AddListener(() => 
                {
                    PurchaseListing(purchasableObject);
                    listing.purchaseButton.interactable = false;
                });
                listing.onPointerEnter.AddListener(() =>
                {
                    GetViewerForListing(purchasableObject).ViewObject(true);
                });
                listing.onPointerExit.AddListener(() =>
                {
                    GetViewerForListing(purchasableObject).ViewObject(false);
                });

                float newViewerXPos = 10 * viewers.Count;
                PurchasableObjectViewer viewer = Instantiate(rtCameraPrefab, listingObjectsTransform);
                viewer.transform.localPosition = new Vector3(newViewerXPos, viewer.transform.position.y, viewer.transform.position.z);
                viewer.InitViewer(purchasableObject);
                viewers.Add(viewer);

                instantiatedObjects.Add(purchasableObject);
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(listingParentRectTransform);
    }

    private void PurchaseListing(PurchasableObject _purchasableObject)
    {
        //Debug.Log($"Purchase {_purchasableObject.objectName} at {Time.frameCount}");
        _purchasableObject.isPurchased = true;
        ComputerScreenManager.instance.purchasableObjectDefinitionReference.MarkObjectAsPurchased(_purchasableObject.objectName);
        ComputerScreenManager.instance.UpdateCurrentMoney(-_purchasableObject.objectPrice);
        ComputerScreenManager.instance.bankingScreenController.AddNewLineItem(_purchasableObject);
    }

    private PurchasableObjectViewer GetViewerForListing(PurchasableObject _purchasableObject)
    {
        foreach(PurchasableObjectViewer viewer in viewers)
        {
            if (viewer.purchasableData == _purchasableObject)
            {
                //Debug.Log($"Return {viewer.gameObject.name} at {Time.frameCount}");
                return viewer;
            }
        }

        Debug.LogError($"NO VIEWER FOUND FOR {_purchasableObject.objectName}!");
        return null;
    }
}
