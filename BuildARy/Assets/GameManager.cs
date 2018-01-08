using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block{
    public Transform blockTransform;
}

public class GameManager : MonoBehaviour {
    private float blockSize = 0.5f;
    public Block[,,] blocks = new Block[20,20,20];
    public GameObject blockPrefab;
    private GameObject foundationObject;
    private Vector3 blockOffset = new Vector3(0.5f,0.5f,0.5f);
    private Vector3 foundationCenter = new Vector3(0, 0, 0);

	void Start () {
        foundationObject = GameObject.Find("Foundation");

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
                if (blocks[x, y, z] == null && blocks[x, y, z-1] == null)
                {
                    if(x <= 12 && z < 12){
                    GameObject go = Instantiate(blockPrefab) as GameObject;
                    go.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
                    PositionBlock(go.transform, index);
                    blocks[x, y, z] = new Block
                    {
                        blockTransform = go.transform
                    };
                    blocks[x, y, z-1] = new Block
                    {
                        blockTransform = go.transform
                    };
                    }
                }
                else {
                    Debug.Log("Error:postion" + index.ToString());
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

}
