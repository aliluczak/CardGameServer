using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameManager : MonoBehaviour
{

    private GameObject cardObject;
    private GameObject networkObject;
    private Card cards;
    private RunServer networkManager;

    void Start()
    {
        cardObject = GameObject.Find("CardBase");
        cards = cardObject.GetComponent<Card>();
        networkObject = GameObject.Find("ServerNetworkManager");
        networkManager = networkObject.GetComponent<RunServer>();
    }

    public void chooseCard(string type, NetworkMessageInfo info, string gameObjectName)
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
