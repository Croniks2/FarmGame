using System.Collections.Generic;
using UnityEngine;


namespace Utils.CheckVisibility
{
    public enum VisibilityOption { AllPoints = 1, PartOfPoints = 2 }

    public static class CheckVisibilityUtility
    {
        #region PublicMethods

        public static bool CheckVisibility(BoxCollider collider, Camera camera, VisibilityOption visibilityOption)
        {
            return CheckWithCollider(collider, camera, visibilityOption);
        }

        public static bool CheckVisibility(Transform transform, Camera camera)
        {
            return CheckWithPivot(transform, camera);
        }

        #endregion

        #region PrivateMethods

        private static bool CheckWithCollider(BoxCollider boxCollider, Camera camera, VisibilityOption visibilityOption)
        {
            Vector3 size = boxCollider.size;
            Vector3 center = boxCollider.center;
            
            float xOffset = size.x / 2;
            float yOffset = size.y / 2;
            float zOffset = size.z / 2;
            
            var worldObjectMatrix = boxCollider.transform.localToWorldMatrix;
            
            Vector3[] vertices = new Vector3[] {
                worldObjectMatrix.MultiplyPoint3x4(new Vector3(center.x + xOffset, center.y + yOffset, center.z + zOffset)), // 0
                worldObjectMatrix.MultiplyPoint3x4(new Vector3(center.x + xOffset, center.y - yOffset, center.z + zOffset)), // 1
                worldObjectMatrix.MultiplyPoint3x4(new Vector3(center.x - xOffset, center.y - yOffset, center.z + zOffset)), // 2
                worldObjectMatrix.MultiplyPoint3x4(new Vector3(center.x - xOffset, center.y + yOffset, center.z + zOffset)), // 3
                worldObjectMatrix.MultiplyPoint3x4(new Vector3(center.x + xOffset, center.y + yOffset, center.z - zOffset)), // 4
                worldObjectMatrix.MultiplyPoint3x4(new Vector3(center.x + xOffset, center.y - yOffset, center.z - zOffset)), // 5
                worldObjectMatrix.MultiplyPoint3x4(new Vector3(center.x - xOffset, center.y - yOffset, center.z - zOffset)), // 6
                worldObjectMatrix.MultiplyPoint3x4(new Vector3(center.x - xOffset, center.y + yOffset, center.z - zOffset)), // 7
            };
            
            //DebugVertices(vertices);

            return CheckVerticesVisibility(vertices, camera, visibilityOption);
        }

        private static bool CheckWithPivot(Transform transform, Camera camera)
        {
            return CheckIfPositionIsVisibleToCamera(transform.position, camera);
        }

        private static bool CheckVerticesVisibility(Vector3[] vertices, Camera camera, VisibilityOption visibilityOption)
        {
            bool isVisible = visibilityOption == VisibilityOption.PartOfPoints ? false : true;
            foreach (Vector3 vertex in vertices)
            {
                if (visibilityOption == VisibilityOption.PartOfPoints)
                {
                    if (CheckIfPositionIsVisibleToCamera(vertex, camera) == true)
                    {
                        return true;
                    }
                }
                else if (visibilityOption == VisibilityOption.AllPoints)
                {
                    if (CheckIfPositionIsVisibleToCamera(vertex, camera) == false)
                    {
                        return false;
                    }
                }
            }

            return isVisible;
        }

        private static Rect bounds = new Rect(0, 0, 1f, 1f);
        private static bool CheckIfPositionIsVisibleToCamera(Vector3 position, Camera camera)
        {
            Vector2 viewportPosition = camera.WorldToViewportPoint(position);
            return bounds.Contains(viewportPosition);
        }

        #endregion

        #region Debug

        private static List<GameObject> _debugObjects;
        private static void DebugVertices(Vector3[] vertices)
        {
            if (_debugObjects == null) _debugObjects = new List<GameObject>();
            if (_debugObjects.Count > 0)
            {
                _debugObjects.ForEach(obj => GameObject.Destroy(obj));
                _debugObjects.Clear();
            }

            for (int i = 0; i < vertices.Length; i++)
            {
                var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.name = (i + 1).ToString();
                sphere.transform.position = vertices[i];
                sphere.transform.localScale = Vector3.one * 0.2f;

                _debugObjects.Add(sphere);
            }
        }

        #endregion
    }
}