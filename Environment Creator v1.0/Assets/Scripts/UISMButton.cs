// UISMButton.cs
//
// Description:
// A button inside of the side menu UI in the world builder screen.
//
// Author:
// t.teulings
//
// Date of last amendment:
// 09/04/2026

using UnityEngine;

public class UISMButton : MonoBehaviour
{
    public Transform trans;
    public float scrollStep = 5f;

    void Awake()
    {
        if (trans == null)
            trans = transform;
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Approximately(scroll, 0f))
            return;

        Vector3 pos = trans.position;
        pos.y += scroll * scrollStep;
        trans.position = pos;
    }
}
