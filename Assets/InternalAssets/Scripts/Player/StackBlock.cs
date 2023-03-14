using UnityEngine;

public class StackBlock : MonoBehaviour
{
    public int ID { get; private set; }

    private PlayerBlockStack _playerBlockStack;

    private Vector3 _initialLocalPosition;
    private Quaternion _initialLocalRotation;
    private Vector3 _initialLocalScalce;

    public bool IsActive { get; private set; }

    public void Setup(int id, PlayerBlockStack playerBlockStack)
    {
        ID = id;
        _playerBlockStack = playerBlockStack;

        _initialLocalPosition = transform.localPosition;
        _initialLocalRotation = transform.localRotation;
        _initialLocalScalce = transform.localScale;
    }

    public void Activate()
    {
        transform.localPosition = _initialLocalPosition;
        transform.localRotation = _initialLocalRotation;
        transform.localScale = _initialLocalScalce;

        IsActive = true;
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        IsActive = false;
        gameObject.SetActive(false);

        _playerBlockStack.ReturnBlock(ID);
    }
}