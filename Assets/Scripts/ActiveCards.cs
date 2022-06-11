using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveCards : MonoBehaviour
{
    private List<Card> allCards = new List<Card>();

    public void SetAllCards(List<Card> cards)
    {
        allCards = cards;
    }

    public List<Card> GetActiveCards()
    {
        List<Card> activeCards = new List<Card>();

        foreach (var card in allCards)
            if (card.gameObject.activeInHierarchy)
                activeCards.Add(card);

        return activeCards;
    }
}
