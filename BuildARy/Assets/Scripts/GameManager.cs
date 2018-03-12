﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Facebook.Unity;
using SocketIO;

public class Block{
    public Transform blockTransform;
    public Vector3 height = new Vector3(0,1,0);
    public string color;
    public string types;
    public bool rotate;
}

public class GameManager : MonoBehaviour {
	public SocketIOComponent socket;
	private string email;
    private float blockSize = 0.5f;
    public Block[,,] blocks = new Block[20,20,20];
    public static GameManager Instance { set; get; }
    private GameObject blockPrefab;
    private bool isRotated = true;
	private bool isUndo = false;
    private GameObject foundationObject;
    public GameObject scrollView;
    private List<GameObject> undoGo = new List<GameObject>();
    private List<Vector3> blockPosition = new List<Vector3>();
    private Vector3 blockOffset = new Vector3(0.5f,0.5f,0.5f);
    private Vector3 foundationCenter = new Vector3(0, 0, 0);
	//public Button[] button1;
	//public int numOfMenu = 3;
	public Button btnro;
    public Button btnun;
    public Button btnbak;
    public Button menu;
    public Button hidemenu;
    public Button showColour;
    public Button hideColour;
    public Button save;
	public Button load;
    public GameObject colour;
    public int count = 0;
    public GameObject go;
    private bool showGUI=false;
	private JSONObject saveData2;
    RectTransform rectTransform;
    Hashtable arguments;
    void Start () {
        Instance = this;
        blockPrefab = Instantiate(Resources.Load("Part_2X1", typeof(GameObject))) as GameObject;
        colour.gameObject.SetActive(false);
        scrollView.gameObject.SetActive(false);
        hidemenu.GetComponent<Button>().onClick.AddListener(hidem);
        hidemenu.gameObject.SetActive(false);
        hideColour.GetComponent<Button>().onClick.AddListener(hideC); 
        hideColour.gameObject.SetActive(false);
        showColour.GetComponent<Button>().onClick.AddListener(showC); 
        foundationObject = GameObject.Find("Foundation");
		btnro.GetComponent<Button> ().onClick.AddListener(rotate);
        btnun.GetComponent<Button>().onClick.AddListener(undo); ;
        btnbak.GetComponent<Button>().onClick.AddListener(back);
        menu.GetComponent<Button>().onClick.AddListener(showmenu);
        save.GetComponent<Button>().onClick.AddListener(saveGame);
		load.GetComponent<Button>().onClick.AddListener(loadGame);
		FB.API("me?fields=email", Facebook.Unity.HttpMethod.GET, GetEmail);
        arguments = SceneManager.GetSceneArguments();
        Debug.Log("Arguments: " + arguments["vocab"] );
        
        /*button1 = new Button[numOfMenu];
		for (var i = 1; i <= numOfMenu; i++) 
		{
			Button btn = GameObject.Find ("btn" + i).GetComponent<Button> ();
			button1 [i] = btn;
			button1 [i].interactable = true;
		}
		button1 [1].onClick.AddListener (rotate);*/
        //Button rotateButton = button1.GetComponents<Button>();
        //rotateButton.onClick.AddListener (rotate);
    }
	void GetEmail(Facebook.Unity.IGraphResult result)
	{
		email = result.ResultDictionary["email"].ToString();
		Debug.Log("email: " + email);
	}
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
			
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit, 30.0f))
            {
                if (!EventSystem.current.IsPointerOverGameObject(/*Input.GetTouch(0).fingerId*/)) {
                Vector3 index = BlockPosition(hit.point);
                Debug.Log(index.ToString());
                Debug.Log("object:"+hit.transform.gameObject);
                if (hit.transform.gameObject.name != "Foundation")
                {
                    index.y -= 1;
                }
                int x = (int)index.x, y=(int)index.y, z = (int)index.z;
                Debug.Log("Y is :" + y);
                if (!isRotated)
                {
                    if (blocks[x, 0, z] == null && blocks[x, 0, z + 1] == null)
                    {
                        if (x <= 12 && z < 12)
                        {
                                index.y = 0;
                                y = 0;
                            go = Instantiate(blockPrefab) as GameObject;
                            go.AddComponent<BoxCollider>();
                            BoxCollider collider = go.GetComponent<BoxCollider>();
                                collider.size = new Vector3(0.5f, 0.5f, 0.5f);
                            go.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
                            PositionBlock(go.transform, index);
                                undoGo.Add(go);
                            Debug.Log("Height:"  + go.transform.position);
							Debug.Log ("1" + isRotated);
                                blocks[x, y, z] = new Block
                                {
                                    blockTransform = go.transform,
                                    height = new Vector3(0, 1, 0),
                                    rotate = false,
                                    color= go.transform.GetComponent<Renderer>().material.color.ToString()

                                };
                            blocks[x, y, z + 1] = new Block
                            {
                                blockTransform = go.transform,
                                height = new Vector3(0, 1, 0),
                                rotate = false,
                                color = go.transform.GetComponent<Renderer>().material.color.ToString()
                            };
                            blockPosition.Add(new Vector3(x, y, z));
                            blockPosition.Add(new Vector3(x, y, z+1));
                        }
                    }
                    else
                    {
                        Vector3 newHeight;
                        if (blocks[x, 0, z] != null)
                        {
                            if (blocks[x, 0, z + 1] != null)
                            {
                                newHeight = blocks[x, 0, z].height.y > blocks[x, 0, z + 1].height.y ? blocks[x, 0, z].height : blocks[x, 0, z + 1].height;
                            }
                            else
                            {
                                newHeight = blocks[x, 0, z].height;
                            }
                        }
                        else
                        {
                            newHeight = blocks[x, 0, z + 1].height;
                        }
                            Vector3 newIndex = new Vector3(index.x, newHeight.y, index.z);
                            if (blocks[x, (int)newIndex.y, z] == null && blocks[x, (int)newIndex.y, z + 1] == null)
                        {
                            if (x <= 12 && z < 12)
                            {
                                go = Instantiate(blockPrefab) as GameObject;
                                go.AddComponent<BoxCollider>();
                                BoxCollider collider = go.GetComponent<BoxCollider>();
                                collider.size = new Vector3(0.5f,0.5f,0.5f);
                                go.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
                                PositionBlock(go.transform, newIndex);
                                    undoGo.Add(go);
                                Debug.Log("Height2:" + go.transform.position);
								Debug.Log ("2" + isRotated);
                                if (blocks[x, 0, z] == null)
                                {
                                    blocks[x, 0, z] = new Block {
                                        blockTransform =null,
                                        height = newHeight + new Vector3(0, 1, 0),
                                        rotate = false,
                                        color = go.transform.GetComponent<Renderer>().material.color.ToString()
                                    };
                                }
                                else
                                {
                                    blocks[x, 0, z].height = newHeight + new Vector3(0, 1, 0);
                                }
                                if (blocks[x, 0, z + 1] == null)
                                {
                                    blocks[x, 0, z + 1] = new Block { blockTransform = null, height = blocks[x, 0, z].height, rotate = false, color = go.transform.GetComponent<Renderer>().material.color.ToString() };
                                }
                                else
                                {
                                    blocks[x, 0, z + 1].height = blocks[x, 0, z].height;
                                }
                                blocks[(int)newIndex.x, (int)newIndex.y, (int)newIndex.z] = new Block
                                {
                                    blockTransform = go.transform,
                                    height = blocks[x, 0, z].height + new Vector3(0, 1, 0),
                                    rotate = false,
                                    color = go.transform.GetComponent<Renderer>().material.color.ToString()
                                };
                                blocks[(int)newIndex.x, (int)newIndex.y, (int)newIndex.z + 1] = new Block
                                {
                                    blockTransform = go.transform,
                                    height = (blocks[x, 0, z+1].height!=null? blocks[x, 0, z+1].height:new Vector3(0,1,0)) + new Vector3(0, 1, 0),
                                    rotate = false,
                                    color = go.transform.GetComponent<Renderer>().material.color.ToString()
                                };
                                    blockPosition.Add(new Vector3((int)newIndex.x, (int)newIndex.y, (int)newIndex.z));
                                    blockPosition.Add(new Vector3((int)newIndex.x, (int)newIndex.y, (int)newIndex.z + 1));
                            }
                        }
                    }
                }
                else {
                        //index.z -= 1;
                        
                        if (blocks[x, 0, z] == null && blocks[x+1, 0, z] == null)
                    {
                        if (x < 12 && z <= 12)
                        {
                                index.y = 0;
                                y = 0;
                            go = Instantiate(blockPrefab) as GameObject;
                            go.AddComponent<BoxCollider>();
                            BoxCollider collider = go.GetComponent<BoxCollider>();
                            collider.size = new Vector3(0.5f,0.5f,0.5f);
                            go.transform.Rotate(0, 0, 90.0f);
                            go.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);    
                            PositionBlock(go.transform, index);
                                undoGo.Add(go);
                            Debug.Log("hit:" + go.transform.position);
							Debug.Log ("3" + isRotated);
                           
                                blocks[x, y, z] = new Block
                            {
                                blockTransform = go.transform,
                                height = new Vector3(0, 1, 0),
                                rotate = true,
                                    color = go.transform.GetComponent<Renderer>().material.color.ToString()
                                };
                            blocks[x+1, y, z] = new Block
                            {
                                blockTransform = go.transform,
                                height = new Vector3(0, 1, 0),
                                rotate = true,
                                color = go.transform.GetComponent<Renderer>().material.color.ToString()
                            };
                                blockPosition.Add(new Vector3(x, y, z));
                                blockPosition.Add(new Vector3(x+1, y, z));
                                
                        }
                    }
                    else
                    {
                            Vector3 newHeight;
                        if (blocks[x, 0, z] != null)
                        {
                            if (blocks[x + 1, 0, z] != null)
                            {
                                newHeight = blocks[x, 0, z].height.y > blocks[x + 1, 0, z].height.y ? blocks[x, 0, z].height : blocks[x + 1, 0, z].height;
                            }
                            else {
                                newHeight = blocks[x, 0, z].height;
                            }
                            }
                        else {
                            newHeight = blocks[x+1, 0, z].height;
                        }
						Debug.Log ("4" + isRotated);
                        //Vector3 newIndex = BlockPosition(hit.point + (hit.normal * blockSize));
                        Vector3 newIndex = new Vector3(index.x, newHeight.y,index.z);
                        //newIndex.z -= 1;
                        if (blocks[(int)newIndex.x, (int)newIndex.y, (int)newIndex.z] == null && blocks[(int)newIndex.x + 1, (int)newIndex.y, (int)newIndex.z] == null)
                        {
                            if (x < 12 && z <= 12)
                            {
                                go = Instantiate(blockPrefab) as GameObject;
                                go.AddComponent<BoxCollider>();
                                BoxCollider collider = go.GetComponent<BoxCollider>();
                                collider.size = new Vector3(1, 0.5f, 1);
                                go.transform.Rotate(0, 0, 90);
                                go.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
                                PositionBlock(go.transform, newIndex);
                                    undoGo.Add(go);
                                    
                                    if (blocks[x, 0, z] == null)
                                {
                                    blocks[x, 0, z] = new Block { blockTransform = null, height = newHeight + new Vector3(0, 1, 0), rotate = true, color = go.transform.GetComponent<Renderer>().material.color.ToString() };
                                }
                                else
                                {
                                    blocks[x, 0, z].height = newHeight + new Vector3(0, 1, 0);
                                }
                                if (blocks[x + 1, 0, z] == null)
                                {
                                    blocks[x + 1, 0, z] = new Block { blockTransform = null, height = blocks[x, 0, z].height, rotate = true, color = go.transform.GetComponent<Renderer>().material.color.ToString() };
                                }
                                else
                                {
                                    blocks[x + 1, 0, z].height = blocks[x, 0, z].height;
                                }
                                blocks[(int)newIndex.x, (int)newIndex.y, (int)newIndex.z] = new Block
                                {
                                    blockTransform = go.transform,
                                    height = blocks[x, 0, z].height,
                                    rotate = true,
                                    color = go.transform.GetComponent<Renderer>().material.color.ToString()
                                };
                                blocks[(int)newIndex.x+1, (int)newIndex.y, (int)newIndex.z] = new Block
                                {
                                    blockTransform = go.transform,
                                    height = blocks[x+1, 0, z].height,
                                    rotate = true,
                                    color = go.transform.GetComponent<Renderer>().material.color.ToString()
                                };
                                    blockPosition.Add(new Vector3((int)newIndex.x, (int)newIndex.y, (int)newIndex.z));
                                    blockPosition.Add(new Vector3((int)newIndex.x+1, (int)newIndex.y, (int)newIndex.z));
                                Debug.Log("put1:" + (int)newIndex.x + " " + (int)newIndex.y + " " + (int)newIndex.z);
                            }
                        }
                    }
                }
            }
        }
        }
    }
    private Vector3 BlockPosition(Vector3 hit)
    {
        int x = (int)(hit.x / blockSize);
        int y = (int)(hit.y / blockSize);
        int z = (int)(hit.z / blockSize);

        return new Vector3(x, y, z);
    }
    public void PositionBlock(Transform t, Vector3 index)
    {
        if (isRotated) { index -= new Vector3(0, 0, 1); }
        t.position = ((index * blockSize) + blockOffset)/*+ (foundationObject.transform.position - foundationCenter)*/;
    }

	void rotate()
	{
		Debug.Log ("Clicked");
		/*count++;
		if (count % 2 == 0) {
			isRotated = true;
		}
		else {
			isRotated = false;
		}*/
		isRotated = !isRotated;
		/*GameObject go = Instantiate(blockPrefab) as GameObject;
		go.transform.Rotate(0, 0, 90);
		go.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);*/
	}
    void undo()
    {
        Debug.Log(undoGo.Count);
        if (undoGo.Count > 0)
        {
                Destroy(undoGo[undoGo.Count-1]);
                undoGo.RemoveAt(undoGo.Count-1);
                Debug.Log("GO:" + go);
                for (int i = blockPosition.Count-1; i >= blockPosition.Count-2; i--)
            {
                Debug.Log("I:" + i);
                blocks[(int)blockPosition[i].x, (int)blockPosition[i].y, (int)blockPosition[i].z] = null;
                if (blockPosition[i].y != 0)
                {
                    Debug.Log("Undo not base now");
                    //blocks[(int)blockPosition[i].x, 0, (int)blockPosition[i].z].height -= new Vector3(0, 1, 0);
                    Debug.Log("base height:" + blocks[(int)blockPosition[i].x, 0, (int)blockPosition[i].z].height);
                    for (int count = (int)blockPosition[i].y; count >= 0; count--) {
                        if (blocks[(int)blockPosition[i].x, count, (int)blockPosition[i].z] != null) {
                            if (blocks[(int)blockPosition[i].x, count, (int)blockPosition[i].z].blockTransform!=null)
                            {
                                Debug.Log("exist block break");
                                break;
                            }
                        }
                        blocks[(int)blockPosition[i].x, 0, (int)blockPosition[i].z].height -= new Vector3(0, 1, 0);
                        if (blocks[(int)blockPosition[i].x, 0, (int)blockPosition[i].z].height.y==0)
                        {
                            blocks[(int)blockPosition[i].x, 0, (int)blockPosition[i].z] = null;
                            Debug.Log("null block now");
                            break;
                        }

                        Debug.Log("base height in loop:" + blocks[(int)blockPosition[i].x, 0, (int)blockPosition[i].z].height);
                    }
                }
                Debug.Log("af" + (int)blockPosition[i].x + (int)blockPosition[i].y + (int)blockPosition[i].z);
            }
            blockPosition.RemoveAt(blockPosition.Count - 1);
            blockPosition.RemoveAt(blockPosition.Count - 1);

        }
    }
    void back()
    {
        SceneManager.LoadScene("menu");
    }
    void showmenu() {
        menu.gameObject.SetActive(false);
        scrollView.gameObject.SetActive(true);
        btnro.gameObject.SetActive(false);
        btnun.gameObject.SetActive(false);
        hidemenu.gameObject.SetActive(true);
    }
    void hidem()
    {
        hidemenu.gameObject.SetActive(false);
        menu.gameObject.SetActive(true);
        scrollView.gameObject.SetActive(false);
        btnro.gameObject.SetActive(true);
        btnun.gameObject.SetActive(true);

    }
    void showC()
    {
        hideColour.gameObject.SetActive(true);
        showColour.gameObject.SetActive(false);
        colour.gameObject.SetActive(true);
    }
    void hideC()
    {
        hideColour.gameObject.SetActive(false);
        showColour.gameObject.SetActive(true);
        colour.gameObject.SetActive(false);
    }
    void saveGame() {
		saveData2 = new JSONObject(JSONObject.Type.ARRAY);
        string saveData="";
        Block[,,] b= GameManager.Instance.blocks;
        for (int x = 0; x < 20; x++)
        {
            for (int y = 0; y < 20; y++)
            {
                for (int z = 0; z < 20; z++)
                {
                    JSONObject saveData1 = new JSONObject(JSONObject.Type.OBJECT);
					JSONObject xyz = new JSONObject(JSONObject.Type.OBJECT);
                    Block currentBlock = b[x, y, z];
                    if (currentBlock == null||currentBlock.blockTransform==null)
                        continue;


                    /* saveData += "position:"+currentBlock.blockTransform.position+","+
                                 "color:"+currentBlock.color + "," +
                                 "type:"+"2*1" +"," + 
                                 "rotate:"+currentBlock.rotate+"&";*/
                    Debug.Log(currentBlock.blockTransform.position);
					xyz.AddField ("x", currentBlock.blockTransform.position.x);
					xyz.AddField ("y", currentBlock.blockTransform.position.y);
					xyz.AddField ("z", currentBlock.blockTransform.position.z);
					saveData1.AddField("position",xyz);
                    saveData1.AddField("color", currentBlock.color);
                    saveData1.AddField("type", "2*1");
                    saveData1.AddField("rotate", currentBlock.rotate);
                    saveData2.Add(saveData1);
                }
            }
        }
		Debug.Log("array0:"+saveData2 [1].GetField("position"));
        Debug.Log(saveData2);
		JSONObject finalData = new JSONObject(JSONObject.Type.OBJECT);
		finalData.AddField("email",email);
		finalData.AddField("createtime",System.DateTime.Now.ToString());
		finalData.AddField("block",saveData2);
		socket.Emit ("SAVE", finalData);
    }
	void loadGame(){
		for (int i = 0; i < saveData2.Count;i++)
		{

			string x = saveData2[i].GetField("position").GetField("x")+"";
			string y = saveData2[i].GetField("position").GetField("y")+"";
			string z = saveData2[i].GetField("position").GetField("z")+"";
			Debug.Log ("xyz:"+float.Parse(x)+y+z);
			Vector3 index =new Vector3(float.Parse(x),float.Parse(y),float.Parse(z));
			go = Instantiate(blockPrefab) as GameObject;
			go.AddComponent<BoxCollider>();
			BoxCollider collider = go.GetComponent<BoxCollider>();
			collider.size = new Vector3(0.5f, 0.5f, 0.5f);
			go.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
			go.transform.position=index;
		}


	}

}
