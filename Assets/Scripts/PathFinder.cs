using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder : Singleton<PathFinder>
{
    [SerializeField] private CardsGrid cardGrid;
    [SerializeField] private LineDrawer lineDrawer;
    
    private Card firstCard;
    private Card secondCard;
    private List<Vector2> pathToTarget;
    private List<Node> checkedNodes = new List<Node>();
    private List<Node> waitingNodes = new List<Node>();

    public event Action OnCardsDeletedEvent;

    private void FindPath()
    {
        var currentPath = GetPath(firstCard.GetPosition(), secondCard.GetPosition());
        
        if (currentPath == null || currentPath.Count <= 0)
        {
            var reversedPath = GetPath(secondCard.GetPosition(), firstCard.GetPosition());

            if (reversedPath == null || reversedPath.Count <= 0)
            {
                SetCellsEmpty(false);
                firstCard = null;
                secondCard = null;
                return;   
            }

            currentPath = reversedPath;
        }
        if (currentPath != null)
            lineDrawer.Draw(currentPath.ToArray());
        
        DeleteCoupleOfCard();
    }

    public void SetCoupleOfCard(Card currentCard)
    {
        if (CanBeFirstCard(currentCard))
        {
            firstCard = currentCard;
            return;
        }
        else if (CanBeSecondCard(currentCard))
        {
            secondCard = currentCard;
            SetCellsEmpty(true);

            if (IsNeighbours())
            {
                Vector2[] positions = 
                {
                    firstCard.GetPosition(),
                    secondCard.GetPosition()
                };
                
                lineDrawer.Draw(positions);
                DeleteCoupleOfCard();
                
                return;
            }
            
            FindPath();
            return;
        }

        firstCard = null;
    }

    private void SetCellsEmpty(bool isEmpty)
    {
        firstCard.SetCellEmpty(isEmpty);
        secondCard.SetCellEmpty(isEmpty);
    }

    private void DeleteCoupleOfCard()
    {
        firstCard.DeleteCard();
        secondCard.DeleteCard();
        
        OnCardsDeletedEvent?.Invoke();
    }

    private bool IsNeighbours()
    {
        Vector2[] neighboursPos = GetNeighboursPosition(firstCard.GetPosition());

        foreach (var currentPos in neighboursPos)
        {
            if (currentPos == (Vector2)secondCard.GetPosition())
                return true;
        }

        return false;
    }

    private bool CanBeFirstCard(Card currentCard)
    {
        return firstCard == null || firstCard.GetPosition() != currentCard.GetPosition() && firstCard.GetId() != currentCard.GetId();
    }
    
    private bool CanBeSecondCard(Card currentCard)
    {
        return firstCard.GetPosition() != currentCard.GetPosition() && firstCard.GetId() == currentCard.GetId();
    }

    private List<Vector2> GetPath(Vector2 start, Vector2 target)
    {
        pathToTarget = new List<Vector2>();
        checkedNodes = new List<Node>();
        waitingNodes = new List<Node>();
        Vector2 startPosition = new Vector2(Mathf.Round(start.x), Mathf.Round(start.y));
        Vector2 targetPosition = new Vector2(Mathf.Round(target.x), Mathf.Round(target.y));
        
        if(startPosition == targetPosition) 
            return pathToTarget;

        Node startNode = new Node(0, startPosition, targetPosition, Vector2.zero,  null);
        checkedNodes.Add(startNode);
        waitingNodes.AddRange(GetNeighbourNodes(startNode));
        
        while(waitingNodes.Count > 0)
        {
            Node nodeToCheck = GetClosestNode();

            if (nodeToCheck.Position == targetPosition)
            {
                return CalculatePathFromNode(nodeToCheck);
            }
            
            waitingNodes.Remove(nodeToCheck);
            
            if (!checkedNodes.Where(x => x.Position == nodeToCheck.Position).Any()) 
            {
                checkedNodes.Add(nodeToCheck);
                waitingNodes.AddRange(GetNeighbourNodes(nodeToCheck));
            }
        }
        
        return pathToTarget;
    }

    private Node GetClosestNode()
    {
        int min = Int32.MaxValue;
        Node closestNode = null;
        
        foreach (var node in waitingNodes)
        {
            if (node.F < min)
            {
                min = node.F;
                closestNode = node;
            }
            else if (node.F == min && node.Direction == node.PreviousNode.Direction)
            {
                closestNode = node;
            }
        }
        
        return closestNode;
    }

    private List<Vector2> CalculatePathFromNode(Node node)
    {
        var path = new List<Vector2>();
        Node currentNode = node;
        int turnsCount = 0;

        while(currentNode.PreviousNode != null)
        {
            if (currentNode.Direction != currentNode.PreviousNode.Direction)
                turnsCount++;

            if (turnsCount > 3)
                return null;
            
            path.Add(new Vector2(currentNode.Position.x, currentNode.Position.y));
            currentNode = currentNode.PreviousNode;
        }
        
        path.Add(new Vector2(currentNode.Position.x, currentNode.Position.y));
        return path;
    }

    private List<Node> GetNeighbourNodes(Node node)
    {
        var neighbours = new List<Node>();
        var newDistanceFromStart = node.DistanceFromStart + 1;
        Vector2[] neighboursPosition = GetNeighboursPosition(node.Position);
        Vector2 right = neighboursPosition[0];
        Vector2 left = neighboursPosition[1];
        Vector2 up = neighboursPosition[2];
        Vector2 down = neighboursPosition[3];

        if (CanBeNeighbour(right))
            neighbours.Add(new Node(newDistanceFromStart, right, node.TargetPosition, Vector2.right, node));
        
        if (CanBeNeighbour(left))
            neighbours.Add(new Node(newDistanceFromStart, left, node.TargetPosition, Vector2.left, node));
        
        if (CanBeNeighbour(up))
            neighbours.Add(new Node(newDistanceFromStart, up, node.TargetPosition, Vector2.up, node));
        
        if (CanBeNeighbour(down))
            neighbours.Add(new Node(newDistanceFromStart, down, node.TargetPosition, Vector2.down, node));
        
        return neighbours;
    }

    private Vector2[] GetNeighboursPosition(Vector2 position)
    {
        Vector2[] neighbours =
        {
            new Vector2(position.x + 1, position.y),
            new Vector2(position.x - 1, position.y),
            new Vector2(position.x, position.y + 1),
            new Vector2(position.x, position.y - 1)
        };

        return neighbours;
    }

    private bool CanBeNeighbour(Vector2 position)
    {
        return cardGrid.IsCellEmpty(position) || cardGrid.IsBorder(position) || position == (Vector2)secondCard.GetPosition();
    }
}