﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : Singleton<PlayerController>
{
    public enum FacingEnum { LEFT, RIGHT };

    public RuntimeAnimatorController grayAnim;
    public RuntimeAnimatorController redAnim;
    public RuntimeAnimatorController blueAnim;
    public RuntimeAnimatorController yellowAnim;

    public float runSpeed;
    public float jumpForce;
    public int pigmentIndex;
    public float hitForce;

    private bool isFacingLeft;
    private bool canJump;
    private bool isHoldingPigment;
    private List<Pigment> colorsObtained = new List<Pigment>();
    private bool followCamOn; // For prototyping only
    private bool invincible;
    private bool inHitStun;
    private Material origMat;
    private Material flashMat;

    private Rigidbody2D body;
    private SpriteRenderer sprite;
    private Animator animator;

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

    protected override void Awake()
    {
        base.Awake();

        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        origMat = sprite.material;
        flashMat = Resources.Load<Material>("Materials/WhiteFlashMat");
    }

    private void Start()
    {
        ToggleCam(); // Set cam to follow
        GainPigment(GameObject.Find("ClearPigment").GetComponent<Pigment>());

    }

    // Update is called once per frame
    void Update()
    {

        // For prototyping only
        if (Input.GetKeyDown(KeyCode.T))
            ToggleCam();

        // For prototyping only
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene("Game");

        if (Input.GetKeyDown(KeyCode.J))
            GoToPreviousPigment();

        if (Input.GetKeyDown(KeyCode.K))
            UsePigment();

        if (Input.GetKeyDown(KeyCode.L))
            GoToNextPigment();

        if (Input.GetButtonDown("Jump") && canJump)
        {
            canJump = false;
            body.velocity = Vector2.zero;
            body.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            animator.SetBool("isJumping", true);
        }
    }

    void FixedUpdate()
    {
        // Player in hitstun
        if (inHitStun)
        {
            return;
        }

        float hSpeed = Input.GetAxisRaw("Horizontal") * runSpeed * Time.fixedDeltaTime;

        if (hSpeed > 0)
        {
            isFacingLeft = false;
            sprite.flipX = false;
            animator.SetBool("isMoving", true);
        }
        else if (hSpeed < 0)
        {
            isFacingLeft = true;
            sprite.flipX = true;
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
        transform.Translate(new Vector3(hSpeed, 0));
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        string tag = col.gameObject.tag;
        if (tag.Equals("Ground"))
        {
            ResetJump();
        }
        else if (tag.Equals("Tile"))
        {
            ColorTile tile = col.GetComponent<ColorTile>();
            tile.SwitchToColor();
            GameManager.instance.CompleteTile(tile);
        }
        else if (tag.Equals("Pigment"))
        {
            GainPigment(col.GetComponent<Pigment>().PickupColor());
        }
    }

    public PigmentColor GetCurrentPigment()
    {
        return colorsObtained[pigmentIndex].pigmentColor;
    }

    public void ResetJump()
    {
        animator.SetBool("isJumping", false);
        canJump = true;
    }

    public void TakeDamage()
    {
        if (invincible)
        {
            return;
        }

        // AudioManager.Instance.PlaySound("Damage", 0.5f);
        StartCoroutine(FlashWhite(.05f, 1f));
        Vector2 hitDir = Vector2.zero;
        if (isFacingLeft)
        {
            hitDir = new Vector2(hitForce, hitForce);
        }
        else
        {
            hitDir = new Vector2(-hitForce, hitForce);
        }
        body.velocity = Vector2.zero;
        body.AddForce(hitDir, ForceMode2D.Impulse);
        canJump = false; //TODO doublecheck this
        invincible = true;
        inHitStun = true;

        GameManager.instance.HitTaken();

        animator.SetBool("isJumping", true);
    }

    private IEnumerator FlashWhite(float flashSpeed, float duration)
    {
        float elapsedTime = 0;
        bool toggleFlash = false;
        while (elapsedTime < duration)
        {
            toggleFlash = !toggleFlash;
            if (toggleFlash)
            {
                sprite.material = flashMat;
            }
            else
            {
                sprite.material = origMat;
            }
            yield return new WaitForSeconds(flashSpeed);
            elapsedTime += flashSpeed;
            if (inHitStun && elapsedTime > duration / 2)
            {
                inHitStun = false;
            }
        }
        animator.SetBool("isJumping", false);
        sprite.material = origMat;
        invincible = false;
    }

    // For prototyping only
    private void ToggleCam()
    {
        if (followCamOn)
        {
            Camera.main.GetComponent<CameraFollow>().SetFollow(null);
        }
        else
        {
            Camera.main.GetComponent<CameraFollow>().SetFollow(gameObject);
        }
        followCamOn = !followCamOn;
    }

    private void GainPigment(Pigment pigmentColor)
    {
        colorsObtained.Add(pigmentColor);
        CycleMenu.instance.AddToMenu(pigmentColor);
        pigmentIndex = colorsObtained.Count - 1;

        if (colorsObtained.Count == 2)
        {
            //PigmentViewer.instance.ShowArrows();
        }
    }


    private void GoToNextPigment()
    {
        pigmentIndex++;
        if (pigmentIndex >= colorsObtained.Count)
            pigmentIndex = 0;

        if (colorsObtained.Count > 1)
        {
            CycleMenu.instance.SpinCW(pigmentIndex);
            //PigmentViewer.instance.ChangeColor(colorsObtained[pigmentIndex]);
        }
    }

    private void GoToPreviousPigment()
    {
        pigmentIndex--;
        if (pigmentIndex < 0)
            pigmentIndex = colorsObtained.Count - 1;

        if (colorsObtained.Count == 0)
        {
            pigmentIndex = 0;
        }
        else
        {
            if (colorsObtained.Count > 1)
                CycleMenu.instance.SpinCCW(pigmentIndex);
            //PigmentViewer.instance.ChangeColor(colorsObtained[pigmentIndex]);
        }
    }

    private void UsePigment()
    {
        if (colorsObtained.Count != 0)
        {
            ToggleManager.instance.TogglePigment(colorsObtained[pigmentIndex]);
        }
    }
}
