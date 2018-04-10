﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;


public class genWord : MonoBehaviour {
    public Button dif;
    public Button med;
    public Button eas;
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
    }
    // Update is called once per frame
    void Update() {

    }
    void onclick(String word,String difty) {
        arguments.Add("vocab", word);
        arguments.Add("diff", difty);
        SceneManager.LoadScene("game", arguments);
        Debug.Log(word);
    }
    

}
