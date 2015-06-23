using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Card : MonoBehaviour {

    internal int id;
    internal string name;
    internal enum CardType { HERO, SPELL};
	//mage, tank & warrior concern Hero
	//common concerns Booster & Skill
	internal enum CardSubType {MAGE, WARRIOR, TANK};
    internal int attack;

    internal Card()
    {

    }

}
