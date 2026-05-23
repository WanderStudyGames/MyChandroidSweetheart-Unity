using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TabGroup : MonoBehaviour
{
    // These must be 1 to 1, same order in hierarchy
    [HideInInspector]
    public List<TabButtonComponent> tabButtons = new();
    public List<GameObject> tabPages = new();

    [SerializeField] private UnityEvent _onPageChange;

    //In case I need to sort the lists by GetSiblingIndex
    //objListOrder.Sort((x, y) => x.OrderDate.CompareTo(y.OrderDate));

    public Color tabIdleColor;
    public Color tabHoverColor;
    public Color tabSelectedColor;
    private TabButtonComponent selectedTab;

    public IEnumerator Start()
    {
        yield return null;
        // Select first tab
        TabButtonComponent tb = null;
        foreach (TabButtonComponent tabButton in tabButtons)
        {
            if (tabButton.transform.GetSiblingIndex() == 0)
                tb = tabButton;
            else { tabButton.Deselect(); }
        }
        if (tb != null)
            OnTabSelected(tb);
    }

    public void Subscribe(TabButtonComponent tabButton)
    {
        tabButtons.Add(tabButton);
        // Sort by order in hierarchy
        tabButtons.Sort((x, y) => x.transform.GetSiblingIndex().CompareTo(y.transform.GetSiblingIndex()));
    }

    public void OnTabEnter(TabButtonComponent tabButton)
    {
        ResetTabs();
        if ((selectedTab == null) || (tabButton != selectedTab))
            tabButton.background.color = tabHoverColor;
    }

    public void OnTabExit(TabButtonComponent tabButton)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButtonComponent tabButton)
    {
        if (selectedTab != null)
        {
            selectedTab.Deselect();
        }

        selectedTab = tabButton;

        selectedTab.Select();

        ResetTabs();
        tabButton.background.color = tabSelectedColor;
        int index = tabButton.transform.GetSiblingIndex();
        SetPageActive(index);
    }

    private void SetPageActive(int index)
    {
        GameObject selection = null;
        for (int i = 0; i < tabPages.Count; i++)
        {
            if (i == index && tabPages[i] != null)
            {
                selection = tabPages[i];
            }
            else if (tabPages[i] != null)
            {
                tabPages[i].SetActive(false);
            }
        }
        if (selection != null)
            selection.SetActive(true);
        _onPageChange?.Invoke();
    }

    public void GoToTab(int index)
    {
        SetPageActive(index);
        foreach (TabButtonComponent tabButton in tabButtons)
        {
            if (tabButton.transform.GetSiblingIndex() == index) { OnTabSelected(tabButton); break; }
        }
    }
    public void GoToTab(GameObject tabPage)
    {
        int index = tabPages.IndexOf(tabPage);
        SetPageActive(index);
        foreach (TabButtonComponent tabButton in tabButtons)
        {
            if (tabButton.transform.GetSiblingIndex() == index) { OnTabSelected(tabButton); break; }
        }
    }

    public void ResetTabs()
    {
        foreach (TabButtonComponent tabButton in tabButtons)
        {
            if ((selectedTab != null) && (tabButton == selectedTab))
                continue;
            tabButton.background.color = tabIdleColor;
        }
    }

    public void NextTab()
    {
        int currentIndex = selectedTab.transform.GetSiblingIndex();
        int nextIndex = currentIndex < tabButtons.Count - 1 ? currentIndex + 1 : tabButtons.Count - 1;
        OnTabSelected(tabButtons[nextIndex]);
    }

    public void PreviousTab()
    {
        int currentIndex = selectedTab.transform.GetSiblingIndex();
        int previousIndex = currentIndex > 0 ? currentIndex - 1 : 0;
        OnTabSelected(tabButtons[previousIndex]);
    }
}
