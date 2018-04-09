using Facebook.MiniJSON;
using Facebook.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class invite : MonoBehaviour {
    private RectTransform friendPanel;
    public RectTransform friendsPa;
    public List<object> friends = new List<object>();
    public Dictionary<string, string> friend = new Dictionary<string, string>();
    public Dictionary<string, object> propic = new Dictionary<string, object>();
    private RectTransform[] friendP = new RectTransform[30];
    private Button[] friendB = new Button[30];
    private int count = 0;
    public Button invi;
    private List<string> tfb = new List<string>();
    private string[] inviteID=new string[30];
    // Use this for initialization
    void Start () {
        FB.API("me?fields=friends{id,name,picture}", Facebook.Unity.HttpMethod.GET, GetFriend);
        friendPanel = Resources.Load("Panel", typeof(RectTransform)) as RectTransform;
        invi.GetComponent<Button>().onClick.AddListener(inviteFriend);
        tfb.Add("2094353304181590");
    }
	
	// Update is called once per frame
	void Update () {
       
    }
    private void GetFriend(IGraphResult result)
    {
        if (result.Error != null)
        {
            Debug.Log("Error: " + result.Error);
        }

        Dictionary<string, object> dict = Json.Deserialize(result.RawResult) as Dictionary<string, object>;
        Debug.Log("fdsf:" + result.Texture);
        object friendsH;

        if (dict.TryGetValue("friends", out friendsH))
        {
            
            friends = (List<object>)(((Dictionary<string, object>)friendsH)["data"]);
            for (int i = 0; i < friends.Count; i++)
            {
                Dictionary<string, object> friendDict = ((Dictionary<string, object>)(friends[i]));
                string idq = "id" + i;
                string nameq = "name" + i;
                friend[idq] = (string)friendDict["id"];
                friend[nameq] = (string)friendDict["name"];
                Debug.Log("name:" + friend[nameq]);
                propic[idq] = (string)friendDict["id"];
                Debug.Log("id:" + propic[idq]);
                
             
            }

        }
        for (int i = 0; i < friend.Count/2; i++)
        {
            string idq = "id" + i;
            string nameq = "name" + i;
            friendP[i] = (RectTransform)Instantiate(friendPanel);
            friendP[i].GetComponentInChildren<Text>().text = friend[nameq];
            Debug.Log("idname:" + i+friendP[i].GetComponentInChildren<Text>().text);
            FB.API(friend[idq]+"/picture", Facebook.Unity.HttpMethod.GET, GetPicture);
            friendP[i].transform.SetParent(friendsPa, false);
            //StartCoroutine(fetchpic(url));
        }

    }
    private void GetPicture(IGraphResult result)
    {

        if (result.Texture != null)
        {

            friendP[count].transform.GetChild(0).GetComponent<Image>().sprite = Sprite.Create(result.Texture, new Rect(0, 0, 50, 50), new Vector2());
            
            Debug.Log("success:" + friendP[count].transform.GetChild(0).GetComponent<Image>().sprite);
            
            count++;
        }
        else
        {
            Debug.Log("error:" + result.Error);
        }
    }
    void inviteFriend()
    {
        FB.AppRequest(
            "ALan,Here is a fre e gift!",
            null,
            new List<object>() { "app_users" },
            null, null, null, null,
            delegate (IAppRequestResult result) {
                Debug.Log(result.RawResult);
            }
        );
        int a = 0;
        for (int i=0;i< friendP.Length; i++)
        {
            string idq = "id" + i;
            if (friendP[i].transform.GetChild(2).GetComponent<Toggle>().isOn)
            {
                Debug.Log("hi:" + friend[idq]);
                inviteID[a] = friend[idq];
                   a++;
            }
        }
        
    }

}
