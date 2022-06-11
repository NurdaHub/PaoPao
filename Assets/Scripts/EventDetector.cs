using UnityEngine;

public class EventDetector : MonoBehaviour
{
    [SerializeField] private Card card;

    private void OnMouseDown()
    {
        card.OnCardSelect();
    }
}
