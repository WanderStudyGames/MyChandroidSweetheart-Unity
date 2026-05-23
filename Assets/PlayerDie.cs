using UnityEngine;

public class PlayerDie : MonoBehaviour
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private SFX _sfx;
    public void Die()
    {
        PlayerManager.Die(_sprite, _sfx);
    }
}
