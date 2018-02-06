using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Block{
    public Transform blockTransform;
    public Vector3 height = new Vector3(0,1,0);
}

public class GameManager : MonoBehaviour {
    private float blockSize = 0.5f;
    public Block[,,] blocks = new Block[20,20,20];
    public GameObject blockPrefab;
    private bool isRotated = true;
	private bool isUndo = false;
    private GameObject foundationObject;
    private Vector3 blockOffset = new Vector3(0.5f,0.5f,0.5f);
    private Vector3 foundationCenter = new Vector3(0, 0, 0);
	//public Button[] button1;
	//public int numOfMenu = 3;
	public Button btnro;
    public Button btnun;
    public Button btnbak;
    public int count = 0;
    public GameObject go;
    public Vector3[] undovec = new Vector3[2];
    Hashtable arguments;
    void Start () {
        foundationObject = GameObject.Find("Foundation");
		Button btn1 = btnro.GetComponent<Button> ();
        BoxCollider2D btn1co = btn1.GetComponent<BoxCollider2D>();
		btn1.onClick.AddListener (rotate);
        Button btn2 = btnun.GetComponent<Button>();
        btn2.onClick.AddListener(undo);
        Button btn3 = btnbak.GetComponent<Button>();
        btn3.onClick.AddListener(back);
        arguments = SceneManager.GetSceneArguments();
        Debug.Log("Arguments: " + arguments["key"] );
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
	
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
			
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit, 30.0f))
            {
                if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) {
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
                            Debug.Log("Height:"  + go.transform.position);
							Debug.Log ("1" + isRotated);
                            blocks[x, y, z] = new Block
                            {
                                blockTransform = go.transform,
                                height = new Vector3(0, 1, 0)
                            };
                            blocks[x, y, z + 1] = new Block
                            {
                                blockTransform = go.transform,
                                height = new Vector3(0, 1, 0)
                            };
                            undovec[0] = new Vector3(x, y, z);
                            undovec[1] = new Vector3(x, y, z+1);
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
                                Debug.Log("Height2:" + go.transform.position);
								Debug.Log ("2" + isRotated);
                                if (blocks[x, 0, z] == null)
                                {
                                    blocks[x, 0, z] = new Block { height = newHeight + new Vector3(0, 1, 0) };
                                }
                                else
                                {
                                    blocks[x, 0, z].height = newHeight + new Vector3(0, 1, 0);
                                }
                                if (blocks[x, 0, z + 1] == null)
                                {
                                    blocks[x, 0, z + 1] = new Block { height = blocks[x, 0, z].height };
                                }
                                else
                                {
                                    blocks[x, 0, z + 1].height = blocks[x, 0, z].height;
                                }
                                blocks[(int)newIndex.x, (int)newIndex.y, (int)newIndex.z] = new Block
                                {
                                    blockTransform = go.transform,
                                    height = blocks[x, 0, z].height + new Vector3(0, 1, 0)
                                };
                                blocks[(int)newIndex.x, (int)newIndex.y, (int)newIndex.z + 1] = new Block
                                {
                                    blockTransform = go.transform,
                                    height = (blocks[x, 0, z+1].height!=null? blocks[x, 0, z+1].height:new Vector3(0,1,0)) + new Vector3(0, 1, 0)
                                };
                                undovec[0] = new Vector3((int)newIndex.x, (int)newIndex.y, (int)newIndex.z);
                                undovec[1] = new Vector3((int)newIndex.x, (int)newIndex.y, (int)newIndex.z + 1);
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
                            Debug.Log("hit:" + go.transform.position);
							Debug.Log ("3" + isRotated);
                            blocks[x, y, z] = new Block
                            {
                                blockTransform = go.transform,
                                height = new Vector3(0, 1, 0)
                            };
                            blocks[x+1, y, z] = new Block
                            {
                                blockTransform = go.transform,
                                height = new Vector3(0, 1, 0)
                            };
                            undovec[0] = new Vector3(x, y, z);
                            undovec[1] = new Vector3(x+1, y, z);
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
                                if (blocks[x, 0, z] == null)
                                {
                                    blocks[x, 0, z] = new Block { height = newHeight + new Vector3(0, 1, 0) };
                                }
                                else
                                {
                                    blocks[x, 0, z].height = newHeight + new Vector3(0, 1, 0);
                                }
                                if (blocks[x + 1, 0, z] == null)
                                {
                                    blocks[x + 1, 0, z] = new Block { height = blocks[x, 0, z].height };
                                }
                                else
                                {
                                    blocks[x + 1, 0, z].height = blocks[x, 0, z].height;
                                }
                                blocks[(int)newIndex.x, (int)newIndex.y, (int)newIndex.z] = new Block
                                {
                                    blockTransform = go.transform,
                                    height = blocks[x, 0, z].height + new Vector3(0, 1, 0)
                                };
                                blocks[(int)newIndex.x+1, (int)newIndex.y, (int)newIndex.z] = new Block
                                {
                                    blockTransform = go.transform,
                                    height = blocks[x+1, 0, z].height + new Vector3(0, 1, 0)
                                };
                                undovec[0] = new Vector3((int)newIndex.x, (int)newIndex.y, (int)newIndex.z);
                                undovec[1] = new Vector3((int)newIndex.x+1, (int)newIndex.y, (int)newIndex.z);
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
        if (go != null)
        {
            Destroy(go);
            Debug.Log("GO:"+go);
            for (int i = 0; i < 2; i++)
            {
                blocks[(int)undovec[i].x, (int)undovec[i].y, (int)undovec[i].z] = null;
                if (undovec[i].y != 0)
                {
                    blocks[(int)undovec[i].x, 0, (int)undovec[i].z].height -= new Vector3(0, 1, 0);
                    if (blocks[(int)undovec[i].x, 0, (int)undovec[i].z].height.y == 0) {
                        blocks[(int)undovec[i].x, 0, (int)undovec[i].z] = null;
                    }
                }
                Debug.Log("af" + (int)undovec[i].x + (int)undovec[i].y + (int)undovec[i].z);
            }
        }
    }
    void back()
    {
        SceneManager.LoadScene("menu");
    }
  

}
