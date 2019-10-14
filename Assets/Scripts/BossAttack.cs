using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossAttack : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animation;
    string state = "idle";
    float time;
    int wait_time;

    float timer = 0;
    float timerMax = 3f;
    float idleTimer = 0;
    float idleTimerMax = 4f;
    GameObject player;

    [SerializeField]
    private Slider healthbarPlayer;
    HealthBarController healthscript;

    [SerializeField]
    private Slider healthbarBoss;

    void Start()
    {
        animation = GetComponent<Animator>();
        player = GameObject.Find("Player");
        healthscript = GetComponent<HealthBarController>();
    }

    // called when the cube hits the floor
    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            if (state == "Down" || state == "Up")
            {
                Debug.Log(healthbarPlayer.GetComponent<HealthBarController>().currentHP);
                healthbarPlayer.GetComponent<HealthBarController>().changeHP(5);
            }
        }
        if(coll.gameObject.tag == "Projectile")
        {
            //Debug.Log(healthbarBoss.GetComponent<HealthBarController>().currentHP);
            healthbarBoss.GetComponent<HealthBarController>().changeHP(10);
        }
    }


    // Update is called once per frame
    void Update()
    {

        if (GameObject.Find("Player") != null)
        {
            if (state != "idle")
            {
                //switch animation
                switch (state)
                {
                    case "Up":
                        animation.SetBool("ComeBack", true);
                        animation.SetBool("GoDown", false);
                        animation.SetBool("DirtKick", false);
                        break;
                    case "Down":
                        animation.SetBool("GoDown", true);
                        animation.SetBool("ComeBack", false);
                        animation.SetBool("DirtKick", false);
                        break;
                    case "InGround":
                        animation.SetBool("GoDown", false);
                        animation.SetBool("ComeBack", false);
                        animation.SetBool("DirtKick", true);
                        break;
                    default:
                        state = "idle";
                        animation.SetBool("GoDown", false);
                        animation.SetBool("ComeBack", false);
                        animation.SetBool("DirtKick", false);
                        break;
                }
            }
            else
            {
                state = "idle";
                idleTimer += Time.deltaTime;
                switch (state)
                {
                    case "Up":
                        animation.SetBool("ComeBack", true);
                        break;
                    case "Down":
                        animation.SetBool("GoDown", true);
                        break;
                    case "InGround":
                        animation.SetBool("DirtKick", true);
                        break;
                    default:
                        state = "idle";
                        animation.SetBool("GoDown", false);
                        animation.SetBool("ComeBack", false);
                        animation.SetBool("DirtKick", false);
                        break;
                }
                if (idleTimer > idleTimerMax)
                {
                    state = "Down";
                    idleTimer = 0;
                }
            }
        } else
        {
            state = "idle";
        }
    }

    bool GoDown()
    {
        while (this.transform.position.y >= -14)
        {
            this.transform.position = new Vector3(transform.position.x, transform.position.y - 1, 0);
        }
        return true;
    }

    Vector3 InGround()
    {
        this.transform.localPosition = new Vector3(player.transform.position.x, -8, 0);
        return this.transform.localPosition;
    }

    bool GoUp(Vector3 shootUpAt)
    {
        while (this.transform.position.y < 3.94f)
        {
            this.transform.Translate(Vector3.up * Time.deltaTime * 2, Space.World);
        }
        return true;
    }

    public bool downcheck = false;
    float currentTime;

    float timerWait = 0;
    float timerComeUp = 0;
    int timerWaitTime = 4;
    Coroutine lastRoutine = null;
    bool secondEnum = false;
    bool thirdEnum = false;

    void FixedUpdate()
    {
        if(GameObject.Find("Player") != null)
        {
            if (state != "idle")
            {
                timer += Time.deltaTime;

                if (downcheck && !thirdEnum)
                {
                    state = "InGround";
                    StartCoroutine(FollowPlayer());
                    timerWait += Time.deltaTime;
                }
                else if (timer >= 0.3f && !downcheck)
                {
                    lastRoutine = StartCoroutine(MoveDownTwo());
                }


                if (timerComeUp >= 0 && secondEnum)
                {

                    timerComeUp += Time.deltaTime;

                    if (timerComeUp > timerWaitTime)
                    {
                        StartCoroutine(UpTwo());
                    }
                }
            }
        }
    }

    private IEnumerator MoveDownTwo()
    {
        while(this.transform.position.y >= -13 && !secondEnum)
        {
            this.transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, 0);
            yield return null;
        }

        if (this.transform.position.y <= -13)
        {
            downcheck = true;
        }
    }

    private IEnumerator FollowPlayer()
    {
        StopCoroutine(lastRoutine); //NOT WORKING
        secondEnum = true;
        if (!thirdEnum)
        {
            //Debug.Log(player.transform.position.y);
            this.transform.position = new Vector3(player.transform.position.x, -3.044103f, 0);
            yield return null;
            currentTime = timer;
        }  
    }

    private IEnumerator UpTwo()
    {
        if (GameObject.Find("Player") != null)
        {
            thirdEnum = true;
            state = "Up";
            while (transform.position.y <= 5.72)
            {
                this.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, 0);
                yield return null;
            }

            if (player.GetComponent<CharacterController2D>().dirRight)
            {
                this.transform.localScale = new Vector3(-1, 1, 3);
            }
            else
            {
                this.transform.localScale = new Vector3(1, 1, 3);
            }

            if (transform.position.y >= 5.72)
            {
                state = "idle";
                downcheck = false;
                timerWait = 0;
                timer = 0;
                secondEnum = false;
                thirdEnum = false;
                lastRoutine = null;
                timerComeUp = 0;
            }
        } else
        {
            state = "idle";
            downcheck = false;
            timerWait = 0;
            timer = 0;
            secondEnum = false;
            thirdEnum = false;
            lastRoutine = null;
            timerComeUp = 0;
        }
    }
}
