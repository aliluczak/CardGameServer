using UnityEngine;
using System.Collections;

public class cardHero : Card {

    internal int hp;
    internal int passive;
    internal CardSubType type;

    internal cardHero(int cardID, string cardName, CardSubType cardType, int cardHP, int cardAttack, int cardPassive)
    {
        this.id = cardID;
        this.name = cardName;
        this.type = cardType;
        this.hp = cardHP;
        this.attack = cardAttack;
        this.passive = cardPassive;
    }

}
