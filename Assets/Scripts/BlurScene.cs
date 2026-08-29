using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BlurScene : MonoBehaviour
{
    public Volume vol;
    private float blurFocalLength = 300;
    private float clearFolarLength = 30;
    public float blurSpeed;
    DepthOfField dofComponent;
    private bool blurring;
    private bool clearing;

    void Awake()
    {
        DepthOfField tmp;
        blurring = false;
        clearing = false;
        if (vol.profile.TryGet<DepthOfField>(out tmp))
        {
            dofComponent = tmp;
        }
    }

    void Update()
    {
        if (blurring == true)
        {
            if (dofComponent.focalLength.value < 300)
            {
                dofComponent.focalLength.value += dofComponent.focalLength.value * (1 * blurSpeed) * Time.deltaTime;
            }
            else
            {
                blurring = false;
            }
        }

        if (clearing == true)
        {
            if (dofComponent.focalLength.value > 30)
            {
                dofComponent.focalLength.value += dofComponent.focalLength.value * (-1 * blurSpeed) * Time.deltaTime;
            }
            else
            {
                clearing = false;
                dofComponent.focalLength.value = 30;
            }
        }
    }

    public void EnableBlur()
    {
        clearing = false;
        blurring = true;
    }

    public void DisableBlur()
    {
        blurring = false;
        clearing = true;
    }
}
