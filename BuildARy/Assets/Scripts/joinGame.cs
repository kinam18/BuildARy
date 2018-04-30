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
    private int picCount = 0;
    public SocketIOComponent socket;
	public Button back;
    private int countFd;
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

    
    public void getUsers(SocketIOEvent evt)
    {
        Debug.Log("test321:");
        JSONObject encodingObject = evt.data["data"][0]["id"];
        Debug.Log("test:" + encodingObject);
        countFd=evt.data["data"].Count;
        Debug.Log("user:" + countFd);
        for (int i = 0; i < evt.data["data"].Count; i++)
        {
            string path = evt.data["data"][i]["id"].ToString().Replace("\"","") + "/picture";
            friend[i] = Instantiate(friendItem);
            string oid=evt.data["data"][i]["_id"].ToString().Replace("\"", "");
            string vocab=evt.data["data"][i]["vocab"].ToString().Replace("\"", "");
            string diff = evt.data["data"][i]["diff"].ToString().Replace("\"", "");
            string category = evt.data["data"][i]["category"].ToString().Replace("\"", "");
            friend[i].GetComponent<Button>().onClick.AddListener(delegate { onclick(oid,vocab,diff,category); });
            friend[i].transform.GetChild(0).GetComponentInChildren<Text>().text = "From:"+ evt.data["data"][i]["name"].ToString().Replace("\"", "");
            friend[i].transform.GetChild(1).GetComponentInChildren<Text>().text = "Type:"+ evt.data["data"][i]["category"].ToString().Replace("\"", "");
            friend[i].transform.GetChild(2).GetComponentInChildren<Text>().text = "Difficulty:"+ evt.data["data"][i]["diff"].ToString().Replace("\"", "");
            friend[i].transform.GetChild(3).GetComponentInChildren<Text>().text = evt.data["data"][i]["createtime"].ToString().Replace("\"", "");
           
            friend[i].transform.SetParent(friendList, false);
        }
    }
    IEnumerator ConnectToServer()
    {
        yield return new WaitForSeconds(1f);
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["id"] = arguments["userId"].ToString();
        JSONObject userid = new JSONObject(data);
        Debug.Log("test3:" + userid);
        socket.Emit("FINDBYID", userid);
        Debug.Log("wrong");
        
    }
	void backMenu()
	{
		SceneManager.LoadScene ("menu");
	}
    void onclick(string gameId, string vocab,string diff,string category)
    {
        arguments.Add("gameId", gameId);
        arguments.Add("vocab", vocab);
        arguments.Add("diff", diff);
        arguments.Add("category", category);
        arguments.Add("userid", arguments["userId"].ToString());
        Debug.Log(gameId + "," + vocab + "," + arguments["userId"]);
        SceneManager.LoadScene("guess", arguments); 
    }
}

