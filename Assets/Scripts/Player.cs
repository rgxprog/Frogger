using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //-------------------------------------------

    private enum FrogState
    {
        inGame,
        sink,
        crash
    }

    //-------------------------------------------

    public GameObject spriteStay, spriteJump;
    public AudioSource jumpSound, fallOnWaterSound, crashSound;

    private readonly float stepSize = 0.66f;
    private Vector3 startPosition;
    private FrogState state;
    private int crashDirection;
    private float crashAngleZ;
    private float spriteSizeX, spriteSizeY;
    private bool jumping;
    private float jumpTime;

    //-------------------------------------------

    private void Awake()
    {
        spriteSizeX = spriteStay.GetComponent<SpriteRenderer>().localBounds.size.x;
        spriteSizeY = spriteStay.GetComponent<SpriteRenderer>().localBounds.size.y;
        startPosition = transform.position;
        state = FrogState.inGame;
        crashDirection = 0;
        crashAngleZ = 0;
        jumping = false;
        jumpTime = 0;
    }

    //-------------------------------------------

    private void ResetPosition()
    {
        transform.position = new Vector3(startPosition.x, startPosition.y, startPosition.z);
        spriteStay.SetActive(true);
        spriteJump.SetActive(false);
        spriteStay.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    //-------------------------------------------

    private void Update()
    {
        if (GameManager.instance.gameState != GameManager.GameState.InGame)
            return;

        if (jumping)
        {
            jumpTime += Time.deltaTime;
            if (jumpTime > 0.1f)
            {
                jumping = false;
                spriteStay.SetActive(true);
                spriteJump.SetActive(false);
            }
        }

        if (state == FrogState.inGame && transform.position.y >= 0.7f && transform.position.y <= 4f && transform.parent == null)
        {
            if (!IsOnTransport())
            {
                ChangeState(FrogState.sink);
            }
        }

        CheckIfGoal();

        switch (state)
        {
            case FrogState.inGame:
                Move();
                CheckBoundaries();
                break;
            case FrogState.sink:
                SinkAnim();
                break;
            case FrogState.crash:
                CrashAnim();
                break;
            default:
                break;
        }
    }

    //-------------------------------------------

    private void Move()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MakeMove(0, transform.up);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MakeMove(-90f, transform.right);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MakeMove(180f, -transform.up);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MakeMove(90f, -transform.right);
        }
    }
    private void MakeMove(float zRotation, Vector3 transformLookAt)
    {
        spriteStay.transform.rotation = spriteJump.transform.rotation = Quaternion.Euler(0, 0, zRotation);
        transform.position += transformLookAt * stepSize;

        jumping = true;
        jumpTime = 0;
        spriteStay.SetActive(false);
        spriteJump.SetActive(true);

        jumpSound.Play();
    }

    //-------------------------------------------

    private void CheckBoundaries()
    {
        if (transform.position.x < -8f + spriteSizeX / 2)
            transform.position = new Vector3(-8f + spriteSizeX / 2, transform.position.y, transform.position.z);
        if (transform.position.x > 8f - spriteSizeX / 2)
            transform.position = new Vector3(8f - spriteSizeX / 2, transform.position.y, transform.position.z);

        if (transform.position.y < startPosition.y - 0.2f)
            transform.position = new Vector3(transform.position.x, startPosition.y, transform.position.z);

    }

    //-------------------------------------------

    private void ChangeState(FrogState newState)
    {
        switch (newState)
        {
            case FrogState.inGame:
                state = FrogState.inGame;
                gameObject.GetComponent<BoxCollider2D>().enabled = true;
                spriteStay.GetComponent<SpriteRenderer>().color = spriteJump.GetComponent<SpriteRenderer>().color  = new Color(1f, 1f, 1f);
                break;
            case FrogState.sink:
                state = FrogState.sink;
                fallOnWaterSound.Play();
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                transform.position = new Vector3(transform.position.x, transform.position.y, 0.5f);
                spriteStay.GetComponent<SpriteRenderer>().color = spriteJump.GetComponent<SpriteRenderer>().color = new Color(0.12f, 0.61f, 0.88f);
                break;
            case FrogState.crash:
                crashSound.Play();
                state = FrogState.crash;
                break;
            default:
                break;
        }
    }

    //-------------------------------------------

    private bool IsOnTransport()
    {
        if (transform.parent != null)
            return true;

        return GameManager.instance.IsOnTransport(transform);
    }

    //-------------------------------------------

    private void SinkAnim()
    {
        float ratio = 0.7f * Time.deltaTime;
        transform.localScale = new Vector3(transform.localScale.x - ratio, transform.localScale.y - ratio, transform.localScale.z);

        if (transform.localScale.x < 0.5f)
        {
            transform.localScale = Vector3.one;
            ResetPosition();
            GameManager.instance.LostLive();
            ChangeState(FrogState.inGame);
        }
    }

    //-------------------------------------------
    
    private void CrashAnim()
    {
        float speed = 3f * Time.deltaTime;
        transform.position = new Vector3(transform.position.x + speed * crashDirection, transform.position.y - speed, transform.position.z);
        crashAngleZ -= 500f * crashDirection * Time.deltaTime;
        spriteStay.transform.rotation = spriteJump.transform.rotation = Quaternion.Euler(0, 0, crashAngleZ);

        if (transform.position.x < -8.5f || transform.position.x > 8.5f || transform.position.y < -5.5f)
        {
            ResetPosition();
            GameManager.instance.LostLive();
            ChangeState(FrogState.inGame);
        }
    }

    //-------------------------------------------

    private void CheckIfGoal()
    {
        if (transform.position.y > 3.4f)
        {
            if (GameManager.instance.IsOnGoal(transform))
            {
                ResetPosition();
            }
            else
            {
                transform.position = new Vector3(transform.position.x, 3.35f, transform.position.z);
            }
        }
    }

    //-------------------------------------------

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Transport") )
        {
            transform.parent = collision.transform;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Car"))
        {
            crashDirection = collision.gameObject.GetComponent<HorizontalMove>().GetHorizontalDirection();
            ChangeState(FrogState.crash);
        }

    }

    //-------------------------------------------

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (transform.parent == collision.transform)
        {
            transform.parent = null;
        }
    }

    //-------------------------------------------
}
