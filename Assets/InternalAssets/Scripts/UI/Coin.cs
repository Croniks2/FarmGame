using UnityEngine;

public class Coin : MonoBehaviour
{
    public RectTransform RectTransform => _rectTransform;
    [SerializeField] private RectTransform _rectTransform;

    private Vector3 _initialPosition;

    private void Awake()
    {
        _initialPosition = _rectTransform.position;

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _rectTransform.position = _initialPosition;
    }
}