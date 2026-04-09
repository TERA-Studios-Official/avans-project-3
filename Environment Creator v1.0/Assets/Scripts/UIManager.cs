// UIManager.cs
//
// Description:
// Manages (most of) the main menu UI.
//
// Authors:
// t.teulings
// Avans Hogeschool
//
// Date of last amendment:
// 09/04/2026

using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject labelObject;

    public GameObject startMenu;
    public GameObject selectionMenu;

    public TMP_Text label;

    public string naam = "Tycho";

    [System.Obsolete]
    void Awake()
    {
        naam = "Tycho";

        // If an inspector-assigned GameObject already contains a TMP component, use it.
        if (labelObject != null)
        {
            label = labelObject.GetComponent<TMP_Text>();
            if (label == null)
            {
                label = labelObject.AddComponent<TextMeshProUGUI>();

                // Basic default styling so the created text is visible and stuff...
                var tmpUgui = label as TextMeshProUGUI;
                if (tmpUgui != null)
                {
                    tmpUgui.fontSize = 24;
                    tmpUgui.alignment = TextAlignmentOptions.Center;
                }
            }

            return;
        }

        label = FindObjectOfType<TextMeshProUGUI>();
        if (label != null)
            return;

        label = FindObjectOfType<TextMeshPro>();
    }

    void Start()
    {
        if (label == null)
        {
            Debug.LogError("UIManager: No TMP text component available. Assign 'labelObject' in the Inspector or add a TextMeshProUGUI/TextMeshPro to a GameObject in the scene.");
            return;
        }

        int randInt = Random.Range(0, 5);

        switch (randInt)
        {
            case 0:
                label.text = $"Welkom {naam}! 42 + 42 = 42";
                break;

            case 1:
                label.text = $"Welkom {naam}! Also try Unreal!";
                break;

            case 2:
                label.text = $"Welkom {naam}! hi";
                break;

            case 3:
                label.text = $"Welkom {naam}! Herobrine removed ;-)";
                break;

            case 4:
                label.text = $"Welkom {naam}! Let our battles begin!";
                break;

            default:
                label.text = $"Welkom {naam}!";
                break;
        }

    }

    /// <summary>
    /// Updates the label / splash text.
    /// </summary>
    /// <param name="newText"></param>
    public void UpdateLabel(string newText)
    {
        if (label == null)
        {
            Debug.LogWarning("UIManager.UpdateLabel called but 'label' is null.");
            return;
        }

        label.text = newText;
    }

    /// <summary>
    /// Toggles the sart menu.
    /// </summary>
    public void ToggleStartMenu()
    {
        startMenu.SetActive(false);
        selectionMenu.SetActive(true);
    }
}
