using System;

public interface IObjectPool
{
    Action<IObjectPool> OnRelease { get; set; }
    void Enable();
    void Disable();
}
