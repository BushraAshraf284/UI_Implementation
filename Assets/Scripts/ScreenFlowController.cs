using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;

public class ScreenFlowController : MonoBehaviour
{
    [Header("Users")]
    public Root _root;
    [Header("Session List")]
    public List<PanelSetting> SessionList = new List<PanelSetting>();
    public List<GameObject> PanelsList = new List<GameObject>();

    public static int Session;
    static int counter;
    //List<int> CurrentFlow;
    //List<GameObject> Panels;
    // int[,] checkValues;


    [Space(5)]
    [Header("InputFields",order=1)]
    [Header("Login InputFields")]
    public TMP_InputField LoginID;
    public TMP_InputField LoginPassword;
    [Header("Login Error Messages")]
    public TMP_Text LoginIDError;
    public TMP_Text LoginPassError;   

    [Header("Sign Up InputFields")]
    public TMP_InputField SignUpPhone;
    public TMP_InputField SignUpEmail;
    public TMP_InputField SignUpPassword;
    public TMP_InputField ReEnterPass;
    public TMP_InputField Name;
    public TMP_InputField[] Pins;
    [Header("Sign Up Error Messages")]
    public TMP_Text PhoneError;
    public TMP_Text EmailError;
    public TMP_Text PasswordError;
    public TMP_Text ReEnterPasswordError;
    public TMP_Text NameError;
    public TMP_Text PinError;

    [Header("Link InputFields")]
    public TMP_InputField LinkEmail;
    public TMP_InputField LinkPassword;
    public TMP_InputField LinkUsername;
    [Header("Link Error Messages")]
    public TMP_Text LinkEmailError;
    public TMP_Text LinkPasswordError;
    public TMP_Text LinkNameError;

    [Header("Welcome Panel")]
    public TMP_Text UserName;
    [Header("Loading Panel")]
    public GameObject LoadingPanel;
    public GameObject loadIcon;

    private string email;
    private string password;
    private string number;
    private string username;
    private string pin;
    private int loggedin;
    private string EmailPattern, NumberPattern, PasswordPattern;
    private List<TMP_Text> ErrorMessages;
    private GameObject prevPanel;
    private int current;

    private void Awake()
    {
        // Will call the following functions on success of the respective API calls
        DelegateHandler.onAndroidSuccess += GetLastScene;

        DelegateHandler.onFacebookSuccess += SavePlayer;
        DelegateHandler.onFacebookSuccess += NextPanel;

        DelegateHandler.onLoginSuccess += SavePlayer;
        DelegateHandler.onLoginSuccess += NextPanel;

        DelegateHandler.onRegisterSuccess += SavePlayer;
        DelegateHandler.onRegisterSuccess += NextPanel;

        DelegateHandler.onLinkSuccess += SavePlayer;
        DelegateHandler.onLinkSuccess += NextPanel;

        

    }
    private void Start()    
    {
        //getting data from json
        var textFile = Resources.Load<TextAsset>("UserLoginInfo");
        Debug.Log(textFile.text);
        _root = JsonUtility.FromJson<Root>(textFile.text);
        Debug.Log("I'm here!");
        DontDestroyOnLoad(LoadingPanel);
        SilentLogin();
        prevPanel = PanelsList[current];
        //SessionList[Session].
        //default values
        pin = "1234";

        //patterns store
        EmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
        + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
        + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
        NumberPattern = @"^\d{10}$";
        PasswordPattern = @"^[\w+\s+0-9]{8,}$";
        //Initializing Error Messages
        ErrorMessages = new List<TMP_Text>() { PhoneError, EmailError, PasswordError, NameError, ReEnterPasswordError, PinError, LoginIDError,LoginPassError,LinkEmailError,LinkNameError,LinkPasswordError};
        SetAllToEmpty();
        Session = 5;
        
     }



    public void SilentLogin()
    {
        StartCoroutine("Loading");           
        Invoke("LoadPlayer", 3.0f);           
      
    }
    public void NextPanel()
    {
        SetAllToEmpty();
        /*if (counter == -1)
        {
            current = ((int)SessionList[Session].NestedList[2]);
            Debug.Log("Current value:" + current); Debug.Log("Current value:" + current);
        }
        else
        {
            Debug.Log("Session:" + Session);
            Debug.Log("Current Panel Name:" + SessionList[Session].NestedList[counter].ToString());
            
        }*/

        Debug.Log("Session:" + Session);
        Debug.Log("counter:"+ counter);
        if (prevPanel != null)
            prevPanel.SetActive(false);
        counter++;
        current = ((int)SessionList[Session].NestedList[counter]);
        Debug.Log("Current value:" + current);
        if (PanelsList[current].tag == "WalletScreen")
        {
            PanelsList[current].SetActive(true);
            prevPanel = PanelsList[current];
            Invoke("PlaySplashScreen", 2.0f);
        }
        else
        {
            Debug.Log("Next Panel Name:" + SessionList[Session].NestedList[counter].ToString());
            PanelsList[current].SetActive(true);
            prevPanel = PanelsList[current];
        }
        
    }

    public void PrevPanel()
    {
        SetAllToEmpty();
        PanelsList[current].SetActive(false);
        Debug.Log("In PrevPanel");
        Debug.Log("Prev Panel" + prevPanel);
        counter--;
        current = ((int)SessionList[Session].NestedList[counter]);
        PanelsList[current].SetActive(true);

    }
    public void ValidateLogin() 
    {       
        if(Validator(EmailPattern,LoginID,LoginIDError) || Validator(NumberPattern, LoginID, LoginIDError))
        {

            ValidateLoginPassword();
            /*if (LoginID.text == email || LoginID.text == number)
            {
               
                Debug.Log("Go to next");
                LoginIDError.SetText("");
            }
            else
            {
                LoginIDError.SetText("Login ID entered is Incorrect.");
                Debug.Log("Login ID is incorect");
            }*/
        }
    }
    public void ValidateLoginPassword()
    {
        if (Validator(PasswordPattern, LoginPassword, LoginPassError))
        {
            DelegateHandler.loginUser(LoginID.text, LoginPassword.text);
            /*if (LoginPassword.text == password)
            {
                SavePlayer();
                NextPanel();
                Debug.Log("LoginPass: true");
                LoginPassError.SetText("");
            }
            else
            {
                LoginPassError.SetText("Login Password entered is Incorrect.");
                Debug.Log("Login Password is incorect");
            }*/
        }      
    }
    public void ValidateEmail() 
    {
        if (Validator(EmailPattern, SignUpEmail, EmailError))
        {
            email = SignUpEmail.text;
            NextPanel();
        }
    }
    public void ValidateSignUpPassword()
    {
        if (Validator(PasswordPattern, SignUpPassword, PasswordError))
        {
            if (Validator(PasswordPattern, ReEnterPass, ReEnterPasswordError))
            {
                if (String.Equals(SignUpPassword.text, ReEnterPass.text))
                {
                    password = SignUpPassword.text;
                    NextPanel();
                }
                else
                {
                    PasswordError.SetText("");
                    ReEnterPasswordError.SetText("Password does not Match.");                  
                }
            }
            else
            {
                PasswordError.SetText("");
                ReEnterPasswordError.SetText("Password does not Match.");
            }            
        }
        else
            PasswordError.SetText("Minimum 8 characters are required!");
    }

    public void ValidateNumber()
    {
        if (Validator(NumberPattern, SignUpPhone, PhoneError))
        {
            number = SignUpPhone.text;
            Debug.Log(number);
            NextPanel();
        }
    }

    public void ValidateCode()
    {
        bool flag = true;
        Debug.Log("Pin Length:" + pin.Length);
        for (int i = 0; i<4; i++)
        {
            if(!String.Equals(Pins[i].text, pin[i].ToString()))
            {
                flag = false;
            }
        }
        Debug.Log("Flag:"+ flag);

        if(!flag)
        {
            PinError.SetText("Incorrect Pin");
            foreach(TMP_InputField pin in Pins)
            {
                pin.text = "";
            }
        }
        else
        {
            PinError.SetText("");
            NextPanel();
        }
    }

    public void ValidateName()
    {
        if(string.IsNullOrWhiteSpace(Name.text))
        {
            
            NameError.SetText("Field cannot be Empty");
        }
        else
        {
            username = Name.text;
            //PlayFabController.Instance.LoginWithAndroid();

            //registerUser call
           
           
        }
    }

    public void ValidateLinkValues()
    {
        if (string.IsNullOrWhiteSpace(LinkUsername.text))
        {
            LinkNameError.SetText("Field cannot be Empty");
        }
        else if (Validator(EmailPattern, LinkEmail, LinkEmailError) || Validator(PasswordPattern, LinkPassword, LinkPasswordError))
        {
            email = LinkEmail.text;
            username = LinkUsername.text;
            password = LinkPassword.text;
            DelegateHandler.linkWithemail(LinkEmail.text, LinkPassword.text,LinkUsername.text);
           
        }
    }


    bool Validator(string pattern, TMP_InputField Tocheck, TMP_Text Error)
    {
        Regex regex = new Regex(pattern);
        if (string.IsNullOrWhiteSpace(Tocheck.text))
        {
            Error.SetText("Field cannot be Empty");
            return false;
        }
        if (!regex.IsMatch(Tocheck.text))
        {
            Error.SetText("You have entered an Invalid Input");
            return false;
        }
        else
        {
            Error.SetText("");
            return true;
        }
            
    }

    void SetAllToEmpty()
    {
        if(ErrorMessages.Count!=0)
        foreach(TMP_Text err in ErrorMessages)
        {
            err.SetText("");
        }
    }


    void PlaySplashScreen()
    {
        /*Panels[CheckForNext(Session)].SetActive(true);
        Panels[7].SetActive(false);
        CurrentFlow.Add(CheckForNext(Session));*/
        NextPanel();
    }

   
    IEnumerator Loading()
    {
        LoadingPanel.SetActive(true);
        while (true)
        {
            float timer = 0f;
            while (timer < 1)
            {
                loadIcon.transform.Rotate(0, 0, -180 * Time.deltaTime);
                timer = timer + Time.deltaTime;
                yield return null;
            }
        }
    }
    void LoadPlayer()
    {
        LoadingPanel.SetActive(false);
        StopCoroutine("Loading");
        Debug.Log("Counter Value:" + counter);
        Debug.Log("In Loading: PlayerPref LoggedIn" + PlayerPrefs.GetInt("LoggedIn"));
        if (PlayerPrefs.GetInt("LoggedIn") == 1)
        {
            email = PlayerPrefs.GetString("Email");
            number = PlayerPrefs.GetString("Number");          
            password = PlayerPrefs.GetString("Password");
            username = PlayerPrefs.GetString("Name");
            loggedin = PlayerPrefs.GetInt("LoggedIn");

           // DelegateHandler.loginUser(email, password);
            UserName.SetText(username);
            Debug.Log("Username"+username);
            counter++;
          

        }
        /* if (PlayerPrefs.GetInt("LoggedIn") == 1)
         {
             counter = SessionList[Session].NestedList.Count - 2;
             Debug.Log("Counter Value:"+ counter);
             UserName.SetText(username);
             NextPanel();
         }*/
    }
    void SavePlayer()
    {
        loggedin = 1;

        
        PlayerPrefs.SetInt("LoggedIn", loggedin);
        PlayerPrefs.SetString("Email", email);
        PlayerPrefs.SetString("Number", number);
        PlayerPrefs.SetString("Password",password);
        PlayerPrefs.SetString("Name", username);
        UserName.SetText(username);
        PlayerPrefs.Save();
    }

   /* Button Functionalities*/

    public void LogOut()
    {
        Application.Quit();
    }

    public void ShowPassword()
    {
        TogglePassword(SignUpPassword);
    }

    public void ShowRepeatPassword()
    {
        TogglePassword(ReEnterPass);
    }

    public void TogglePassword(TMP_InputField userPassword)
    {
        if (userPassword.contentType == TMP_InputField.ContentType.Password)
        {
            userPassword.contentType = TMP_InputField.ContentType.Standard;
        }
        else
        {
            userPassword.contentType = TMP_InputField.ContentType.Password;
        }
        userPassword.ForceLabelUpdate();
    }

    public void SwitchSession()
    {
        prevPanel.SetActive(false);
        //current = ((int)p);
        PanelsList[current].SetActive(true);
        if (Session == 2)
            Session = 0;
        else
            Session = 2;
        counter--;
        NextPanel();
    }

  /*******************  LOGINS  */

    public void GetLastScene()
    {
        if(PlayerPrefs.GetInt("LoggedIn") == 0)
        {
            Session = 5;
            counter = -1;
        }
        NextPanel();
    }
    

   

    /*private void Start()
  {
      //checkValues = new int[10, 10];
      //CurrentFlow = new List<int>();
      //[Session, current]

      //Login - Session 0
      checkValues[0,1] = 9;
      //Sign Up with Number - Session 1
      checkValues[2,3] = 6;
      checkValues[2,4] = 5;
      checkValues[2,5] = 3;
      //Sign Up with Email - Session 2
      checkValues[1,3] = 5;
      checkValues[1,4] = 6;
      checkValues[1,5] = 4;
      //Sign Up with Wallet - Session 3
      checkValues[3,2] = 6;

      //Setting the initial screen in the flow
      //CurrentFlow.Add(0);


  }*/

    /*  public void NextPanel(int nextIndex)
      {

          Panels[CurrentFlow[CurrentFlow.Count - 1]].SetActive(false); // Setting Current Screen to false


          if(nextIndex==7) //Play Splash Scren if Wallet Screen
          {
              Debug.Log("Invoking Funtion");
              Panels[7].SetActive(true);
              Invoke("PlaySplashScreen", 2.0f);
          }            
          else
          {
              if (CheckForNext(Session) != 0)
              {
                  nextIndex = CheckForNext(Session);

              }
              Panels[nextIndex].SetActive(true);
              CurrentFlow.Add(nextIndex);
          }  

      }*/

    /*public void PrevPanel()
    {
         int prevIndex = CurrentFlow[CurrentFlow.Count - 2]; //getting value from 2nd last index, last index gives current index
          int currentIndex = CurrentFlow[CurrentFlow.Count - 1]; // getting current index
          Debug.Log("/nPrevious Index is: " + prevIndex);
          Debug.Log("Current Index is: " + currentIndex);
          printList();
          Panels[prevIndex].SetActive(true);
          Panels[currentIndex].SetActive(false);
          CurrentFlow.Remove(currentIndex);// remove the current panel from the flow       

    }*/
    /* public int CheckForNext(int Session)
     {
         printList();
         int listCurrent = CurrentFlow[CurrentFlow.Count - 1];
         // int listPrev = CurrentFlow[CurrentFlow.Count - 2];
        Debug.Log("Session:" + Session);
         Debug.Log("List Current:"+listCurrent);        
         Debug.Log("returned Value: "+ checkValues[Session,listCurrent]);
         return checkValues[Session,listCurrent];        

     }*/

    /*public void printList()
    {
        print("Values in list:");
        foreach(var val in CurrentFlow)
        {
            print(val);
        }
    }*/
}
