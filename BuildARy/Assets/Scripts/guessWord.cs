using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SocketIO;
using System;

public class guessWord : MonoBehaviour {

    private string word = "";
	private string difficulty;
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
	private string friendID;
    private bool checkedans = false;
    public Text checkText;
    private int selectCount = 0;
	private float score;
	private float addscore;
	public Slider scoreBar;
	public Text scoreText;
	public Text levelText;
    private JSONObject gameId;
    private Hashtable arguments = new Hashtable();
    public SocketIOComponent socket;
    private JSONObject saveData2;
    private JSONObject finalData;
	private JSONObject userScore=new JSONObject();
	private string userID;
    private GameObject blockPrefab;
    public GameObject go;
    public Block[,,] blocks = new Block[20, 20, 20];
    private string blockColor = "White";
	public Button back;
    public Button showTran;
    // Use this for initialization
    void Start () {
        arguments = SceneManager.GetSceneArguments();
        
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["_id"] = arguments["gameId"].ToString();
        word = arguments["vocab"].ToString();
		difficulty = arguments ["diff"].ToString ();
        gameId = new JSONObject(data);
		userID = arguments ["userid"].ToString();
        Debug.Log("user ID: " + userID);
        Debug.Log("word:"+word);
        StartCoroutine(ConnectToServer());
        socket.On("GETWITHDATA", getGameData);
		socket.On ("GETUSER", getUser);
        submit.GetComponent<Button>().onClick.AddListener(popUp);
        check.GetComponent<Button>().onClick.AddListener(checkAnswer);
		back.GetComponent<Button> ().onClick.AddListener (backHome);
		panel = GameObject.Find("guessPanel");
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
            if (change.GetComponentInChildren<Text>().text != "")
            {
                Button goButton = Instantiate(guessB);
                goButton.transform.SetParent(random, false);
                goButton.onClick.AddListener(delegate { selectWord(goButton); });
                goButton.GetComponentInChildren<Text>().text = change.GetComponentInChildren<Text>().text;
                change.GetComponentInChildren<Text>().text = "";
            }
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
				addscore = 3;
				//ScoreCalculation ();
				break;
			case "difficult":
				score += 5;
				addscore = 5;
				//ScoreCalculation ();
				break;
			default:
				score += 1;
				addscore = 1;
				//ScoreCalculation ();
				break;
			}
            Debug.Log("score:" + addscore);

			userScore.AddField ("score", addscore);
			userScore.AddField ("user1", arguments ["userid"].ToString());
            Debug.Log("id1:"+arguments["userid"].ToString());
			userScore.AddField ("user2", friendID.Replace("\"",""));
            Debug.Log("id2:"+friendID);
        }
        else
        {
            checkText.text = "Wrong answer";
            answer = "";
			//scoreBar.gameObject.SetActive(false);
			//levelText.enabled = false;
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
            StartCoroutine(addScore());
            
        }
        else
        {

            popup.enabled = false;
			panel.gameObject.SetActive (true);
            Debug.Log ("123");
        }
    }
    IEnumerator addScore()
    {
        yield return new WaitForSeconds(1.0f);
        socket.Emit("SHARE", finalData);
        yield return new WaitForSeconds(1.0f);
        socket.Emit("ADDSCORE", userScore);
        Debug.Log("success");
        SceneManager.LoadScene("menu");
    }
    public void getGameData(SocketIOEvent evt)
    {
        Debug.Log("alan");
        Debug.Log("json:" + evt.data);
        //saveData2 = new JSONObject(JSONObject.Type.ARRAY);
        //saveData2.Add(evt.data["block"]);
        saveData2 = evt.data["block"];
		friendID = evt.data ["id"].ToString ();
        finalData = evt.data;
        Debug.Log("data2:" + saveData2.ToString());
        loadGame();
    }
	void getUser(SocketIOEvent evt)
	{
		Debug.Log ("get user success");
		score = Convert.ToSingle(evt.data ["score"].ToString());
	}
    IEnumerator ConnectToServer()
    {
        yield return new WaitForSeconds(1f);
        socket.Emit("GETWITHDATA", gameId);
		yield return new WaitForSeconds (1f);
        Dictionary<string, string> userid = new Dictionary<string, string>();
        userid["id"] = userID;
        JSONObject user = new JSONObject(userid);
		socket.Emit ("GETUSER", user);
    }
    void loadGame()
    {
        for (int i = 0; i < saveData2.Count; i++)
        {

            string x = saveData2[i].GetField("position").GetField("x") + "";
            string y = saveData2[i].GetField("position").GetField("y") + "";
            string z = saveData2[i].GetField("position").GetField("z") + "";
            Debug.Log("xyz:" + float.Parse(x) + y + z);
            string arrayindex = saveData2[i].GetField("arrayindex").ToString().Replace("\"", "");
            string[] indexarray = arrayindex.Split(',');
            Vector3 index = new Vector3(float.Parse(x), float.Parse(y), float.Parse(z));
            string heightstring = saveData2[i].GetField("height").ToString().Replace("\"", "");
            int rotate = System.Int32.Parse(saveData2[i].GetField("rotate").ToString());
            Vector3 newheight = new Vector3(0, (float)Char.GetNumericValue(heightstring[0]), 0);
            Debug.Log("height:" + newheight);
            blockPrefab = Resources.Load((saveData2[i].GetField("type") + "").Replace("\"", ""), typeof(GameObject)) as GameObject;
            go = Instantiate(blockPrefab) as GameObject;
            go.GetComponent<Renderer>().material = Resources.Load((saveData2[i].GetField("color") + "").Substring(1, (saveData2[i].GetField("color") + "").Length - 2), typeof(Material)) as Material;
            go.AddComponent<BoxCollider>();
            Debug.Log("go:" + go);
            Debug.Log("blockPrefab:" + blockPrefab);
            Debug.Log("index:" + indexarray[0]);
            Vector3 blockIndex = new Vector3(Convert.ToSingle(indexarray[0]), Convert.ToSingle(indexarray[1]), Convert.ToSingle(indexarray[2]));
            Debug.Log("Array Index:" + blockIndex);
            BoxCollider collider = go.GetComponent<BoxCollider>();
            collider.size = new Vector3(0.5f, 0.5f, 0.5f);
            go.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
            go.transform.position = index;
            if (rotate == 0 || rotate == 180)
            {
                int stringIndex = blockPrefab.transform.name.ToString().IndexOf('X');
                int length = (int)System.Char.GetNumericValue(blockPrefab.transform.name.ToString()[stringIndex - 1]);
                int width = (int)System.Char.GetNumericValue(blockPrefab.transform.name.ToString()[stringIndex + 1]);
                bool roof = blockPrefab.transform.name.ToString().Contains("roof");
                if (length == 2 && width == 1)
                {
                    width = 2;
                    length = 1;
                }
                go.transform.Rotate(0, 0, rotate);
                if (length == 1 && width == 2) { go.transform.Rotate(0, 0, 90.0f); }
                blocks[(int)blockIndex.x, (int)blockIndex.y, (int)blockIndex.z] = new Block
                {
                    blockTransform = go.transform,
                    height = newheight,
                    rotate = rotate,
                    disable = saveData2[i].GetField("disable").ToString().Equals("true"),
                    color = saveData2[i].GetField("color").ToString().Replace("\"", ""),
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
                go.transform.Rotate(0, 0, rotate);
                if (length == 2 && width == 1) { go.transform.Rotate(0, 0, 270.0f); }
                blocks[(int)blockIndex.x, (int)blockIndex.y, (int)blockIndex.z] = new Block
                {
                    blockTransform = go.transform,
                    height = newheight,
                    rotate = rotate,
                    disable = saveData2[i].GetField("disable").ToString().Equals("true"),
                    color = saveData2[i].GetField("color").ToString().Replace("\"", ""),
                    type = blockPrefab.transform.name.ToString().Replace("(Clone)", "")
                };
            }
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
