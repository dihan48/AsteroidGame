public delegate void OnRelease(IObjectPool objectInPool);

public interface IObjectPool
{
    event OnRelease onRelease;
    void Enable();
    void Disable();
}
