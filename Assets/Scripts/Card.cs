using System;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private int id;
    
    private Cell cell;
    
    public event Action<Card> OnCardDeleteAction;

    public void Init(Cell _cell)
    {
        cell = _cell;
        SetCellEmpty(false);
    }

    public int GetId()
    {
        return id;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void OnCardSelect()
    {
        PathFinder.Instance.SetCoupleOfCard(this);
    }

    public void DeleteCard()
    {
        OnCardDeleteAction?.Invoke(this);
        Destroy(gameObject);
    }

    public void SetCellEmpty(bool isEmpty)
    {
        cell.IsEmpty = isEmpty;
    }
}