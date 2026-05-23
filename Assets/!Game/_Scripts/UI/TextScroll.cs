using System;
using System.Collections;
using UnityEngine;

public class TextScroll
{

    //use lerp rounded instead of just adding a letter each time
    public static IEnumerator Scroll(string finalText, float charactersPerSecond, Action<string> characterAction, Action completeAction = null)
    {
        int i = 0;
        float timeSinceLastCharacter = 0;
        while (i < finalText.Length)
        {
            timeSinceLastCharacter += Time.unscaledDeltaTime;
            var charactersSinceLastCharacter = timeSinceLastCharacter / charactersPerSecond;
            while (i < finalText.Length && charactersSinceLastCharacter >= 1)
            {
                while (i < finalText.Length && finalText[i] == '<')
                {
                    i++;
                    while (i < finalText.Length && finalText[i] != '>') { i++; }
                }

                if (i < finalText.Length) { i++; }

                charactersSinceLastCharacter--;
                timeSinceLastCharacter = 0;
                characterAction?.Invoke(finalText.Substring(0, i));
            }
            yield return null;
        }
        completeAction?.Invoke();
    }
}
