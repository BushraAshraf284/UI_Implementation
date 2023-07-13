using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabSelect : MonoBehaviour
{
    public TABSNAME tab;

    public void OpenTab()
    {
        GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>().CreateTypes((int)tab);
    }
}
