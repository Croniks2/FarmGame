using UnityEngine;

public class FieldBlock : MonoBehaviour
{
    [SerializeField] private BedCell _bedCell;

    public void Collect()
    {
        _bedCell.Grow(120f);
    }
}