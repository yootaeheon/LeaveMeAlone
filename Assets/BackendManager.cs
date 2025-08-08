using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Database;
using UnityEngine;
using UnityEngine.Events;

public class BackendManager : MonoBehaviour
{
    public static BackendManager Instance { get; private set; }

    private FirebaseApp app;
    public static FirebaseApp App => Instance.app;

    private FirebaseAuth auth;
    public static FirebaseAuth Auth => Instance.auth;

    private FirebaseDatabase database;
    public static FirebaseDatabase Database => Instance.database;

    private FirebaseUser user;
    public static FirebaseUser User { get { return Instance.user; } set { Instance.user = value; } }

    public bool OnFirebaseReady;

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        CheckDependency();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void CheckDependency()
    {
        FirebaseApp.CheckAndFixDependenciesAsync()
            .ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                app = FirebaseApp.DefaultInstance;
                auth = FirebaseAuth.DefaultInstance;
                database = FirebaseDatabase.DefaultInstance;
                database.GoOnline();

                OnFirebaseReady = true;
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependesncies: {task.Result}");
                app = null;
                auth = null;
                database = null;
            }
        });
    }
}