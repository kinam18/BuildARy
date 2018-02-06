using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Facebook.Unity;



public class createBut : MonoBehaviour
{
    public Button CreateButton;
    public Button LogoutButton;
    public Renderer rend;
    public Text name;
    public GameObject propic;
    public string fbName;
    private Texture2D profilePic;
    // Use this for inisstialization
    void Start()
    {
        CreateButton.GetComponent<Button>().onClick.AddListener(onclik);
        LogoutButton.GetComponent<Button>().onClick.AddListener(CallFBLogout);
        FB.API("me?fields=first_name", Facebook.Unity.HttpMethod.GET, GetFacebookData);
        FB.API("me/picture", Facebook.Unity.HttpMethod.GET, GetPicture);
    }
    void Update()
    {

    }
    void onclik()
    {
        SceneManager.LoadScene("word");
    }
    void GetFacebookData(Facebook.Unity.IGraphResult result)
    {
        fbName = result.ResultDictionary["first_name"].ToString();
        name.text = "Hello!" + fbName;
        Debug.Log("fbName: " + fbName);
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
}