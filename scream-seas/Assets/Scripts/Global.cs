using UnityEngine;

public class Global : MonoBehaviour {

    private static Global instance;

    public static bool Destructing { get; private set; }
    
    public InputManager Input { get; private set; }
    public MapManager Maps { get; private set; }
    public AudioManager Audio { get; private set; }
    public SerializationManager Serialization { get; private set; }
    public Dispatch Dispatch { get; private set; }
    public MessengerManager MessengerManager { get; private set; }

    public GameData Data => Serialization.Data;
    public SystemData SystemData => Serialization.SystemData;
    public Messenger Messenger => MessengerManager.Current;

    public static Global Instance() {
        if (instance == null) {
            GameObject globalObject = new GameObject("Globals");
            instance = globalObject.AddComponent<Global>();
            instance.InstantiateManagers();
        }

        return instance;
    }

    public void Awake() {
        DontDestroyOnLoad(gameObject);
        MoonSharp.Interpreter.UserData.RegisterAssembly();
    }

    public void Start() {
        Serialization.SystemData.SettingFullScreen.OnModify += () => {
            SetFullscreenMode();
        };
        SetFullscreenMode();
    }

    public void OnDestroy() {
        Destructing = true;
    }

    private void InstantiateManagers() {
        Dispatch = gameObject.AddComponent<Dispatch>();
        Serialization = gameObject.AddComponent<SerializationManager>();
        Input = gameObject.AddComponent<InputManager>();
        Maps = gameObject.AddComponent<MapManager>();
        Audio = gameObject.AddComponent<AudioManager>();
        MessengerManager = gameObject.AddComponent<MessengerManager>();

        MessengerManager.Current = new Messenger();
    }

    private void SetFullscreenMode() {
        Screen.fullScreen = Serialization.SystemData.SettingFullScreen.Value;
    }
}
