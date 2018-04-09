using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class revision : MonoBehaviour {
    public RectTransform friendList;
    private RectTransform friendItem;
    public Hashtable arguments;
    // Use this for initialization
    void Start () {
        friendItem = Resources.Load("RevisionGame", typeof(RectTransform)) as RectTransform;

        for(int i = 0; i < 5; i++)
        {
            RectTransform friend =Instantiate(friendItem);
            friend.GetComponent<Button>().onClick.AddListener(delegate { onclick("gameId"); });
            friend.transform.GetChild(0).GetComponentInChildren<Text>().text = "Name: Alan alan";
            friend.transform.GetChild(1).GetComponentInChildren<Text>().text = " Alan type";
            friend.transform.GetChild(2).GetComponentInChildren<Text>().text = " Alan difficulty";
            friend.transform.GetChild(3).GetComponentInChildren<Text>().text = "Vocab: Alan alan";
            friend.transform.GetChild(4).GetComponentInChildren<Text>().text = "Date: Alan alan";
            friend.transform.SetParent(friendList, false);
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void onclick(string gameId)
    {
        arguments.Add("gameId", gameId);
        SceneManager.LoadScene("load", arguments);
    }
}
