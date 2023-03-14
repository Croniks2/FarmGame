using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Utils.CheckVisibility;

public class TakeSetObjectsVisibleByCamera : MonoBehaviour
{
    public Camera checkCamera = null;
    public Transform result;
    public GameObject[] exceptions;
    public static TakeSetObjectsVisibleByCamera Instance;
    
    private GameObject[] _gameObjects;
    private List<CheckVisibilityComponent> _checkObjects = new List<CheckVisibilityComponent>();

    private void Awake()
    {
        checkCamera = checkCamera == null ? Camera.main : checkCamera;  
        Instance = this;

        _gameObjects = GameObject.FindObjectsOfType<GameObject>();

        for (int i = 0; i < _gameObjects.Length; i++)
        {
            GameObject gameObj = _gameObjects[i];

            bool isException = false;
            for (int j = 0; j < exceptions.Length; j++)
            {
                if(gameObj == exceptions[j])
                {
                    isException = true;
                    break;
                }
            }

            if(isException == true || gameObj.activeSelf == false || gameObj.transform.parent != transform)
            {
                continue;
            }

            var visibilityComponent = gameObj.AddComponent<CheckVisibilityComponent>();
            var boxCollider = gameObj.AddComponent<BoxCollider>();

            visibilityComponent.BoxCollider = boxCollider;

            _checkObjects.Add(visibilityComponent);
        }

        StartCoroutine(CheckObjects());

        Debug.Log($"Complete 1 step !!!");
    }

    private IEnumerator CheckObjects()
    {
        yield return new WaitForSeconds(5f);

        _checkObjects.ForEach(c => {

            bool isVisible = c.CheckVisibility(checkCamera, VisibilityOption.PartOfPoints);

            if(isVisible == true)
            {
                c.transform.parent = result;
            }
        });

        Debug.Log($"Complete 2 step !!!");
    }
}