using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class NetworkController : MonoBehaviour
{
    
    public const string DATA_URL_SIGNUP = "http://192.168.0.57/mmo2d/signup.php";
    public const string DATA_URL_LOGIN = "http://192.168.0.57/mmo2d/login.php";

    // Pre-Selection
    public Button loginButtonPannel,signupButtonPannel;

    // Invalid Text
    public Text InvalidUser,invalidPassword,serverOff,passwordNotEqual,userExist,blackSpaceSignup,blackSpaceLogin,userCreated,invalidUserName;

    // Login and Register Panels
    public GameObject loginPanel, signupPanel,firstPanel;

    // Character Selection Panels
    public GameObject characterPanel;

    // Back Button
    public Button backLoginButton,backSignupButton;
    
    // Register
    public InputField usernameSignup,passwordSignup,passwordVerification,email;
    public Button joinButton;

    // Login
    public InputField usernameLogin,passwordLogin;
    public Button loginButton;
    
    void Start(){
        loginButtonPannel.onClick.AddListener(openLoginPanel);
        signupButtonPannel.onClick.AddListener(openSignupPanel);
        checkAgain();
    }

    void checkAgain(){
        InvalidUser.enabled = false;
        invalidPassword.enabled = false;
        serverOff.enabled = false;
        passwordNotEqual.enabled = false;
        userExist.enabled = false;
        blackSpaceSignup.enabled = false;
        blackSpaceLogin.enabled = false;
        userCreated.enabled = false;
        invalidUserName.enabled = false;
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            backPanel();
        }
    }
    

    public void openSignupPanel(){
        signupPanel.SetActive(true);
        checkAgain();
        
        backSignupButton.onClick.AddListener(backPanel);        

        signupButtonPannel.gameObject.SetActive(false);
        loginButtonPannel.gameObject.SetActive(false);
    }

    public void openLoginPanel(){
        loginPanel.SetActive(true);
        checkAgain();
        backLoginButton.onClick.AddListener(backPanel);

        signupButtonPannel.gameObject.SetActive(false);
        loginButtonPannel.gameObject.SetActive(false);
    }

    public void backPanel(){
        checkAgain();
        signupPanel.SetActive(false);
        loginPanel.SetActive(false);
        signupButtonPannel.gameObject.SetActive(true);
        loginButtonPannel.gameObject.SetActive(true);
    }

    void openCharacterSelection(){
        checkAgain();
        characterPanel.SetActive(true);
        loginPanel.SetActive(false);
        firstPanel.SetActive(false);
    }

    public void signComplete(){
        if(usernameSignup.text == "" || passwordSignup.text == "" || passwordVerification.text  == "" || email.text  == ""){
            blackSpaceSignup.enabled = true;
            return;
        }

        if(passwordSignup.text != passwordVerification.text){
            passwordNotEqual.enabled = true;
            return;
        }
        if(usernameSignup.text.Length <= 6){
            invalidUserName.enabled = true;
            return;
        }


        StartCoroutine(ProcessRequest(usernameSignup.text,passwordSignup.text,DATA_URL_SIGNUP,email.text));
        Debug.Log("ProcessRequest()");
    }

    public void loginComplete(){
        if(usernameLogin.text == "" || passwordLogin.text == ""){
            blackSpaceLogin.enabled = true;
            return;
        }

        StartCoroutine(ProcessRequest(usernameLogin.text,passwordLogin.text,DATA_URL_LOGIN));
        Debug.Log("ProcessRequest()");
    }

    IEnumerator ProcessRequest(string username, string password, string url, string email = null){
        checkAgain();
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);
        form.AddField("email", email == null ? "" : email);

        WWW request = new WWW(url,form);

        yield return request;


        if(string.IsNullOrEmpty(request.error)){
            string test = request.text;
            string[] test2 = test.Split(' ');  

            int id;
            bool validId = int.TryParse(test2[1],out id);
            int requestCode;
            bool validRequest = int.TryParse(test2[0],out requestCode);

            if(validRequest == false){
                requestCode = 6;
                StopAllCoroutines();
            }

            // 0 == Signup complete
            // 1 == Username already exist
            // 2 == Failed creating a user
            // 3 == Login Sucessful
            // 4 == Password invalid
            // 5 == Username invalid
            // 6 == Server off
            switch(requestCode){
                case 0:
                    loginPanel.SetActive(true);
                    userCreated.enabled = true;
                    signupPanel.SetActive(false);
                    break;
                case 1:
                    userExist.enabled = true;
                    break;
                case 2:
                    Debug.Log("Failed creating a user!");
                    break;
                case 3:
                    Debug.Log("Login Sucessful!");
                    // UserData userData = (UserData)ScriptableObject.CreateInstance(typeof(UserData));
                    UserConfig.Instance.userData.Username = username;
                    UserConfig.Instance.userData.Id = id;
                    // UserConfig.Instance.userData.Username = username;
                    // AssetDatabase.CreateAsset(userData,"Assets/UserData.asset");
                    // AssetDatabase.SaveAssets();

                    openCharacterSelection();

                    // SceneManager.LoadScene("Main");
                    break;
                case 4:
                    invalidPassword.enabled = true;
                    break;
                case 5:
                    InvalidUser.enabled = true;
                    break;
                case 6:
                    serverOff.enabled = true;
                    break;
            }
        }
    }

}
