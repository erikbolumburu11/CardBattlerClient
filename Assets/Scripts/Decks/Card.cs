using System;

[Serializable]
public class DeckCard {
    public int id;
    public Card card;
    public int quantity;
}

[Serializable]
public class Card {
    public int id;
    public string name;
    public int damage;
    public int health;
    public string imageURL;
}