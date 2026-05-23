using System.Collections.Generic;


public class StringFunctions
{
    public static string[] EvaluateTextPages(string text, int charLimit)
    {
        List<string> pages = new();
        string[] substrings = text.Split(" ", System.StringSplitOptions.RemoveEmptyEntries);
        //List<string> asdf = new(substrings);
        //substrings = asdf.ToArray();
        string currentPage = "";
        foreach (string substr in substrings)
        {

            //split substrings (words) by line breaks, which are treated like page breaks in the UI
            string[] words = substr.Split("\n", System.StringSplitOptions.None);
            //words[0] will always be a word from the same page
            //words[1] and onward will occupy their own pages
            if (words[0].Contains("Chandroid")) { words[0] = $"<color=#FF80FF>{words[0]}</color>"; }
            if (currentPage.RemoveTags().Length + words[0].RemoveTags().Length + 1 < charLimit)
            {
                //if word begins the page, no space is added before the word
                currentPage += (pages.Count == 0 && currentPage == "") ? words[0] : " " + words[0];
            }
            else
            {
                //if not enough space left for word on page, page break
                pages.Add(currentPage);
                currentPage = " " + words[0];
            }
            //if words[] has more than one item, page break for every subsequent words[] item
            if (words.Length > 1)
            {
                for (int i = 1; i < words.Length; i++)
                {
                    pages.Add(currentPage);
                    currentPage = words[i];
                }
            }
        }
        //add final page to list of pages, since final page will contain less than the character limit
        pages.Add(currentPage);

        return pages.ToArray();
    }
    public static string ProcessGameText(string input)
    {
        return input.Replace("<playername>", $"<color=#96D3FF>{SetPlayerName.PlayerName}</color>")
            .Replace("<companionname>", $"<color=#FF80FF>{CompanionData.CompanionName}</color>")
            .Replace("dragon", "<color=#56FF56>dragon</color>");
    }

}
