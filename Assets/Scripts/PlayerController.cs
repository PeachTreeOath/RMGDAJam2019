using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public enum FacingEnum { LEFT, RIGHT };

    public float runSpeed;
    public float jumpForce;

    private bool isFacingLeft;
    private bool canJump;
    private List<Pigment> colorsObtained = new List<Pigment>();
    private int pigmentIndex;
    private bool followCamOn; // For prototyping only

    private Rigidbody2D rBody;
    private SpriteRenderer sprite;

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

        rBody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        ToggleCam(); // Set cam to follow
    }

    // Update is called once per frame
    void Update()
    {
        // For prototyping only
        if (Input.GetKeyDown(KeyCode.T))
            ToggleCam();

        if (Input.GetKeyDown(KeyCode.J))
            GoToPreviousPigment();

        if (Input.GetKeyDown(KeyCode.K))
            UsePigment();

        if (Input.GetKeyDown(KeyCode.L))
            GoToNextPigment();

        float hSpeed = Input.GetAxisRaw("Horizontal") * runSpeed * Time.deltaTime;

        if (hSpeed > 0)
        {
            isFacingLeft = false;
            sprite.flipX = false;
        }
        else if (hSpeed < 0)
        {
            isFacingLeft = true;
            sprite.flipX = true;
        }
        else
        {
            // Any logic needed for neutral
        }
        transform.Translate(new Vector3(hSpeed, 0));

        if (Input.GetButtonDown("Jump") && canJump)
        {
            canJump = false;
            rBody.velocity = Vector2.zero;
            rBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag.Equals("Ground"))
        {
            ResetJump();
        }

        if (col.gameObject.tag.Equals("Pigment"))
        {
            GainPigment(col.GetComponent<Pigment>().PickupColor());
        }
    }


    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag.Equals("Pigment"))
        {

        }
    }

    public void ResetJump()
    {
        canJump = true;
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
        if (colorsObtained.Count == 1)
        {
            pigmentIndex = 0;
            PigmentViewer.instance.ChangeColor(colorsObtained[pigmentIndex]);
        }
        if (colorsObtained.Count == 2)
        {
            PigmentViewer.instance.ShowArrows();
        }
    }


    private void GoToNextPigment()
    {
        pigmentIndex++;
        if (pigmentIndex >= colorsObtained.Count)
            pigmentIndex = 0;

        if (colorsObtained.Count != 0)
        {
            PigmentViewer.instance.ChangeColor(colorsObtained[pigmentIndex]);
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
            PigmentViewer.instance.ChangeColor(colorsObtained[pigmentIndex]);
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
