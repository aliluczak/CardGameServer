using UnityEngine;
using System.Collections;
using System.Data;
using Mono.Data.SqliteClient;

public class DatabaseManager : MonoBehaviour {

    /*
     *class connecting to database, returning specific data, using SQL command strings, each function needs its parameters and type
     *to specify, need to specify attributes
     */

    private string _constr = "URI=file" + Application.dataPath;
    internal IDbConnection _dbc;
    internal IDbCommand _dbcm;
    internal IDataReader _dbr;

	// Use this for initialization, initializes all needed gameobjects and components
	void Start () {
	
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
