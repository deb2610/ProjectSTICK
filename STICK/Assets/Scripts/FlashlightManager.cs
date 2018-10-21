using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashlightManager : MonoBehaviour {

    public GameObject Flashlight;
    public int numSteps = 3;
    public GameObject canvasGameObject;
    private Image FlashLightImage;
    private const string FlashlightSpriteLocation = "Sprites/flashlight";

    private float HeadlightMax;
    private float FootlightMax;
    private float PointlightMax;

    private Light headlight;
    private Light footlight;
    private Light pointlight;

    private int currentStep;

	// Use this for initialization
	void Start () {
        currentStep = numSteps;

        // Grab the flashlight   
        GameObject flashLightSprite = canvasGameObject.transform.Find("Flash").gameObject;
        FlashLightImage = flashLightSprite.GetComponent(typeof(Image)) as Image;
        string flashlightLoc = FlashlightSpriteLocation + currentStep.ToString();
        Debug.Log(flashlightLoc);
        FlashLightImage.overrideSprite = Resources.Load<Sprite>(FlashlightSpriteLocation + currentStep.ToString());

        // Grab the light objects
        GameObject headlightGO = Flashlight.transform.Find("Headlight").gameObject;
        headlight = headlightGO.GetComponent(typeof(Light)) as Light;
        HeadlightMax = headlight.intensity;

        GameObject footlightGO = Flashlight.transform.Find("Footlight").gameObject;
        footlight = footlightGO.GetComponent(typeof(Light)) as Light;
        FootlightMax = footlight.intensity;

        GameObject pointlightGO = Flashlight.transform.Find("Point Light").gameObject;
        pointlight = pointlightGO.GetComponent(typeof(Light)) as Light;
        PointlightMax = pointlight.intensity;
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    /// <summary>
    /// Reduces the intesnsity of the flashlight by one step. If the flashlight is at 0, then it leaves the intensity at 0
    /// </summary>
    public void ReduceIntensity()
    {
        currentStep--;
        if (currentStep < 0)
        {
            currentStep = 0;
        }
        UpdateLighting();
    }

    /// <summary>
    /// Restores the flashlight to max intensity
    /// </summary>
    public void RestoreHealth()
    {
        RestoreHealth(numSteps);
    }

    /// <summary>
    /// Restores the intensity by the number of steps
    /// </summary>
    /// <param name="stepsToIncrease">the number of steps to restore intensity by (most likely 1)</param>
    public void RestoreHealth(int stepsToIncrease)
    {
        currentStep += stepsToIncrease;
        if (currentStep > numSteps)
        {
            currentStep = numSteps;
        }
        UpdateLighting();
    }

    private void UpdateLighting()
    {
        // Update headlight
        headlight.intensity = HeadlightMax / numSteps * currentStep;

        // Update footlight
        footlight.intensity = FootlightMax / numSteps * currentStep;

        // Update point light
        pointlight.intensity = PointlightMax / numSteps * currentStep;

        // Update UI
        FlashLightImage.overrideSprite = Resources.Load<Sprite>(FlashlightSpriteLocation + currentStep.ToString());
    }

    public bool IsBatteryFull()
    {
        return numSteps == currentStep;
    }
}
