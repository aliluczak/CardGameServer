﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;



public class GameManager : MonoBehaviour
{

    private GameObject networkObject;
    private Card cards;
    private RunServer networkManager;
    public List<string> commonCards;
    private Player playerComponent;
    private cardHero hero;
    private cardSpell spell;
    private List<Card> board;
    private PlayersData playerData;
    private CardData cardData;

    private List<bool> boardA;
    private List<bool> boardB;

    bool movingPhaseActive;
    bool endMovingPhaseA;
    bool endMovingPhaseB;

    bool drawingCard1;
    bool drawingCard2;
    bool drawingCard3;

    bool cardMoved;

    Player activePlayer;
    

    //TODO complete all needed parameters
      

    //players
    internal Player playerA;
    internal Player playerB;

    //
    void Start()
    {
        
        cards = GetComponent<Card>();

        networkObject = GameObject.Find("ServerNetworkManager");
        networkManager = networkObject.GetComponent<RunServer>();

        playerData = GameObject.Find("Data").GetComponent<PlayersData>();
        cardData = GameObject.Find("Data").GetComponent<CardData>();

        commonCards = new List<string>();

        playerComponent = GetComponent<Player>();

        boardA = new List<bool>();
        boardB = new List<bool>();

        for (int i = 0; i < 5; i++)
        {
            boardA.Add(false);
            boardB.Add(false);
        }
        //TODO must have players login to create specific player representation

        movingPhaseActive = false;
        playerA = new Player();
        playerB = new Player();
        
    }

    //generates deck of common cards for both players

    void generatesCommonDeck()
    {
        commonCards.Add("Kula ognia");
        commonCards.Add("Piorun");
        commonCards.Add("Wyleczenie");
        commonCards.Add("Uzdrowienie");
        commonCards.Add("Ściana lodu");
        commonCards.Add("Mur");
        commonCards.Add("Blokada");
        commonCards.Add("Tarcza");
        commonCards.Add("Pułapka");
        commonCards.Add("Sopel lodu");
        commonCards.Add("Trująca strzała");
        commonCards.Add("Kula energii");
    }
    //gameplay

    // 0 - heroA
    // 1 - supportA
    // 2  -randomA
    // 3 - random2A
    // 4 - random3A
    // 5 - heroB
    // 6 - supportB
    // 7  -randomB
    // 8 - random2B
    // 9 - random3B


    //TODO manages all the gameplay with end conditions
    internal void gameplay()
    {
        movingPhaseActive = false;
        
        generatesCommonDeck();

        startGame();

        do
        {
            drawCardsForPlayer(playerA);
            drawCardsForPlayer(playerB);

            movingPhase();


            actionPhase();
            //TODO 

            
        }
        while (playerA.hp == 0 
   //         || playerBHP == 0
            );

        if (playerA.hp == 0)
        {
            networkManager.sendLoseInfo(playerA.playerMessage);
            networkManager.sendWinInfo(playerB.playerMessage);
        }
        else
        {
            networkManager.sendLoseInfo(playerB.playerMessage);
            networkManager.sendWinInfo(playerA.playerMessage);
        }

    }


    private void drawCardsForPlayer(Player player)
    {
        string tempCard = chooseCard("HERO", player);
        cardHero card = cardData.getHero(tempCard); 
        board[2] =card;

        if (player.Equals(playerA))
        {
            boardA[2] = true;

        }
        else
            boardB[2] = true;

        networkManager.sendCard(player.playerMessage, card.id, card.name, "Hero", card.hp,card.attack, card.passive, "", 0, 0);
        StartCoroutine(waitForCardAdded(1));

        if (!drawingCard1)
        {
            tempCard = chooseCard(player);
            card = cardData.getHero(tempCard);

            
            if (card == null)
            {
                cardSpell spell = cardData.getSpell(tempCard);
                networkManager.sendCard(player.playerMessage, spell.id, spell.name, "Spell", 0, spell.attack, 0, spell.description, spell.healing, spell.intercept);
                board[3] = spell;
                StartCoroutine(waitForCardAdded(2));
            }
            else
            {
                
                networkManager.sendCard(player.playerMessage, card.id, card.name, "Hero", card.hp, card.attack, card.passive, "", 0, 0);
                board[3] = hero;
                StartCoroutine(waitForCardAdded(2));

            }

            if (!drawingCard2)
            {


                if (player.Equals(playerA))
                    boardA[3] = true;
                else
                    boardB[3] = true;

                tempCard = chooseCard(player);
                card = cardData.getHero(tempCard);


                if (card == null)
                {
                    cardSpell spell = cardData.getSpell(tempCard);
                    networkManager.sendCard(player.playerMessage, spell.id, spell.name, "Spell", 0, spell.attack, 0, spell.description, spell.healing, spell.intercept);
                    board[4] = spell;
                    StartCoroutine(waitForCardAdded(2));
                }
                else
                {

                    networkManager.sendCard(player.playerMessage, card.id, card.name, "Hero", card.hp, card.attack, card.passive, "", 0, 0);
                    board[4] = hero;
                    StartCoroutine(waitForCardAdded(2));

                }


                if (!drawingCard3)
                {
                    if (player.Equals(playerA))
                        boardA[4] = true;
                    else
                        boardB[4] = true;
                }
            }
        }
    }

    IEnumerator waitForCardAdded(int i)
    {
        switch (i)
        {
            case 1:
                {
                    while (!drawingCard1)
                    {
                        yield return null;
                    }
                    break;
                }
            case 2:
                {
                    while (!drawingCard2)
                    {
                        yield return null;
                    }
                    break;
                }
            case 3:
                {
                    while (!drawingCard3)
                    {
                        yield return null;
                    }
                    break;
                }
        }
    }

    private void movingPhase()
    {
        bool infoSent = false;
        endMovingPhaseA = false;
        endMovingPhaseB = false;
        changeActivePlayer();
        
        setCardUnmoved();
        while (!endMovingPhaseA || !endMovingPhaseB)
        {
            if (cardMoved)
            {
                setCardUnmoved();
                infoSent = false;
            }

            if (!infoSent)
            {
                networkManager.sendMovingPhaseInfo(activePlayer.playerMessage);
                infoSent = true;
            }
            waitForCardMovedInfo();
        }
           

      /*  if (!movingPhaseActive)
        {
            networkManager.sendMovingPhaseInfo(playerA.playerMessage);
            networkManager.sendMovingPhaseInfo(playerB.playerMessage);

            setMovingPhaseActive();
        }

        if (movingPhaseActive)
        {
            waitForMovingPhaseEnd();
        } */

    }

    private void waitForMovingPhaseEnd()
    {
        StartCoroutine(waitForMovingPhaseResponse());
    }

    IEnumerator waitForMovingPhaseResponse()
    {
        while (!endMovingPhaseA || !endMovingPhaseB)
        {
            yield return null;
        }


    }

    private void waitForCardMovedInfo()
    {
        StartCoroutine(waitForCardMoved());
    }

    IEnumerator waitForCardMoved()
    {
        while (!cardMoved)
        {
            yield return null;
        }
        changeActivePlayer();
    }

    private void changeActivePlayer()
    {
        if (activePlayer.Equals(playerA))
        {
            activePlayer = playerB;
        }
        else
            activePlayer = playerA;
    } 
    //TODO takes to connected players into one game, sends request to choose heros for the game
    private void startGame()
    {
        playerA.commonCards = generateDecks();
        playerA.personalCards = playerData.getPlayerCards(playerA.login);
        playerB.commonCards = generateDecks();
        playerB.personalCards = playerData.getPlayerCards(playerB.login);
        board = new List<Card>();

        for (int i = 0; i < 10; i++)
        {
            board.Add(null);
        }
    }

    public void setEndMovingPhase(NetworkMessageInfo info)
    {
        if (info.sender.Equals(playerA.playerMessage.sender))
        {
            endMovingPhaseA = true;
        }
        else
            endMovingPhaseB = true;
    }

    internal void setCardMoved()
    {
        cardMoved = true;
    }

    internal void setCardUnmoved()
    {
        cardMoved = false;
    }

    //generate decks for this game, to add player parameter
    private List<string> generateDecks()
    {
        return commonCards;
    }

    private void setMovingPhaseActive()
    {
        movingPhaseActive = true;
    }

	// function that chooses one random card of specific type from all cards 
	//TODO database connection
   internal string chooseCard(string type, Player player)
    {
       
        if (type.Equals("HERO"))
            {
                int random = Random.Range(0, player.personalCards.Count);
                string data = player.personalCards[random];
                player.personalCards.Remove(data);
                return data;
            }
        else
            {
                int random = Random.Range(0, player.commonCards.Count);
                string data = player.commonCards[random];
                player.commonCards.Remove(data);
                return data;
            }
    }

   internal string chooseCard(Player player)
   {
       Dictionary<string, bool> tempDeck = new Dictionary<string, bool>();

       foreach (string c in player.personalCards)
       {
           tempDeck.Add(c, false);
       }

       foreach (string c in player.commonCards)
       {
           tempDeck.Add(c, true);
       }

       int random = Random.Range(0, tempDeck.Count);

       if (tempDeck.ElementAt(random).Value.Equals(false))
           player.personalCards.Remove(tempDeck.ElementAt(random).Key);
       else
           player.commonCards.Remove(tempDeck.ElementAt(random).Key);

       return tempDeck.ElementAt(random).Key;
   }
  
    //move warrior card to warrior field
    public void moveCard(int from, int to, NetworkMessageInfo info)
    {
        if (info.sender.Equals(playerA.playerMessage.sender))
        {
            board[to] = board[from];
            boardA[from] = false;
            boardA[to] = true;
            networkManager.sendCardMovedInfo(info, from, to);
            setCardMoved();
        }
        else if (info.sender.Equals(playerB.playerMessage.sender))
        {
            board[to + 5] = board[from + 5];
            boardB[from] = false;
            boardB[to] = true;
            networkManager.sendCardMovedInfo(info, from, to);
            setCardMoved();
        }
        else
            networkManager.sendCardCannotBeMovedInfo(info);
    }

    //use magic card
    public void useMagicCard()
    {

    }

    //attack phase
    private void actionPhase()
    {
        
    }

    internal void setDrawingCard()
    {
        drawingCard1 = true;
        drawingCard2 = true;
        drawingCard3 = true;
    }

    internal void setDrawingCard(int i)
    {
        switch (i)
        {
            case 1:
                {
                    drawingCard1 = false;
                    break;
                }
            case 2:
                {
                    drawingCard2 = false;
                    break;
                }
            case 3:
                {
                    drawingCard3 = false;
                    break;
                }
        }
    }

}
