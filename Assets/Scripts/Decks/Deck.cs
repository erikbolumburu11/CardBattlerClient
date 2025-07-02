using System;

[Serializable]
public class Deck {
    public int id;
    public string name;
    public DeckCard[] cards;

    public Deck(string name){
        this.name = name;
        cards = new DeckCard[0];
    }
}