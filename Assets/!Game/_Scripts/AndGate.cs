using UnityEngine;
using UnityEngine.Events;

public class AndGate : DeviceComponent, ISignalReceivable
{
    private int _onners;
    [SerializeField] private int inputCount;
    [SerializeField] private UnityEvent _output;

    public override void Off()
    {
        _onners--;
    }
    private void Start()
    {

    }
    public override void On()
    {

        _onners++;
        if (_onners >= inputCount && isActiveAndEnabled)
        {
            _output.Invoke();
        }
    }

    public override void Int(int i)
    {
    }
    public override void SingleClick()
    {
    }
}
