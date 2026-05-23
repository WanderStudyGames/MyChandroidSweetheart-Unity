using QFSW.QC;
using UnityEngine;

[CreateAssetMenu(fileName = "Wallets", menuName = "ScriptableObjects/Wallets")]
public class Wallets : ScriptableObject
{
    private static Wallets instance;
    private void OnEnable()
    {
        instance = this;
    }
    [SerializeField] private Wallet _jolts;
    public static Wallet Jolts => instance?._jolts;
    [Command("jolts-add")]
    private static void JoltsAdd(int jolts) { Jolts.Add(jolts); }
    [Command("jolts-set")]
    private static void JoltsSet(int jolts) { Jolts.Set(jolts); }
    [Command("jolts-clear")]
    private static void JoltsClear() { Jolts.Clear(); }
    [SerializeField] private Wallet _fabrics;
    public static Wallet Fabrics => instance?._fabrics;
    [Command("fabrics-add")]
    private static void FabricsAdd(int fabrics) { Fabrics.Add(fabrics); }
    [Command("fabrics-set")]
    private static void FabricsSet(int fabrics) { Fabrics.Set(fabrics); }
    [Command("fabrics-clear")]
    private static void FabricsClear() { Fabrics.Clear(); }
}