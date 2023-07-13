using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{    
    public void NextPanel()
    {            
        GameObject.FindGameObjectWithTag("ScreenFlowController").GetComponent<ScreenFlowController>().NextPanel();
    }

    public void PrevPanel()
    {
        GameObject.FindGameObjectWithTag("ScreenFlowController").GetComponent<ScreenFlowController>().PrevPanel();
    }     

}
