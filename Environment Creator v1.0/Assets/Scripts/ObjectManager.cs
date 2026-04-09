// ObjectManager.cs
//
// Description:
// Manager for all objects.
//
// Author:
// t.teulings
//
// Date of last amendment:
// 09/04/2026

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectManager : MonoBehaviour
{
    public GameObject UISideMenu;

    public List<GameObject> prefabObjects;
    private List<GameObject> placedObjects = new List<GameObject>();

    /// <summary>
    /// Places a new object in the current environment.
    /// </summary>
    /// <param name="index"></param>
    public void PlaceNewObject2D(int index)
    {
        if (prefabObjects == null || prefabObjects.Count == 0)
        {
            Debug.LogWarning("ObjectManager.PlaceNewObject2D: No prefabs assigned.");
            return;
        }

        if (index < 0 || index >= prefabObjects.Count)
        {
            Debug.LogWarning($"ObjectManager.PlaceNewObject2D: index {index} out of range.");
            return;
        }

        if (UISideMenu != null)
            UISideMenu.SetActive(false);

        GameObject instanceOfPrefab = Instantiate(prefabObjects[index], Vector3.zero, Quaternion.identity);
        instanceOfPrefab.SetActive(true);

        var rect = instanceOfPrefab.GetComponent<RectTransform>();
        if (rect != null)
        {
            var canvas = FindObjectOfType<Canvas>();
            if (canvas != null)
            {
                instanceOfPrefab.transform.SetParent(canvas.transform, false);
                RectTransform canvasRect = canvas.GetComponent<RectTransform>();
                Vector2 localPoint;
                Camera eventCam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Input.mousePosition, eventCam, out localPoint))
                {
                    rect.anchoredPosition = localPoint;
                }
            }
            else
            {
                Debug.LogWarning("Placed UI prefab but no Canvas found in scene. It may be off-screen.");
            }
        }
        else
        {
            var cam = Camera.main;
            if (cam != null)
            {
                float desiredZ = instanceOfPrefab.transform.position.z;
                float zDistance = Mathf.Abs(cam.transform.position.z - desiredZ);
                Vector3 mouseScreen = Input.mousePosition;
                mouseScreen.z = zDistance;
                Vector3 worldPos = cam.ScreenToWorldPoint(mouseScreen);
                worldPos.z = desiredZ;
                instanceOfPrefab.transform.position = worldPos;
            }
            else
            {
                Debug.LogWarning("ObjectManager.PlaceNewObject2D: No Camera tagged MainCamera found. Instantiated object left at (0,0,0).");
            }
        }

        placedObjects.Add(instanceOfPrefab);

        Draggable draggable = instanceOfPrefab.GetComponent<Draggable>();
        if (draggable == null)
        {
            Debug.LogWarning("Placed prefab does not contain a Draggable component.");
            return;
        }

        draggable.objectManager = this;
        draggable.isDragging = true;
    }

    /// <summary>
    /// Displays the user interface side menu.
    /// </summary>
    public void ShowMenu()
    {
        if (UISideMenu != null)
            UISideMenu.SetActive(true);
    }
    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
