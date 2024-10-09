using UnityEngine;
using UnityEngine.UI;
using TMPro; // If using TextMeshPro

public class SliderValueDisplay : MonoBehaviour
{
    public Slider slider; // Assign your slider here in the inspector
    public TMP_Text sliderValueText; // If using TextMeshPro
    // public Text sliderValueText; // Uncomment this if using regular Text

    void Start()
    {
        // Set the initial value of the slider text
        UpdateSliderValue(slider.value);
        
        // Add a listener to the slider to handle value changes
        slider.onValueChanged.AddListener(UpdateSliderValue);
    }

    // This function will be called whenever the slider value changes
    public void UpdateSliderValue(float value)
    {
        // Update the text with the current slider value
        sliderValueText.text = value.ToString("F2"); // Adjust the format as needed (e.g., "F0" for no decimals)
    }
}