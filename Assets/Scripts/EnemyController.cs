using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum FacingEnum { LEFT, RIGHT };
    public enum EnemyState { STANDING, WALKING, CHARGING }

    public float runSpeed;
    public float chargeSpeed;
    public float pauseTime;
    public float sightRadius;
    public List<float> xWaypoints = new List<float>();
    public GameObject enemyVision; // Inspector assigned reference to child
    public SpriteRenderer bangSprite;

    private bool isFacingLeft;
    private int currWaypointIdx = -1;
    private float pauseStartTime;

    private Rigidbody2D rBody;
    private SpriteRenderer sprite;
    private Animator animator;

    private EnemyState state = EnemyState.STANDING;
    public FacingEnum GetFacing()
    {
        if (isFacingLeft)
        {
            return FacingEnum.LEFT;
        }
        else
        {
            return FacingEnum.RIGHT;
        }
    }

    void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        ChooseNextWaypoint();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToDest = 0;
        float hSpeed = 0;
        Vector3 dest = Vector3.zero;
        switch (state)
        {
            case EnemyState.STANDING:
                // We're paused, just return unless we're past counter
                if (pauseStartTime + pauseTime >= Time.time)
                {
                    // Not past pause timer yet
                    return;
                }
                else
                {
                    bangSprite.enabled = false;
                    ChooseNextWaypoint();
                }
                break;
            case EnemyState.WALKING:
                hSpeed = runSpeed * Time.deltaTime;
                dest = new Vector2(xWaypoints[currWaypointIdx], transform.position.y);
                transform.position = Vector3.MoveTowards(transform.position, dest, hSpeed);
                distanceToDest = transform.position.x - dest.x;

                if (distanceToDest > 0)
                {
                    isFacingLeft = true;
                    sprite.flipX = true;
                    enemyVision.transform.localScale = new Vector3(-1, enemyVision.transform.localScale.y, enemyVision.transform.localScale.z);
                    animator.SetBool("isMoving", true);
                }
                else if (distanceToDest < 0)
                {
                    isFacingLeft = false;
                    sprite.flipX = false;
                    enemyVision.transform.localScale = new Vector3(1, enemyVision.transform.localScale.y, enemyVision.transform.localScale.z);
                    animator.SetBool("isMoving", true);
                }

                // Check if we need to pause at waypoint
                if (Mathf.Abs(distanceToDest) < 0.001f)
                {
                    pauseStartTime = Time.time;
                    animator.SetBool("isMoving", false);
                    state = EnemyState.STANDING;
                    bangSprite.enabled = false;
                    return;
                }
                break;
            case EnemyState.CHARGING:
                hSpeed = chargeSpeed * Time.deltaTime;
                dest = PlayerController.instance.transform.position;
                transform.position = Vector3.MoveTowards(transform.position, dest, hSpeed);
                distanceToDest = transform.position.x - dest.x;

                //TODO: Set Sprites to charging
                if (distanceToDest > 0)
                {
                    isFacingLeft = true;
                    sprite.flipX = true;
                    enemyVision.transform.localScale = new Vector3(-1, enemyVision.transform.localScale.y, enemyVision.transform.localScale.z);
                    animator.SetBool("isMoving", true);
                }
                else if (distanceToDest < 0)
                {
                    isFacingLeft = false;
                    sprite.flipX = false;
                    enemyVision.transform.localScale = new Vector3(1, enemyVision.transform.localScale.y, enemyVision.transform.localScale.z);
                    animator.SetBool("isMoving", true);
                }

                break;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag.Equals("Player"))
        {
            PlayerController.instance.TakeDamage();
        }
    }

    public void DetectedPlayer(bool detect)
    {
        if (detect && PlayerController.instance.GetCurrentPigment() != PigmentColor.NONE)
        {
            state = EnemyState.CHARGING;
            bangSprite.enabled = true;
        }

        if (!detect)
        {
            pauseStartTime = Time.time;
            state = EnemyState.STANDING;
            bangSprite.enabled = false;
        }
    }

    private void ChooseNextWaypoint()
    {
        if (xWaypoints.Count == 1)
            Debug.LogError("NOT ENOUGH WAYPOINTS");

        if (xWaypoints.Count == 0) // Allow for stationary enemies
            return;

        int nextWayPointIdx = currWaypointIdx;
        while (nextWayPointIdx == currWaypointIdx)
        {
            nextWayPointIdx = UnityEngine.Random.Range(0, xWaypoints.Count);
        }
        currWaypointIdx = nextWayPointIdx;
        state = EnemyState.WALKING;
    }
}
