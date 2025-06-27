using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public static class Request
{
    public static IEnumerator Send<TResponse>(string endpoint, string method, object body, bool authenticated, Action<TResponse> onSuccess, Action<string> onError)
    {
        string json = body != null ? JsonUtility.ToJson(body) : null;

        yield return SendRequest(endpoint, method, json, authenticated, (req) => {
            if (req.result == UnityWebRequest.Result.Success)
            {
                TResponse result = JsonUtility.FromJson<TResponse>(req.downloadHandler.text);
                onSuccess(result);
            }
            else
            {
                onError?.Invoke(req.downloadHandler.text);
            }
        });
    }

    public static IEnumerator SendRequest(string endpoint, string method, string jsonBody, bool authenticated, Action<UnityWebRequest> onComplete)
    {
        string url = $"{ConnectionManager.instance.Hostname}/{endpoint}";
        UnityWebRequest request = new UnityWebRequest(url, method);

        if (!string.IsNullOrEmpty(jsonBody))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        }

        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        if(authenticated){
            string authToken = PlayerPrefs.GetString("auth_token", null);
            if (!string.IsNullOrEmpty(authToken)){
                request.SetRequestHeader("Authorization", $"Bearer {authToken}");
            }
        }

        yield return request.SendWebRequest();

        onComplete?.Invoke(request);
    }
}
