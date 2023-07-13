using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionSet : MonoBehaviour
{
    public int Session;

    public void SetSession()
    {
        Debug.Log("Setting Session:"+ Session);
        ScreenFlowController.Session = Session;
    }
  
}
