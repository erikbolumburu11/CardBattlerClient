using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckUIManager : MonoBehaviour
{
    [SerializeField] TMP_Dropdown deckDropdown;

    [SerializeField] Transform equippedCardGrid;
    [SerializeField] Transform selectableCardGrid;

    [SerializeField] GameObject cardPrefab;

    void OnEnable() => DeckManager.OnDecksUpdated += UpdateUI;
    void OnDisable() => DeckManager.OnDecksUpdated -= UpdateUI;

    Deck selectedDeck;

    List<GameObject> instantiatedDeckCards;
    List<GameObject> instantiatedSelectableCards;

    void Awake()
    {
        instantiatedDeckCards = new();
        instantiatedSelectableCards = new();
    }

    public void UpdateUI(){
        UpdateDeckDropdown();
        selectedDeck = DeckManager.instance.decks[deckDropdown.value];
        UpdateCardSelection();
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

    void UpdateCardSelection(){
        ClearSelectableCards();
        foreach (Card card in DeckManager.instance.selectableCards)
        {
            GameObject cardUI = SelectableCard.InstantiateCard(card, selectableCardGrid, selectedDeck);
            instantiatedSelectableCards.Add(cardUI);
        }
    }

    void ClearDeckCards(){
        for (int i = instantiatedDeckCards.Count - 1; i >= 0; i--)
        {
            Destroy(instantiatedDeckCards[i]);
        }
        instantiatedDeckCards.Clear();
    }

    void ClearSelectableCards(){
        for (int i = instantiatedSelectableCards.Count - 1; i >= 0; i--)
        {
            Destroy(instantiatedSelectableCards[i]);
        }
        instantiatedSelectableCards.Clear();
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
