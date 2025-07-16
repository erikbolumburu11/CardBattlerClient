using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquippedCard : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text quantityText;

    public Button decreaseQuantityButton;
    public Button increaseQuantityButton;

    public static GameObject InstantiateCard(DeckCard deckCard, Transform parent, Deck deck){
        GameObject cardPrefab = Resources.Load<GameObject>("Prefabs/Card/EquippedCard");
        if (cardPrefab == null)
        {
            Debug.LogError("EquippedCard prefab not found!");
            return null;
        }

        GameObject instance = Instantiate(cardPrefab, parent);
        EquippedCard ec = instance.GetComponent<EquippedCard>();

        ec.nameText.text = deckCard.card.name;
        ec.quantityText.text = deckCard.quantity.ToString();

        ec.decreaseQuantityButton.onClick.AddListener(() => {
            CoroutineRunner.instance.Run(DeckManager.instance.RemoveCard(deck.id, deckCard.card.id));
        });

        ec.increaseQuantityButton.onClick.AddListener(() => {
            CoroutineRunner.instance.Run(DeckManager.instance.AddCard(deck.id, deckCard.card.id));
        });

        return instance;
    }
}