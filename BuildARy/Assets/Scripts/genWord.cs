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
    public string[] difw = { "dif", "fic", "ult" };
    public string[] medw = { "me", "di", "um" };
    public string[] easw = { "e", "a", "s", "y" };
    public int randomdif;
    public int randommed;
    public int randomeas;
    Hashtable arguments = new Hashtable();
    
    // Use this for initialization
    void Start() {
        arguments.Add("key","value");
        System.Random random = new System.Random();
        randomdif = random.Next(0, 3);
        randommed = random.Next(0, 3);
        randomeas = random.Next(0, 4);
        GameObject.Find("difButton").GetComponentInChildren<Text>().text = difw[randomdif];
        GameObject.Find("medButton").GetComponentInChildren<Text>().text = medw[randommed];
        GameObject.Find("easButton").GetComponentInChildren<Text>().text = easw[randomeas];
        dif.GetComponent<Button>().onClick.AddListener(delegate { onclick(difw[randomdif]); });
        med.GetComponent<Button>().onClick.AddListener(delegate { onclick(medw[randommed]); });
        eas.GetComponent<Button>().onClick.AddListener(delegate { onclick(easw[randomeas]); });
    }

    // Update is called once per frame
    void Update() {

    }
    void onclick(String word) {
        SceneManager.LoadScene("game", arguments);
        Debug.Log(word);
    }
}
