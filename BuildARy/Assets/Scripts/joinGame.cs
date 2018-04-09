using Facebook.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class joinGame : MonoBehaviour {
    public RectTransform friendList;
    private RectTransform friendItem;
    public Hashtable arguments;
    private RectTransform[] friend = new RectTransform[30];
    private Sprite[] pics = new Sprite[30];
    private int picCount = 0;
    // Use this for initialization
    void Start()
    {
        friendItem = Resources.Load("JoinItem", typeof(RectTransform)) as RectTransform;

        for (int i = 0; i < 5; i++)
        {
            FB.API("2094353304181590/picture", Facebook.Unity.HttpMethod.GET, GetPicture);
            friend[i] = Instantiate(friendItem);
            friend[i].GetComponent<Button>().onClick.AddListener(delegate { onclick("gameId"); });
            friend[i].transform.GetChild(0).GetComponentInChildren<Text>().text = "Name: Alan alan";
            friend[i].transform.GetChild(1).GetComponentInChildren<Text>().text = " Alan type";
            friend[i].transform.GetChild(2).GetComponentInChildren<Text>().text = " Alan difficulty";
            friend[i].transform.GetChild(3).GetComponentInChildren<Text>().text = "date: Alan alan";
            
            friend[i].transform.SetParent(friendList, false);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
    void onclick(string gameId)
    {
        arguments.Add("gameId", gameId);
        SceneManager.LoadScene("load", arguments);
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
}

