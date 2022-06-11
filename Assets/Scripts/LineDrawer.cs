using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    [SerializeField] private LineRenderer line;

    public void Draw(Vector2[] positions)
    {
        line.positionCount = positions.Length;
        
        for (int i = 0; i < positions.Length; i++)
            line.SetPosition(i, positions[i]);
    }

    public void DeleteLine()
    {
        line.positionCount = 0;
    }
}
