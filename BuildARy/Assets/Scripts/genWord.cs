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
    Hashtable arguments;
    Hashtable arguments2 = new Hashtable();
    // Use this for initialization
    void Start() {
        arguments = SceneManager.GetSceneArguments();
        GameObject.Find("difButton").GetComponentInChildren<Text>().text = ((String)arguments["dif"]);
        GameObject.Find("medButton").GetComponentInChildren<Text>().text = ((String)arguments["med"]);
        GameObject.Find("easButton").GetComponentInChildren<Text>().text = ((String)arguments["easy"]);
        dif.GetComponent<Button>().onClick.AddListener(delegate { onclick((String)arguments["dif"]); });
        med.GetComponent<Button>().onClick.AddListener(delegate { onclick((String)arguments["med"]); });
        eas.GetComponent<Button>().onClick.AddListener(delegate { onclick((String)arguments["easy"]); });
    }
    // Update is called once per frame
    void Update() {

    }
    void onclick(String word) {
        arguments2.Add("vocab", word);
        SceneManager.LoadScene("game", arguments2);
        Debug.Log(word);
    }
  
}
