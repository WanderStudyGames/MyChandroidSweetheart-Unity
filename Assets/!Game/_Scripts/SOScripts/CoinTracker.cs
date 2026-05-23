using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Editor/Coin Tracker", fileName = "Coin Tracker")]
public class CoinTracker : ScriptableObject
{
    public static CoinTracker Instance;
    private void OnEnable()
    {
        Instance = this;
    }
    [System.Serializable]
    public struct CoinID
    {
        public int ID;
        public bool Taken;
    }
    [SerializeField]
    private CoinID[] _coins;
    public CoinID[] Coins => _coins;
    private void Reset()
    {
        _coins = new CoinID[1001];
        for (int i = 0; i < _coins.Length; i++)
        {
            _coins[i].ID = i;
        }
    }
    public void TakenUpTo(int index)
    {
        for (int i = 0; i < Mathf.Min(index, _coins.Length); i++)
        {
            _coins[i].Taken = true;
        }
    }
}
