using UnityEngine;
using UnityEngine.EventSystems;

public class PlaceableArea : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    string TargetObjectName;
    [SerializeField]
    Material UnplacedMaterial;
    PurchasableObject TargetObject;
    MeshFilter ModifiableMeshFilter;
    MeshRenderer ModifiableMeshRenderer;
    bool Highlighted;

    void Start()
    {
        foreach(PurchasableObject pe in ComputerScreenManager.instance.purchasableObjectDefinitionReference.purchasableObjects)
        {
            if(pe.objectName == TargetObjectName)
            {
                TargetObject = pe;
                break;
            }
        }

        ModifiableMeshFilter = GetComponentInChildren<MeshFilter>();
        ModifiableMeshFilter.mesh = null;
        ModifiableMeshRenderer = GetComponentInChildren<MeshRenderer>();
        ModifiableMeshRenderer.material = UnplacedMaterial;


        if(TargetObject == null)
        {
            Debug.Log("Bad Object Name");
        }

        Highlighted = false;
    }

    // Pointer Enter (MouseOver)
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse On");
        Highlighted = true;
        if(TargetObject.isPurchased && !TargetObject.isPlaced)
        {
            ModifiableMeshFilter.mesh = TargetObject.objectModel.GetComponent<MeshFilter>().sharedMesh;
        }
    }

    // Pointer Exit (Mouse no longer Over)
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse Off");
        Highlighted = false;
        if(!TargetObject.isPlaced)
        {
            ModifiableMeshFilter.mesh = null;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(Highlighted && TargetObject.isPurchased)
        {
            ComputerScreenManager.instance.purchasableObjectDefinitionReference.MarkObjectAsPlaced(TargetObject.objectName);
            ModifiableMeshRenderer.material = TargetObject.objectRTMaterial;
        }
    }
}
