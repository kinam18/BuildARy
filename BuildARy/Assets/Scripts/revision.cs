using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;

public class revision : MonoBehaviour {
    public RectTransform friendList;
    private RectTransform friendItem;
    public Hashtable arguments=new Hashtable();
    public SocketIOComponent socket;
    private RectTransform[] friend = new RectTransform[30];
	public Button back;

    // Use this for initialization
    void Start () {
        friendItem = Resources.Load("RevisionGame", typeof(RectTransform)) as RectTransform;
        arguments = SceneManager.GetSceneArguments();
        StartCoroutine(ConnectToServer());
        socket.On("REVISION", getUsers);
		back.GetComponent<Button> ().onClick.AddListener (backHome);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void onclick(string gameId, string vocab)
    {
        arguments.Add("gameId", gameId);
        arguments.Add("vocab", vocab);
        SceneManager.LoadScene("revisionGame", arguments);
    }
    IEnumerator ConnectToServer()
    {
        yield return new WaitForSeconds(1f);
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["id"] = arguments["userId"].ToString();
        JSONObject userid = new JSONObject(data);
        Debug.Log("test3:" + userid);
        socket.Emit("REVISION", userid);
        Debug.Log("wrong");

    }
    public void getUsers(SocketIOEvent evt)
    {
        Debug.Log("test321:");
        JSONObject encodingObject = evt.data["data"][0]["id"];
        Debug.Log("test:" + encodingObject);
        for (int i = 0; i < evt.data["data"].Count; i++)
        {
            friend[i] = Instantiate(friendItem);
            string oid = evt.data["data"][i]["_id"].ToString().Replace("\"", "");
            string vocab = evt.data["data"][i]["vocab"].ToString().Replace("\"", "");
            friend[i].GetComponent<Button>().onClick.AddListener(delegate { onclick(oid, vocab); });
            friend[i].transform.GetChild(0).GetComponentInChildren<Text>().text = "Name:" + evt.data["data"][i]["name"].ToString().Replace("\"", "");
            friend[i].transform.GetChild(1).GetComponentInChildren<Text>().text = "Type:" + evt.data["data"][i]["category"].ToString().Replace("\"", "");
            friend[i].transform.GetChild(2).GetComponentInChildren<Text>().text = "Difficulty:" + evt.data["data"][i]["diff"].ToString().Replace("\"", "");
            friend[i].transform.GetChild(3).GetComponentInChildren<Text>().text = "Word:" + evt.data["data"][i]["vocab"].ToString().Replace("\"", "");
            friend[i].transform.GetChild(4).GetComponentInChildren<Text>().text = "date:" + evt.data["data"][i]["createtime"].ToString().Replace("\"", "");

            friend[i].transform.SetParent(friendList, false);
        }
    }
	void backHome()
	{
		SceneManager.LoadScene ("menu");
	}
}
