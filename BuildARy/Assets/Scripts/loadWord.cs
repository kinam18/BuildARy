using SocketIO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadWord : MonoBehaviour {
    public SocketIOComponent socket;
    Hashtable arguments;
    // Use this for initialization
    void Start () {
        StartCoroutine(ConnectToServer());
        socket.On("GENERATOR", GetWord);
        arguments = SceneManager.GetSceneArguments();
    }
	
	// Update is called once per frame
	void Update () {
        while ((String)arguments["easy"] != null) {
            SceneManager.LoadScene("word", arguments);
            break;
        }
	}
    IEnumerator ConnectToServer()
    {
        yield return new WaitForSeconds(0.5f);
        socket.Emit("USER_CONNECT");
        yield return new WaitForSeconds(1f);
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["category"] = (String)arguments["category"];
        socket.Emit("GENERATOR", new JSONObject(data));
    }
    private void GetWord(SocketIOEvent evt)
    {

        JSONObject encodingObject = evt.data;
        Debug.Log(encodingObject.GetField("easy"));
        Debug.Log(encodingObject.GetField("medium"));
        Debug.Log(encodingObject.GetField("difficult"));
        Debug.Log("encode" + encodingObject.type);
        String dif = ("" + encodingObject.GetField("difficult")).Substring(1, ("" + encodingObject.GetField("difficult")).Length - 2);
        String med = ("" + encodingObject.GetField("medium")).Substring(1, ("" + encodingObject.GetField("medium")).Length - 2);
        String easy = ("" + encodingObject.GetField("easy")).Substring(1, ("" + encodingObject.GetField("easy")).Length - 2);
        arguments.Add("dif", dif);
        arguments.Add("med", med);
        arguments.Add("easy", easy);
    }
}
