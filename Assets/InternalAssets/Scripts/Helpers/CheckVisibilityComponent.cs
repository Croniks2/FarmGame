using System;

using UnityEngine;


namespace Utils.CheckVisibility
{
    public class CheckVisibilityComponent : MonoBehaviour
    {
        public event Action<bool> CheckVisibilityEvent;

        public BoxCollider BoxCollider { get => _boxCollider; set => _boxCollider = value; }
        [SerializeField, Header("If you don't specify a collider, the object's pivot will be used for checking")]
        private BoxCollider _boxCollider;

        [SerializeField, Header("If you don't specify a collider, this setting is ignored")]
        private VisibilityOption _visibilityOption = VisibilityOption.PartOfPoints;

        [SerializeField, Header("If you do not specify a camera, the Camera.main will be used")]
        private Camera _camera;

        [SerializeField] private bool _checkOnStart = false;


        private void Awake()
        {
            if (_camera == null) _camera = Camera.main;
        }

        private void Start()
        {
            if (_checkOnStart == true)
            {
                CheckVisibility();
            }
        }

        //private void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.F))
        //    {
        //        Debug.Log($"{CheckVisibility()}");
        //    }
        //}

        public bool CheckVisibility(Camera camera = null, VisibilityOption visibilityOption = 0)
        {
            _camera = camera == null ? _camera : camera;
            _visibilityOption = visibilityOption == 0 ? _visibilityOption : visibilityOption;

            bool isVisible = false;
            if (_boxCollider == null)
            {
                isVisible = CheckVisibilityUtility.CheckVisibility(transform, _camera);
            }
            else
            {
                isVisible = CheckVisibilityUtility.CheckVisibility(_boxCollider, _camera, _visibilityOption);
            }

            CheckVisibilityEvent?.Invoke(isVisible);
            return isVisible;
        }
    }
}