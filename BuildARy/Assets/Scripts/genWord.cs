using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;


public class genWord : MonoBehaviour {
    public Button dif;
    public Button med;
    public Button eas;
	public Button back;
    public Text userDefin;
    public Button submit;
    public RectTransform tranlsteP;
    public Button showTran;
    Hashtable arguments=new Hashtable();
      // Use this for initialization
    void Start() {
       
        arguments = SceneManager.GetSceneArguments();
        GameObject.Find("difButton").GetComponentInChildren<Text>().text = ((String)arguments["dif"]);
        GameObject.Find("medButton").GetComponentInChildren<Text>().text = ((String)arguments["med"]);
        GameObject.Find("easButton").GetComponentInChildren<Text>().text = ((String)arguments["easy"]);
        dif.GetComponent<Button>().onClick.AddListener(delegate { onclick((String)arguments["dif"],"difficult"); });
        med.GetComponent<Button>().onClick.AddListener(delegate { onclick((String)arguments["med"], "medium"); });
        eas.GetComponent<Button>().onClick.AddListener(delegate { onclick((String)arguments["easy"],"easy"); });
		//back.GetComponent<Button> ().onClick.AddListener (backHome);
        submit.GetComponent<Button>().onClick.AddListener(delegate { onclick(userDefin.text, "custom"); });
        showTran.GetComponent<Button>().onClick.AddListener(showTranslateP);

    }
    // Update is called once per frame
    void Update() {

    }
    void onclick(String word,String difty) {
        if(word != "") 
        { 
            arguments.Add("vocab", word);
            arguments.Add("diff", difty);
            arguments.Add("checkNewGame", "true");
            SceneManager.LoadScene("game", arguments);
            Debug.Log(word);
        }
    }
    
	void backHome()
	{
		SceneManager.LoadScene ("menu");
	}
    void showTranslateP()
    {
        tranlsteP.gameObject.SetActive(true);
    }
}
