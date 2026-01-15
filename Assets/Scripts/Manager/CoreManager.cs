using System.Resources;
using UnityEditor.EditorTools;
using UnityEngine;

public class CoreManager : MonoSingleton<CoreManager>
{
    private ResourceManager _resource;
    private PoolManager _pool;
    private SoundManager _sound;
    private UIManager _ui;
    private DataManager _data;
    private EventManager _event;
    private SceneControlManager _scene;

    public static ResourceManager Resource => Instance._resource ?? (Instance._resource = new ResourceManager());
    public static PoolManager Pool => Instance._pool ?? (Instance._pool = Instance.gameObject.AddComponent<PoolManager>());
    public static SoundManager Sound => Instance._sound ?? (Instance._sound = Instance.gameObject.AddComponent<SoundManager>());
    public static UIManager UI => Instance._ui ?? (Instance._ui = Instance.gameObject.AddComponent<UIManager>());
    public static DataManager Data => Instance._data ?? (Instance._data = new DataManager());
    public static EventManager Event => Instance._event ?? (Instance._event = new EventManager());
    public static SceneControlManager Scene => Instance._scene ?? (Instance._scene = new SceneControlManager());

    protected override void Awake()
    {
        base.Awake();
        InitManagers();
    }

    private void InitManagers()
    {
        _resource = new ResourceManager();
        _data = new DataManager();
        _event = new EventManager();
        _scene = new SceneControlManager();

        _pool = gameObject.AddComponent<PoolManager>();
        _sound = gameObject.AddComponent<SoundManager>();
        _ui = gameObject.AddComponent<UIManager>();

        _pool.Init();
        _sound.Init();
        _ui.Init();
    }
}