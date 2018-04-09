using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class guessWord : MonoBehaviour {

    private string word = "strawberry";
	private string difficulty = "medium";
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
    // Use this for initialization
    void Start () {
        submit.GetComponent<Button>().onClick.AddListener(popUp);
        check.GetComponent<Button>().onClick.AddListener(checkAnswer);
		panel = GameObject.Find("guessPanel");

        for (int i = 0; i < 5; i++)
        {
            if (i == 0)
            {
                randomWord = word + (char)('a' + Random.Range(0, 26));
            }
            else
            {
                randomWord = randomWord + (char)('a' + Random.Range(0, 26));
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
            int num = Random.Range(0, len-i);
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
            SceneManager.LoadScene("menu");
        }
        else
        {
            popup.enabled = false;
			panel.gameObject.SetActive (true);
			Debug.Log ("123");
        }
    }
}
