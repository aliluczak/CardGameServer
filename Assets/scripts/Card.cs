using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Card : MonoBehaviour {

    public string name;
    public enum CardType { HERO, BOOSTER, SKILL };
    public List<CardType> cardType;
    public List<int> attack;
    public List<int> defense;
}
