// HideStartMenu.cs
//
// Description:
// Hides the start menu.
//
// Author:
// t.teulings
//
// Date of last amendment:
// 09/04/2026

using UnityEngine;

public class HideStartMenu : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject sideMenu;

    /// <summary>
    /// Goodbye!
    /// </summary>
    public void HideStartMenu_()
    {
        startMenu.SetActive(false);
        sideMenu.SetActive(true);
    }
}
