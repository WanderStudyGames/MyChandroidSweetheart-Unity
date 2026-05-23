using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class SignalInput : MonoBehaviour, ISignalReceivable
{
    [FormerlySerializedAs("_tallyOnOffs")]
    [SerializeField] private bool _tallyOnOff = true;
    [SerializeField] private bool _reverseOnOff;
    [SerializeField] private bool _sendEventsWhenDisabled = false;
    [SerializeField] private UnityEvent _on;
    [SerializeField] private UnityEvent _off;
    [SerializeField] private UnityEvent _singleClick;
    [SerializeField] private UnityEvent<int> _int;
    [SerializeField] private DeviceComponent _device;
    public event Action UpdateLineRenderer;

    private void OnEnable()
    {
        UpdateLineRenderer?.Invoke();
    }

    [field: SerializeField] public RewireTarget RewireTarget { get; private set; }


    private int _onners;
    public void On()
    {
        if (!isActiveAndEnabled && !_sendEventsWhenDisabled) { _onners += 1; return; }
        if (_onners == 0)
        {
            if (_reverseOnOff)
            {
                _off.Invoke();
                _device?.Off();
            }
            else
            {
                _on.Invoke();
                _device?.On();
            }
        }
        _onners += 1;
        if (!_tallyOnOff)
        {
            _onners = (int)Mathf.Clamp01(_onners);
        }
    }
    public void Off()
    {
        _onners -= 1;
        if (!_tallyOnOff)
        {
            _onners = (int)Mathf.Clamp01(_onners);
        }
        if (!isActiveAndEnabled && !_sendEventsWhenDisabled) return;
        if (_onners == 0)
        {
            if (_reverseOnOff)
            {
                _on.Invoke();
                _device?.On();
            }
            else
            {
                _off.Invoke();
                _device?.Off();
            }
        }
    }
    public void SingleClick()
    {
        if (!isActiveAndEnabled && !_sendEventsWhenDisabled) return;
        _singleClick.Invoke();
        _device?.SingleClick();
    }
    public void Int(int i)
    {
        if (!isActiveAndEnabled && !_sendEventsWhenDisabled) return;
        _int.Invoke(i);
        _device?.Int(i);
    }

}
