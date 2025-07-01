using System;
using System.Collections;
using UnityEngine;

public class AuthenticationManager : MonoBehaviour
{
    public static AuthenticationManager instance;
    public User user;

    public static event Action OnPlayerLoggedIn;

    void Awake()
    {
        if(instance != null) {
            Destroy(gameObject);
            return;
        }
        else {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        StartCoroutine(GetLoginStatus((loggedIn) => {
            if(loggedIn) {
                Login(PlayerPrefs.GetString("auth_token"));
            }
        }));
    }

    public static IEnumerator GetLoginStatus(Action<bool> onComplete){
        yield return Request.Send<LoginStatusResponse>(
            "auth/status",
            "GET",
            null,
            authenticated: true,
            onSuccess: (res) => {
                onComplete?.Invoke(res.loggedIn); 
            },
            onError: (err) => {
                Debug.LogError("Failed to check login status: " + err);
                onComplete?.Invoke(false);
            }
        );
    }

    public static IEnumerator GetUser(Action<User> onComplete){
        yield return Request.Send<User>(
            "auth/userdata",
            "GET",
            null,
            authenticated: true,
            onSuccess: (user) => {
                onComplete?.Invoke(user);
            },
            onError: (err) => {
                Debug.LogError("Failed to get user data: " + err);
            }
        );
    }

    public static void Login(string token){
        PlayerPrefs.SetString("auth_token", token);
        CoroutineRunner.instance.Run(GetUser((user) => {
            instance.user = user;
            OnPlayerLoggedIn?.Invoke();
            MenuManager.instance.ClearHistory();
            MenuManager.instance.OpenMenu(MenuManager.instance.mainMenu);
        }));
    }
}

[Serializable]
class LoginStatusResponse
{
    public bool loggedIn;
}