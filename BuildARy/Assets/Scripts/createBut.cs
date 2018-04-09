using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Facebook.Unity;
using SocketIO;



public class createBut : MonoBehaviour
{
    public SocketIOComponent socket;
    public Button CreateButton;
    public Button LogoutButton;
    public Renderer rend;
    public Button joinGame;
    public Text name;
    public GameObject propic;
    private string fbName;
    private string email;
    private Texture2D profilePic;
    // Use this for inisstialization
    void Start()
    {
        CreateButton.GetComponent<Button>().onClick.AddListener(onclik);
        LogoutButton.GetComponent<Button>().onClick.AddListener(CallFBLogout);
        joinGame.GetComponent<Button>().onClick.AddListener(joinG);
        FB.API("me?fields=first_name", Facebook.Unity.HttpMethod.GET, GetFacebookData);
        FB.API("me?fields=email", Facebook.Unity.HttpMethod.GET, GetEmail);
        FB.API("me/picture", Facebook.Unity.HttpMethod.GET, GetPicture);
        socket.On("LOGIN", OnUserLogin);
    }
    void Update()
    {

    }
    void onclik()
    {
        SceneManager.LoadScene("category");
    }
        private void OnUserLogin(SocketIOEvent evt)
    {
        Debug.Log("Get the message from server is :" + evt.data);
    }
    void GetFacebookData(Facebook.Unity.IGraphResult result)
    {
        fbName = result.ResultDictionary["first_name"].ToString();
        name.text = "Hello!" + fbName;
        Debug.Log("fbName: " + fbName);
    }
    void GetEmail(Facebook.Unity.IGraphResult result)
    {
        email = result.ResultDictionary["email"].ToString();
        Debug.Log("email: " + email);
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["email"] = email;
        socket.Emit("LOGIN", new JSONObject(data));
    }
    private void GetPicture(IGraphResult result)
    {

        if (result.Texture != null)
        {
            Image img = propic.GetComponent<Image>();
            img.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 50, 50), new Vector2());
            Debug.Log("success:" + img);

        }
        else
        {
            Debug.Log("error:" + result.Error);
        }
    }
    private void CallFBLogout()
        {
            FB.LogOut();
        if (!FB.IsLoggedIn)
        {
            name.text = null;
            Debug.Log("logouted");
            SceneManager.LoadScene("login");
        }
        }
    void joinG()
    {
        SceneManager.LoadScene("join");
    }
}