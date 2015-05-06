using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class RunServer : MonoBehaviour {

    public string connectionIP = "127.0.0.1";
    public int connectionPort = 8000;
    private string infoHistory="";
    private NetworkView networkView;
    private GameObject gameManagerObject;
    private GameObject serverNetworkManager;
    private GameManager gameManager;
    

   //private string registerGameName = "alav5112021";

    void Start()
    {
        serverNetworkManager = GameObject.Find("ServerNetworkManager");
        networkView = serverNetworkManager.GetComponent<NetworkView>();
        gameManagerObject = GameObject.Find("GameManager");
        gameManager = gameManagerObject.GetComponent<GameManager>();
    }

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

    public void sendCard(NetworkMessageInfo info, int attack, int defense, string gameObjectName) 
    {
        networkView.RPC("addCard", info.sender, attack, defense, gameObjectName);
    }

    public void noCard(NetworkMessageInfo info)
    {
        networkView.RPC("noCard", info.sender);
    }

    [RPC]
    void noCard() { }
    
    [RPC]
    void addCard(int attack, int defense, string gameObjectName) { }

    [RPC]
    void Register(string username, string password, NetworkMessageInfo info)
    {

        if (!PlayerPrefs.HasKey("username"))
            PlayerPrefs.SetString("username", "");

        if (!PlayerPrefs.HasKey("password"))
            PlayerPrefs.SetString("password", "");

        string[] registeredUsernames = PlayerPrefs.GetString("username").Split();
        string[] passwords = PlayerPrefs.GetString("password").Split();
        bool notRegistered = true;
        foreach (string s in registeredUsernames)
        {
            if (s.Equals(username))
            {   
                networkView.RPC("usernameExists", info.sender);
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
            networkView.RPC("userRegistered", info.sender);
        }
        
    }

    [RPC]
    void Login(string username, string password, NetworkMessageInfo info)
    {
        string[] usernames;
        string[] passwords ;
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
            networkView.RPC("userNotFound", info.sender);
        }

        passwords = PlayerPrefs.GetString("password").Split();

        if (!passwords[lineIndex].Equals(password))
        {
            networkView.RPC("wrongPassword", info.sender);
        }
        else
        {
            networkView.RPC("loginSuccess", info.sender);
        }

    }

    [RPC]
    void cardRequest(string cardType, string gameObjectName, NetworkMessageInfo info)
    {
        gameManager.chooseCard(cardType, info, gameObjectName);
    }

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

  /*void OnMasterServerEvent(MasterServerEvent masterServerEvent)
    {
        if (masterServerEvent == MasterServerEvent.RegistrationSucceeded)
            infoHistory += "Registration succeeded";
    }
    */
 //   [RPC]
}
