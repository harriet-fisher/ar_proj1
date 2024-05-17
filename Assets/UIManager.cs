using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject uiPanel;
    public GameObject actionMenu;

    public void ContinueButton()
    {
        uiPanel.SetActive(false);
    }

    public void ShowMenu(bool visible)
    {
        actionMenu.SetActive(visible);
    }

    void Start()
    {
        uiPanel.SetActive(true);
        actionMenu.SetActive(false);
    }
}