using System.Collections;
using System.Runtime.CompilerServices;

using EventBusSystem;
using Example;

using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    [SerializeField] private FloatingJoystick _joystick;
    [SerializeField] private PlayerCharacterAnimationSystem _animationSystem;
    [SerializeField] private PlayerCharacterMoweSystem _moweSystem;
    [SerializeField] private PlayerCharacterCollectSystem _collectSystem;
    [SerializeField] private PlayerCharacterSellSystem _sellSystem;
    [SerializeField] private Rigidbody _rigidBody;
    
    [SerializeField, Space] private float _moveSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 1f;
    [SerializeField] private float _timeToMoveExtractionPoint = 1f;
    [SerializeField] private float _timeToRotateWhenMoveToExtraction = 1f;

    [SerializeField] private int _oneBlockCost = 15;

    private int _coinsCount = 0;
    private int _blocksCount = 0;
    
    private void OnValidate()
    {
        if(_timeToMoveExtractionPoint <= 0f) _timeToMoveExtractionPoint = 0.016f;
        if(_timeToRotateWhenMoveToExtraction <= 0f) _timeToRotateWhenMoveToExtraction = 0.016f;
    }

    private void OnEnable()
    {
        _moweSystem.StartMoweEvent += StartMoweEventHandler;
        _collectSystem.NewGrassBlockReceivedEvent += NewGrassBlockReceivedEventHandler;
        _sellSystem.SellBlockEvent += SellBlockEventHandler;
    }

    private void OnDisable()
    {
        _moweSystem.StartMoweEvent -= StartMoweEventHandler;
        _collectSystem.NewGrassBlockReceivedEvent -= NewGrassBlockReceivedEventHandler;
        _sellSystem.SellBlockEvent -= SellBlockEventHandler;
    }

    public void FixedUpdate()
    {
        if(_moweSystem.IsMowing == true)
        {
            return;
        }

        if (_sellSystem.IsSaling == true)
        {
            _rigidBody.velocity = Vector3.zero;
            return;
        }
        
        Vector3 velocity = new Vector3(_joystick.Horizontal, 0f, _joystick.Vertical);
        velocity.Normalize();

        _rigidBody.velocity = velocity * _moveSpeed;

        if(velocity.sqrMagnitude > 0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(velocity.normalized, transform.up);
            targetRotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
            transform.rotation = targetRotation;
        }
    }

    private void StartMoweEventHandler(Vector3 extractionPoint)
    {
        StartCoroutine(MoveToExtractionPointAndLaunchMoweAnimation(extractionPoint));
    }
    
    private IEnumerator MoveToExtractionPointAndLaunchMoweAnimation(Vector3 extractionPoint)
    {
        _animationSystem.enabled = false;
        _animationSystem.StartRunAnimation(true);

        Vector3 startPosition = transform.position;
        Vector3 endPosition = extractionPoint;

        Vector3 direction = extractionPoint - new Vector3(transform.position.x, extractionPoint.y, transform.position.z);
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.LookRotation(direction.normalized, transform.up);

        float moveLerpStep = Time.deltaTime / _timeToMoveExtractionPoint;
        float rotationLerpStep = Time.deltaTime / _timeToRotateWhenMoveToExtraction;
        float moveLerpDistanceTraveled = 0f;
        float rotationLerpDistanceTraveled = 0f;

        bool reachMoveDestination = false;
        bool reachRotationDestination = false;

        while (true)
        {
            if (reachMoveDestination == true && reachRotationDestination == true)
            {
                break;
            }

            if (moveLerpDistanceTraveled < 1f)
            {
                moveLerpDistanceTraveled += moveLerpStep;
                transform.position = Vector3.Lerp(startPosition, endPosition, moveLerpDistanceTraveled);
            }
            else
            {
                reachMoveDestination = true;
            }

            if (rotationLerpDistanceTraveled < 1f)
            {
                rotationLerpDistanceTraveled += rotationLerpStep;
                transform.rotation = Quaternion.Lerp(startRotation, endRotation, rotationLerpDistanceTraveled);
            }
            else
            {
                reachRotationDestination = true;
            }

            yield return null;
        }

        Quaternion additionalTurn = Quaternion.AngleAxis(45f, Vector3.up);
        startRotation = transform.rotation;
        endRotation = Quaternion.LookRotation(Vector3.forward, transform.up) * additionalTurn;
        rotationLerpDistanceTraveled = 0f;

        while (true)
        {
            if (rotationLerpDistanceTraveled >= 1f)
            {
                break;
            }

            rotationLerpDistanceTraveled += rotationLerpStep;
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, rotationLerpDistanceTraveled);

            yield return null;
        }

        _animationSystem.enabled = true;
        _animationSystem.StartSickleAnimation();
    }

    private void NewGrassBlockReceivedEventHandler()
    {
        _blocksCount++;
        EventBus.RaiseEvent<IBlocksChangeHandler>(h => h.HandleBlocksChange(_blocksCount));
    }

    private void SellBlockEventHandler()
    {
        _blocksCount--;
        EventBus.RaiseEvent<IBlocksChangeHandler>(h => h.HandleBlocksChange(_blocksCount));

        _coinsCount++;
        EventBus.RaiseEvent<ICoinsChangeHandler>(h => h.HandleCoinsChange(_coinsCount * _oneBlockCost));
    }
}