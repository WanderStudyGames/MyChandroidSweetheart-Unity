using UnityEngine;

public interface IScannerSelectable
{
    public bool Select();
    public void Deselect();
    public void Click();
    public void UnClick();
    public Sprite Icon { get; }
    public bool Enabled { get; }
}
