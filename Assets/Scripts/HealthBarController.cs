using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    // Start is called before the first frame update
    private Slider healthBar;
    public int currentHP = 100;
    public GameObject gameobj;
    void Start()
    {

    }

    private void Awake()
    {
        healthBar = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = currentHP;

        if (currentHP == 0)
        {
            if(GameObject.Find("TY") != null)
            {
                GameObject.Find("TY").transform.position = new Vector3(11.50f, 5.19f, 0);
            }
            Destroy(gameobj);
        }
    }

    public void changeHP(int dHP)
    {
        currentHP -= dHP;
    }

}
