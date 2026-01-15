using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    class Pool
    {
        public GameObject Original { get; private set; }
        public Transform Root { get; set; }

        private Stack<Poolable> _poolStack = new Stack<Poolable>();

        public void Init(GameObject original, int count = 5)
        {
            Original = original;
            Root = new GameObject().transform;
            Root.name = $"{original.name}_Root";

            for (int i = 0; i < count; i++)
            {
                Push(Create());
            }
        }

        private Poolable Create()
        {
            GameObject go = Instantiate(Original);
            go.name = Original.name;
            return go.GetOrAddComponent<Poolable>();
        }

        public void Push(Poolable poolable)
        {
            if (poolable == null) return;

            poolable.transform.SetParent(Root);
            poolable.gameObject.SetActive(false);
            poolable.IsUsing = false;

            _poolStack.Push(poolable);
        }

        public Poolable Pop(Transform parent)
        {
            Poolable poolable;

            if (_poolStack.Count > 0)
                poolable = _poolStack.Pop();
            else
                poolable = Create();

            poolable.gameObject.SetActive(true);
            poolable.transform.SetParent(parent);
            poolable.IsUsing = true;

            return poolable;
        }
    }

    private Dictionary<string, Pool> _poolDict = new Dictionary<string, Pool>();
    private Transform _root;

    public void Init()
    {
        if (_root == null)
        {
            _root = new GameObject("@Pool_Root").transform;
            _root.SetParent(transform);
        }
    }

    public void CreatePool(GameObject original, int count = 5)
    {
        Pool pool = new Pool();
        pool.Init(original, count);
        pool.Root.SetParent(_root);

        _poolDict.Add(original.name, pool);
    }

    public void Push(Poolable poolable)
    {
        string name = poolable.gameObject.name;
        if (_poolDict.ContainsKey(name) == false)
        {
            Destroy(poolable.gameObject);
            return;
        }

        _poolDict[name].Push(poolable);
    }

    public Poolable Pop(GameObject original, Transform parent = null)
    {
        if (_poolDict.ContainsKey(original.name) == false)
            CreatePool(original);

        return _poolDict[original.name].Pop(parent);
    }

    public GameObject GetOriginal(string name)
    {
        if (_poolDict.ContainsKey(name) == false)
            return null;
        return _poolDict[name].Original;
    }

    public void Clear()
    {
        foreach (Transform child in _root)
            Destroy(child.gameObject);

        _poolDict.Clear();
    }
}