using UnityEngine;
using System.Collections;

public class cardHero : Card {

    internal enum CardSubType { MAGE, WARRIOR, TANK };
    internal int hp;
    internal int passive;
    internal CardSubType type;

    internal cardHero(int cardID, string cardName, CardSubType cardType, int cardHP, int cardAttack, int cardPassive)
    {
        id = cardID;
        name = cardName;
        type = cardType;
        hp = cardHP;
        attack = cardAttack;
        passive = cardPassive;
    }
}
