using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class drag_drop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        canvasGroup.alpha = 0.8f;
        canvasGroup.blocksRaycasts = false;
        originalPosition = GetComponent<RectTransform>().anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // Ensure the drop target is valid
        if (eventData.pointerEnter != null)
        {
            // Get the tag of the dragged object
            string draggedObjectTag = this.gameObject.tag;

            // Get the tag of the drop target (eventData.pointerEnter)
            string dropTargetTag = eventData.pointerEnter.tag;

            // Check if the tags match
            if (draggedObjectTag == dropTargetTag)
            {
                Debug.Log("Tags match! Item dropped.");

                // Get the RectTransform of the dragged object
                RectTransform draggedRectTransform = GetComponent<RectTransform>();

                // Set the dragged object's parent to the drop target
                draggedRectTransform.SetParent(eventData.pointerEnter.transform);

                // Reset the position of the dragged object relative to its new parent (the drop target)
                draggedRectTransform.anchoredPosition = Vector2.zero;

            }
            else
            {
                Debug.Log("Tags do not match. Returning to original position.");
                GetComponent<RectTransform>().anchoredPosition = originalPosition; // Return to original position
            }
        }
        else
        {
            // No valid drop target, return the item to its original position
            GetComponent<RectTransform>().anchoredPosition = originalPosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }
}