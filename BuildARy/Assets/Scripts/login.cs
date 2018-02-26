using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Facebook.Unity;
using SocketIO;




public class login : MonoBehaviour {

    // Use this for initialization
    public SocketIOComponent socket;
    public Button loginButton;
    public Boolean label=false;
    int time_int=5;
    void Start () {
        loginButton = GetComponent<Button> ();
        loginButton.onClick.AddListener(onclik);
        Awake();
        StartCoroutine(ConnectToServer());
        socket.On("USER_CONNECTED", OnUserConnected);
    }

    IEnumerator ConnectToServer() {
        yield return new WaitForSeconds(0.5f);
        socket.Emit("USER_CONNECT");
        yield return new WaitForSeconds(1f);
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["category"] = "food";
        socket.Emit("GENERATOR",new JSONObject(data));
    }

    private void OnUserConnected(SocketIOEvent evt) {
        Debug.Log("Get the message from server is :" + evt.data);
    }

	// Update is called once per frame
	void Update () {
        
    }
    void onclik()
    {
        /*InputField email = GameObject.Find("EmailInput").GetComponent<InputField>();
        InputField password = GameObject.Find("PasswordInput").GetComponent<InputField>();
        Debug.Log("emai:"+email.text+",password:"+password.text);
        print("success");
        if (password.text != "alan")
        {
            InvokeRepeating("timer", 1, 1);
            label = true;
        }
        else {
            SceneManager.LoadScene("menu");
        }*/
        List<string> perms = new List<string>() { "public_profile", "email", "user_friends" };
        FB.LogInWithReadPermissions(perms, AuthCallback);

    }
    void timer()
    {
        time_int -= 1;
        if (time_int == 0)
        {
            label = false;
            CancelInvoke("timer");
        }

    }
    void OnGUI()
    {
        if (label)
        GUI.Label(new Rect(640, 350, 150, 20), "error password");
    }
    void Awake()
    {
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }
    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            // AccessToken class will have session details
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            // Print current access token's User ID
            Debug.Log(aToken.UserId);
            label = true;
            // Print current access token's granted permissions
            foreach (string perm in aToken.Permissions)
            {
                Debug.Log(perm);
            }
            SceneManager.LoadScene("menu");
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }
}
