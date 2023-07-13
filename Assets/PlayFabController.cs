using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Facebook.Unity;
using LoginResult = PlayFab.ClientModels.LoginResult;

public class PlayFabController : MonoBehaviour
{

    //public RewardsInfo rewardsJson;
    public RewardsInfo rewardsJson;
    private static  PlayFabController _instance;
   
    public static PlayFabController Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        LoginWithAndroid();
        DelegateHandler.registerUser += RegisterUser;
        DelegateHandler.loginUser += LoginUser;
        DelegateHandler.linkWithemail += LinkWithEmail;
    }

    public void LinkWithEmail(string email, string password, string username)
    {
        PlayFabClientAPI.AddUsernamePassword(new PlayFab.ClientModels.AddUsernamePasswordRequest
        {
            Username = username,
            Email = email,
            Password = password
        },
        addUsernamePasswordResult =>
        {
            Debug.Log("Username:" + addUsernamePasswordResult.Username);
            DelegateHandler.onLinkSuccess();
        }, Error);
    }      
 
 
    private void Error(PlayFabError error)
    {
        Debug.Log("Link With Email Failed!");
        Debug.Log(error.GenerateErrorReport());
    }
    
   

    private void RegisterUser(string email, string password, string username)
    {
        var registerRequest = new RegisterPlayFabUserRequest { Email = email, Password = password, Username = username };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest,
        success =>
        {
            DelegateHandler.onRegisterSuccess();
            Debug.Log("Sign Up Succeeded!");
        },
        failure =>
        {
            Debug.Log("Sign Up Failed!" + failure.ErrorMessage);
        }
        );
    }

    private void LoginUser(string email, string password)
    {
        var request = new LoginWithEmailAddressRequest { Email = email, Password = password };

        PlayFabClientAPI.LoginWithEmailAddress(request,
            success =>
            {
                DelegateHandler.onLoginSuccess();
                Debug.Log("Logged In Successfully!");
            },
            failure =>
            {
                //LoginPassError.SetText("Login Password entered is Incorrect.");
                Debug.Log("Logged In Unsuccessfull!");
            }
        );
    }

    public void LoginWithAndroid()
    {
        var requestAndroid = new LoginWithAndroidDeviceIDRequest
        {
            AndroidDeviceId = SystemInfo.deviceUniqueIdentifier,
            AndroidDevice = SystemInfo.deviceModel,
            OS = SystemInfo.operatingSystem,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid,
        success =>
        {
            Debug.Log("Successfully Logged in to Android");
            //counter = SessionList[Session].NestedList.Count - 2;
            DelegateHandler.onAndroidSuccess();
            GetServerData();

        },
        failure =>
        {
            Debug.Log("Did not Login to Android");
        });
    }

    public void LoginWithFacebook()
    {
        FB.Init(OnFacebookInitialized);
    }

    private void OnFacebookInitialized()
    {
        // Once Facebook SDK is initialized, if we are logged in, we log out to demonstrate the entire authentication cycle.
        if (FB.IsLoggedIn)
            FB.LogOut();

        // We invoke basic login procedure and pass in the callback to process the result
        FB.LogInWithReadPermissions(null, OnFacebookLoggedIn);
    }

    private void OnFacebookLoggedIn(ILoginResult result)
    {
        // If result has no errors, it means we have authenticated in Facebook successfully
        if (result == null || string.IsNullOrEmpty(result.Error))
        {
            /*
             * We proceed with making a call to PlayFab API. We pass in current Facebook AccessToken and let it create
             * and account using CreateAccount flag set to true. We also pass the callback for Success and Failure results
             */
            PlayFabClientAPI.LoginWithFacebook(new LoginWithFacebookRequest { AccessToken = AccessToken.CurrentAccessToken.TokenString },
                OnPlayfabFacebookAuthComplete, OnPlayfabFacebookAuthFailed);
        }
        else
        {
            // If Facebook authentication failed, we stop the cycle with the message
            Debug.Log("Fb login Failed" + result.Error);
        }
    }

    // When processing both results, we just set the message, explaining what's going on.
    private void OnPlayfabFacebookAuthComplete(LoginResult result)
    {
        //SetMessage("PlayFab Facebook Auth Complete. Session ticket: " + result.SessionTicket);
        Debug.Log("FB Login Working!");
        DelegateHandler.onFacebookSuccess();
    }
    //goes to next panel       


    private void OnPlayfabFacebookAuthFailed(PlayFabError error)
    {
        Debug.Log("FB Login Failed." + error);
    }


    public void LinkWithFacebook()
    {
        FB.Init(LogToFB);
    }

    public void LogToFB()
    {
        FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email" }, AuthCallBack);
    }

    private void AuthCallBack(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            var request = new LinkFacebookAccountRequest { AccessToken = Facebook.Unity.AccessToken.CurrentAccessToken.TokenString };

            PlayFabClientAPI.LinkFacebookAccount(request, OnLinkWithFBSuccess, OnLinkWithFBFailure);
        }
        else
        {
            Debug.Log("Facebook Linking Not Done!");
        }
           
    }
    private void OnLinkWithFBSuccess(LinkFacebookAccountResult result)
    {
        Debug.Log("Facebook account successfully linked");
        DelegateHandler.onFacebookSuccess();
    }

    private void OnLinkWithFBFailure(PlayFabError error)
    {
        Debug.Log("Unable to link FB account");
    }

    /*********************** Getting/Setting Data from Server ****************************/

   
    private void GetServerData()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "GetData",
            FunctionParameter = new { key = "RewardsInfo" }
            /* GeneratePlayStreamEvent = true, // Optional - Shows this event in PlayStream*/
        }, SetData, OnErrorShared);
    }

    private void SetData(ExecuteCloudScriptResult result)
    {
        // CloudScript returns arbitrary results, so you have to evaluate them one step and one parameter at a time
        //Debug.Log(result.FunctionResult.ToString());


        rewardsJson = JsonUtility.FromJson<RewardsInfo>(result.FunctionResult.ToString());

        //rewardsJson.Rewards.Clear();
        //rewardsJson= JsonUtility.FromJson<RewardsInfo>(result.FunctionResult.ToString());
        //Debug.Log(rewardsJson.Rewards);


        /* object messageValue;
         jsonResult.TryGetValue("messageValue", out messageValue); // note how "messageValue" directly corresponds to the JSON values set in CloudScript
         Debug.Log((string)messageValue);*/
    }

    private static void OnErrorShared(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }


    public void UpdatePlayerData()
    {
        rewardsJson.Rewards[3].Type = "Epic";
        string json = JsonUtility.ToJson(rewardsJson);
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "SetPlayerData",
            FunctionParameter = new { key = "RewardsInfo" , data = json}
            /* GeneratePlayStreamEvent = true, // Optional - Shows this event in PlayStream*/
        }, OnSuccess, OnErrorShared);
    }

    private void OnSuccess(ExecuteCloudScriptResult result)
    {
        Debug.Log(result);


    }
    }
