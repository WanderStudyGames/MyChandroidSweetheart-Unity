using System.Collections;
using UnityEngine;

public class HeartMonitor : DeviceComponent
{
    [SerializeField, Range(2, 10)] private float _beatInterval = 2f;
    [SerializeField] private SignalOutput[] _signalOutputs;
    private int _beatIndex;
    public override void On()
    {
        StartSequence();
    }

    public override void Off()
    {
        StopSequence();
    }

    public override void SingleClick()
    {
        StartSequence();
    }

    public override void Int(int i)
    {
        if (i <= 0)
        {
            StopSequence();
        }
        else
        {
            StartSequence();
        }
    }
    public void StartSequence()
    {
        if (_signalOutputs.Length == 0) { Debug.LogError("No Signal Outputs assigned to HeartMonitor", gameObject); return; }
        IEnumerator Co_Sequence()
        {
            while (true)
            {
                if (_signalOutputs[_beatIndex] != null)
                    _signalOutputs[_beatIndex].On();
                yield return new WaitForSeconds(_beatInterval);
                if (_signalOutputs[_beatIndex] != null)
                    _signalOutputs[_beatIndex].Off();
                _beatIndex = _beatIndex + 1 >= _signalOutputs.Length ? 0 : _beatIndex + 1;
            }
        }
        StopAllCoroutines();
        _beatIndex = 0;
        StartCoroutine(Co_Sequence());
    }
    public void StopSequence()
    {
        foreach (SignalOutput output in _signalOutputs)
        {
            output.Off();
        }
        StopAllCoroutines();
    }
}
