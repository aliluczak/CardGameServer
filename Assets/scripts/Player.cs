using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

    CardData cardData;

    internal string login;
    internal int hp;
    internal List<string> personalCards;
    internal List<string> commonCards;
    internal NetworkMessageInfo playerMessage;



    public Player(string playersLogin, NetworkMessageInfo info)
    {
        this.login = playersLogin;
        this.hp = 10;
        this.playerMessage = info;

        //gets personal cards
        this.personalCards = new List<string>();
        this.commonCards = new List<string>();

    }

    public Player()
    {
        this.hp = 10;
        this.commonCards = new List<string>();
    }

    public void setLogin(string login, NetworkMessageInfo info)
    {
        this.login = login;
        this.playerMessage = info;
    }

    
}
