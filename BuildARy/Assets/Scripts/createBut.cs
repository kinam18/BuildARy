using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class createBut : MonoBehaviour
{
    public Button CreateButton;
    // Use this for initialization
    void Start()
    {
        CreateButton = GetComponent<Button>();
        CreateButton.onClick.AddListener(onclik);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void onclik()
    {
        SceneManager.LoadScene("game");
    }
}