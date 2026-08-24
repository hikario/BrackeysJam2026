using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseScreenController : MonoBehaviour
{
    [SerializeField] private PurchasableObjectDefinition purchasableObjectDefinitionReference;
    [SerializeField] public PurchasableListing listingPrefab;
    [SerializeField] public PurchasableObjectViewer rtCameraPrefab;
    private List<PurchasableObjectViewer> viewers = new List<PurchasableObjectViewer>();
    [SerializeField] public RectTransform listingParentRectTransform;
    [SerializeField] public Transform listingObjectsTransform;
    [SerializeField] private int currentObjectTier = 0;


    private void OnEnable()
    {
        foreach (PurchasableObject purchasableObject in purchasableObjectDefinitionReference.purchasableObjects)
        {
            if (currentObjectTier >= purchasableObject.objectTier)
            {
                Debug.Log($"Instantiate listing for {purchasableObject.objectName} at {Time.frameCount}");
                PurchasableListing listing = Instantiate(listingPrefab, listingParentRectTransform);
                listing.InitListing(purchasableObject);
                listing.purchaseButton.onClick.AddListener(() => 
                {
                    PurchaseListing(purchasableObject);
                    GetViewerForListing(purchasableObject).ViewObject(false);
                    listing.purchaseButton.interactable = false;
                });
                listing.onPointerEnter.AddListener(() =>
                {
                    GetViewerForListing(listing.purchasableData).ViewObject(true);
                });
                listing.onPointerExit.AddListener(() =>
                {
                    GetViewerForListing(listing.purchasableData).ViewObject(false);
                });

                float newViewerXPos = 10 * viewers.Count;
                PurchasableObjectViewer viewer = Instantiate(rtCameraPrefab, listingObjectsTransform);
                viewer.transform.localPosition = new Vector3(newViewerXPos, viewer.transform.position.y, viewer.transform.position.z);
                viewer.InitViewer(purchasableObject);
                viewers.Add(viewer);
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(listingParentRectTransform);
    }

    private void PurchaseListing(PurchasableObject _purchasableObject)
    {
        _purchasableObject.isPurchased = true;

        GetViewerForListing(_purchasableObject).ViewObject(false);
    }

    private PurchasableObjectViewer GetViewerForListing(PurchasableObject _purchasableObject)
    {
        foreach(PurchasableObjectViewer viewer in viewers)
        {
            if (viewer.purchasableData == _purchasableObject)
            {
                return viewer;
            }
        }

        Debug.LogError($"NO VIEWER FOUND FOR {_purchasableObject.objectName}!");
        return null;
    }
}
