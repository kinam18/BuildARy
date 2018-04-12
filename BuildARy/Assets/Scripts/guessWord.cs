﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SocketIO;
using System;

public class guessWord : MonoBehaviour {

    private string word = "";
	private string difficulty = "medium";
    public RectTransform tranlsteP;
    public RectTransform guess;
	public GameObject panel;
    public GameObject guessSize;
    public RectTransform random;
    private Button guessB;
    private int randomNumber;
    private string randomWord;
    private string finalWord;
    private string wordSelected=null;
    public Button submit;
    public Canvas popup;
    private string[] guessAnsArr=new string[30];
    private string guessAns;
    public Button check;
    private string answer = "";
    private bool checkedans = false;
    public Text checkText;
    private int selectCount = 0;
	private double score = 97;
	public Slider scoreBar;
	public Text scoreText;
	public Text levelText;
    private JSONObject gameId;
    private Hashtable arguments = new Hashtable();
    public SocketIOComponent socket;
    private JSONObject saveData2;
    private JSONObject finalData;
    private GameObject blockPrefab;
    public GameObject go;
    public Block[,,] blocks = new Block[20, 20, 20];
    private string blockColor = "White";
    public Button showTran;
    // Use this for initialization
    void Start () {
        arguments = SceneManager.GetSceneArguments();
        
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["_id"] = arguments["gameId"].ToString();
        word = arguments["vocab"].ToString();
        gameId = new JSONObject(data);
        Debug.Log("word:"+word);
        StartCoroutine(ConnectToServer());
        socket.On("GETWITHDATA", getGameData);
        submit.GetComponent<Button>().onClick.AddListener(popUp);
        check.GetComponent<Button>().onClick.AddListener(checkAnswer);
        showTran.GetComponent<Button>().onClick.AddListener(showTranslateP);
        panel = GameObject.Find("guessPanel");

        for (int i = 0; i < 5; i++)
        {
            if (i == 0)
            {
                randomWord = word + (char)('a' + UnityEngine.Random.Range(0, 26));
            }
            else
            {
                randomWord = randomWord + (char)('a' + UnityEngine.Random.Range(0, 26));
            }
        }
        int first = word.IndexOf(" ");
        int second = word.IndexOf(" ", first + 1);
        int third = word.IndexOf("-");
        randomWord = randomWord.Replace(" ", "");
        randomWord = randomWord.Replace("-","");
        Debug.Log("ewqe:" + randomWord);
        string test = randomWord;
        int len = randomWord.Length-1 ;
        for (int i = 0; i < test.Length; i++)
        {
            int num = UnityEngine.Random.Range(0, len-i);
            finalWord = finalWord + randomWord[num];
            randomWord = randomWord.Remove(num, 1);
        }
        guessB = Resources.Load("Button", typeof(Button)) as Button;
        for (int i = 0; i < word.Length; i++) {
            if (i == first || i == second) {
                Button goButton = Instantiate(guessB);
                goButton.transform.SetParent(guess, false);
                goButton.enabled = false;
                goButton.GetComponentInChildren<CanvasRenderer>().SetAlpha(0);
                goButton.GetComponentInChildren<Text>().color = Color.clear;
            }
            else if (i==third)
            {
                Button goButton = Instantiate(guessB);
                goButton.transform.SetParent(guess, false);
                goButton.GetComponentInChildren<Text>().text = "-";

            }
            else
            {
                Button goButton = Instantiate(guessB);
                goButton.transform.SetParent(guess, false);
                goButton.onClick.AddListener(delegate { changeWord(goButton); });
            }
        }
        
        for (int i = 0; i < test.Length; i++)
        {
                Button goButton = Instantiate(guessB);
                goButton.transform.SetParent(random, false);
                goButton.GetComponentInChildren<Text>().text = "" + finalWord[i];
                goButton.onClick.AddListener(delegate { selectWord(goButton); });
        }

        Debug.Log("rand:" + test);
        Debug.Log("rand:" + finalWord);
        Debug.Log(guessSize.GetComponentInChildren<Button>());
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void changeWord(Button change)
    {

        if (wordSelected != null)
        {
            change.GetComponentInChildren<Text>().text = wordSelected;
            wordSelected = null;
            selectCount = 0;
        }
        else if (change.GetComponentInChildren<Text>().text !="")
        {
            Button goButton = Instantiate(guessB);
            goButton.transform.SetParent(random, false);
            goButton.onClick.AddListener(delegate { selectWord(goButton); });
            goButton.GetComponentInChildren<Text>().text = change.GetComponentInChildren<Text>().text;
            change.GetComponentInChildren<Text>().text = "";
        }
            


       
        
    }
    void selectWord(Button select)
    {
        if (selectCount == 0)
        {
            wordSelected = select.GetComponentInChildren<Text>().text;
            select.gameObject.SetActive(false);
            selectCount = 1;
        }
    }
    void popUp() {
        popup.enabled = true;
		panel.gameObject.SetActive (false);
        foreach (Transform element in guess)
        {
            answer= answer + element.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text;

        }
        if (answer== word.Replace(" ", ""))
        {
            checkText.text = "Correct";
            checkedans = true;
			switch (difficulty) {
			case "medium":
				score += 3;
				ScoreCalculation ();
				break;
			case "difficult":
				score += 5;
				ScoreCalculation ();
				break;
			default:
				score += 1;
				ScoreCalculation ();
				break;
			}
        }
        else
        {
            checkText.text = "Wrong answer";
			scoreBar.gameObject.SetActive(false);
			levelText.enabled = false;
        }

    }

	void ScoreCalculation(){
		if (score >= 10 && score < 30) {
			levelText.text = "Level 2";
			scoreBar.GetComponentInChildren<Text> ().text = score + "/" + "30";
			scoreBar.value = ((float)score/30.0f) * 100.0f;
		} else if (score >= 30 && score < 60) {
			levelText.text = "Level 3";
			scoreBar.GetComponentInChildren<Text> ().text = score + "/" + "60";
			scoreBar.value = ((float)score/60.0f) * 100.0f;
		} else if (score >= 60 && score < 100) {
			levelText.text = "Level 4";
			scoreBar.GetComponentInChildren<Text> ().text = score + "/" + "100";
			scoreBar.value = ((float)score/100.0f) * 100.0f;
		} else if (score >= 100) {
			levelText.text = "Level 5";
			scoreBar.GetComponentInChildren<Text> ().text = score + "/" + "200";
			scoreBar.value = ((float)score/200.0f) * 100.0f;
		} else {
			levelText.text = "Level 1";
			scoreBar.GetComponentInChildren<Text> ().text = score + "/" + "10";
			scoreBar.value = ((float)score/10.0f) * 100.0f;
		}

		Debug.Log (scoreBar.value.ToString());
	}

    void checkAnswer()
    {
        if (checkedans)
        {
            string ID = arguments["userId"].ToString();
            if (finalData["Answered"] == null)
            {
                finalData.AddField("Answered", ID);
            }
            else
            {
                finalData.AddField("Answered", finalData["Answered"].ToString().Replace("\"","")+","+ ID);
            }
            string finalNotAnswered = finalData["notAnswered"].ToString().Replace("," + (ID), "").Replace((ID), "");
            finalData.AddField("notAnswered", finalNotAnswered);
            socket.Emit("SHARE", finalData);
            SceneManager.LoadScene("menu");
        }
        else
        {
            popup.enabled = false;
			panel.gameObject.SetActive (true);
			Debug.Log ("123");
        }
    }
    public void getGameData(SocketIOEvent evt)
    {
        Debug.Log("alan");
        Debug.Log("json:" + evt.data);
        //saveData2 = new JSONObject(JSONObject.Type.ARRAY);
        //saveData2.Add(evt.data["block"]);
        saveData2 = evt.data["block"];
        finalData = evt.data;
        Debug.Log("data2:" + saveData2.ToString());
        loadGame();
    }
    IEnumerator ConnectToServer()
    {
        yield return new WaitForSeconds(0.5f);
        socket.Emit("GETWITHDATA", gameId);

    }
    void loadGame()
    {
        for (int i = 0; i < saveData2.Count; i++)
        {

            string x = saveData2[i].GetField("position").GetField("x") + "";
            string y = saveData2[i].GetField("position").GetField("y") + "";
            string z = saveData2[i].GetField("position").GetField("z") + "";
            Debug.Log("xyz:" + float.Parse(x) + y + z);
            string arrayindex = saveData2[i].GetField("arrayindex").ToString();
            Vector3 index = new Vector3(float.Parse(x), float.Parse(y), float.Parse(z));
            string heightstring = saveData2[i].GetField("height").ToString().Replace("\"", "");
            Vector3 newheight = new Vector3(0, (float)Char.GetNumericValue(heightstring[0]), 0);
            Debug.Log("height:" + newheight);
            blockPrefab = Resources.Load((saveData2[i].GetField("type") + "").Replace("\"", ""), typeof(GameObject)) as GameObject;
            go = Instantiate(blockPrefab) as GameObject;
            go.GetComponent<Renderer>().material = Resources.Load((saveData2[i].GetField("color") + "").Substring(1, (saveData2[i].GetField("color") + "").Length - 2), typeof(Material)) as Material;
            go.AddComponent<BoxCollider>();
            Vector3 blockIndex = new Vector3((float)Char.GetNumericValue(arrayindex[1]), (float)Char.GetNumericValue(arrayindex[2]), (float)Char.GetNumericValue(arrayindex[3]));
            Debug.Log("Array Index:" + blockIndex);
            BoxCollider collider = go.GetComponent<BoxCollider>();
            collider.size = new Vector3(0.5f, 0.5f, 0.5f);
            go.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
            go.transform.position = index;
            if (saveData2[i].GetField("rotate").ToString().Equals("true"))
            {
                int stringIndex = blockPrefab.transform.name.ToString().IndexOf('X');
                int length = (int)System.Char.GetNumericValue(blockPrefab.transform.name.ToString()[stringIndex - 1]);
                int width = (int)System.Char.GetNumericValue(blockPrefab.transform.name.ToString()[stringIndex + 1]);
                if (length == 2 && width == 1)
                {
                    width = 2;
                    length = 1;
                }
                if (length == 1 && width == 2) { go.transform.Rotate(0, 0, 90.0f); }
                blocks[(int)blockIndex.x, (int)blockIndex.y, (int)blockIndex.z] = new Block
                {
                    blockTransform = go.transform,
                    height = newheight,
                    rotate = true,
                    color = blockColor,
                    type = blockPrefab.transform.name.ToString().Replace("(Clone)", "")
                };
            }
            else
            {
                int stringIndex = blockPrefab.transform.name.ToString().IndexOf('X');
                int width = (int)System.Char.GetNumericValue(blockPrefab.transform.name.ToString()[stringIndex - 1]);
                int length = (int)System.Char.GetNumericValue(blockPrefab.transform.name.ToString()[stringIndex + 1]);
                if (length == 1 && width == 2)
                {
                    width = 1;
                    length = 2;
                }
                go.transform.Rotate(0, 0, 90.0f);
                if (length == 2 && width == 1) { go.transform.Rotate(0, 0, 270.0f); }
                blocks[(int)blockIndex.x, (int)blockIndex.y, (int)blockIndex.z] = new Block
                {
                    blockTransform = go.transform,
                    height = newheight,
                    rotate = false,
                    color = blockColor,
                    type = blockPrefab.transform.name.ToString().Replace("(Clone)", "")
                };
            }
        }

    }
    void showTranslateP()
    {
        tranlsteP.gameObject.SetActive(true);
    }
}
