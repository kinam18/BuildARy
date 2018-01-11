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
    private bool isRotated = false;
    private GameObject foundationObject;
    private Vector3 blockOffset = new Vector3(0.5f,0.5f,0.5f);
    private Vector3 foundationCenter = new Vector3(0, 0, 0);
	//public Button[] button1;
	//public int numOfMenu = 3;
	public Button btn1;
	public int count = 0;
	private EventSystem es;

	void Start () {
        foundationObject = GameObject.Find("Foundation");
		btn1 = GetComponent<Button> ();
		btn1.onClick.AddListener (rotate);
		es = FindObjectOfType<EventSystem> ();
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
			if (es.IsPointerOverGameObject ())
				return;
            RaycastHit hit;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit, 30.0f))
            {
                Vector3 index = BlockPosition(hit.point);
                Debug.Log(index.ToString());
                int x = (int)index.x, y=(int)index.y, z = (int)index.z;
                if (!isRotated)
                {
                    if (blocks[x, y, z] == null && blocks[x, y, z + 1] == null)
                    {
                        if (x <= 12 && z < 12)
                        {
                            GameObject go = Instantiate(blockPrefab) as GameObject;
                            go.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
                            PositionBlock(go.transform, index);
<<<<<<< HEAD
                            Debug.Log("Height:"  + go.transform.position);
							Debug.Log ("1" + isRotated);
=======
>>>>>>> 920577fe753821de6bdabc93b9854fe9d6929f9b
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
                        }
                    }
                    else
                    {
                        Vector3 newHeight;
                        if (blocks[x, y, z] != null)
                        {
                            if (blocks[x, y, z + 1] != null)
                            {
                                newHeight = blocks[x, y, z].height.y > blocks[x, y, z + 1].height.y ? blocks[x, y, z].height : blocks[x, y, z + 1].height;
                            }
                            else
                            {
                                newHeight = blocks[x, y, z].height;
                            }
                        }
                        else
                        {
                            newHeight = blocks[x, y, z + 1].height;
                        }
                        Vector3 newIndex = BlockPosition(hit.point + (newHeight* blockSize));
                        if (blocks[x, (int)newIndex.y, z] == null && blocks[x, (int)newIndex.y, z + 1] == null)
                        {
                            if (x <= 12 && z < 12)
                            {
                                GameObject go = Instantiate(blockPrefab) as GameObject;
                                go.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
                                PositionBlock(go.transform, newIndex);
<<<<<<< HEAD
                                Debug.Log("Height2:" + go.transform.position);
								Debug.Log ("2" + isRotated);
=======
                                if (blocks[x, y, z] == null)
                                {
                                    blocks[x, y, z] = new Block { height = newHeight + new Vector3(0, 1, 0) };
                                }
                                else
                                {
                                    blocks[x, y, z].height = newHeight + new Vector3(0, 1, 0);
                                }
                                if (blocks[x, y, z + 1] == null)
                                {
                                    blocks[x, y, z + 1] = new Block { height = blocks[x, y, z].height };
                                }
                                else
                                {
                                    blocks[x, y, z + 1].height = blocks[x, y, z].height;
                                }
>>>>>>> 920577fe753821de6bdabc93b9854fe9d6929f9b
                                blocks[(int)newIndex.x, (int)newIndex.y, (int)newIndex.z] = new Block
                                {
                                    blockTransform = go.transform,
                                    height = blocks[x, y, z].height + new Vector3(0, 1, 0)
                                };
                                blocks[(int)newIndex.x, (int)newIndex.y, (int)newIndex.z + 1] = new Block
                                {
                                    blockTransform = go.transform,
                                    height = (blocks[x, y, z+1].height!=null? blocks[x, y, z+1].height:new Vector3(0,1,0)) + new Vector3(0, 1, 0)
                                };
                                
                            }
                        }
                    }
                }
<<<<<<< HEAD
				else
				{
=======
<<<<<<< HEAD
                else {
                    index.z -= 1;
=======
				if(isRotated) {
>>>>>>> ea43f5a09995a67a8f660354a8bfd3e9e859f538
>>>>>>> 920577fe753821de6bdabc93b9854fe9d6929f9b
                    if (blocks[x, y, z] == null && blocks[x+1, y, z] == null)
                    {
                        if (x < 12 && z <= 12)
                        {
                            GameObject go = Instantiate(blockPrefab) as GameObject;
							go.transform.Rotate(0, 0, 90.0f);
                            go.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
                            
                            PositionBlock(go.transform, index);
<<<<<<< HEAD
                            Debug.Log("hit:" + go.transform.position);
							Debug.Log ("3" + isRotated);
=======
>>>>>>> 920577fe753821de6bdabc93b9854fe9d6929f9b
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
                        }
                    }
                    else
                    {
                        Vector3 newHeight;
                        if (blocks[x, y, z] != null)
                        {
                            if (blocks[x + 1, y, z] != null)
                            {
                                newHeight = blocks[x, y, z].height.y > blocks[x + 1, y, z].height.y ? blocks[x, y, z].height : blocks[x + 1, y, z].height;
                            }
                            else {
                                newHeight = blocks[x, y, z].height;
                            }
                            }
                        else {
                            newHeight = blocks[x+1, y, z].height;
                        }
<<<<<<< HEAD
						Debug.Log ("4" + isRotated);
                        Vector3 newIndex = BlockPosition(hit.point + (blocks[x, y, z].height * blockSize));
=======
                        Vector3 newIndex = BlockPosition(hit.point + (newHeight * blockSize));
                        newIndex.z -= 1;
>>>>>>> 920577fe753821de6bdabc93b9854fe9d6929f9b
                        if (blocks[(int)newIndex.x, (int)newIndex.y, (int)newIndex.z] == null && blocks[(int)newIndex.x + 1, (int)newIndex.y, (int)newIndex.z] == null)
                        {
                            if (x < 12 && z <= 12)
                            {
                                GameObject go = Instantiate(blockPrefab) as GameObject;
                                go.transform.Rotate(0, 0, 90);
                                go.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
                                
                                PositionBlock(go.transform, newIndex);
                                if (blocks[x, y, z] == null)
                                {
                                    blocks[x, y, z] = new Block { height = newHeight + new Vector3(0, 1, 0) };
                                }
                                else
                                {
                                    blocks[x, y, z].height = newHeight + new Vector3(0, 1, 0);
                                }
                                if (blocks[x + 1, y, z] == null)
                                {
                                    blocks[x + 1, y, z] = new Block { height = blocks[x, y, z].height };
                                }
                                else
                                {
                                    blocks[x + 1, y, z].height = blocks[x, y, z].height;
                                }
                                blocks[(int)newIndex.x, (int)newIndex.y, (int)newIndex.z] = new Block
                                {
                                    blockTransform = go.transform,
                                    height = blocks[x, y, z].height + new Vector3(0, 1, 0)
                                };
                                blocks[(int)newIndex.x+1, (int)newIndex.y, (int)newIndex.z] = new Block
                                {
                                    blockTransform = go.transform,
                                    height = (blocks[x+1, y, z].height!=null? blocks[x + 1, y, z].height:new Vector3(0,1,0)) + new Vector3(0, 1, 0)
                                };
                                
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
        t.position = ((index * blockSize) + blockOffset)/* + (foundationObject.transform.position - foundationCenter)*/;
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
<<<<<<< HEAD

=======
		
>>>>>>> 373c4130937e129472e08d3b22db4fb59b0e6856
}
