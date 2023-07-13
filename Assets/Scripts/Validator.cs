using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Validator : MonoBehaviour
{
    public TMP_InputField LoginIDField;
    public TMP_InputField PasswordField;
    public TMP_Text ErrorMessage;

    /*public void Validate()
    {
        GameObject.FindGameObjectWithTag("ScreenFlowController").GetComponent<ScreenFlowController>().ValidateLogin(LoginIDField,PasswordField,ErrorMessage);
    }*/
}
