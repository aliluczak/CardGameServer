using UnityEngine;
using System.Collections;
using System.Data;
using System;
using Mono.Data.SqliteClient;
using System.Collections.Generic;


public class DatabaseManager : MonoBehaviour
{

    /*
     *class connecting to database, returning specific data, using SQL command strings, each function needs its parameters and type
     *to specify, need to specify attributes
     */

    private string _constr = "URI=file:database.db";
    internal IDbConnection _dbc;
    internal IDbCommand _dbcm;
    internal IDataReader _dbr;
    internal List<int?> card_list;
    internal List<string> card_info;
    internal List<string> player_info;
    internal GameObject card;
    internal cardHero hero;
    internal cardSpell spell;
    internal Card.CardSubType sub_type;
    private string Class;
    
    //TODO function connecting to database, probably use parameter
    internal void connectToDatabase()
    {
        _dbc = new SqliteConnection(_constr);
        _dbc.Open();
    }


    void Start()
    {
        card_list = new List<int?>();
        card = GameObject.Find("GameManager");
        hero = card.GetComponent<cardHero>();
        spell = card.GetComponent<cardSpell>();
    }




    // TODO returns specific player's data
    internal List<string> getPlayer(string player)
    {
        string sql;
        string _constr = "URI=file:/dataBase.db"; //Path to database.


        _dbc = new SqliteConnection(_constr);
        _dbc.Open(); //Open connection to the database.
        _dbcm = _dbc.CreateCommand();
        sql = "SELECT * FROM Gracze WHERE Nick = '" + player + "' ";
        _dbcm.CommandText = sql;
        _dbr = _dbcm.ExecuteReader();

        while (_dbr.Read())
        {

            string Nick = _dbr.GetString(0);
            player_info.Add(Nick);

            string password = _dbr.GetString(1);
            player_info.Add(password);

            string Email = _dbr.GetString(2);
            player_info.Add(Email);

            Console.WriteLine("Nick:" + Nick + "Email:" + Email);
            Debug.Log(Nick + Email);
            //GUI.Box(new Rect(Screen.width - 270, Screen.height - 55, 260, 30), "Copyright" + player + "2014");
        }



        // clean up
        _dbr.Close();
        _dbr = null;
        _dbcm.Dispose();
        _dbcm = null;
        _dbc.Close();
        _dbc = null;

        return player_info;
    }


    // Use this for initialization, initializes all needed gameobjects and components
    void getRanking(string player)
    {
        //" + Application.dataPath + "
        string sql;
        string _constr = "URI=file:/dataBase.db"; //Path to database.
        //IDbConnection _dbc;
        //IDbCommand _dbcm;
        //IDataReader _dbr;

        _dbc = new SqliteConnection(_constr);
        _dbc.Open(); //Open connection to the database.
        _dbcm = _dbc.CreateCommand();
        sql = "SELECT Nick, Wygrane, Remisy, Przegrane FROM Gracze WHERE Nick = '" + player + "' ;";
        _dbcm.CommandText = sql;
        _dbr = _dbcm.ExecuteReader();

        // lista (Ranking) graczy
        while (_dbr.Read())
        {
            string Nick = _dbr.GetString(1);
            string Wygrane = _dbr.GetString(4);
            string Remisy = _dbr.GetString(5);
            string Przegrane = _dbr.GetString(6);
            Console.WriteLine("Nick:" + Nick + "Wygrane:" + Wygrane + "Remisy:" + Remisy + "Przegrane:" + Przegrane);
            Debug.Log(Nick + Wygrane + Remisy + Przegrane);
        }

        // clean up
        _dbr.Close();
        _dbr = null;
        _dbcm.Dispose();
        _dbcm = null;
        _dbc.Close();
        _dbc = null;

    }



    //TODO returns player's cards
    internal List<int?> getPlayersCards(string player)
    {
        string sql;
        string _constr = "URI=file:/dataBase.db"; //Path to database.


        _dbc = new SqliteConnection(_constr);
        _dbc.Open(); //Open connection to the database.
        _dbcm = _dbc.CreateCommand();
        sql = "SELECT * FROM Gracze WHERE Nick = '" + player + "' ;";
        _dbcm.CommandText = sql;
        _dbr = _dbcm.ExecuteReader();


        // player
        while (_dbr.Read())
        {
            //string Nick = _dbr.GetString(0);
            int? Karta1 = _dbr.GetInt16(3);
            card_list.Add(Karta1);
            int? Karta2 = _dbr.GetInt16(4);
            card_list.Add(Karta2);
            int? Karta3 = _dbr.GetInt16(5);
            card_list.Add(Karta3);
            int? Karta4 = _dbr.GetInt16(6);
            card_list.Add(Karta4);
            int? Karta5 = _dbr.GetInt16(7);
            card_list.Add(Karta5);
            int? Karta6 = _dbr.GetInt16(8);
            card_list.Add(Karta6);
            int? Karta7 = _dbr.GetInt16(9);
            card_list.Add(Karta7);
            int? Karta8 = _dbr.GetInt16(10);
            card_list.Add(Karta8);
            int? Karta9 = _dbr.GetInt16(11);
            card_list.Add(Karta9);
            int? Karta10 = _dbr.GetInt16(12);
            card_list.Add(Karta10);

            Console.WriteLine("Karta nr1:" + Karta1 + "Karta nr2:" + Karta2 + "Karta nr3:" + Karta3
                + "Karta nr4:" + Karta4 + "Karta nr5:" + Karta5 + "Karta nr6:" + Karta6 + "Karta nr7:" + Karta7 + "Karta nr8:" + Karta8
                + "Karta nr9:" + Karta9 + "Karta nr10:" + Karta10);
            Debug.Log(Karta1 + Karta2 + Karta3 + Karta4 + Karta5 + Karta6 + Karta7 + Karta8 + Karta9 + Karta10);
        }

        card_list.RemoveAll(i => i == null);

        // clean up
        _dbr.Close();
        _dbr = null;
        _dbcm.Dispose();
        _dbcm = null;
        _dbc.Close();
        _dbc = null;

        return card_list;
    }



    //TODO returns random card of specific type
    internal List<string> getCard(Card.CardType type, string index)
    {

        string _constr = "URI=file:/dataBase.db"; //Path to database.


        _dbc = new SqliteConnection(_constr);
        _dbc.Open();
        _dbcm = _dbc.CreateCommand();

        if (type == Card.CardType.HERO)
        {
            _dbcm.CommandText = "SELECT * FROM Postacie WHERE NAZWA = '" + index + "'";
            _dbr = _dbcm.ExecuteReader();


            while (_dbr.Read()) // Read() returns true if there is still a result line to read
            {

                //System.Console.WriteLine(_dbr["text"]);
                //int ID = _dbr.GetInt16(0);
                string Nazwa = _dbr.GetString(0);
                card_info.Add(Nazwa);
                string Klasa = _dbr.GetString(2);
                card_info.Add(Klasa);
                string HP = _dbr.GetString(3);
                card_info.Add(HP);
                string DMG = _dbr.GetString(4);
                card_info.Add(DMG);
                string Bierne = _dbr.GetString(5);
                card_info.Add(Bierne);
                string Skill = _dbr.GetString(6);
                //card_info.Add(Skill);
                string Ulti = _dbr.GetString(7);
                //card_info.Add(Ulti);
                string Koszt = _dbr.GetString(8);
                //card_info.Add(Koszt);

                Console.WriteLine("Nazwa:" + Nazwa + "Klasa:" + Klasa + "HP:" + HP + "DMG:" + DMG + "Bierne:" + Bierne + "Skill:" + Skill + "Ulti:" + Ulti + "Koszt:" + Koszt);
                Debug.Log(Nazwa + Klasa + HP + DMG + Bierne + Skill + Ulti + Koszt);

            }

        }

        _dbr.Close();
        _dbr = null;


        if (type == Card.CardType.SPELL)
        {
            _dbcm.CommandText = "SELECT * FROM Czary WHERE ID = '" + index + "'";
            _dbr = _dbcm.ExecuteReader();


            while (_dbr.Read()) // Read() returns true if there is still a result line to read
            {

                //System.Console.WriteLine(_dbr["text"]);
                int ID = _dbr.GetInt16(0);
                card_info.Add(ID.ToString());
                string Nazwa = _dbr.GetString(1);
                card_info.Add(Nazwa);
                string Opis = _dbr.GetString(2);
                card_info.Add(Opis);
                string Atak = _dbr.GetString(3);
                card_info.Add(Atak);
                string Leczenie = _dbr.GetString(4);
                card_info.Add(Leczenie);
                string Przechwycenie = _dbr.GetString(5);
                card_info.Add(Przechwycenie);


                Console.WriteLine("Nazwa:" + Nazwa + "Opis:" + Opis + "Atak:" + Atak + "Leczenie:" + Leczenie + "Przechwycenie:" + Przechwycenie);
                Debug.Log(Nazwa + Opis + Atak + Leczenie + Przechwycenie);

            }

        }

        // clean up
        _dbr.Close();
        _dbr = null;
        _dbcm.Dispose();
        _dbcm = null;
        _dbc.Close();
        _dbc = null;

        return card_info;
    }


    internal void addAccount(string name, string pass, string email, int card)
    {
        string sql;
        string _constr = "URI=file:database.db";
        //IDbConnection _dbc;
        //IDbCommand _dbcm;
        //IDataReader _dbr;

        _dbc = new SqliteConnection(_constr);
        _dbc.Open();
        _dbcm = _dbc.CreateCommand();
        sql = "INSERT INTO Gracze (Nick, Haslo, Mail, Karta1_ID) VALUES ('" + name + "','" + pass + "','" + email + "','" + card + "');";
        _dbcm.CommandText = sql;
        _dbcm.ExecuteNonQuery();

        //_dbr = _dbcm.ExecuteReader();  //niepotrzebne
        //account = new account{GraczID = gracz, Nick = name, Mail = email, Wygrane = win, Remisy = draw, Przegrane = loss, Karta1_ID}

    }

    internal Card.CardSubType getHeroClass(int i)
    {

        string sql;
        string _constr = "URI=file:/dataBase.db"; //Path to database.


        _dbc = new SqliteConnection(_constr);
        _dbc.Open(); //Open connection to the database.
        _dbcm = _dbc.CreateCommand();
        sql = "SELECT * FROM Klasa WHERE Id = '" + i + "' ";
        _dbcm.CommandText = sql;
        _dbr = _dbcm.ExecuteReader();

        while (_dbr.Read())
        {
            Class = _dbr.GetString(1);


            Console.WriteLine("Class:" + Class);
            Debug.Log(Class);

        }

        // clean up
        _dbr.Close();
        _dbr = null;
        _dbcm.Dispose();
        _dbcm = null;
        _dbc.Close();
        _dbc = null;


        switch (Class)
        {
            case "Mag":
                sub_type = Card.CardSubType.MAGE;
                break;
            case "Wojownik":
                sub_type = Card.CardSubType.WARRIOR;
                break;
            case "Tank":
                sub_type = Card.CardSubType.TANK;
                break;

        }

        return sub_type;
    }


}




//string _constr = "URI=file:" + Application.dataPath + "/dataBase.db";
//sqlite_conn = new SQLiteConnection("Data Source=database.db;Version=3;New=True;Compress=True;");