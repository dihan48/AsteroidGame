using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

    private List<IObjectPool> _available = new List<IObjectPool>();
    private List<IObjectPool> _inUse = new List<IObjectPool>();

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

        if(_available.Count == 0)
        {
            objectInPool = Instantiate(prefab, transform).GetComponent<IObjectPool>();
        }
        else
        {
            objectInPool = _available[0];
            _available.Remove(objectInPool);
        }

        objectInPool.Enable();
        objectInPool.onRelease += Release;

        _inUse.Add(objectInPool);
        return objectInPool;
    }

    public void Release(IObjectPool objectInPool)
    {
        if (_inUse.Remove(objectInPool) == false)
        {
            return;
        }
        else
        {
            objectInPool.onRelease -= Release;
            objectInPool.Disable();
            _available.Add(objectInPool);
        }

        if (_inUse.Count == 0)
        {
            onInUseEmpty?.Invoke();
        }
    }

    public void AllRelease()
    {
        _inUse.ForEach(delegate (IObjectPool objectInPool)
        {
            objectInPool.onRelease -= Release;
            objectInPool.Disable();
            _available.Add(objectInPool);
        });

        _inUse.Clear();
    }
}
