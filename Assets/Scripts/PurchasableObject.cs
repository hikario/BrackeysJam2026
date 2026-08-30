using System;
using UnityEngine;

[Serializable]
public class PurchasableObject
{
    public string objectName;
    public int objectTier = 0;
    public bool isPurchased;
    public bool isPlaced;
    public string objectDescription;
    public float objectPrice;
    public GameObject objectModel;
    public RenderTexture objectRT;
    public Material objectRTMaterial;
    public float viewerScale = 1;
    public Vector3 viewerStartPosition = Vector3.zero;
    public Vector3 viewerStartRotation = Vector3.zero;
}