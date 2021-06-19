using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

    private List<IObjectPool> available = new List<IObjectPool>();
    private List<IObjectPool> inUse = new List<IObjectPool>();

    public List<IObjectPool> InUse { get => new List<IObjectPool>(inUse); }

    public Action onInUseEmpty;

    private void Awake()
    {
        if (prefab.GetComponent<IObjectPool>() == null)
        {
            throw new NullReferenceException("Prefab doesn't have IObjectPool");
        }
    }

    public IObjectPool Get()
    {
        IObjectPool objectInPool;

        if(available.Count == 0)
        {
            objectInPool = Instantiate(prefab, transform).GetComponent<IObjectPool>();
        }
        else
        {
            objectInPool = available[0];
            available.Remove(objectInPool);
        }

        objectInPool.Enable();
        objectInPool.OnRelease += Release;

        inUse.Add(objectInPool);
        return objectInPool;
    }

    public void Release(IObjectPool objectInPool)
    {
        if (inUse.Remove(objectInPool) == false)
        {
            return;
        }
        else
        {
            objectInPool.OnRelease -= Release;
            objectInPool.Disable();
            available.Add(objectInPool);
        }

        if (inUse.Count == 0)
        {
            onInUseEmpty?.Invoke();
        }
    }

    public void AllRelease()
    {
        inUse.ForEach((IObjectPool objectInPool) =>
        {
            objectInPool.OnRelease -= Release;
            objectInPool.Disable();
            available.Add(objectInPool);
        });

        inUse.Clear();
    }
}
