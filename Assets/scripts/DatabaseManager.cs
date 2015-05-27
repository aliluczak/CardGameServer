using UnityEngine;
using System.Collections;
using System.Data;
using Mono.Data.SqliteClient;

public class DatabaseManager : MonoBehaviour {

    /*
     *class connecting to database, returning specific data, using SQL command strings, each function needs its parameters and type
     *to specify, need to specify attributes
     */

	private string _constr = "URI=file:blabla.db";
    internal IDbConnection _dbc;
    internal IDbCommand _dbcm;
    internal IDataReader _dbr;

	// Use this for initialization, initializes all needed gameobjects and components
    void Start()
    {

        string sql;
        string _constr = "URI=file:" + Application.dataPath + "/DataBase.db"; //Path to database.
        IDbConnection _dbc;
        IDbCommand _dbcm;
        IDataReader _dbr;

        _dbc = new SqliteConnection(_constr);
        _dbc.Open(); //Open connection to the database.
        _dbcm = _dbc.CreateCommand();
        sql = "SELECT Nick, Wygrane, Remisy, Przegrane FROM Gracze order by Nick desc";
        _dbcm.CommandText = sql;
        _dbr = _dbcm.ExecuteReader();

        // lista (Ranking) graczy
        while (_dbr.Read())
        {
            string Nick = _dbr.GetString(1);
            string Wygrane = _dbr.GetString(4);
            string Remisy = _dbr.GetString(5);
            string Przegrane = _dbr.GetString(6);
            //Console.WriteLine(&amp;amp;quot;Name: &amp;amp;quot; + Nick + &amp;amp;quot; &amp;amp;quot; + Wygrane);
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

    //TODO function connecting to database, probably use parameter
    internal void connectToDatabase()
    {
        _dbc = new SqliteConnection(_constr);
        _dbc.Open();
    }

    // TODO returns specific player's data
    internal string getPlayer()
    {
        return "";
    }

    //TODO returns player's cards
    internal void getPlayersCards()
    {

    }

    //TODO returns random card of specific type
    internal void chooseCardOfType()
    {

    }

    internal void addAccount()
    {

    }
}
