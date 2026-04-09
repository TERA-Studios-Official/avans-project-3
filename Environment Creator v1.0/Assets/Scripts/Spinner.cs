// Spinner.cs
//
// Description:
// That one pointless thingy (I literally changed nothing in this scipt).
//
// Author:
// Avans Hogeschool
//
// Date of last amendment:
// 08/03/2026

using UnityEngine;

public class Spinner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float speed = 100f;

    void Update()
    {

        transform.Rotate(new Vector3(0,0,1), speed * Time.deltaTime);

        var mainCamera = Camera.main;
        Debug.Log("Ik draai!");
    }

}
