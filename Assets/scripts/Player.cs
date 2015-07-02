using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

    internal string login;
    internal int hp;
    internal List<string> personalCards;
    internal List<string> commonCards;
    internal List<string> cemetary;
    internal NetworkMessageInfo playerMessage;

    public Player(string playersLogin, NetworkMessageInfo info)
    {
        this.login = playersLogin;
        this.hp = 10;
        this.playerMessage = info;

        //gets personal cards
        this.personalCards = new List<string>();
        this.commonCards = new List<string>();
        this.cemetary = new List<string>();
    }
}
