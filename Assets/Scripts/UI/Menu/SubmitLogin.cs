using System;
using TMPro;
using UnityEngine;

[Serializable]
public class LoginResult {
    public string token;
}

[Serializable]
public class LoginRequest {
    public string username;
    public string password;

    public LoginRequest(string username, string password) {
        this.username = username;
        this.password = password;
    }
}

public class SubmitLogin : MonoBehaviour
{
    [SerializeField] TMP_InputField usernameField;
    [SerializeField] TMP_InputField passwordField;

    [SerializeField] TMP_Text errorText;

    public void Submit(){
        StartCoroutine(Request.Send<LoginResult>(
            "auth/login",
            "POST",
            new LoginRequest(usernameField.text, passwordField.text),
            false,
            (res) => {
                AuthenticationManager.Login(res.token);
            },
            (err) => {
                if(string.IsNullOrWhiteSpace(err)){
                    Debug.LogError("Can't reach server.");
                    errorText.text = "Can't Reach Server";
                }
                else{
                    Debug.LogError("Login Failed: " + err);
                    errorText.text = err;
                }
            }
        ));
    }
}