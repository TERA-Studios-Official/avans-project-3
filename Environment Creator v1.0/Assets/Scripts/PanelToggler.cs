// PanelToggler.cs
//
// Description:
// Toggles panels. That's not a panel, that's a crusher. We sell those too.
//
// Author:
// t.teulings
//
// Date of last amendment:
// 09/04/2026

using UnityEngine;

public class PanelToggler : MonoBehaviour
{
    public GameObject loginPanel;
    public GameObject settingsPanel;
    public GameObject worldSelectionPanel;

    public DBGate dbGate;
    void Start()
    {
       settingsPanel.SetActive(false); 
       worldSelectionPanel.SetActive(false);
       loginPanel.SetActive(true);
    }

    /// <summary>
    /// Toggles a panel on or off, based on it's current state.
    /// /// </summary>
    /// <param name="panel"></param>
    public void TogglePanel(GameObject panel)
    {
        bool shouldShow = panel.activeSelf;
        panel.SetActive(!shouldShow);
    }
}
