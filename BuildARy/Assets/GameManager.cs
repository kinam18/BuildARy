using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block{
    public Transform blockTransform;
    public Vector3 height = new Vector3(0,1,0);
}

public class GameManager : MonoBehaviour {
    private float blockSize = 0.5f;
    public Block[,,] blocks = new Block[20,20,20];
    public GameObject blockPrefab;
    private bool isRotated = true;
    private GameObject foundationObject;
    private Vector3 blockOffset = new Vector3(0.5f,0.5f,0.5f);
    private Vector3 foundationCenter = new Vector3(0, 0, 0);
	public Button button1;

	void Start () {
        foundationObject = GameObject.Find("Foundation");
		/*Button rotateButton = button1.GetComponents<Button>();
		rotateButton.onClick.AddListener (rotate);*/
	}
	
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
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
                            Debug.Log("Height:"  + go.transform.position);
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
                        if (blocks[x, y, z] == null){
                            blocks[x, y, z] = new Block { height = blocks[x, y, z - 1].height };
                        }
                        Vector3 newIndex = BlockPosition(hit.point + (blocks[x, y, z].height * blockSize));
                        if (blocks[x, (int)newIndex.y, z] == null && blocks[x, (int)newIndex.y, z + 1] == null)
                        {
                            if (x <= 12 && z < 12)
                            {
                                GameObject go = Instantiate(blockPrefab) as GameObject;
                                go.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
                                PositionBlock(go.transform, newIndex);
                                Debug.Log("Height2:" + go.transform.position);
                                blocks[(int)newIndex.x, (int)newIndex.y, (int)newIndex.z] = new Block
                                {
                                    blockTransform = go.transform,
                                    height = blocks[x, y, z].height + new Vector3(0, 1, 0)
                                };
                                blocks[(int)newIndex.x, (int)newIndex.y, (int)newIndex.z + 1] = new Block
                                {
                                    blockTransform = go.transform,
                                    height = blocks[x, y, z + 1].height + new Vector3(0, 1, 0)
                                };
                                blocks[x, y, z].height += new Vector3(0, 1, 0);
                                blocks[x, y, z + 1] = new Block { height = blocks[x, y, z].height };
                            }
                        }
                    }
                }
                else {
                    if (blocks[x, y, z] == null && blocks[x+1, y, z] == null)
                    {
                        if (x < 12 && z <= 12)
                        {
                            GameObject go = Instantiate(blockPrefab) as GameObject;
                            go.transform.Rotate(0, 0, 90);
                            go.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
                            index.z -= 1;
                            PositionBlock(go.transform, index);
                            Debug.Log("hit:" + go.transform.position);
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
                        if (blocks[x, y, z] == null)
                        {
                            blocks[x, y, z] = new Block { height = blocks[x-1, y, z].height };
                        }
                        Vector3 newIndex = BlockPosition(hit.point + (blocks[x, y, z].height * blockSize));
                        if (blocks[(int)newIndex.x, (int)newIndex.y, (int)newIndex.z] == null && blocks[(int)newIndex.x + 1, (int)newIndex.y, (int)newIndex.z] == null)
                        {
                            if (x < 12 && z <= 12)
                            {
                                GameObject go = Instantiate(blockPrefab) as GameObject;
                                go.transform.Rotate(0, 0, 90);
                                go.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
                                newIndex.z -= 1;
                                PositionBlock(go.transform, newIndex);
                                blocks[(int)newIndex.x, (int)newIndex.y, (int)newIndex.z] = new Block
                                {
                                    blockTransform = go.transform,
                                    height = blocks[x, y, z].height + new Vector3(0, 1, 0)
                                };
                                blocks[(int)newIndex.x+1, (int)newIndex.y, (int)newIndex.z] = new Block
                                {
                                    blockTransform = go.transform,
                                    height = blocks[x+1, y, z].height + new Vector3(0, 1, 0)
                                };
                                blocks[x, y, z].height += new Vector3(0, 1, 0);
                                blocks[x+1, y, z] = new Block { height = blocks[x, y, z].height };
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
	}

}
