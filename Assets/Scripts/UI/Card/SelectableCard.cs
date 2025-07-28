using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectableCard : MonoBehaviour
{
    public Image image;

    public TMP_Text nameText;
    public TMP_Text attackText;
    public TMP_Text healthText;

    public Button addToDeckButton;

    public static GameObject InstantiateCard(Card card, Transform parent, Deck deck){
        GameObject cardPrefab = Resources.Load<GameObject>("Prefabs/Card/SelectableCard");
        if (cardPrefab == null)
        {
            Debug.LogError("SelectableCard prefab not found!");
            return null;
        }

        GameObject instance = Instantiate(cardPrefab, parent);
        SelectableCard sc = instance.GetComponent<SelectableCard>();

        CoroutineRunner.instance.Run(card.GetSprite((sprite) => {
            sc.image.sprite = sprite;
        }));

        sc.nameText.text = card.name;
        sc.attackText.text = card.damage.ToString();
        sc.healthText.text = card.health.ToString();

        sc.addToDeckButton.onClick.AddListener(() => {
            CoroutineRunner.instance.Run(DeckManager.instance.AddCard(deck.id, card.id));
        });

        return instance;
    }
}