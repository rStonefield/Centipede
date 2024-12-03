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

        // Destroy the highlighted button
        Destroy(buttonListHandler.lastHighlightedButton);

        // Remove the task from the saved list
        buttonListHandler.UpdateTaskListAfterDeletion();

        // Clear the reference to the highlighted button
        buttonListHandler.lastHighlightedButton = null;
    }
}
