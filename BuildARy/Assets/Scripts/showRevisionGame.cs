﻿using SocketIO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class showRevisionGame : MonoBehaviour {

    private string word = "";
    private JSONObject gameId;
    private Hashtable arguments = new Hashtable();
    public SocketIOComponent socket;
    private JSONObject saveData2;
    private JSONObject finalData;
    private GameObject blockPrefab;
    public GameObject go;
    public Block[,,] blocks = new Block[20, 20, 20];
    private string blockColor = "White";
    public Button back;
    
    // Use this for initialization
    void Start () {
        arguments = SceneManager.GetSceneArguments();
        Dictionary<string, string> data = new Dictionary<string, string>();
        word = arguments["vocab"].ToString();
        data["_id"] = arguments["gameId"].ToString();
        word = arguments["vocab"].ToString();
        gameId = new JSONObject(data);
        Debug.Log("word:" + word);
        StartCoroutine(ConnectToServer());
        socket.On("GETWITHDATA", getGameData);
        back.GetComponent<Button>().onClick.AddListener(backHome);
    }
	
	// Update is called once per frame
	void Update () {
		
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
        yield return new WaitForSeconds(1f);
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
        SceneManager.LoadScene("menu");
    }
}
