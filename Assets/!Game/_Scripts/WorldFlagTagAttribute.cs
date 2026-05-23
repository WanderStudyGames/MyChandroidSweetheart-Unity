using QFSW.QC;
#if UNITY_EDITOR
#endif

public static partial class WorldData
{
    public sealed class WorldFlagTagAttribute : SuggestorTagAttribute
    {
        private readonly IQcSuggestorTag[] _tags = { new WorldFlagTag() };

        public override IQcSuggestorTag[] GetSuggestorTags()
        {
            return _tags;
        }
    }
}
