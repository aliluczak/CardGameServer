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
    

   //private string registerGameName = "alav5112021";

    //initilizes all needed gameobjects and components
    void Start()
    {
        serverNetworkManager = GameObject.Find("ServerNetworkManager");
        serverNetworkView = serverNetworkManager.GetComponent<NetworkView>();
        gameManagerObject = GameObject.Find("GameManager");
        gameManager = gameManagerObject.GetComponent<GameManager>();
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
    void lose() {}

	[RPC]
	void tie() {}
	
	




    //RPCs received from player

    //TODO change to connect with database, add card to choose
	
	//received card request from player
    [RPC]
    void cardRequest(string cardType, string gameObjectName, NetworkMessageInfo info)
    {
        gameManager.chooseCard(cardType, info, gameObjectName);
    }
    //TODO what card was chosen for game
 
    [RPC]
	void chosenCardForGame(string cardType, string gameObjectName, NetworkMessageInfo info)
    {
		//gameManager.chooseCard (cardType, info, gameObjectName);

    }
    [RPC]
    void Register(string username, string password, NetworkMessageInfo info)
    {

        if (!PlayerPrefs.HasKey("username"))
            PlayerPrefs.SetString("username", "");

        if (!PlayerPrefs.HasKey("password"))
            PlayerPrefs.SetString("password", "");

        string[] registeredUsernames = PlayerPrefs.GetString("username").Split();
        bool notRegistered = true;
        foreach (string s in registeredUsernames)
        {
            if (s.Equals(username))
            {
                serverNetworkView.RPC("usernameExists", info.sender);
                notRegistered = false;
                break;
            }
           
        }

        if (notRegistered)
        {
            string newUsername = PlayerPrefs.GetString("username");
            newUsername += username + "\n";
            string newPassword = PlayerPrefs.GetString("password");
            newPassword += password + "\n";

            PlayerPrefs.SetString("username", newUsername);
            PlayerPrefs.SetString("password", newPassword);
            serverNetworkView.RPC("userRegistered", info.sender);


            //TODO choosing several random cards of heros and send RPC for player to choose

        }
        
    }


    //TODO change connection to database
    [RPC]
    void Login(string username, string password, NetworkMessageInfo info)
    {
        string[] usernames;
        string[] passwords;
        int lineIndex = 0;
        bool userNotFound = true;

        usernames = PlayerPrefs.GetString("username").Split();
        
        foreach (string s in usernames)
        { 
            if (s.Equals(username)) {
                userNotFound = false;
                break;
            }
           
                lineIndex++;
        }

        if (userNotFound)
        {
            serverNetworkView.RPC("userNotFound", info.sender);
        }

        passwords = PlayerPrefs.GetString("password").Split();

        if (!passwords[lineIndex].Equals(password))
        {
            serverNetworkView.RPC("wrongPassword", info.sender);
        }
        else
        {
            serverNetworkView.RPC("loginSuccess", info.sender);
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
