using System;
using System.Collections;
using UnityEngine;

public class AuthenticationManager : MonoBehaviour
{
    public static AuthenticationManager instance;
    public User user;

    void Awake()
    {
        if(instance != null) {
            Destroy(gameObject);
        }
        else {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
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
}

[Serializable]
class LoginStatusResponse
{
    public bool loggedIn;
}