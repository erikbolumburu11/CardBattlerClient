using System;
using System.Collections;
using System.Collections.Generic;
using  UnityEngine;

public class DeckManager : MonoBehaviour
{
    public static DeckManager instance;

    public List<Deck> decks;

    public static event Action OnDecksGot;

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

    void OnEnable() => AuthenticationManager.OnPlayerLoggedIn += OnLogin;
    void OnDisable() => AuthenticationManager.OnPlayerLoggedIn -= OnLogin;

    void OnLogin(){
        StartCoroutine(GetDecks());
    }

    public IEnumerator GetDecks(){
        yield return Request.Send<Deck[]>(
            "deck/get",
            "GET",
            null,
            authenticated: true,
            onSuccess: (decks) => {
                this.decks = new(decks);
                OnDecksGot?.Invoke();
            },
            onError: (err) => {
                Debug.LogError("Failed to get deck data: " + err);
            }
        );
    }
}
