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
    ColorAdjustments satComponent;
    private bool blurring;
    private bool clearing;
    public bool desat;
    public bool saturate;

    void Awake()
    {
        DepthOfField tmp;
        ColorAdjustments sat;
        blurring = false;
        clearing = false;
        desat = false;
        saturate = false;
        if (vol.profile.TryGet<DepthOfField>(out tmp))
        {
            dofComponent = tmp;
        }

        if (vol.profile.TryGet<ColorAdjustments>(out sat))
        {
            satComponent = sat;
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

        if (desat == true)
        {
            if (satComponent.saturation.value > -100)
            {
                satComponent.saturation.value += satComponent.saturation.value - (blurSpeed * Time.deltaTime);
            }
            else
            {
                desat = false;
            }
        }

        if (saturate == true)
        {
            if (satComponent.saturation.value <= 0)
            {
                satComponent.saturation.value = 0;
            }
            else
            {
                saturate = false;
                satComponent.saturation.value = 0;
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

    public void Desaturate()
    {
        saturate = false;
        desat = true;
    }

    public void Saturate()
    {
        desat = false;
        saturate = true;
    }
}
