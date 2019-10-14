using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossRoomSwitch : MonoBehaviour
{
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D trigger)
    {
        Debug.Log("thing");
        if (trigger.gameObject.tag == "Player")
        {
            Debug.Log("trigger!");
            SceneManager.LoadScene("New Scene");
            player = GameObject.Find("Player");
            //player.GetComponent<CharacterController2D>().BATTLETIME = true;
        }
    }
}
