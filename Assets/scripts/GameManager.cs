using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameManager : MonoBehaviour
{

    private GameObject cardObject;
    private GameObject networkObject;
    private Card cards;
    private RunServer networkManager;

    //TODO complete all needed parameters
    internal struct Player 
    {
        internal string login;
        internal int hp;
        internal List<Card> deck;
        internal List<Card> cemetary;
        internal Card hero;
        internal Card support;
        internal Card randomHero;
        internal Card randomCard1;
        internal Card randomCard2;
    }

    //players
    internal Player playerA;
    internal Player playerB;


    void Start()
    {
        cardObject = GameObject.Find("CardBase");
        cards = cardObject.GetComponent<Card>();
        networkObject = GameObject.Find("ServerNetworkManager");
        networkManager = networkObject.GetComponent<RunServer>();
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

    //TODO generate decks for this game
    private void generateDecks()
    {

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
