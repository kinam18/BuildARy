using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Facebook.Unity;
using Facebook.MiniJSON;
using SocketIO;

public class join : MonoBehaviour {
	public SocketIOComponent socket;
	public GameObject prefabButton;
	public RectTransform ParentPanel;
	public GameObject pic;
	private bool isDone = false;
	private Text name;
	public List<object> friends = new List<object>();
	public List<object> picture = new List<object>();
	public Dictionary<string, string> friend = new Dictionary<string, string>();
	public Dictionary<int, string> friendlist = new Dictionary<int, string>();
	public Dictionary<string, object> propic = new Dictionary<string, object> ();
	private string url;
	private WWW www;
	// Use this for initialization
	void Start () {
		FB.API("me?fields=friends{id,name,picture}", Facebook.Unity.HttpMethod.GET, GetFriend);
		if(isDone){
		foreach (KeyValuePair<string,string> entry in friend) {
			GameObject goButton = (GameObject)Instantiate (prefabButton);
			goButton.transform.SetParent (ParentPanel, false);
			goButton.transform.localScale = new Vector3 (1, 1, 1);

			Button temp = goButton.GetComponent<Button> ();
			Debug.Log ("id:" + entry.Key);
			name.text = entry.Value;
			www = new WWW (url);
			Image img = pic.GetComponent<Image>();
			img.material.mainTexture = www.texture;
			int x = 0;
			int.TryParse (entry.Key, out x);
				temp.onClick.AddListener (() => ButtonClicked (x));
		}
	}
		socket.On("LOGIN", OnUserLogin);
	}
	
	// Update is called once per frame
	private void OnUserLogin(SocketIOEvent evt)
	{
		Debug.Log("Get the message from server is :" + evt.data);
	}
	void ButtonClicked(int buttonNo){
		Debug.Log ("Button clicked = " + buttonNo);
	}

	private void GetFriend(IGraphResult result)
	{
		if (result.Error != null) 
		{
			Debug.Log ("Error: " + result.Error);
		}

		Dictionary<string, object> dict = Json.Deserialize(result.RawResult) as Dictionary<string, object>;

		object friendsH;
		string friendName;

		if (dict.TryGetValue("friends", out friendsH))
		{

			friends = (List<object>)(((Dictionary<string, object>)friendsH)["data"]);
			if (friends.Count > 0)
			{
				Dictionary<string, object> friendDict = ((Dictionary<string, object>)(friends[0]));
				friend["id"] = (string)friendDict["id"];
				friend["name"] = (string)friendDict["name"];
				Debug.Log("name:" + friend["name"]);
				propic ["id"] = (string)friendDict ["id"];
				propic ["picture"] = friendDict ["picture"];
				picture = (List<object>)(((Dictionary<string, object>)propic) ["data"]);
				if (picture.Count > 0)
				{
					url = (string)picture[2];
					Debug.Log ("url:" + url);
			    }
		    }

	}
		isDone = true;
}
}
