using System;

public interface IObjectPool
{
    Action<IObjectPool> onRelease { get; set; }
    void Enable();
    void Disable();
}
