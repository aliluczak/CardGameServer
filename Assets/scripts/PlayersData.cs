using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class PlayersData : MonoBehaviour {

    List<List<string>> data;
    List<string> cards;
    private RunServer server;
 
	// Use this for initialization
	void Start () {
        data = new List<List<string>>();
        cards = new List<string>();
        server = GameObject.Find("ServerNetworkManager").GetComponent<RunServer>();
        addData();
	}

    void addData()
    {
        List<string>  player= new List<string>();
        List<string> player2 = new List<string>();
        player2.Add("ala");
        player2.Add("cos");
        player2.Add("12");
        player2.Add("14");
        player2.Add("16");
        player2.Add("18");
        data.Add(player2);
        player.Clear();

        player.Add("ola");
        player.Add("cos");
        player.Add("13");
        player.Add("14");
        player.Add("15");
        player.Add("17");
        data.Add(player);

    }
   
    public List<string> getPlayer(string username)
    {
        List<string> player = (from sublist in data
                               from item in sublist
                               where item.Equals(username)
                               select sublist).FirstOrDefault();
        return player;
    }

    public List<string> getPlayerCards(string username)
    {
        List<string> cards = new List<string>();
        List<string> player = (from sublist in data
                               from item in sublist
                               where item.Equals(username)
                               select sublist).FirstOrDefault();

        cards.Add(player[2]);
        cards.Add(player[3]);
        cards.Add(player[4]);
        cards.Add(player[5]);

        return cards;
    }

	

}
