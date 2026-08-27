using UnityEngine;

public class PurchasableObjectViewer : MonoBehaviour
{
    [SerializeField] public PurchasableObject purchasableData;
    [SerializeField] private Camera rtCamera;
    [SerializeField] private Transform objectContainer;
    [SerializeField] private Animator anim;

    public void InitViewer(PurchasableObject purchasable)
    {
        purchasableData = purchasable;

        this.name = purchasableData.objectName + " Viewer";
        rtCamera.targetTexture = purchasable.objectRT;
        GameObject model = Instantiate(purchasable.objectModel, objectContainer, false);
        model.transform.localPosition = Vector3.zero;
        model.transform.localScale = model.transform.localScale * purchasable.viewerScale;
        model.transform.localRotation = Quaternion.Euler(purchasable.viewerStartRotation);

        // Need to invoke so we don't turn off the camera on the first frame before it renders
        Invoke("DisableCamera", .5f);
    }

    public void ViewObject(bool view)
    {
        //Debug.Log($"View Object for {gameObject.name} is {view} at {Time.frameCount}");
        anim.SetBool("viewObject", view);

        if (view)
        {
            rtCamera.enabled = true;
        }
        else
        {
            Invoke("DisableCamera", .5f);
        }
    }

    private void DisableCamera()
    {
        rtCamera.enabled = false;
    }
}
