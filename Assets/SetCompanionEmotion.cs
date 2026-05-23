using UnityEngine;

public class SetCompanionEmotion : MonoBehaviour
{
    private enum Emotions
    {
        Bashful
    }
    public void SetBool(bool value)
    {
        CompanionManager.SetAnimationBool("Bashful", value);
    }
    public void SetBoolTrue(string name) { CompanionManager.SetAnimationBool(name, true); }
    public void SetBoolFalse(string name) { CompanionManager.SetAnimationBool(name, false); }
    public void SetTrigger(string name) { CompanionManager.SetAnimationTrigger(name); }
}
