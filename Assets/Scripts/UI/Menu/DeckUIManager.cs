using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DeckUIManager : MonoBehaviour
{
    [SerializeField] TMP_Dropdown deckDropdown;

    [SerializeField] Transform equippedCardGrid;

    [SerializeField] GameObject cardPrefab;

    void OnEnable() => DeckManager.OnDecksGot += UpdateUI;
    void OnDisable() => DeckManager.OnDecksGot -= UpdateUI;

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
            for (int i = 0; i < deckCard.quantity; i++)
            {
                GameObject cardUI = Instantiate(cardPrefab, equippedCardGrid);
                instantiatedDeckCards.Add(cardUI);
            }
        }
    }

    void ClearDeckCards(){
        for (int i = instantiatedDeckCards.Count - 1; i >= 0; i--)
        {
            Destroy(instantiatedDeckCards[i]);
        }
        instantiatedDeckCards.Clear();
    }
}
