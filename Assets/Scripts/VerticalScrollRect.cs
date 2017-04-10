using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VerticalScrollRect : MonoBehaviour {

    public RectTransform _unlockArea;
    public RectTransform _testButtonRectTransform;

    private ScrollRect _scrollRect;
    private RectTransform _thisRectTransform;
    private RectTransform _contentRectTransform;

    private float _topPadding = 20f;
    private float _lowerPadding = 269f;

    void Awake()
    {
        _scrollRect = GetComponent<ScrollRect>();
        if (!_scrollRect) { Debug.LogError("No Scroll rect component", this); }
        _thisRectTransform = GetComponent<RectTransform>();
        if (!_thisRectTransform) { Debug.LogError("Rect transform not found", this); }
        _contentRectTransform = _scrollRect.content;

    }

    public void ResetPosition()
    {
        _scrollRect.verticalNormalizedPosition = 1f;
    }

    public void GoToBottom()
    {
        _scrollRect.verticalNormalizedPosition = 0;
    }

    public void ScrollToShow(RectTransform buttonRectTransform)
    {
        Vector3 buttonToTopOfScrollRect = _thisRectTransform.localPosition - buttonRectTransform.localPosition;//vector del botón al principio del grid
        float contentHeightDifference = (_contentRectTransform.rect.height - _thisRectTransform.rect.height);//si no es positivo no hay scroll

        float nonNormalizedButtonPosition = (_contentRectTransform.rect.height - buttonToTopOfScrollRect.y);//Si el final de la lista es 0 y el principio la altura, esto realmente define la posición sin normalizar
        float currentScrollRectPosition = _scrollRect.normalizedPosition.y * contentHeightDifference;//si este num es mayor que la posición sin normalizar del botón, el botón está por debajo (la posición del scroll rect está definida como la longitud de lo que queda por debajo)

        if (currentScrollRectPosition > nonNormalizedButtonPosition - buttonRectTransform.rect.height / 2 - _lowerPadding)//por debajo
        {
            float newScrollRectPosition = nonNormalizedButtonPosition - buttonRectTransform.rect.height / 2 - _lowerPadding;
            float newNormalizedScrollRectPosition = newScrollRectPosition / _thisRectTransform.rect.height;
            _scrollRect.normalizedPosition = new Vector2(0, newNormalizedScrollRectPosition);
        }
        else if (contentHeightDifference - buttonToTopOfScrollRect.y + buttonRectTransform.rect.height / 2 + _topPadding > currentScrollRectPosition)//encima
        {
            float newScrollRectPosition = contentHeightDifference - buttonToTopOfScrollRect.y + buttonRectTransform.rect.height / 2 + _topPadding;
            float newNormalizedScrollRectPosition = newScrollRectPosition / contentHeightDifference;
            _scrollRect.normalizedPosition = new Vector2(0, newNormalizedScrollRectPosition);
        }
    }
}
