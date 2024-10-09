using UnityEngine;
using UnityEngine.UI;
using TMPro; // For TextMeshPro

public class SliderStepInterval : MonoBehaviour
{
    public Slider slider; // Assign your slider here in the inspector
    public TMP_Text sliderValueText; // If using TextMeshPro
    // public Text sliderValueText; // Uncomment if using regular Text
    public float stepSize = 15.0f; // Set the desired step interval here (e.g., 5 for steps of 5 units)

    void Start()
    {
        // Set the initial slider value and update the displayed text
        UpdateSliderValue(slider.value);
        
        // Add listener to call UpdateSliderValue when the slider changes
        slider.onValueChanged.AddListener(UpdateSliderValue);
    }

    // This function will round the slider value to the nearest step
    public void UpdateSliderValue(float value)
    {
        // Snap the slider value to the nearest step
        float steppedValue = Mathf.Round(value / stepSize) * stepSize;

        // Update the slider value to the snapped value
        slider.value = steppedValue;

        // Update the displayed text to show the stepped value
        sliderValueText.text = steppedValue.ToString("F2"); // Adjust formatting as needed
    }
}