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
    public Button revision;
    public Button loadGame;
    public Text name;
    public GameObject propic;
    private string fbName;
    private string id;
    private Texture2D profilePic;
	private double score = 77;
	public Slider scoreBar;
	public Text scoreText;
	public Text levelText;
    public Hashtable arguments=new Hashtable();
    // Use this for inisstialization
    void Start()
    {
        CreateButton.GetComponent<Button>().onClick.AddListener(onclik);
        LogoutButton.GetComponent<Button>().onClick.AddListener(CallFBLogout);
        joinGame.GetComponent<Button>().onClick.AddListener(joinG);
        revision.GetComponent<Button>().onClick.AddListener(reviG);
        loadGame.GetComponent<Button>().onClick.AddListener(loadG);
        FB.API("me?fields=first_name", Facebook.Unity.HttpMethod.GET, GetFacebookData);
        FB.API("me?fields=id", Facebook.Unity.HttpMethod.GET, GetId);
        FB.API("me/picture", Facebook.Unity.HttpMethod.GET, GetPicture);
        socket.On("LOGIN", OnUserLogin);
		ScoreCalculation ();
		//scoreBarLen = Screen.width / 2;
    }
    void Update()
    {
		//ScoreCalculation (0);
    }

	/*void OnGUI()
	{
		GUI.Box (new Rect (50, 50, scoreBarLen, 20),score + "/" + maxScore);
	}*/
	public void ScoreCalculation(){
		if (score >= 10 && score < 30) {
			levelText.text = "Level 2";
			scoreBar.GetComponentInChildren<Text> ().text = score + "/" + "30";
			scoreBar.value = ((float)score/30.0f) * 100.0f;
		} else if (score >= 30 && score < 60) {
			levelText.text = "Level 3";
			scoreBar.GetComponentInChildren<Text> ().text = score + "/" + "60";
			scoreBar.value = ((float)score/60.0f) * 100.0f;
		} else if (score >= 60 && score < 100) {
			levelText.text = "Level 4";
			scoreBar.GetComponentInChildren<Text> ().text = score + "/" + "100";
			scoreBar.value = ((float)score/100.0f) * 100.0f;
		} else if (score >= 100) {
			levelText.text = "Level 5";
			scoreBar.GetComponentInChildren<Text> ().text = score + "/" + "200";
			scoreBar.value = ((float)score/200.0f) * 100.0f;
		} else {
			levelText.text = "Level 1";
			scoreBar.GetComponentInChildren<Text> ().text = score + "/" + "10";
			scoreBar.value = ((float)score/10.0f) * 100.0f;
		}

		Debug.Log (scoreBar.value.ToString());
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
    void GetId(Facebook.Unity.IGraphResult result)
    {
        id = result.ResultDictionary["id"].ToString();
        Debug.Log("email: " + id);
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["id"] = id;
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
        arguments.Add("userId", id);
        SceneManager.LoadScene("join",arguments);
    }
    void reviG()
    {
        arguments.Add("userId", id);
        SceneManager.LoadScene("revision", arguments);
    }
    void loadG()
    {
        arguments.Add("userId", id);
        SceneManager.LoadScene("continueGame", arguments);
    }
}