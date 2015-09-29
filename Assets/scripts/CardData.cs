using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CardData : MonoBehaviour {

    private GameObject gameManagerObject;
    private Card card;
    private cardHero cardhero;
    private cardSpell cardspell;
    private List<Card> cardsData;
	// Use this for initialization
    void Awake()
    {
        cardsData = new List<Card>();
        gameManagerObject = GameObject.Find("GameManager");
        cardhero = gameManagerObject.GetComponent<cardHero>();
        cardspell = gameManagerObject.GetComponent<cardSpell>();

        addData();
    }

    void addData()
    {
        cardSpell spell = new cardSpell(1, "Blokada", "Blokuje 1 punkt ataku", 0, 0, 1);
        cardsData.Add(spell);
        cardSpell spell2 = new cardSpell(2, "Kula energii", "Zadaje 2 punkty ataku", 2, 0, 0);
        cardsData.Add(spell2);
        cardSpell spell3 = new cardSpell(3, "Kula ognia", "Zadaje 2 punkty obrażeń", 2, 0, 0);
        cardsData.Add(spell3);
        cardSpell spell4 = new cardSpell(4, "Mur", "Blokuje 2 punkty ataku", 0, 0, 2);
        cardsData.Add(spell4);
        cardSpell spell5 = new cardSpell(5, "Piorun", "Zadaje 2 punkty obrażeń", 2, 0, 0);
        cardsData.Add(spell5);
        cardSpell spell6 = new cardSpell(6, "Pułapka", "Zadaje 1 punkt ataku", 1, 0, 0);
        cardsData.Add(spell6);
        cardSpell spell7 = new cardSpell(7, "Sopel lodu", "Zadaje 1 pkt ataku", 1, 0, 0);
        cardsData.Add(spell7);
        cardSpell spell8 = new cardSpell(8, "Trujaca strzała", "Zadaje 2 pkty ataku", 2, 0, 0);
        cardsData.Add(spell8);
        cardSpell spell9 = new cardSpell(9, "Uzdrowienie", "Leczy 2 hp", 0, 2, 0);
        cardsData.Add(spell9);
        cardSpell spell10 = new cardSpell(10, "Wyleczenie", "Leczy 1 hp", 0, 1, 0);
        cardsData.Add(spell10);
        cardSpell spell11 = new cardSpell(11, "Ściana lodu", "Blokuje 1 punkt ataku", 0, 0, 1);
        cardsData.Add(spell11);
        cardHero hero = new cardHero(12, "Creep1", Card.CardSubType.WARRIOR, 5, 2, 1);
        cardsData.Add(hero);
        cardHero hero2 = new cardHero(13, "Creep2", Card.CardSubType.TANK, 6, 1, 5);
        cardsData.Add(hero2);
        cardHero hero3 = new cardHero(14, "Creep3", Card.CardSubType.MAGE, 4, 2, 3);
        cardsData.Add(hero3);
        cardHero hero4 = new cardHero(15, "Aatrox", Card.CardSubType.WARRIOR, 7, 4, 1);
        cardsData.Add(hero4);
        cardHero hero5 = new cardHero(16, "Ahri", Card.CardSubType.MAGE, 5, 4, 3);
        cardsData.Add(hero5);
        cardHero hero6 = new cardHero(17, "Akali", Card.CardSubType.MAGE, 5, 4, 3);
        cardsData.Add(hero6);
        cardHero hero7 = new cardHero(18, "Alistar", Card.CardSubType.TANK, 10, 1, 4);
        cardsData.Add(hero7);
        cardHero hero8 = new cardHero(19, "Amumu", Card.CardSubType.TANK, 9, 2, 4);
        cardsData.Add(hero8);
    }

    public cardHero getHero(string name) 
    {
        if (cardsData.Find(ByName(name)) as cardHero)
        {
            return cardsData.Find(ByName(name)) as cardHero;
        }
        else return null;
    }

    public cardSpell getSpell(string name)
    {
        cardSpell spell = cardsData.Find(ByName(name)) as cardSpell;
        return spell;
    }

    static Predicate<Card> ByName(string name)
    {
        return delegate(Card card)
        {
            return card.name == name;
        };
    }
}
