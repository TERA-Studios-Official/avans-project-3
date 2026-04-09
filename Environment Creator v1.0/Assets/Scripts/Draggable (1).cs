// Draggable.cs
//
// Description:
// A draggable is an object that is part of an environment, and can be placed, moved, and deleted.
//
// Author:
// t.teulings
//
// Date of last amendment:
// 09/04/2026

using System;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    public Transform trans;

    [NonSerialized] public ObjectManager objectManager;
    [NonSerialized] public bool isDragging = false;

    private RectTransform rectTrans;
    private Canvas parentCanvas;
    private Camera cam;

    private void Awake()
    {
        if (trans == null)
            trans = transform;

        rectTrans = GetComponent<RectTransform>();
        parentCanvas = GetComponentInParent<Canvas>();
        cam = Camera.main;
    }

    /// <summary>
    /// Toggles the dragging state and displays the menu when dragging is stopped.
    /// </summary>
    public void SwitchDragging()
    {
        isDragging = !isDragging;
        if (!isDragging)
        {
            objectManager.ShowMenu();
        }
    }

    public void Update()
    {
        if (isDragging)
            MoveToCursor();

        if (Input.GetKeyDown(KeyCode.Delete) && isDragging)
            Destroy(gameObject);

    }

    /// <summary>
    /// Determines whether the current object is associated with a valid UI element and canvas.
    /// </summary>
    /// <returns>true if both the UI transform and parent canvas are present; otherwise, false.</returns>
    private bool IsUI()
    {
        return rectTrans != null && parentCanvas != null;
    }

    /// <summary>
    /// But Cursor, I love you!
    /// </summary>
    public void MoveToCursor()
    {
        if (IsUI())
        {
            RectTransform canvasRect = parentCanvas.GetComponent<RectTransform>();
            Vector2 localPoint;

            Camera eventCam = parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : cam;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Input.mousePosition, eventCam, out localPoint))
            {
                rectTrans.anchoredPosition = localPoint;
            }
        }
        else
        {
            // Check twice since the first time we could resolve a possible error, but the second time we don't really care anymore.
            if (cam == null)
                cam = Camera.main;

            if (cam == null)
                return;

            Vector3 mouseScreen = Input.mousePosition;

            mouseScreen.z = Mathf.Abs(cam.transform.position.z - trans.position.z);

            Vector3 worldPos = cam.ScreenToWorldPoint(mouseScreen);
            worldPos.z = trans.position.z; // keep original zeddie
            trans.position = worldPos;
        }
    }
}
