using UnityEngine;


public class PlayerCharacterAnimationSystem : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator _animator;
    
    private const string MOWE = "Mowe";
    private const string RUN = "Run";

    
    private void FixedUpdate()
    {
        if(_rigidbody.velocity.sqrMagnitude > 0f)
        {
            _animator.SetBool(RUN, true);
        }
        else
        {
            _animator.SetBool(RUN, false);
        }
    }

    public void StartSickleAnimation()
    {
        _animator.SetTrigger(MOWE);
    }

    public void StartRunAnimation(bool on)
    {
        _animator.SetBool(RUN, on);
    }
}