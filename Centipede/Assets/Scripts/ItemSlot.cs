using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler {

	public void OnDrop(PointerEventData eventData){
		Debug.Log("OnDrop");
		if (eventData.pointerDrag != null && eventData.pointerEnter.CompareTag("Slot")){
		
            // Get the RectTransform of the dragged object
            RectTransform draggedRectTransform = eventData.pointerDrag.GetComponent<RectTransform>();
            
            // Set the dragged object's parent to the current object (the drop target)
            draggedRectTransform.SetParent(GetComponent<RectTransform>());

            // Reset the position of the dragged object relative to its new parent (the drop target)
            draggedRectTransform.anchoredPosition = Vector2.zero;

            // Optionally, you can adjust this to align with the center or other part of the box
            // draggedRectTransform.localPosition = Vector3.zero; // This centers the object within the parent
        }
	}
}

