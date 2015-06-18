using UnityEngine;
using System.Collections;

public class cardSpell :  Card {

    internal string description;
    internal int healing;
    internal int intercept;

    public cardSpell(int idCard, string cardName, string cardDescription, int cardAttack, int cardHealing, int cardIntercept)
    {
        id = idCard;
        name = cardName;
        description = cardDescription;
        attack = cardAttack;
        healing = cardHealing;
        intercept = cardIntercept;
    }


}
