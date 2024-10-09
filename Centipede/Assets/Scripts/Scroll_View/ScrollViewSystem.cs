using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewSystem : MonoBehaviour
{
    private ScrollRect _scrollRect;

    [SerializeField] private ScrollButton _leftButton;
    [SerializeField] private ScrollButton _rightButton;
    [SerializeField] private ScrollButton _bottoButton;
    [SerializeField] private ScrollButton _topButton;


    [SerializeField] private float scrollSpeed = 0.01f;

    void Start()
    {
        _scrollRect = GetComponent<ScrollRect>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_leftButton != null)
        {
            if(_leftButton.isDown)
            {
                ScrollLeft();
            }
        }
        if (_rightButton != null)
        {
            if(_rightButton.isDown)
            {
                ScrollRight();
            }
        }
        if (_bottoButton != null)
        {
            if(_bottoButton.isDown)
            {
                ScrollBotttom();
            }
        }
        if (_topButton != null)
        {
            if(_topButton.isDown)
            {
                ScrollTop();
            }
        }
    }

    private void ScrollLeft()
    {
        if(_scrollRect != null)
        {
            if(_scrollRect.horizontalNormalizedPosition >= 0f)
            {
                _scrollRect.horizontalNormalizedPosition -= scrollSpeed;
            }
        }
    }

    private void ScrollRight()
    {
        if(_scrollRect != null)
        {
            if(_scrollRect.horizontalNormalizedPosition <= 1f)
            {
                _scrollRect.horizontalNormalizedPosition += scrollSpeed;
            }
        }
    }

    private void ScrollTop()
    {
        if(_scrollRect != null)
        {
            if(_scrollRect.verticalNormalizedPosition <= 1f)
            {
                _scrollRect.horizontalNormalizedPosition += scrollSpeed;
            }
        }
    }

    private void ScrollBotttom()
    {
        if(_scrollRect != null)
        {
            if(_scrollRect.horizontalNormalizedPosition >= 0f)
            {
                _scrollRect.verticalNormalizedPosition -= scrollSpeed;
            }
        }
    }
}
