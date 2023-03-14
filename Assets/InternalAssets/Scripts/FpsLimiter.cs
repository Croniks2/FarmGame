using UnityEngine;

public class FpsLimiter : MonoBehaviour
{
    [SerializeField] private int _targetFPS = 60;

    private void OnValidate()
    {
        Application.targetFrameRate = _targetFPS;
    }
}