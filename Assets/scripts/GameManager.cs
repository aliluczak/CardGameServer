using UnityEngine;
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
    private DatabaseManager databaseManager;

    private List<bool> boardA;
    private List<bool> boardB;

    bool movingPhaseActive;
    bool endMovingPhaseA;
    bool endMovingPhaseB;
    

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

        databaseManager = GameObject.Find("Database").GetComponent<DatabaseManager>();

        commonCards = new List<string>();

        playerComponent = GetComponent<Player>();

        board = new List<Card>();
        boardA = new List<bool>();
        boardB = new List<bool>();

        for (int i = 0; i < 5; i++)
        {
            boardA.Add(false);
            boardB.Add(false);
        }
        //TODO must have players login to create specific player representation

        movingPhaseActive = false;
        endMovingPhaseA = false;
        endMovingPhaseB = false;
        
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
        board = new List<Card>();

        generatesCommonDeck();

        startGame();

        Player activePlayer = playerA;
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
        List<string> templist = databaseManager.getCard(Card.CardType.HERO, tempCard);
        Card temp = new cardHero(int.Parse(templist[0]), templist[1], databaseManager.getHeroClass(int.Parse(templist[2])), int.Parse(templist[3]), int.Parse(templist[4]), int.Parse(templist[5]));
        board[2] = temp;

        if (player.Equals(playerA))
            boardA[2] = true;
        else
            boardB[2] = true;

        templist.Clear();
        tempCard = chooseCard(player);
        templist = databaseManager.getCard(Card.CardType.HERO, tempCard);

        Card temp2;
        if (templist == null)
        {
            templist = databaseManager.getCard(Card.CardType.SPELL, tempCard);
            temp2 = new cardSpell(int.Parse(templist[0]), templist[1], templist[2], int.Parse(templist[3]), int.Parse(templist[4]), int.Parse(templist[5]));
        }
        else
        {
            temp2 = new cardHero(int.Parse(templist[0]), templist[1], databaseManager.getHeroClass(int.Parse(templist[2])), int.Parse(templist[3]), int.Parse(templist[4]), int.Parse(templist[5]));
        }

        board[3] = temp2;

        if (player.Equals(playerA))
            boardA[3] = true;
        else
            boardB[3] = true;

        templist.Clear();
        tempCard = chooseCard(player);
        templist = databaseManager.getCard(Card.CardType.HERO, tempCard);

        Card temp3;
        if (templist == null)
        {
            templist = databaseManager.getCard(Card.CardType.SPELL, tempCard);
            temp3 = new cardSpell(int.Parse(templist[0]), templist[1], templist[2], int.Parse(templist[3]), int.Parse(templist[4]), int.Parse(templist[5]));
        }
        else
        {
            temp3 = new cardHero(int.Parse(templist[0]), templist[1], databaseManager.getHeroClass(int.Parse(templist[2])), int.Parse(templist[3]), int.Parse(templist[4]), int.Parse(templist[5]));
        }

        board[4] = temp3;

        if (player.Equals(playerA))
            boardA[4] = true;
        else
            boardB[4] = true;
    }

    private void movingPhase()
    {
        if (!movingPhaseActive)
        {
            networkManager.sendMovingPhaseInfo(playerA.playerMessage);
            networkManager.sendMovingPhaseInfo(playerB.playerMessage);
            setMovingPhaseActive();
        }

        if (movingPhaseActive)
        {
            waitForMovingPhaseEnd();
        }
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
    //TODO takes to connected players into one game, sends request to choose heros for the game
    private void startGame()
    {
        playerA.commonCards = generateDecks();
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
        }
        else if (info.sender.Equals(playerB.playerMessage.sender))
        {
            board[to + 5] = board[from + 5];
            boardB[from] = false;
            boardB[to] = true;
            networkManager.sendCardMovedInfo(info, from, to);
        }
        else
            networkManager.sendCardCannotBeMovedInfo(info);
    }

    //use magic card
    private void useMagicCard()
    {

    }

    //attack phase
    private void actionPhase()
    {

    }


}
