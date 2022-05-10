using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleLogic : MonoBehaviour
{
    //-------------------------------------------

    private enum TurtleState
    {
        Swim,
        Sink
    }

    //-------------------------------------------

    public Sprite[] sprites;

    private TurtleState turtleState;
    private bool enableSink = false;
    private float animationTime = 1f;
    private float timeToSink = 4f;
    private float currentAnimationTime, currentSinkTime;
    private SpriteRenderer spriteRenderer;
    private int currentSprite;
    private BoxCollider2D boxCollider;

    //-------------------------------------------

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        currentAnimationTime = 0;
        currentSinkTime = 0;
        turtleState = TurtleState.Swim;
        currentSprite = 0;
    }

    //-------------------------------------------

    private void Update()
    {
        if (!enableSink)
            return;

        if (turtleState == TurtleState.Swim)
        {
            currentSinkTime += Time.deltaTime;

            if (currentSinkTime > timeToSink)
            {
                turtleState = TurtleState.Sink;
                currentSinkTime = 0;
            }
        }

        if (turtleState == TurtleState.Sink)
        {
            currentAnimationTime += Time.deltaTime;

            if (currentAnimationTime > animationTime)
            {
                currentAnimationTime = 0;
                currentSprite = currentSprite < sprites.Length - 1 ? currentSprite + 1 : 0;
                spriteRenderer.sprite = sprites[currentSprite];

                if (currentSprite == 0)
                {
                    turtleState = TurtleState.Swim;
                }

                if (spriteRenderer.sprite.name == "Turtle_5")
                {
                    boxCollider.enabled = false;
                }
                else
                {
                    boxCollider.enabled = true;
                }
            }
        }
    }


    //-------------------------------------------

    public void SetEnableSink()
    {
        enableSink = true;
    }

    //-------------------------------------------

    public bool IsTotalSink()
    {
        return spriteRenderer.sprite.name == "Turtle_5";
    }

    //-------------------------------------------
}
