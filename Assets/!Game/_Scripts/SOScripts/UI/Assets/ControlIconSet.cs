using UnityEngine;

public abstract class ControlIconSet : ScriptableObject
{
    public abstract bool TryGetSprite(string input, out Sprite sprite);

}
