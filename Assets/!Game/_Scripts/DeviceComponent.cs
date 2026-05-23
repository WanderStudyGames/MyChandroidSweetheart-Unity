using UnityEngine;

public abstract class DeviceComponent : MonoBehaviour, ISignalReceivable
{
    public abstract void On();
    public abstract void Off();
    public abstract void SingleClick();
    public abstract void Int(int i);
}