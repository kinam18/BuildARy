using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;
public class continueGame : MonoBehaviour {

    public RectTransform friendList;
    private RectTransform friendItem;
    public Hashtable arguments = new Hashtable();
    public SocketIOComponent socket;
    private RectTransform[] friend = new RectTransform[30];
	public Button back;

    // Use this for initialization
    void Start()
    {
        friendItem = Resources.Load("continueGame", typeof(RectTransform)) as RectTransform;
        arguments = SceneManager.GetSceneArguments();
        StartCoroutine(ConnectToServer());
        socket.On("LOADGAMELIST", getUsers);
		back.GetComponent<Button> ().onClick.AddListener (backHome);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void onclick(string gameId, string vocab,string diff,string category)
    {
        arguments.Add("gameId", gameId);
        arguments.Add("vocab", vocab);
        arguments.Add("diff", diff);
        arguments.Add("category", category);
        arguments.Add("checkNewGame", "false");
        SceneManager.LoadScene("game", arguments);
    }
    IEnumerator ConnectToServer()
    {
        yield return new WaitForSeconds(1f);
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["id"] = arguments["userId"].ToString();
        JSONObject userid = new JSONObject(data);
        Debug.Log("test3:" + userid);
        socket.Emit("LOADGAMELIST", userid);
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
            string diff = evt.data["data"][i]["diff"].ToString().Replace("\"", "");
            string category = evt.data["data"][i]["category"].ToString().Replace("\"", "");
            friend[i].GetComponent<Button>().onClick.AddListener(delegate { onclick(oid, vocab, diff, category); });
            friend[i].transform.GetChild(0).GetComponentInChildren<Text>().text = "Type:" + evt.data["data"][i]["category"].ToString().Replace("\"", "");
            friend[i].transform.GetChild(1).GetComponentInChildren<Text>().text = "Difficulty:" + evt.data["data"][i]["diff"].ToString().Replace("\"", "");
            friend[i].transform.GetChild(2).GetComponentInChildren<Text>().text = "Word:" + evt.data["data"][i]["vocab"].ToString().Replace("\"", "");
            friend[i].transform.GetChild(3).GetComponentInChildren<Text>().text =   evt.data["data"][i]["createtime"].ToString().Replace("\"", "");

            friend[i].transform.SetParent(friendList, false);
        }
    }
	void backHome()
	{
		SceneManager.LoadScene ("menu");
	}
}
