using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckUIManager : MonoBehaviour
{
    [SerializeField] TMP_Dropdown deckDropdown;

    [SerializeField] Transform equippedCardGrid;

    [SerializeField] GameObject cardPrefab;

    void OnEnable() => DeckManager.OnDecksUpdated += UpdateUI;
    void OnDisable() => DeckManager.OnDecksUpdated -= UpdateUI;

    Deck selectedDeck;

    List<GameObject> instantiatedDeckCards;

    void Awake()
    {
        instantiatedDeckCards = new();
    }

    public void UpdateUI(){
        UpdateDeckDropdown();
        selectedDeck = DeckManager.instance.decks[deckDropdown.value];
        UpdateDeckCards();
    }

    void UpdateDeckDropdown(){
        int selectedIndex = deckDropdown.value;

        deckDropdown.ClearOptions();
        deckDropdown.AddOptions(DeckManager.instance.decks.Select((x) => x.name).ToList());

        deckDropdown.SetValueWithoutNotify(selectedIndex);

        deckDropdown.RefreshShownValue();
    }

    void UpdateDeckCards(){
        if(selectedDeck == null) return;

        ClearDeckCards();
        foreach (DeckCard deckCard in selectedDeck.cards)
        {
            GameObject cardUI = EquippedCard.InstantiateCard(deckCard, equippedCardGrid, selectedDeck);
            instantiatedDeckCards.Add(cardUI);
        }
    }

    void ClearDeckCards(){
        for (int i = instantiatedDeckCards.Count - 1; i >= 0; i--)
        {
            Destroy(instantiatedDeckCards[i]);
        }
        instantiatedDeckCards.Clear();
    }

    public void CreateDeckButton(){
        TextInputPopup.SpawnPopup(
            "Deck Name",
            (deckName) => StartCoroutine(DeckManager.instance.CreateDeck(deckName))
        );
    }

    public void DeleteDeckButton(){
        ConfirmPopup.SpawnPopup(
            $"Delete {selectedDeck.name}?",
            () => StartCoroutine(DeckManager.instance.DeleteDeck(selectedDeck.id))
        );
    }
}
