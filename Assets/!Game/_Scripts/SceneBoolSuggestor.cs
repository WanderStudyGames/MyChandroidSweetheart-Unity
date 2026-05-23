using QFSW.QC;
using System.Collections.Generic;
#if UNITY_EDITOR
#endif

public static partial class WorldData
{
    public class SceneBoolSuggestor : BasicCachedQcSuggestor<string>
    {
        protected override bool CanProvideSuggestions(SuggestionContext context, SuggestorOptions options)
        {
            return context.HasTag<SceneBoolTag>();
        }

        protected override IQcSuggestion ItemToSuggestion(string abilityName)
        {
            return new RawSuggestion(abilityName, true);
        }

        protected override IEnumerable<string> GetItems(SuggestionContext context, SuggestorOptions options)
        {
            return SceneBools.GetValues();
        }
    }
    public struct SceneBoolTag : IQcSuggestorTag
    {

    }
    public sealed class SceneBoolTagAttribute : SuggestorTagAttribute
    {
        private readonly IQcSuggestorTag[] _tags = { new SceneBoolTag() };

        public override IQcSuggestorTag[] GetSuggestorTags()
        {
            return _tags;
        }
    }
}
