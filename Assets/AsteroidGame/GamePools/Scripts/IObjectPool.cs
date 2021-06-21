using System;

public interface IObjectPool
{
    event Action<IObjectPool> OnRelease;

    void Enable();
    void Disable();
}
