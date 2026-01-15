using UnityEngine;
using System.Collections.Generic;

public class ResourceManager
{
    private Dictionary<string, Object> _resources = new Dictionary<string, Object>();

    public T Load<T>(string path) where T : Object
    {
        if (_resources.TryGetValue(path, out Object cachedResource))
        {
            return cachedResource as T;
        }

        T resource = Resources.Load<T>(path);
        if (resource != null)
        {
            _resources.Add(path, resource);
        }
        return resource;
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>(path);
        if (original == null)
        {
            return null;
        }

        if (original.GetComponent<Poolable>() != null)
        {
            return CoreManager.Pool.Pop(original, parent).gameObject;
        }

        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;
        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null) return;

        Poolable poolable = go.GetComponent<Poolable>();
        if (poolable != null)
        {
            CoreManager.Pool.Push(poolable);
            return;
        }

        Object.Destroy(go);
    }
}