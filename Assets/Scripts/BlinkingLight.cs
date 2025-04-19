using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// Makes a Light component fade its intensity up and down smoothly.
/// </summary>
[RequireComponent(typeof(Light))]
public class BlinkingLight : MonoBehaviour
{
    public float fadeSpeed = 10.0f;


    
    private Light lightComponent;
    private float maxIntensity = 8;
    private float minIntensity = 0;

    // State tracking
    private bool isFadingOut = true; // Start by fading out from max intensity

    void Start()
    {
        lightComponent = GetComponent<Light>();
        maxIntensity = lightComponent.intensity;
    }

    void Update()
    {
        if (lightComponent == null) return;

        float intensityChange = Time.deltaTime * math.abs(fadeSpeed); // abs to deal with potential negative user input
        HandleIntesityChange(intensityChange);

    }

    void HandleIntesityChange(float intensityChange)
    {
        if (isFadingOut)
        {
            lightComponent.intensity -= intensityChange;
            if (lightComponent.intensity <= minIntensity)
            {
                lightComponent.intensity = minIntensity;
                isFadingOut = false;
            }
        }
        else
        {
            lightComponent.intensity += intensityChange;
            if (lightComponent.intensity >= maxIntensity)
            {
                lightComponent.intensity = maxIntensity;
                isFadingOut = true;
            }
        }
    }
}

