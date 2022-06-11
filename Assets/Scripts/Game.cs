using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private CardSpawner cardSpawner;
    [SerializeField] private CardsGrid cardsGrid;
    [SerializeField] private LineDrawer lineDrawer;
    [SerializeField] private PathFinder pathFinder;

    private void Start()
    {
        pathFinder.OnCardsDeletedEvent += OnCardsDeleted;
    }

    private void OnDisable()
    {
        pathFinder.OnCardsDeletedEvent -= OnCardsDeleted;
    }

    private void OnCardsDeleted()
    {
        if (cardSpawner.HasActiveCards())
            return;
        
        RestartGame();
    }

    public void RestartGame()
    {
        lineDrawer.DeleteLine();
        cardsGrid.ResetCells();
        cardSpawner.RespawnCards();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
