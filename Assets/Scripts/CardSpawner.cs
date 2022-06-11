using System.Collections.Generic;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    [SerializeField] private Card[] cards;
    [SerializeField] private CardsGrid cardsGrid;

    private List<Card> activeCards = new List<Card>();

    private void Start()
    {
        RandomSpawn();
    }

    private void RandomSpawn()
    {
        while (cardsGrid.HasEmptyCells())
        {
            foreach (var card in cards)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (!cardsGrid.HasEmptyCells())
                        return;

                    Cell cell = cardsGrid.GetRandomEmptyCell();
                    Vector3 position = cell.Position;
                    Card newCard = Instantiate(card, position, Quaternion.identity, transform);
                    newCard.Init(cell);
                    newCard.OnCardDeleteAction += OnCardDelete;
                    activeCards.Add(newCard);
                }
            }
        }
    }

    private void DeleteAllCards()
    {
        if (transform.childCount <= 0)
            return;
        
        var allCards = transform.GetComponentsInChildren<Card>();

        foreach (var card in allCards)
        {
            card.DeleteCard();
        }
        
        activeCards.Clear();
    }

    private void OnCardDelete(Card card)
    {
        activeCards.Remove(card);
    }

    public void RespawnCards()
    {
        DeleteAllCards();
        RandomSpawn();
    }

    public bool HasActiveCards()
    {
        if (activeCards.Count > 0)
            return true;

        return false;
    }
}
