using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class RunServer : MonoBehaviour {

    public string connectionIP = "127.0.0.1";
    public int connectionPort = 8000;
    private string infoHistory="";
    private NetworkView serverNetworkView;
    private GameObject gameManagerObject;
    private GameObject serverNetworkManager;
    private GameManager gameManager;
    private GameObject dataBaseObject;
    private DatabaseManager dataBaseManager;
    

   //private string registerGameName = "alav5112021";

    //initilizes all needed gameobjects and components
    void Start()
    {
        serverNetworkManager = GameObject.Find("ServerNetworkManager");
        serverNetworkView = serverNetworkManager.GetComponent<NetworkView>();
        gameManagerObject = GameObject.Find("GameManager");
        gameManager = gameManagerObject.GetComponent<GameManager>();
        dataBaseObject = GameObject.Find("Database");
        dataBaseManager = dataBaseObject.GetComponent<DatabaseManager>();
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

    void OnPlayerConnected(NetworkPlayer player)
    {
        infoHistory += "Player succesfully connected from " + player.ipAddress + "\n";

    }

    void OnPlayerDisconnected(NetworkPlayer player)
    {
        infoHistory += "Player succesfully disconnected from " +player.ipAddress + "\n";
    }


    //functions sending something to player

    //function sending specific card parameters to player
    public void sendCard(NetworkMessageInfo info, int attack, int defense, string gameObjectName) 
    {
        serverNetworkView.RPC("addCard", info.sender, attack, defense, gameObjectName);
    }

    //function sending information that no card was chosen
    public void noCard(NetworkMessageInfo info)
    {
        serverNetworkView.RPC("noCard", info.sender);
    }

    //TODO function sending request for player to choose card for specific game, needs add a RPC
	//olal31
	public void sendChooseCardRequest(NetworkMessageInfo info, string cardType, string gameObjectName)
    {
		serverNetworkView.RPC ("cardRequest", info.sender, cardType, gameObjectName);
    }

    public void sendWinInfo(NetworkMessageInfo info)
    {
        serverNetworkView.RPC("win", info.sender);
    }

    public void sendLoseInfo(NetworkMessageInfo info)
    {
        serverNetworkView.RPC("lose", info.sender);
    }

	public void sendTieInfo(NetworkMessageInfo info)
	{
		serverNetworkView.RPC("tie", info.sender);
	}

    public void sendCardMovedInfo(NetworkMessageInfo info, int from, int to)
    {
        serverNetworkView.RPC("cardMoved", info.sender, from, to);
    }

    public void sendCardCannotBeMovedInfo(NetworkMessageInfo info)
    {
        serverNetworkView.RPC("cardCannotBeMoved", info.sender);
    }


    //RPCs sent to player
    [RPC]
    void noCard() { }
    
    [RPC]
    void addCard(int attack, int defense, string gameObjectName) { }

    [RPC]
    void userRegistered() { }

    [RPC]
    void usernameExists() { }

    [RPC]
    void userNotFound() { }

    [RPC]
    void wrongPassword() { }

    [RPC]
    void loginSuccess() { }

    [RPC]
    void win() {}

    [RPC]
    void cardMoved() { }

    [RPC]
    void cardCannotBeMoved() { }

	
	




    //RPCs received from player

    //TODO change to connect with database, add card to choose
	
  
    //TODO what card was chosen for game
 
    [RPC]
	void chosenCardForGame(string cardType, string gameObjectName, NetworkMessageInfo info)
    {
		//gameManager.chooseCard (cardType, info, gameObjectName);

    }
    [RPC]
    void Register(string username, string password, NetworkMessageInfo info)
    {
        bool notRegistered = true;
        List<string> data = dataBaseManager.getPlayer(username);

        if (data!= null)
        {
            serverNetworkView.RPC("usernameExists", info.sender);
            notRegistered = false;
            Debug.Log("usernameExists");
        }
        else
        {
    
       //     dataBaseManager.addAccount(username, password, "blablaba", 1);
            serverNetworkView.RPC("userRegistered", info.sender);
            Debug.Log("user registered");
            //TODO choosing several random cards of heros and send RPC for player to choose

        }
        
    }

    [RPC]
    void moveCard(int from, int to, NetworkMessageInfo info)
    {
        gameManager.moveCard(from, to, info);
    }

    //TODO change connection to database
    [RPC]
    void Login(string username, string password, NetworkMessageInfo info)
    {
        List<string> playerInfo = new List<string>();
        playerInfo = dataBaseManager.getPlayer(username);

        if (playerInfo == null)
        {
            serverNetworkView.RPC("userNotFound", info.sender);
        }

        else
        {
            if (playerInfo[1].Equals(password))
            {
                if (gameManager.playerA = null)
                    gameManager.playerA = new Player(username, info);
                else
                {
                    gameManager.playerB = new Player(username, info);
                    serverNetworkView.RPC("loginSuccess", info.sender);
                    gameManager.gameplay();
                }
     //           gameManager.gameplay();
                
            }
            else
                serverNetworkView.RPC("wrongPassword", info.sender);
        }

    }

  /*void OnMasterServerEvent(MasterServerEvent masterServerEvent)
    {
        if (masterServerEvent == MasterServerEvent.RegistrationSucceeded)
            infoHistory += "Registration succeeded";
    }
    */
 //   [RPC]
}
