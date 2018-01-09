using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class login : MonoBehaviour {

    // Use this for initialization
    public Button loginButton;
    public Boolean label=false;
    int time_int=5;
    void Start () {
        loginButton = GetComponent<Button> ();
        loginButton.onClick.AddListener(() =>  onclik());
    }
	
	// Update is called once per frame
	void Update () {
        
    }
    void onclik()
    {
        InputField email = GameObject.Find("EmailInput").GetComponent<InputField>();
        InputField password = GameObject.Find("PasswordInput").GetComponent<InputField>();
        Debug.Log("emai:"+email.text+",password:"+password.text);
        print("success");
        if (password.text != "alan")
        {
            InvokeRepeating("timer", 1, 1);
            label = true;
        }
        else {
        }

    }
    void timer()
    {
        time_int -= 1;
        if (time_int == 0)
        {
            label = false;
            CancelInvoke("timer");
        }

    }
    void OnGUI()
    {
        if (label)
        GUI.Label(new Rect(640, 350, 150, 20), "error password");
    }
}
