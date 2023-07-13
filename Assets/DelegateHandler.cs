using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegateHandler : MonoBehaviour
{
    public delegate void SuccessDelegate();
    public static SuccessDelegate onAndroidSuccess;
    public static SuccessDelegate onFacebookSuccess;
    public static SuccessDelegate onLoginSuccess;
    public static SuccessDelegate onRegisterSuccess;
    public static SuccessDelegate onLinkSuccess;

    public delegate void UserDelegate(string email, string password);    
    public static UserDelegate loginUser;

    public delegate void RegisterDelegate(string email, string password, string username);
    public static RegisterDelegate registerUser;
    public static RegisterDelegate linkWithemail;



}
