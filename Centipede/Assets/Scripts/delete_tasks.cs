using UnityEngine;

public class DeleteButtonHandler : MonoBehaviour
{
    public ButtonListHandler buttonListHandler; // Reference to ButtonListHandler script

    public void OnDeleteButtonClicked()
    {
        // Check if any button is highlighted
        if (buttonListHandler.lastHighlightedButton == null)
        {
            Debug.LogWarning("No button is highlighted for deletion!");
            return;
        }

        // Get the button text
        string buttonText = buttonListHandler.lastHighlightedButton.GetComponentInChildren<TMPro.TMP_Text>().text;

        // Remove the task from persistent storage
        buttonListHandler.RemoveTask(buttonText);

        // Destroy the highlighted button
        Destroy(buttonListHandler.lastHighlightedButton);

        // Clear the reference to the highlighted button
        buttonListHandler.lastHighlightedButton = null;
    }
}
