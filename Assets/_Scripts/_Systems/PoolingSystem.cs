//Shady
using UnityEngine;
using Shady.Core.Singletons;
using Sirenix.OdinInspector;
using System.Collections.Generic;

#pragma warning disable 0414

internal interface IPoolable
{
    /// <summary>
    /// Unique ID for distinguishing between multiple prefabs of same Type.
    /// </summary>
    int ID {get;}

    /// <summary>
    /// Transform reference to get from interface.
    /// </summary>
    Transform transform {get;}

    /// <summary>
    /// Method called when Object is Made.
    /// </summary>
    internal void Init();

    /// <summary>
    /// Method called when Object is gotten from Pool.
    /// </summary>
    internal void Spawn();

    /// <summary>
    /// Call this method after getting an object from Pool.
    /// </summary>
    internal void Reset();

}//interface end

[System.Serializable]
public sealed class Pool
{
    //===================================================
    // FIELDS
    //===================================================
    [FoldoutGroup("$_name")]
    [Switch]
    [OnValueChanged(nameof(HideToggle))]
    [SerializeField] bool _hideInHierarchy = true;

    [FoldoutGroup("$_name")]
    [LabelText("ID")]
    [DisplayAsString]
    [SerializeField] int _ID = 1;

    [FoldoutGroup("$_name")]
    [DisableInPlayMode]
    [SerializeField] int _poolSize = 10;

    [FoldoutGroup("$_name")]
    [Required, AssetsOnly]
    [OnValueChanged(nameof(SetName)), OnInspectorInit(nameof(SetName))]
    [ValidateInput(nameof(ValidatePrefab), "Prefab must have a script implementing IPoolable!")]
    [InlineEditor(InlineEditorModes.LargePreview, Expanded = false)]
    [DisableInPlayMode]
    [SerializeField] GameObject _prefab = null;

    [FoldoutGroup("$_name")]
    [Switch]
    [SerializeField] bool _debug = false;

    [FoldoutGroup("$_name")]
    [ShowIf(nameof(_debug), true), ReadOnly]
    [SerializeField] GameObject _poolParent = null;

    [FoldoutGroup("$_name")]
    [ShowIf(nameof(_debug), true), ShowInInspector, ReadOnly]
    Queue<IPoolable> _pool = new Queue<IPoolable>();

    //===================================================
    // PRIVATE FIELDS
    //===================================================
    private string _name = "Pool";

    //===================================================
    // PROPERTIES
    //===================================================
    internal int        ID     => _ID;
    internal GameObject Prefab => _prefab;

    //===================================================
    // METHODS
    //===================================================
    private bool ValidatePrefab()
    {
        bool validation = this._prefab == null ? false : (this._prefab.GetComponent<IPoolable>() == null ? false : true);

        if(validation is true)
            _ID = _prefab.GetComponent<IPoolable>().ID;
        else
            _ID = -1;
        
        return validation;
    }//ValidatePrefab()

    internal void SetName() => _name = $"{(_prefab == null ? string.Empty : _prefab.name)}-{_ID}";

    internal void MakePool(Transform parent)
    {
        if(!_prefab)
            return;

        _poolParent = new GameObject($"{_prefab.name} {_ID}");
        _poolParent.transform.SetParent(parent);

        if(_hideInHierarchy)
            _poolParent.hideFlags = HideFlags.HideInHierarchy;

        for(int i=0 ; i<_poolSize ; i++)
        {
            IPoolable poolable = GameObject.Instantiate(_prefab, _poolParent.transform).GetComponent<IPoolable>();
            poolable.transform.SetActive(false);
            _pool.Enqueue(poolable);
            poolable.Init();
            // poolable.BackToPool += ()=>Enqueue(poolable);
        }//loop end
    }//MakePool() end

    internal void Enqueue(IPoolable obj)
    {
        obj.transform.SetActive(false);
        obj.Reset();
        obj.transform.SetParent(_poolParent.transform);
        obj.transform.ResetLocalPos();
        _pool.Enqueue(obj);
    }//Enqueue() end
    
    internal IPoolable Dequeue()
    {
        if(_pool.Count == 0)
        {
            for(int i=0 ; i<10 ; i++)
            {
                IPoolable poolable = GameObject.Instantiate(_prefab, _poolParent.transform).GetComponent<IPoolable>();
                poolable.transform.SetActive(false);
                _pool.Enqueue(poolable);
                poolable.Init();
                // poolable.BackToPool += ()=>Enqueue(poolable);
            }//loop end
        }//if end

        IPoolable obj = _pool.Dequeue();
        obj.Spawn();

        return obj;
    }//Dequeue() end

    private void HideToggle()
    {
        if(_poolParent == null)
            return;

        if(_hideInHierarchy)
            _poolParent.hideFlags = HideFlags.HideInHierarchy;
        else
            _poolParent.hideFlags = HideFlags.None;
    }//HideToggle() end

}//class end

[HideMonoScript]
public sealed class PoolingSystem : Singleton<PoolingSystem>
{
    //===================================================
    // FIELDS
    //===================================================
    [Title("POOLING SYSTEM", titleAlignment: TitleAlignments.Centered)]
    [OnValueChanged(nameof(SetName)), OnInspectorInit(nameof(SetName))]
    [SerializeField] List<Pool> _pools = new List<Pool>();

    //===================================================
    // METHODS
    //===================================================
    private void SetName() => _pools.ForEach(pool => pool.SetName());

    internal override void Init()
    {
        base.Init();

        if(InstanceNotSelf)
            return;
        
        for(int i=0 ; i<_pools.Count ; i++)
            _pools[i].MakePool(transform);

    }//Init() end

    /// <summary>
    /// Method return Object of Type <T> from pool against ID passed as argument.
    /// </summary>
    public T GetObjectOfType<T>(int id = 1) where T : Component =>
        (T)(_pools.Find(p => (p.Prefab.GetComponent<T>() != null && p.ID == id))?.Dequeue());

    /// <summary>
    /// Method puts Object of Type <T> against ID passed as argument back to Pool.
    /// </summary>
    public void BackToPool<T>(int id, T value) where T : Component
    {
        Pool pool = _pools.Find(p => p.Prefab.GetComponent<T>() != null && p.ID == id); 
        
        if(pool is null)
        {
            Debug.Log($"Type {nameof(T)} or {id} not found in Pool!");
            return;
        }//if end
        
        pool.Enqueue(value?.GetComponent<IPoolable>());
    }//BackToPool() end

}//class end