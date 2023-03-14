using System;
using System.Collections;

using UnityEngine;


public class PlayerCharacterMoweSystem : MonoBehaviour
{
    public event Action<Vector3> StartMoweEvent;
    public event Action MoweGrassEvent;

    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private LayerMask _layerMask;

    [SerializeField, Space] private float _moweDuration = 1f;
    [SerializeField] private float _moweGrassTimePosition = 1f;
    
    [SerializeField] private float _delayBeforeMowe = 1f;

    private float _timeElapsedBeforeMowe = 0f;

    public bool IsMowing => _isMowing;
    private bool _isMowing = false;


    private void FixedUpdate()
    {
        if(_isMowing == true)
        {
            return;
        }

        if (_rigidbody.velocity.sqrMagnitude <= 0f)
        {
            Vector3 origin = transform.position + new Vector3(0f, 0.5f, 0f);

            if (Physics.Raycast(origin, Vector3.down, out RaycastHit hitInfo, 5f, _layerMask, QueryTriggerInteraction.Collide))
            {
                _timeElapsedBeforeMowe += Time.fixedDeltaTime;

                if (_timeElapsedBeforeMowe >= _delayBeforeMowe)
                {
                    BedCell grass = hitInfo.transform.GetComponent<BedCell>();
                    StartCoroutine(LaunchMowe(grass));
                }
            }
        }
        else
        {
            _timeElapsedBeforeMowe = 0f;
        }
    }

    private IEnumerator LaunchMowe(BedCell grass)
    {
        _isMowing = true;
        Vector3 extractionPoint = grass.ExtractionPoint.transform.position;

        StartMoweEvent?.Invoke(extractionPoint);

        yield return new WaitForSeconds(_moweGrassTimePosition);

        grass.Cut();

        yield return new WaitForSeconds(_moweDuration - _moweGrassTimePosition);

        _isMowing = false;
        _timeElapsedBeforeMowe = 0f;
    }
}