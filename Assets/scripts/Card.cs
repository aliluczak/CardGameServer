using UnityEngine;
using System.Collections;
using System.Collections.Generic;

internal class Card : MonoBehaviour {

    internal string name;
    internal enum CardType { HERO, BOOSTER, SKILL };
    internal List<CardType> cardType;
    internal List<int> attack;
    internal List<int> defense;
}
