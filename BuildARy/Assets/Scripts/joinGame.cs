using Facebook.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;

public class joinGame : MonoBehaviour {
    public RectTransform friendList;
    private RectTransform friendItem;
    public Hashtable arguments=new Hashtable();
    private RectTransform[] friend = new RectTransform[30];
    private Sprite[] pics = new Sprite[30];
    private int picCount = 0;
    public SocketIOComponent socket;
	public Button back;
    // Use this for initialization
  
    void Start()
    {
        arguments = SceneManager.GetSceneArguments();
        friendItem = Resources.Load("JoinItem", typeof(RectTransform)) as RectTransform;
        StartCoroutine(ConnectToServer());
        socket.On("FINDBYID", getUsers);
		back.GetComponent<Button> ().onClick.AddListener (backMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GetPicture(IGraphResult result)
    {

        if (result.Texture != null)
        {
            for (int i = 0; i < 5; i++)
            {
                friend[i].transform.GetChild(4).GetComponentInChildren<Image>().sprite = Sprite.Create(result.Texture, new Rect(0, 0, 50, 50), new Vector2());
                Debug.Log("success:" + picCount + result.Texture);
            }
        }
        else
        {
            Debug.Log("error:" + result.Error);
        }
    }
    public void getUsers(SocketIOEvent evt)
    {
        Debug.Log("test321:");
        JSONObject encodingObject = evt.data["data"][0]["id"];
        Debug.Log("test:" + encodingObject);
        for (int i = 0; i < evt.data["data"].Count; i++)
        {
            string path = evt.data["data"][i]["id"].ToString().Replace("\"","") + "/picture";
            FB.API(path, Facebook.Unity.HttpMethod.GET, GetPicture);
            friend[i] = Instantiate(friendItem);
            string oid=evt.data["data"][i]["_id"].ToString().Replace("\"", "");
            string vocab=evt.data["data"][i]["vocab"].ToString().Replace("\"", "");
            friend[i].GetComponent<Button>().onClick.AddListener(delegate { onclick(oid,vocab); });
            friend[i].transform.GetChild(0).GetComponentInChildren<Text>().text = "Name:"+ evt.data["data"][i]["name"].ToString().Replace("\"", "");
            friend[i].transform.GetChild(1).GetComponentInChildren<Text>().text = "Type:"+ evt.data["data"][i]["category"].ToString().Replace("\"", "");
            friend[i].transform.GetChild(2).GetComponentInChildren<Text>().text = "Difficulty:"+ evt.data["data"][i]["diff"].ToString().Replace("\"", "");
            friend[i].transform.GetChild(3).GetComponentInChildren<Text>().text = "date:"+ evt.data["data"][i]["createtime"].ToString().Replace("\"", ""); 

            friend[i].transform.SetParent(friendList, false);
        }
    }
    IEnumerator ConnectToServer()
    {
        yield return new WaitForSeconds(0.5f);
        socket.Emit("USER_CONNECT");
        yield return new WaitForSeconds(1f);
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["id"] = arguments["userId"].ToString();
        JSONObject userid = new JSONObject(data);
        Debug.Log("test3:" + userid);
        socket.Emit("FINDBYID", userid);
        Debug.Log("wrong");
        
    }
<<<<<<< HEAD
	void backMenu()
	{
		SceneManager.LoadScene ("menu");
	}
=======
    void onclick(string gameId, string vocab)
    {
        arguments.Add("gameId", gameId);
        arguments.Add("vocab", vocab);
        arguments.Add("userid", arguments["userId"].ToString());
        Debug.Log(gameId + "," + vocab + "," + arguments["userId"]);
        SceneManager.LoadScene("guess", arguments); 
    }
>>>>>>> 06aa3218297276bd58dce998855a79771e380685
}

