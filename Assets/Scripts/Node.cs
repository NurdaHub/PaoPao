using UnityEngine;

public class Node
{
    public Vector2 Position { get; }
    public Vector2 TargetPosition { get; }
    public Vector2 Direction { get; }
    public Node PreviousNode { get; }
    public int F { get; } // F=G+H
    public int DistanceFromStart { get; }

    public Node(int distanceFromStart, Vector2 nodePosition, Vector2 targetPosition, Vector2 direction, Node previousNode)
    {
        Position = nodePosition;
        TargetPosition = targetPosition;
        Direction = direction;
        PreviousNode = previousNode;
        DistanceFromStart = distanceFromStart;
        var distanceToTarget = (int)Mathf.Abs(targetPosition.x - Position.x) + (int)Mathf.Abs(targetPosition.y - Position.y);
        F = DistanceFromStart + distanceToTarget;
    }
}