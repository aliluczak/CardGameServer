using UnityEngine;
using System.Collections;

public class cardHero : Card {

    internal int hp;
    internal int passive;
    internal CardSubType type;
    private int p1;
    private string p2;
    private int p3;
    private int p4;
    private int p5;

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
