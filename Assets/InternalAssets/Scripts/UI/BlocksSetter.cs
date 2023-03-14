using EventBusSystem;

using TMPro;

using UnityEngine;

public class BlocksSetter : MonoBehaviour, IBlocksChangeHandler
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private string _format;

    private void OnEnable()
    {
        EventBus.Subscribe(this);

        _text.text = string.Format(_format,"0");
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(this);
    }

    public void HandleBlocksChange(int count)
    {
        _text.text = string.Format(_format, count.ToString());
    }
}
