using UnityEngine;

[CreateAssetMenu(fileName = "Player Swim Profile", menuName = "ScriptableObjects/Player/Player Swim Profile")]
public class PlayerSwimProfile : PlayerComponentProfile
{
    [SerializeField] private string waterLayer;
    public string WaterLayer => waterLayer;
    public Sprite DrowningSprite;
}
