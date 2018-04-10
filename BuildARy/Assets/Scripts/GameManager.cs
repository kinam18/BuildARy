using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Facebook.Unity;
using SocketIO;

public class Block{
    public Transform blockTransform;
    public Vector3 height = new Vector3(0,1,0);
    public string color;
    public string type;
    public bool rotate;
}

public class GameManager : MonoBehaviour {
	public SocketIOComponent socket;
	private string id;
    private float blockSize = 0.5f;
    public Block[,,] blocks = new Block[20,20,20];
    public static GameManager Instance { set; get; }
    private GameObject blockPrefab;
    private bool isRotated = true;
	private bool isUndo = false;
    private GameObject foundationObject;
    public GameObject scrollView;
    public GameObject scrollBar;
    private List<GameObject> undoGo = new List<GameObject>();
    private List<Vector3> blockPosition = new List<Vector3>();
    private Vector3 blockOffset = new Vector3(0.5f,0.0f,0.5f);
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
    public Button twoone;
    public Button twotwo;
    public Button twofour;
    public Button twosix;
    public Button twoeight;
    public GameObject twot1;
    public GameObject twot2;
    public GameObject twot4;
    public GameObject twot6;
    public GameObject twot8;
    public Material blackS;
    public Button black;
    public Button red;
    public Button orange;
    public Button yellow;
    public Button green;
    public Button blue;
    public Button indigo;
    public Button violet;
    public Button white;
    public GameObject colour;
    public int count = 0;
    public GameObject go;
    public RectTransform popupSave;
    public RectTransform popupBack;
    public Button closeSave;
    public Button closeBack;
    public Button backYes;
    public Button backNo;
    public Button saveonly;
    public Button saveShare;
    private bool showGUI=false;
	private JSONObject saveData2;
    private string blocktype;
    private Material mat;
    private string blockColor="White";
    RectTransform rectTransform;
    Hashtable arguments;

    void Start () {
        Instance = this;
        blockPrefab = Resources.Load("Part_2X1", typeof(GameObject)) as GameObject;
        mat= Resources.Load("White", typeof(Material)) as Material;
        colour.gameObject.SetActive(false);
        popupBack.gameObject.SetActive(false);
        popupSave.gameObject.SetActive(false);
        scrollView.gameObject.SetActive(false);
        scrollBar.gameObject.SetActive(false);
        hidemenu.gameObject.SetActive(false);
        twoone.GetComponent<Button>().onClick.AddListener(delegate { setBlockType("Part_2X1"); });
        twotwo.GetComponent<Button>().onClick.AddListener(delegate { setBlockType("Part_2X2"); });
        twofour.GetComponent<Button>().onClick.AddListener(delegate { setBlockType("Part_2X4"); });
        twosix.GetComponent<Button>().onClick.AddListener(delegate { setBlockType("Part_2X6"); });
        twoeight.GetComponent<Button>().onClick.AddListener(delegate { setBlockType("Part_2X8"); });
        black.GetComponent<Button>().onClick.AddListener(delegate { setColor("Black"); });
        red.GetComponent<Button>().onClick.AddListener(delegate { setColor("Red"); });
        orange.GetComponent<Button>().onClick.AddListener(delegate { setColor("Orange"); });
        yellow.GetComponent<Button>().onClick.AddListener(delegate { setColor("Yellow"); });
        green.GetComponent<Button>().onClick.AddListener(delegate { setColor("Green"); });
        blue.GetComponent<Button>().onClick.AddListener(delegate { setColor("Blue"); });
        indigo.GetComponent<Button>().onClick.AddListener(delegate { setColor("Indigo"); });
        violet.GetComponent<Button>().onClick.AddListener(delegate { setColor("Violet"); });
        white.GetComponent<Button>().onClick.AddListener(delegate { setColor("White"); });
        closeBack.GetComponent<Button>().onClick.AddListener(delegate { closeB(); });
        backNo.GetComponent<Button>().onClick.AddListener(delegate { closeB(); });
        closeSave.GetComponent<Button>().onClick.AddListener(closeS);
        backYes.GetComponent<Button>().onClick.AddListener(backmenu);
        saveonly.GetComponent<Button>().onClick.AddListener(sendOnly);
        saveShare.GetComponent<Button>().onClick.AddListener(sendShare);
        Debug.Log("fsdfsd：" + twoone.GetComponent<Button>());
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
       
       
        FB.API("me?fields=id", Facebook.Unity.HttpMethod.GET, GetId);
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
	void GetId(Facebook.Unity.IGraphResult result)
	{
		id = result.ResultDictionary["id"].ToString();
		Debug.Log("id: " + id);
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
                /*if (hit.transform.gameObject.name != "Foundation")
                {
                    index.y -= 1;
                }*/
                int x = (int)index.x, y=(int)index.y, z = (int)index.z;
                Debug.Log("Y is :" + y);
                if (isRotated)
                {
                        int stringIndex = blockPrefab.transform.name.ToString().IndexOf('X');
                        int length= (int)System.Char.GetNumericValue(blockPrefab.transform.name.ToString()[stringIndex - 1]);
                        int width = (int)System.Char.GetNumericValue(blockPrefab.transform.name.ToString()[stringIndex + 1]);
                        if (length == 2 && width == 1)
                        {
                            width = 2;
                            length = 1;
                        }
                        bool isBase = true;
                        if (width != 2)
                        {
                            x = x - width / 2 +1;
                        }
                        if (length > 1)
                        {
                            z -= length / 2;
                        }
                        for (int i = 0; i < length; i++)
                        {
                            for (int j = 0; j < width; j++)
                            {
                                if (blocks[x + j, 0, z + i] != null)
                                {
                                    isBase = false;
                                    Debug.Log("isBase is false");
                                    break;
                                }
                            }
                        }
                        if (isBase)
                        {
                            if (x + width <= 13  && z + length <= 13 && x>2 &&z>2)
                            {
                                index.y = 0;
                                y = 0;
                            go = Instantiate(blockPrefab) as GameObject;
                            go.GetComponent<Renderer>().material = mat;
                            go.AddComponent<BoxCollider>();
                            BoxCollider collider = go.GetComponent<BoxCollider>();
                                collider.size = new Vector3(0.5f, 0.5f, 0.5f);
                                if (length == 1 && width == 2) { go.transform.Rotate(0, 0, 90.0f); }
                                go.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
                            PositionBlock(go.transform, index);
                                undoGo.Add(go);
                            Debug.Log("Height:"  + go.transform.position);
							Debug.Log ("1" + isRotated);

                                /*blocks[x, y, z] = new Block
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
                            blockPosition.Add(new Vector3(x, y, z+1));*/
                                for (int i = 0; i < length; i++)
                                {
                                    for (int j = 0; j < width; j++)
                                    {
                                        blocks[x + j, y, z + i] = new Block
                                        {
                                            blockTransform = go.transform,
                                            height = new Vector3(0, 1, 0),
                                            rotate = true,
                                            color = blockColor,
                                            type = blockPrefab.transform.name.ToString().Replace("(Clone)","")
                                        };
                                        blockPosition.Add(new Vector3(x + j, y, z + i));
                                    }
                                }
                            }
                    }
                    else
                    {
                            /*Vector3 newHeight;
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
                            }*/
                            Vector3 newHeight = new Vector3(0, 0, 0);
                            for (int i = 0; i < length; i++)
                            {
                                for (int j = 0; j < width; j++)
                                {
                                    if (blocks[x + j, 0, z + i] != null)
                                    {
                                        if (blocks[x + j, 0, z + i].height.y > newHeight.y)
                                        {
                                            newHeight = blocks[x + j, 0, z + i].height;
                                        }
                                    }
                                }
                            }
                            Vector3 newIndex = new Vector3(index.x, newHeight.y, index.z);
                            if (!isBase)
                        {
                                if (x + width <= 13 && z + length <= 13 && x > 2 && z > 2)
                                {
                                    go = Instantiate(blockPrefab) as GameObject;
                                go.GetComponent<Renderer>().material = mat;
                                go.AddComponent<BoxCollider>();
                                BoxCollider collider = go.GetComponent<BoxCollider>();
                                collider.size = new Vector3(0.5f,0.5f,0.5f);
                                    if (length == 1 && width == 2) { go.transform.Rotate(0, 0, 90.0f); }
                                    go.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
                                PositionBlock(go.transform, newIndex);
                                    undoGo.Add(go);
                                Debug.Log("Height2:" + go.transform.position);
								Debug.Log ("2" + isRotated);
                                    /*if (blocks[x, 0, z] == null)
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
                                        blockPosition.Add(new Vector3((int)newIndex.x, (int)newIndex.y, (int)newIndex.z + 1));*/
                                    for (int i = 0; i < length; i++)
                                    {
                                        for (int j = 0; j < width; j++)
                                        {
                                            if (blocks[x + j, 0, z + i] == null)
                                            {
                                                blocks[x + j, 0, z + i] = new Block { blockTransform = null, height = newHeight + new Vector3(0, 1, 0), rotate = true, color = go.transform.GetComponent<Renderer>().material.color.ToString() };
                                            }
                                            else
                                            {
                                                blocks[x + j, 0, z + i].height = newHeight + new Vector3(0, 1, 0);
                                            }
                                            blocks[x + j,(int) newIndex.y, z + i] = new Block
                                            {
                                                blockTransform = go.transform,
                                                height = blocks[x + j, 0, z + i].height,
                                                rotate = true,
                                                color = blockColor,
                                                type = blockPrefab.transform.name.ToString().Replace("(Clone)", "")
                                            };
                                            blockPosition.Add(new Vector3(x + j, (int)newIndex.y, z + i));
                                        }
                                    }
                                }
                        }
                    }
                }
                else {
                        //index.z -= 1;
                        int stringIndex = blockPrefab.transform.name.ToString().IndexOf('X');
                        int width = (int)System.Char.GetNumericValue(blockPrefab.transform.name.ToString()[stringIndex - 1]);
                        int length = (int)System.Char.GetNumericValue(blockPrefab.transform.name.ToString()[stringIndex + 1]);
                        if (length == 1 && width == 2)
                        {
                            width = 1;
                            length = 2;
                        }
                        if (width >1)
                        {
                            x = x - width / 2 +1;
                        }
                        if (length !=2)
                        {
                            z = z-length / 2+1;
                        }
                        Debug.Log("Width:" + width + "Length:" + length);
                        bool isBase = true;
                        for (int i = 0; i < length; i++)
                        {
                            for (int j = 0; j < width; j++)
                            {
                                if (blocks[x+j, 0, z+i] != null) {
                                    isBase = false;
                                    break;
                                }
                            }
                        }
                                if (isBase)
                    {
                            if (x + width <= 13 && z + length <= 13 && x > 2 && z > 2)
                            {
                                index.y = 0;
                                y = 0;
                            go = Instantiate(blockPrefab) as GameObject;
                            go.GetComponent<Renderer>().material = mat;
                            go.AddComponent<BoxCollider>();
                            BoxCollider collider = go.GetComponent<BoxCollider>();
                            collider.size = new Vector3(0.5f,0.5f,0.5f);
                            go.transform.Rotate(0, 0, 90.0f);
                                if (length == 2 && width == 1) { go.transform.Rotate(0, 0, 270.0f); }
                                go.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);    
                            PositionBlock(go.transform, index);
                                undoGo.Add(go);
                            Debug.Log("hit:" + go.transform.position);
							Debug.Log ("3" + isRotated);
                                /*blocks[x, y, z] = new Block
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
                                blockPosition.Add(new Vector3(x+1, y, z));*/
                                for (int i = 0; i < length; i++) {
                                    for (int j = 0; j < width; j++) {
                                        blocks[x + j, y, z+i] = new Block
                                        {
                                            blockTransform = go.transform,
                                            height = new Vector3(0, 1, 0),
                                            rotate = false,
                                            color = blockColor,
                                            type = blockPrefab.transform.name.ToString().Replace("(Clone)", "")
                                        };
                                        blockPosition.Add(new Vector3(x+j, y, z+i));
                                    }
                                }
                                
                        }
                    }
                    else
                    {
                            Vector3 newHeight=new Vector3(0,0,0);
                            for (int i = 0; i < length; i++)
                            {
                                for (int j = 0; j < width; j++)
                                {
                                    if (blocks[x+j, 0, z+i] != null) {
                                        if (blocks[x + j, 0, z + i].height.y > newHeight.y) {
                                            newHeight = blocks[x + j, 0, z + i].height;
                                        }
                                    }
                                }
                            }
                                    /*if (blocks[x, 0, z] != null)
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
                        }*/
						Debug.Log ("4" + isRotated);
                            //Vector3 newIndex = BlockPosition(hit.point + (hit.normal * blockSize));
                            Vector3 newIndex = new Vector3(index.x, newHeight.y,index.z);
                        //newIndex.z -= 1;
                        if (!isBase)
                        {
                                if (x + width <= 13 && z + length <= 13 && x > 2 && z > 2)
                                {
                                    go = Instantiate(blockPrefab) as GameObject;
                                    go.GetComponent<Renderer>().material = mat;
                                    go.AddComponent<BoxCollider>();
                                BoxCollider collider = go.GetComponent<BoxCollider>();
                                collider.size = new Vector3(1, 0.5f, 1);
                                go.transform.Rotate(0, 0, 90);
                                    if (length == 2 && width == 1) { go.transform.Rotate(0, 0, 270.0f); }
                                    go.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
                                PositionBlock(go.transform, newIndex);
                                    undoGo.Add(go);
                                    
                                    /*if (blocks[x, 0, z] == null)
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
                                    blockPosition.Add(new Vector3((int)newIndex.x+1, (int)newIndex.y, (int)newIndex.z));*/
                                    for (int i = 0; i < length; i++)
                                    {
                                        for (int j = 0; j < width; j++)
                                        {
                                            if (blocks[x+j, 0, z+i] == null)
                                            {
                                                blocks[x + j, 0, z + i] = new Block { blockTransform = null, height = newHeight + new Vector3(0, 1, 0), rotate = true, color = go.transform.GetComponent<Renderer>().material.color.ToString() };
                                            }
                                            else
                                            {
                                                blocks[x + j, 0, z + i].height = newHeight + new Vector3(0, 1, 0);
                                            }
                                            blocks[x + j, (int)newIndex.y, z + i] = new Block
                                            {
                                                blockTransform = go.transform,
                                                height = blocks[x + j, 0, z + i].height,
                                                rotate = false,
                                                color = blockColor,
                                                type = blockPrefab.transform.name.ToString().Replace("(Clone)", "")
                                            };
                                            blockPosition.Add(new Vector3(x + j, (int)newIndex.y, z + i));
                                        }
                                    }
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
            int stringIndex = undoGo[undoGo.Count - 1].transform.name.ToString().IndexOf('X');
            int length = (int)System.Char.GetNumericValue(undoGo[undoGo.Count - 1].transform.name.ToString()[stringIndex - 1]);
            int width = (int)System.Char.GetNumericValue(undoGo[undoGo.Count - 1].transform.name.ToString()[stringIndex + 1]);
            int size = length * width;
            Debug.Log("size:" + size);
            Destroy(undoGo[undoGo.Count-1]);
                undoGo.RemoveAt(undoGo.Count-1);
                for (int i = blockPosition.Count-1; i >= blockPosition.Count-size; i--)
            {
                Debug.Log("block pos y:" + blockPosition[i].y);
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
                for(int i = 0; i < size; i++) {
                Debug.Log("position remove:" + blockPosition[blockPosition.Count - 1]);
                blockPosition.RemoveAt(blockPosition.Count - 1); }

        }
    }
    void back()
    {
        popupBack.gameObject.SetActive(true);
        hidemenu.gameObject.SetActive(false);
        menu.gameObject.SetActive(true);
        scrollView.gameObject.SetActive(false);
        btnro.gameObject.SetActive(true);
        btnun.gameObject.SetActive(true);
        scrollBar.gameObject.SetActive(false);
        hideColour.gameObject.SetActive(false);
        showColour.gameObject.SetActive(true);
        colour.gameObject.SetActive(false);
    }
    void showmenu() {
        menu.gameObject.SetActive(false);
        scrollView.gameObject.SetActive(true);
        btnro.gameObject.SetActive(false);
        btnun.gameObject.SetActive(false);
        hidemenu.gameObject.SetActive(true);
        scrollBar.gameObject.SetActive(true);
    }
    void hidem()
    {
        hidemenu.gameObject.SetActive(false);
        menu.gameObject.SetActive(true);
        scrollView.gameObject.SetActive(false);
        btnro.gameObject.SetActive(true);
        btnun.gameObject.SetActive(true);
        scrollBar.gameObject.SetActive(false);

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
        popupSave.gameObject.SetActive(true);
        hidemenu.gameObject.SetActive(false);
        menu.gameObject.SetActive(true);
        scrollView.gameObject.SetActive(false);
        btnro.gameObject.SetActive(true);
        btnun.gameObject.SetActive(true);
        scrollBar.gameObject.SetActive(false);
        hideColour.gameObject.SetActive(false);
        showColour.gameObject.SetActive(true);
        colour.gameObject.SetActive(false);
    }
	void loadGame(){
		for (int i = 0; i < saveData2.Count;i++)
		{

			string x = saveData2[i].GetField("position").GetField("x")+"";
			string y = saveData2[i].GetField("position").GetField("y")+"";
			string z = saveData2[i].GetField("position").GetField("z")+"";
			Debug.Log ("xyz:"+float.Parse(x)+y+z);
            string arrayindex=saveData2[i].GetField("arrayindex").ToString();
			Vector3 index =new Vector3(float.Parse(x),float.Parse(y),float.Parse(z));
            string heightstring = saveData2[i].GetField("height").ToString().Replace("\"", "");
            Vector3 newheight = new Vector3(0, (float)Char.GetNumericValue(heightstring[0]), 0);
            Debug.Log("height:" + newheight);
            blockPrefab = Resources.Load((saveData2[i].GetField("type")+"").Replace("\"",""), typeof(GameObject)) as GameObject;
            go = Instantiate(blockPrefab) as GameObject;
            go.GetComponent<Renderer>().material = Resources.Load((saveData2[i].GetField("color") + "").Substring(1, (saveData2[i].GetField("color") + "").Length-2), typeof(Material)) as Material; 
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


	
    void setBlockType(string type)
    {
        blocktype = type;
        Debug.Log("type:" + type);
        blockPrefab = Resources.Load(type, typeof(GameObject)) as GameObject;
    }
    void setColor(string color)
    {
        mat = Resources.Load(color, typeof(Material)) as Material;
        twot1.GetComponent<Renderer>().material = mat; 
        twot2.GetComponent<Renderer>().material = mat;
        twot4.GetComponent<Renderer>().material = mat;
        twot6.GetComponent<Renderer>().material = mat;
        twot8.GetComponent<Renderer>().material = mat;
        blockColor = color;
    }
    void closeS()
    {
        popupSave.gameObject.SetActive(false);
        
    }
    void closeB()
    {
        popupBack.gameObject.SetActive(false);

    }
    void sendOnly()
    {
        saveData2 = new JSONObject(JSONObject.Type.ARRAY);
        string saveData = "";
        Block[,,] b = GameManager.Instance.blocks;
        for (int y = 0; y < 20; y++)
        {
            for (int x = 0; x < 20; x++)
            {
                for (int z = 0; z < 20; z++)
                {
                    JSONObject saveData1 = new JSONObject(JSONObject.Type.OBJECT);
                    JSONObject xyz = new JSONObject(JSONObject.Type.OBJECT);
                    Block currentBlock = b[x, y, z];
                    if (currentBlock == null || currentBlock.blockTransform == null)
                        continue;


                    /* saveData += "position:"+currentBlock.blockTransform.position+","+
                                 "color:"+currentBlock.color + "," +
                                 "type:"+"2*1" +"," + 
                                 "rotate:"+currentBlock.rotate+"&";*/
                    Debug.Log(currentBlock.blockTransform.position);
                    xyz.AddField("x", currentBlock.blockTransform.position.x);
                    xyz.AddField("y", currentBlock.blockTransform.position.y);
                    xyz.AddField("z", currentBlock.blockTransform.position.z);
                    saveData1.AddField("arrayindex", x + "" + y + "" + z);
                    saveData1.AddField("position", xyz);
                    saveData1.AddField("height", currentBlock.height.y);
                    saveData1.AddField("color", currentBlock.color);
                    saveData1.AddField("type", currentBlock.type);
                    saveData1.AddField("rotate", currentBlock.rotate);
                    saveData2.Add(saveData1);
                }
            }
        }
        Debug.Log("array0:" + saveData2[1].GetField("position"));
        Debug.Log(saveData2);
        JSONObject finalData = new JSONObject(JSONObject.Type.OBJECT);
        finalData.AddField("id", id);
        finalData.AddField("vocab", "alan");
        finalData.AddField("createtime", System.DateTime.Now.ToString());
        finalData.AddField("block", saveData2);
        finalData.AddField("invite", id);
        if (finalData != null)
        {
            FB.AppRequest(
            "ALan,Here is a free gift!",
            null,
            new List<object>() { "app_users" },
            null, null, null, "ALan title",
            delegate (IAppRequestResult result) {
                Debug.Log(result.RawResult);
                socket.Emit("SAVE", finalData);
                SceneManager.LoadScene("menu");
            }
        );
        }
        socket.Emit("SAVE", finalData);
    }
    void sendShare()
    {

    }
    void backmenu()
    {
        SceneManager.LoadScene("menu");
    }
}
