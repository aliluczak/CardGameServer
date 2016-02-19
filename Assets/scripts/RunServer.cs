using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class MyMSGTypes
{
    // msgs sent from server
    public static short SendCard = 1001;
    public static short NoCard = 1002;
    public static short CardRequest = 1003;
    public static short Win = 1004;
    public static short Lose = 1005;
    public static short UserRegistered = 1005;
    public static short UsernameExists = 1006;
    public static short UserNotFound = 1007;
    public static short WrongPassword = 1008;
    public static short LoginSuccess = 1009;
    public static short CardMoved = 1010;
    public static short cardCannotBeMoved = 1011;
    public static short movingPhaseBegins = 1012;
    public static short drawingCards = 1013;
    public static short waitForAnotherPlayer = 1014;
    public static short Tie = 1022;

    // msgs sent from client
    public static short Register = 1016;
    public static short MoveCardRequest = 1017;
    public static short Login = 1018;
    public static short CardAdded = 1019;
    public static short magic = 1020;
    public static short endMovePhase = 1021;

}

public class RunServer : MonoBehaviour {

    public string connectionIP = "127.0.0.1";
    public int connectionPort = 8000;
    private string infoHistory = "";
    private NetworkView serverNetworkView;
    private GameObject gameManagerObject;
    private GameObject serverNetworkManager;
    private GameManager gameManager;
    private GameObject dataBaseObject;
    private PlayersData playerData;
    private CardData cardData;


    //private string registerGameName = "alav5112021";

    //initilizes all needed gameobjects and components
    void Start()
    {
        serverNetworkManager = GameObject.Find("ServerNetworkManager");
        serverNetworkView = serverNetworkManager.GetComponent<NetworkView>();
        gameManagerObject = GameObject.Find("GameManager");
        gameManager = gameManagerObject.GetComponent<GameManager>();
        dataBaseObject = GameObject.Find("Data");
        playerData = dataBaseObject.GetComponent<PlayersData>();
        cardData = dataBaseObject.GetComponent<CardData>();
        registerMsgHandlers();
    }

    private void registerMsgHandlers()
    {
        NetworkServer.RegisterHandler(MyMSGTypes.Register, Register);
        NetworkServer.RegisterHandler(MyMSGTypes.MoveCardRequest, moveCardRequest);
        NetworkServer.RegisterHandler(MyMSGTypes.Login, Login);
        NetworkServer.RegisterHandler(MyMSGTypes.CardAdded, cardAdded);
        NetworkServer.RegisterHandler(MyMSGTypes.magic, magic);
        NetworkServer.RegisterHandler(MyMSGTypes.endMovePhase, endMovePhase);

    }

    public void textMessage(string message)
    {
        infoHistory += message;
    }
    //server interface
    void OnGUI()
    {
        if (Network.peerType == NetworkPeerType.Disconnected){
            connectionIP = GUILayout.TextField(connectionIP);
            connectionPort = int.Parse(GUILayout.TextField(connectionPort.ToString()));

            if (GUILayout.Button("Start Server"))
            {
                try{
                    Network.InitializeServer(2, connectionPort,false);
   //                 MasterServer.RegisterHost(registerGameName, "Card Game Project", "Trying and adapting card game");
                } 
                
                catch(Exception)
                {
                    infoHistory+="Cannot create a server, please check IP and port";
                }
            }
        }
        else {
            GUILayout.Box(infoHistory, GUILayout.Height(200), GUILayout.Width(200));

            if (GUILayout.Button("Disconnect"))
            {
                Network.Disconnect();
                Application.Quit();
            }
        }
     }


    // server Unity functions

    void OnServerInitialized()
    {
        infoHistory += "Server succesfullly initialized\n";
    }



    //functions sending something to player

    //function sending specific card parameters to player
    public void sendCard(int player, int cardID, string cardName, string cardType, int cardHP, int cardAttack, int cardPassive, string cardDescription, int cardHealing, int cardIntercept) 
    {
        var msg = new CardMessage();
        msg.cardID = cardID;
        msg.cardName = cardName;
        msg.cardType = cardType;
        msg.cardHP = cardHP;
        msg.cardAttack = cardAttack;
        msg.cardPassive = cardPassive;
        msg.cardDescription = cardDescription;
        msg.cardHealing = cardHealing;
        msg.cardIntercept = cardIntercept;

        NetworkServer.SendToClient(player, MyMSGTypes.SendCard, msg);
    }

    //function sending information that no card was chosen
    public void noCard(int player)
    {
        var msg = new IntegerMessage();
        NetworkServer.SendToClient(player, MyMSGTypes.NoCard, msg);
    }

  
    //TODO function sending request for player to choose card for specific game, needs add a RPC
	//olal31
    //string cardType, string gameObjectName
	public void sendChooseCardRequest(int player, string cardType, string gameObjectName)
    {
        var msg = new CardRequestMessage();
        msg.cardType = cardType;
        msg.gameObjectName = gameObjectName;

        NetworkServer.SendToClient(player, MyMSGTypes.CardRequest, msg);
    }


    public void sendWinInfo(int player)
    {
        var msg = new IntegerMessage();
        NetworkServer.SendToClient(player, MyMSGTypes.Win, msg);
    }

    public void sendLoseInfo(int player)
    {
        var msg =new IntegerMessage();
        NetworkServer.SendToClient(player, MyMSGTypes.Lose, msg);
    }

	public void sendTieInfo(int player)
	{
        var msg = new IntegerMessage();
        NetworkServer.SendToClient(player, MyMSGTypes.Tie, msg);
	}

    public void sendCardMovedInfo(int player, int from, int to)
    {
        var msg = new MoveMessage();
        msg.from = from;
        msg.to = to;
        NetworkServer.SendToClient(player, MyMSGTypes.CardMoved, msg);
    }

    public void sendCardCannotBeMovedInfo(int player)
    {
        var msg = new IntegerMessage();
        NetworkServer.SendToClient(player, MyMSGTypes.cardCannotBeMoved, msg);
    }

    public void sendMovingPhaseInfo(int player)
    {
        var msg = new IntegerMessage();
        NetworkServer.SendToClient(player, MyMSGTypes.movingPhaseBegins, msg);
    }

    public void sendDrawingCardInfo(int player)
    {
        var msg = new IntegerMessage();

        NetworkServer.SendToClient(player, MyMSGTypes.drawingCards, msg);
    }

    public void waitForAnotherPlayerInfo(int player) 
    {
        var msg = new IntegerMessage();
        NetworkServer.SendToClient(player, MyMSGTypes.waitForAnotherPlayer, msg);
    }


    //RPCs received from player

    //TODO change to connect with database, add card to choose
  
    //TODO what card was chosen for game
 
	public void chosenCardForGame(string cardType, string gameObjectName, NetworkMessageInfo info)
    {
		//gameManager.chooseCard (cardType, info, gameObjectName);

    }

   public void Register(NetworkMessage msg)
    {
        var reader = msg.ReadMessage<RegisterLoginMessage>();
        List<string> data = playerData.getPlayer(reader.userData[0]);

        if (data!= null)
        {
            var newMsg = new IntegerMessage(); 
           NetworkServer.SendToClient(msg.channelId, MyMSGTypes.UsernameExists, newMsg);
           Debug.Log("usernameExists");
        }
        else
        {
            var newMsg = new IntegerMessage();

            //     dataBaseManager.addAccount(username, password, "blablaba", 1);
            NetworkServer.SendToClient(msg.channelId, MyMSGTypes.UserRegistered, newMsg);
            Debug.Log("user registered");
            //TODO choosing several random cards of heros and send RPC for player to choose

        }
        
    }

   public void moveCardRequest(NetworkMessage msg)
    {
        MoveMessage move = msg.ReadMessage<MoveMessage>();
        gameManager.moveCard(move.from, move.to, msg.channelId);
    }

    //TODO change connection to database
    void Login(NetworkMessage msg)
    {
        var reader = msg.ReadMessage<RegisterLoginMessage>();
        infoHistory += "Player "+reader.userData[0]+ " tries to log";
        List<string> playerInfo = new List<string>();
        playerInfo = playerData.getPlayer(reader.userData[0]);

        if (playerInfo == null)
        {
            var newMsg = new IntegerMessage();
            NetworkServer.SendToClient(msg.channelId, MyMSGTypes.UserNotFound, newMsg);
        }

        else
        {
            if (playerInfo[1].Equals(reader.userData[1]))
            {
                if (gameManager.playerA.login==null)
                {
                    gameManager.playerA.setLogin(reader.userData[0], msg.channelId);

                    var newMsg = new IntegerMessage();
                    NetworkServer.SendToClient(msg.channelId, MyMSGTypes.LoginSuccess, newMsg);
                    NetworkServer.SendToClient(msg.channelId, MyMSGTypes.waitForAnotherPlayer, newMsg);
                }
                else
                {
                    gameManager.playerB.setLogin(reader.userData[0], msg.channelId);

                    var newMsg = new IntegerMessage();
                    NetworkServer.SendToClient(msg.channelId, MyMSGTypes.LoginSuccess, newMsg);
                    gameManager.gameplay();
                }
     //           gameManager.gameplay();
                
            }
            else
            {
                var newMsg = new IntegerMessage();
                NetworkServer.SendToClient(msg.channelId, MyMSGTypes.WrongPassword, newMsg);
            }
                
        }

    }

    void cardAdded(NetworkMessage msg)
    {
        CardNumberMessage number = msg.ReadMessage<CardNumberMessage>();
        gameManager.setDrawingCard(number.cardNumber);
    }

    void magic(NetworkMessage msg)
    {
        CardNumberMessage number = msg.ReadMessage<CardNumberMessage>();
        gameManager.useMagicCard(number.cardNumber);
    }

    void endMovePhase(NetworkMessage msg) {

    }

  /*void OnMasterServerEvent(MasterServerEvent masterServerEvent)
    {
        if (masterServerEvent == MasterServerEvent.RegistrationSucceeded)
            infoHistory += "Registration succeeded";
    }
    */
 //   [RPC]
}
