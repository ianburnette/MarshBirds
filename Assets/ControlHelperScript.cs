using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControlHelperScript : MonoBehaviour {

    public PlayerMove moveScript;
    public PlayerCrouch crouchScript;
    public PlayerSlide slideScript;
    public InWaterScript waterScript;
    public Throwing throwScript;
    public PlayerLadder ladderScript;

    public Rigidbody rb;
    public Color[] activeColors;
    public Image[] allImages;
    public Text[] allTexts;
    public Image jumpImage, grabImage, throwImage, storeImage, runImage, crouchImage, pauseImage, selectImage;
    public Text crossText, circleText, squareText, triangleText, r1Text, l1Text, startText, selectText;
    public Sprite crossSprite, circleSprite, squareSprite, triangleSprite, r1Sprite, l1Sprite, psStartSprite, psSelectSprite;
    public Sprite Xsprite, Asprite, Bsprite, Ysprite, rbSprite, lbSprite, xbStartSprite, xbSelectSprite;
    public Sprite kbJump, kbGrab, kbThrow, kbRun, kbStore, kbCrouch, kbStart, kbSelect;
    //jump is X, grab is circle, throw is square, store is triangle, run is r1

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        UpdateCrossButton();
        UpdateSquareButton();
        UpdateCircleButton();
        UpdateTriangleButton();
        FadeButtonsIfDisabled();
        DebugControls();
	}

    void DebugControls()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetController(0);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SetController(1);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            SetController(2);
    }

    void UpdateCrossButton()
    {
        if (waterScript.inWater)
        {
            if (waterScript.distToSurface < waterScript.surfaceDistMargin)
                crossText.text = "JUMP";
            else
                crossText.text = "";
        }
        else
        {
            if (moveScript.enabled && moveScript.grounded)
                crossText.text = "JUMP";
            else if (moveScript.enabled && !moveScript.grounded)
            {
           
                if (rb.velocity.x != 0 || rb.velocity.z != 0)
                    crossText.text = "SLIDE";
                else
                    crossText.text = "";
            }
            else if (slideScript.sliding && slideScript.canGetUp)
                crossText.text = "JUMP";
            else
                crossText.text = "";
        }
    }

    void UpdateCircleButton()
    {
        if (throwScript.canGrab && throwScript.heldObj == null)
        {
            circleText.text = "GRAB";
        }else if (throwScript.heldObj != null)
        {
            circleText.text = "DROP";
        }else if (ladderScript.inZone && !ladderScript.onLadder)
        {
            circleText.text = "GET ON";
        }else if (ladderScript.onLadder)
        {
            circleText.text = "LET GO";
        }
        else
        {
            circleText.text = "";
        }
    }

    void UpdateTriangleButton()
    {

    }

    void UpdateSquareButton()
    {
        if (throwScript.enabled)
        {
            if (throwScript.heldObj != null)
                squareText.text = "THROW";
            else
                squareText.text = "";
        }
        else
        {
            squareText.text = "";
        }
    }

    void FadeButtonsIfDisabled()
    {
       for (int i = 0; i<allImages.Length; i++)
        {
            if (allTexts[i].text == "")
                allImages[i].color = activeColors[1];
            else
                allImages[i].color = activeColors[0];
        }
    }

    void SetController(int controllerStyle)
    {
        //0 is PS, 1 is xb, 2 is kb
        if (controllerStyle == 0) 
        {
            jumpImage.sprite = crossSprite;
            grabImage.sprite = circleSprite;
            throwImage.sprite = squareSprite;
            storeImage.sprite = triangleSprite;
            runImage.sprite = r1Sprite;
            pauseImage.sprite = psStartSprite;
            selectImage.sprite = psSelectSprite;
        }else if (controllerStyle == 1)
        {
            jumpImage.sprite = Asprite;
            grabImage.sprite = Bsprite;
            throwImage.sprite = Xsprite;
            storeImage.sprite = Ysprite;
            runImage.sprite = rbSprite;
            pauseImage.sprite = xbStartSprite;
            selectImage.sprite = xbSelectSprite;
        }else if (controllerStyle == 2)
        {
            jumpImage.sprite = kbJump;
            grabImage.sprite = kbGrab;
            throwImage.sprite = kbThrow;
            storeImage.sprite = kbStore;
            runImage.sprite = kbRun;
            pauseImage.sprite = kbStart;
            selectImage.sprite = kbSelect;
        }
    }
}
