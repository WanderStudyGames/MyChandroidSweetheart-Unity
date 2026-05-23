using UnityEngine;
using UnityEngine.UI;

public class ReadGameplaySettings : MonoBehaviour
{
    [SerializeField] private PlayerLookProfile _playerLookProfile;
    [SerializeField] private CompanionData _companionData;

    [SerializeField] private Slider _lookSensitivity;
    [SerializeField] private Slider _virtualMouseSpeed;
    [SerializeField] private Toggle _invertMouse;
    [SerializeField] private Toggle _bustPhysics;
    [SerializeField] private Toggle _kioskMode;

    private void OnEnable()
    {
        _lookSensitivity.value = _playerLookProfile.LookSpeed;
        _lookSensitivity.onValueChanged.Invoke(_lookSensitivity.value);
        _invertMouse.isOn = _playerLookProfile.InvertLook;

        _bustPhysics.isOn = _companionData.BustPhysics;
        _virtualMouseSpeed.value = UIManager.VirtualMouseSpeed;
        _virtualMouseSpeed.onValueChanged.Invoke(_virtualMouseSpeed.value);
        _kioskMode.isOn = SaveSystem.IsKiosk;
    }
}
