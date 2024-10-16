using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler {

    public void OnDrop(PointerEventData eventData){
        Debug.Log("OnDrop");

        // Ensure both the dragged object and the drop target exist
        if (eventData.pointerDrag != null && eventData.pointerEnter != null) {

            // Get the tag of the dragged object
            string draggedObjectTag = eventData.pointerDrag.tag;

            // Get the tag of the drop target (this object)
            string dropTargetTag = eventData.pointerEnter.tag;

            // Check if the tags match
            if (draggedObjectTag == dropTargetTag | dropTargetTag == "All_Inv") {
                Debug.Log("Tags match! Item dropped.");

                // Get the RectTransform of the dragged object
                RectTransform draggedRectTransform = eventData.pointerDrag.GetComponent<RectTransform>();

                // Set the dragged object's parent to the current object (the drop target)
                draggedRectTransform.SetParent(eventData.pointerEnter.transform);

                // Reset the position of the dragged object relative to its new parent (the drop target)
                draggedRectTransform.anchoredPosition = Vector2.zero;

                // Optionally, you can align the object to the center
                // draggedRectTransform.localPosition = Vector3.zero; // Centers the object
            }
            
            else {
                Debug.Log("Tags do not match. Cannot drop here.");
            }
        }
    }
}

