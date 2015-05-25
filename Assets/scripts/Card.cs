using UnityEngine;
using System.Collections;
using System.Collections.Generic;

internal class Card : MonoBehaviour {

    internal string name;
    internal enum CardType { HERO, BOOSTER, SKILL};
	//mage, tank & warrior concern Hero
	//common concerns Booster & Skill
	internal enum CardSubType {MAGE, TANK, WARRIOR, COMMON};
    internal List<CardType> cardType;
    internal List<int> attack;
    internal List<int> defense;
}
