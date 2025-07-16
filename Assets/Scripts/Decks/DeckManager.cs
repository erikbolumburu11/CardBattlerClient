using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public static DeckManager instance;

    public List<Deck> decks;

    public static event Action OnDecksUpdated;

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
                OnDecksUpdated?.Invoke();
            },
            onError: (err) => {
                Debug.LogError("Failed to get deck data: " + err);
            }
        );
    }

    [Serializable] public class CreateDeckRequestBody{ public string name; }
    public IEnumerator CreateDeck(string name){
        yield return Request.Send<object>(
            "deck/new",
            "POST",
            new CreateDeckRequestBody(){name = name},
            authenticated: true,
            onSuccess: (res) => {
                decks.Add(new Deck(name));
                OnDecksUpdated?.Invoke();
                StartCoroutine(GetDecks());
            },
            onError: (err) => Debug.LogError("Failed to create deck: " + err)
        );
    }

    [Serializable] public class DeleteDeckRequestBody{ public int id; }
    public IEnumerator DeleteDeck(int id){
        yield return Request.Send<object>(
            "deck/delete",
            "POST",
            new DeleteDeckRequestBody(){id = id},
            authenticated: true,
            onSuccess: (res) => {
                decks.Remove(decks.Where(x => x.id == id).Single());
                OnDecksUpdated?.Invoke();
                StartCoroutine(GetDecks());
            },
            onError: (err) => Debug.LogError("Failed to delete deck: " + err)
        );
    }

    [Serializable] public class ModifyDeckCardRequestBody{
        public int deckId; 
        public int cardId;
    }

    public IEnumerator AddCard(int deckId, int cardId){
        yield return Request.Send<object>(
            "deck/add",
            "POST",
            new ModifyDeckCardRequestBody(){
                deckId = deckId,
                cardId = cardId
            },
            authenticated: true,
            onSuccess: (res) => {
                OnDecksUpdated?.Invoke();
                StartCoroutine(GetDecks());
            },
            onError: (err) => Debug.LogError("Failed to add card: " + err)
        );
    }

    public IEnumerator RemoveCard(int deckId, int cardId){
        yield return Request.Send<object>(
            "deck/remove",
            "POST",
            new ModifyDeckCardRequestBody(){
                deckId = deckId,
                cardId = cardId
            },
            authenticated: true,
            onSuccess: (res) => {
                OnDecksUpdated?.Invoke();
                StartCoroutine(GetDecks());
            },
            onError: (err) => Debug.LogError("Failed to remove card: " + err)
        );
    }
}
