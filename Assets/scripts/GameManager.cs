using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class GameManager : MonoBehaviour
{

    private GameObject cardObject;
    private GameObject networkObject;
    private Card cards;
    private RunServer networkManager;
    private List<Card> commonCards;

    //TODO complete all needed parameters
    public struct Player 
    {
        public string login;
        internal int hp;
        internal List<Card> personalCards;
        internal List<Card> deck;
        internal List<Card> cemetary;
        internal Card hero;
        internal Card support;
        internal Card randomHero;
        internal Card randomCard1;
        internal Card randomCard2;
    }

    //players
    public Player playerA;

    //
    void Start()
    {
        cardObject = GameObject.Find("CardBase");
        cards = cardObject.GetComponent<Card>();
        networkObject = GameObject.Find("ServerNetworkManager");
        networkManager = networkObject.GetComponent<RunServer>();

        commonCards = new List<Card>();

    }

    //generates dek of common cards
    void addCommonCards(List<Card> cards)
    {

    }

    //gameplay

    //TODO manages all the gameplay with end conditions
    internal void gameplay()
    {

    }

    //TODO takes to connected players into one game, sends request to choose heros for the game
    private void startGame()
    {

    }

    //TODO generate decks for this game, to add player parameter
    private List<Card> generateDecks(Player player)
    {
        List<Card> deck = new List<Card>();
        List<Card> tempDeck = new List<Card>();
 
        tempDeck.AddRange(player.personalCards);
        tempDeck.AddRange(commonCards);

        while (tempDeck.Count != 0)
        {
            int index = Random.Range(0, tempDeck.Count);
            deck.Add(tempDeck[index]);
            tempDeck.RemoveAt(index);
        }

        return deck;
   }


    internal void chooseCard(string type, NetworkMessageInfo info, string gameObjectName)
    {
        bool somethingAdded = false;
        List<int> chosenCards = new List<int>();
        for (int i = 0; i < cards.cardType.Count; i++)
        {
            if (type.Equals("HERO") && cards.cardType[i] == Card.CardType.HERO)
            {
                chosenCards.Add(i);
                somethingAdded = true;
            }


            if (type.Equals("BOOSTER") && cards.cardType[i] == Card.CardType.BOOSTER)
            {
                chosenCards.Add(i);
                somethingAdded = true;
            }


            if (type.Equals("SKILL") && cards.cardType[i] == Card.CardType.SKILL)
            {
                chosenCards.Add(i);
                somethingAdded = true;
            }

        }


        if (somethingAdded)
        {

            int chosenOne = Random.Range(0, chosenCards.Count);
            networkManager.sendCard(info, cards.attack[chosenOne], cards.defense[chosenOne], gameObjectName);
        }
        else
            networkManager.noCard(info);

    }
}
