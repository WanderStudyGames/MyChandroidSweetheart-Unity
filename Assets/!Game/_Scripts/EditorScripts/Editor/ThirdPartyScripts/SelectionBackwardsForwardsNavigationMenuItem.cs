using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FMPUtils.Editor
{
    [InitializeOnLoad]
    public static class SelectionBackwardsForwardsNavigationMenuItem
    {
        public static event System.Action historyOrSelectionChanged;

        private static readonly int historyCount = 50;
        private static int currentHistoryIndex = -1;
        private static bool ignoreSelectionChangeProcessing;
        private static List<Object[]> selectionHistory = new List<Object[]>(historyCount);

        public static bool HasNextHistoryEntry => currentHistoryIndex < selectionHistory.Count - 1;
        public static bool HasPreviousHistoryEntry => currentHistoryIndex > 0;

        public static string NavigateForwardsHotkeyText => "(Ctrl + Alt + Y)";
        public static string NavigateBackwardsHotkeyText => "(Ctrl + Alt + Z)";

        // Since the static class is constructed twice, make sure that there is only 1 subscription: 
        // https://answers.unity.com/questions/1339703/initializeonload-for-editor-load-only.html
        static SelectionBackwardsForwardsNavigationMenuItem()
        {
            Selection.selectionChanged -= HandleSelectionChanged;
            Selection.selectionChanged += HandleSelectionChanged;
        }

        private static void HandleSelectionChanged()
        {
            // Don't record a selection if we apply a selection from the history (a non-direct selection)
            if (!ignoreSelectionChangeProcessing)
                RecordSelectionHistory();

            historyOrSelectionChanged?.Invoke();
        }

        private static void RecordSelectionHistory()
        {
            Object[] currentSelection = Selection.objects;
            if (currentSelection.Length > 0)
            {
                // Only record the selection history if there is a selection (that is not identical to the last one)
                if (currentHistoryIndex < 0 || !CompareSelections(currentSelection, selectionHistory[currentHistoryIndex]))
                {
                    // Make sure to invalidate the selection history after the current index when the user has made a new selection choice
                    selectionHistory.RemoveRange(currentHistoryIndex + 1, selectionHistory.Count - currentHistoryIndex - 1);
                    if (selectionHistory.Count >= historyCount)
                        selectionHistory.RemoveAt(0);
                    selectionHistory.Add(currentSelection);
                    currentHistoryIndex = selectionHistory.Count - 1;
                }
            }
        }

        private static bool CompareSelections(Object[] selection1, Object[] selection2)
        {
            if (selection1.Length != selection2.Length)
            {
                return false;
            }
            for (int i = 0; i < selection1.Length; i++)
            {
                if (selection1[i] != selection2[i])
                {
                    return false;
                }
            }
            return true;
        }

        // Ctrl + Alt + Z
        [MenuItem("Edit/Selection - Navigate Back %&z")]
        public static void NavigateSelectionBackwards()
        {
            if (HasPreviousHistoryEntry)
            {
                ignoreSelectionChangeProcessing = true;
                currentHistoryIndex--;
                Selection.objects = selectionHistory[currentHistoryIndex];
                historyOrSelectionChanged?.Invoke();
                ignoreSelectionChangeProcessing = false;
            }
            
        }

        // Ctrl + Alt + Y
        [MenuItem("Edit/Selection - Navigate Forward %&y")]
        public static void NavigateSelectionForwards()
        {
            if (HasNextHistoryEntry)
            {
                ignoreSelectionChangeProcessing = true;
                currentHistoryIndex++;
                Selection.objects = selectionHistory[currentHistoryIndex];
                historyOrSelectionChanged?.Invoke();
                ignoreSelectionChangeProcessing = false;
            }
        }
    }
}
