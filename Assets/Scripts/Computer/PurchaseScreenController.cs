using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseScreenController : MonoBehaviour
{
    [SerializeField] public List<PurchasableObject> purchasableObjects = new List<PurchasableObject>();
    [SerializeField] public PurchasableListing listingPrefab;
    [SerializeField] public Camera rtCameraPrefab;
    private List<Camera> rtCameras = new List<Camera>();
    [SerializeField] public RectTransform listingParentRectTransform;
    [SerializeField] public Transform listingObjectsTransform;
    [SerializeField] private int currentObjectTier = 0;


    private void OnEnable()
    {
        foreach (PurchasableObject purchasableObject in purchasableObjects)
        {
            if (currentObjectTier >= purchasableObject.objectTier)
            {
                Debug.Log($"Instantiate listing for {purchasableObject.objectName} at {Time.frameCount}");
                PurchasableListing listing = Instantiate(listingPrefab, listingParentRectTransform);
                listing.InitListing(purchasableObject);
                listing.purchaseButton.onClick.AddListener(() => 
                {
                    PurchaseListing(purchasableObject);
                    listing.purchaseButton.interactable = false;
                });

                Camera rtCam = Instantiate(rtCameraPrefab, listingObjectsTransform);
                GameObject model = Instantiate(purchasableObject.objectModel, rtCam.GetComponentInChildren<Transform>(), false);
                model.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(listingParentRectTransform);
    }

    private void PurchaseListing(PurchasableObject _purchasableObject)
    {
        _purchasableObject.isPurchased = true;
    }
}
