using UnityEngine;

public class DeleteButtonHandler : MonoBehaviour
{
    private GameObject highlightedButton; // Reference to the highlighted button

    public void SetHighlightedButton(GameObject button)
    {
        highlightedButton = button; // Set the button to be deleted
    }

    public void OnDeleteButtonClicked()
    {
        if (highlightedButton != null)
        {
            // Destroy the highlighted button
            Destroy(highlightedButton);

            // Clear the reference after deletion
            highlightedButton = null;

            Debug.Log("Task deleted successfully.");
        }
        else
        {
            Debug.LogWarning("No button is highlighted for deletion.");
        }
    }
}
