using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;
using System.IO;

public class AI_General : MonoBehaviour
{
    public float speed;
    public float stoppingDistance;
    public float retreatDistance;
    public float alertRadius;

    private float timeBTWShots;
    public float startTimeBTWShots;

    public GameObject projectile;
    public Transform player;
    public Animator alertAnim;


    // Wander
    public float wanderSpeed;
    private float waitTime;
    public float startWaitTime;

    public Transform movePoint;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    private bool isWandering = true;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        alertAnim = GetComponent<Animator>();
        timeBTWShots = startTimeBTWShots;
        waitTime = startWaitTime;
        movePoint.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        
    }

    // Update is called once per frame
    void Update()
    {
        Wander();


        if (Vector2.Distance(transform.position, player.position) > stoppingDistance && Vector2.Distance(transform.position, player.position) < alertRadius)
        {
            isWandering = false;
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            alertAnim.SetBool("isAlerted", true);

        }
        else if (Vector2.Distance(transform.position, player.position) < stoppingDistance && Vector2.Distance(transform.position, player.position) > retreatDistance)
        {
            isWandering = false;
            transform.position = transform.position;
            shotCooldown();
            alertAnim.SetBool("isAlerted", true);
        }
        else if (Vector2.Distance(transform.position, player.position) < retreatDistance)
        {
            isWandering = false;
            transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
            shotCooldown();
            alertAnim.SetBool("isAlerted", true);
        }
        else
        {
            alertAnim.SetBool("isAlerted", false);
            isWandering = true;
        }



    }

    private void shotCooldown()
    {
        if (timeBTWShots <= 0)
        {
            Instantiate(projectile, transform.position, Quaternion.identity);
            timeBTWShots = startTimeBTWShots;
        }
        else
        {
            timeBTWShots -= Time.deltaTime;
        }
    }

    private void Wander()
    {
        if (isWandering == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, movePoint.position, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, movePoint.position) < 0.2f)
            {
                if (waitTime <= 0)
                {
                    movePoint.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
                    waitTime = startWaitTime;
                }
                else
                {
                    waitTime -= Time.deltaTime;
                }
            }
        }
    }
}
