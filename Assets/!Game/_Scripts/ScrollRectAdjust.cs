using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollRectAdjust : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private GameObject ElementPrefab;
    private RectTransform contentRect;
    private List<GameObject> elements;


    void Start()
    {
        contentRect = scrollRect.content;

        elements = new List<GameObject>();

        foreach (Transform child in contentRect.transform)
        {

        }



        foreach (GameObject go in elements)
        {
            EventTrigger eventTrigger = go.GetComponent<EventTrigger>();

            eventTrigger.AddListener(EventTriggerType.Select, OnSelect);
        }
        scrollRect.horizontalNormalizedPosition = 0;
        EventSystem.current.SetSelectedGameObject(elements[0]);
    }

    private void OnDestroy()
    {
        foreach (GameObject go in elements)
        {
            EventTrigger eventTrigger = go.GetComponentInChildren<EventTrigger>();
            eventTrigger.RemoveListener(EventTriggerType.Select, OnSelect);
        }
    }

    private void OnSelect(BaseEventData baseEventData)
    {
        RectTransform selectedRectTransform = baseEventData.selectedObject.GetComponent<RectTransform>();

        var width = scrollRect.GetComponent<RectTransform>().rect.width;
        var contentWidth = contentRect.rect.width;
        var overflow = (contentWidth - width) / 2f;

        var leftBorder = overflow - selectedRectTransform.offsetMin.x;
        var rightBorder = -(overflow + (selectedRectTransform.offsetMax.x - contentWidth));

        if (leftBorder > contentRect.anchoredPosition.x)
            contentRect.anchoredPosition = new Vector2(leftBorder, contentRect.anchoredPosition.y);
        else if (rightBorder < contentRect.anchoredPosition.x)
            contentRect.anchoredPosition = new Vector2(rightBorder, contentRect.anchoredPosition.y);
    }
}
