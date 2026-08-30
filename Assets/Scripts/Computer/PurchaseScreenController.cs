using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using FMODUnity;

public class PurchaseScreenController : ComputerScreen
{
    [SerializeField] public bool resetOnPlay = true;
    [SerializeField] public PurchasableListing listingPrefab;
    [SerializeField] public PurchasableObjectViewer rtCameraPrefab;
    private List<PurchasableObject> instantiatedObjects = new List<PurchasableObject>();
    private List<PurchasableListing> listings = new List<PurchasableListing>();
    private List<PurchasableObjectViewer> viewers = new List<PurchasableObjectViewer>();
    [SerializeField] public RectTransform listingParentRectTransform;
    [SerializeField] public RectTransform listingDisplayRectTransform;
    [SerializeField] public Transform listingObjectsTransform;
    [SerializeField] private PurchasableListing currentlyHighlightedListing;
    [SerializeField] private EventReference purchaseAudioEvent;

    public void UpdatePurchasableObjects()
    {
        foreach (PurchasableObject purchasableObject in ComputerScreenManager.instance.purchasableObjectDefinitionReference.purchasableObjects)
        {
            if (ComputerScreenManager.currentSequence >= purchasableObject.objectTier && !instantiatedObjects.Contains(purchasableObject))
            {
                //Debug.Log($"Instantiate listing for {purchasableObject.objectName} at {Time.frameCount}");
                GameObject listingContainer = new GameObject();
                listingContainer.name = purchasableObject.objectName + "Listing Container";
                RectTransform listingContainerRectTransform = listingContainer.AddComponent<RectTransform>();
                listingContainer.transform.SetParent(listingParentRectTransform);
                listingContainerRectTransform.localPosition = Vector3.zero;
                listingContainerRectTransform.localRotation = Quaternion.identity;
                listingContainerRectTransform.localScale = Vector3.one;

                PurchasableListing listing = Instantiate(listingPrefab);
                listing.transform.SetParent(listingContainer.transform);
                RectTransform listingRectTransform = listing.GetComponent<RectTransform>();
                listingRectTransform.localPosition = Vector3.zero;
                listingRectTransform.localRotation = Quaternion.identity;
                listingRectTransform.localScale = Vector3.one;
                listing.containerRectTransform = listingContainerRectTransform;
                listing.InitListing(purchasableObject);
                listing.purchaseButton.onClick.AddListener(() => 
                {
                    PurchaseListing(purchasableObject);
                    listing.purchaseButton.interactable = false;
                });
                listing.onPointerEnter.AddListener(() =>
                {
                    SetNewHighlightedListing(listing);
                });
                listing.onPointerExit.AddListener(() =>
                {
                    SetNewHighlightedListing(null);
                });

                float newViewerXPos = 10 * viewers.Count;
                PurchasableObjectViewer viewer = Instantiate(rtCameraPrefab, listingObjectsTransform);
                viewer.transform.localPosition = new Vector3(newViewerXPos, viewer.transform.position.y, viewer.transform.position.z);
                viewer.InitViewer(purchasableObject);
                viewers.Add(viewer);

                listings.Add(listing);
                instantiatedObjects.Add(purchasableObject);
            }
        }

        if (ComputerScreenManager.currentSequence == 3)
        {
            foreach (PurchasableListing listing in listings)
            {
                listing.purchaseButton.interactable = false;
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(listingParentRectTransform);
    }

    private void PurchaseListing(PurchasableObject _purchasableObject)
    {
        //Debug.Log($"Purchase {_purchasableObject.objectName} at {Time.frameCount}");
        _purchasableObject.isPurchased = true;
        purchaseAudioEvent.PlayOneShot();
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

    private void SetNewHighlightedListing(PurchasableListing listing)
    {
        if (currentlyHighlightedListing != null && currentlyHighlightedListing != listing)
        {
            currentlyHighlightedListing.purchaseButton.gameObject.transform.SetParent(currentlyHighlightedListing.containerRectTransform);
            GetViewerForListing(currentlyHighlightedListing.purchasableData).ViewObject(false);
        }

        if (listing == null)
        {
            currentlyHighlightedListing = null;
            return;
        }

        currentlyHighlightedListing = listing;
        GetViewerForListing(currentlyHighlightedListing.purchasableData).ViewObject(true);
        currentlyHighlightedListing.purchaseButton.gameObject.transform.SetParent(listingDisplayRectTransform);
        currentlyHighlightedListing.purchaseButton.gameObject.transform.SetAsLastSibling();
    }
}
