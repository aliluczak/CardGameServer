using UnityEngine;
using System.Collections;

public class cardSpell :  Card {

    internal string description;
    internal int healing;
    internal int intercept;

    public cardSpell(int idCard, string cardName, string cardDescription, int cardAttack, int cardHealing, int cardIntercept)
    {
        this.id = idCard;
        this.name = cardName;
        this.description = cardDescription;
        this.attack = cardAttack;
        this.healing = cardHealing;
        this.intercept = cardIntercept;
    }


}
