using UnityEngine;

public class GameField : MonoBehaviour
{
    public Transform PointOfSale => _pointOfSale;
    [SerializeField] private Transform _pointOfSale;
}