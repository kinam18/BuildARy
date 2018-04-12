using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;

public class translate : MonoBehaviour {
    public RectTransform translatP;
    public Text result;
    public Dropdown chieng;
    public Button ok;
    public SocketIOComponent socket;
    public Text input;
    public Button close;

	// Use this for initialization
	void Start () {
        translatP.gameObject.SetActive(false);
        ok.GetComponent<Button>().onClick.AddListener(submit);
        close.GetComponent<Button>().onClick.AddListener(closeP);
        socket.On("TOENGLISH", res);
        socket.On("TOCHINESE", res);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void submit()
    {
        Debug.Log("test:" + chieng.GetComponent<Dropdown>().value.ToString());
        if (chieng.GetComponent<Dropdown>().value == 1)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["word"] = input.text.ToString();
            JSONObject word = new JSONObject(data);
            socket.Emit("TOENGLISH", word);
        }
        else
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["word"] = input.text.ToString();
            JSONObject word = new JSONObject(data);
            socket.Emit("TOCHINESE", word);
        }
    }
    void res(SocketIOEvent evt)
    {
        Debug.Log(evt.data["result"].ToString());
        Debug.Log("2:"+evt.data);
        result.text= "Result: "+evt.data["result"].ToString();
    }
   void closeP()
    {
        translatP.gameObject.SetActive(false);
    }
}
