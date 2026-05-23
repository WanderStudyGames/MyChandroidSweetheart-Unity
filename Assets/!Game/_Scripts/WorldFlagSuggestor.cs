using System.Collections.Generic;
using QFSW.QC;
#if UNITY_EDITOR
#endif

public static partial class WorldData
{
    public class WorldFlagSuggestor : BasicCachedQcSuggestor<string>
    {
        protected override bool CanProvideSuggestions(SuggestionContext context, SuggestorOptions options)
        {
            return context.HasTag<WorldFlagTag>();
        }

        protected override IQcSuggestion ItemToSuggestion(string abilityName)
        {
            return new RawSuggestion(abilityName, true);
        }

        protected override IEnumerable<string> GetItems(SuggestionContext context, SuggestorOptions options)
        {
            return WorldFlags.GetValues();
        }
    }
}
