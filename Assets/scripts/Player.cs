using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

    CardData cardData;

    internal string login;
    internal int hp;
    internal List<string> personalCards;
    internal List<string> commonCards;
    internal int channel;



    public Player(string playersLogin, int channel)
    {
        this.login = playersLogin;
        this.hp = 10;
        this.channel = channel;

        //gets personal cards
        this.personalCards = new List<string>();
        this.commonCards = new List<string>();

    }

    public Player()
    {
        this.hp = 10;
        this.commonCards = new List<string>();
    }

    public void setLogin(string login, int channel)
    {
        this.login = login;
        this.channel = channel;
    }

    
}
