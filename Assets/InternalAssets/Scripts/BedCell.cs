using DG.Tweening;

using UnityEngine;


public class BedCell : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private BoxCollider _collider;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField, Space] private Transform _grassBlock;

    [SerializeField] private float _growScale = 1.4f;
    private Vector3 _scale;
    
    public Transform ExtractionPoint => _extractionPoint;
    [SerializeField] private Transform _extractionPoint;

    [SerializeField, Space] private Material _youngGrassMaterial;
    [SerializeField] private Material _grassMaterial;

    private bool _isIincreased = false;

    private void Awake()
    {
        _scale = _transform.localScale;
        _meshRenderer.material = _youngGrassMaterial;
    }

    private void Start()
    {
        Grow(5f);
    }
    
    public void Cut()
    {
        _collider.enabled = false;
        _isIincreased = false;

        _transform.localScale = new Vector3(_scale.x, 0f, _scale.z);
        _meshRenderer.material = _youngGrassMaterial;
        
        _grassBlock.gameObject.SetActive(true);
    }

    public void Grow(float time)
    {
        _transform.DOScaleY(_growScale, time)
           .SetRecyclable(true)
           .OnComplete(() => 
           { 
               _collider.enabled = true; 
               _isIincreased = true;
               _meshRenderer.material = _grassMaterial;
           })
           .Play();
    }
}