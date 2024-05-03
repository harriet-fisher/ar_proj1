using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject uiPanel;

    public void ContinueButton()
    {
        uiPanel.SetActive(false);
    }

    void Start()
    {
        uiPanel.SetActive(true);
    }
}