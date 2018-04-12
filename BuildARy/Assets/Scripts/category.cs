using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class category : MonoBehaviour {
    public Button cat1;
    public Button cat2;
    public Button cat3;
	public Button back;
    Hashtable arguments = new Hashtable();
    // Use this for initialization
    void Start () {
        cat1.GetComponent<Button>().onClick.AddListener(delegate { onclick("food"); });
        cat2.GetComponent<Button>().onClick.AddListener(delegate { onclick("sports"); });
        cat3.GetComponent<Button>().onClick.AddListener(delegate { onclick("place"); });
		back.GetComponent<Button> ().onClick.AddListener (backHome);
    }
	
	// Update is called once per frame
	void Update () {
        
    }
    void onclick(string word)
    {
        arguments.Add("category", word);
        SceneManager.LoadScene("loadword", arguments);
        Debug.Log(word);
    }
	void backHome()
	{
		SceneManager.LoadScene ("menu");
	}
}
