using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMove : MonoBehaviour
{
    //-------------------------------------------

    private int horizontalDirection;
    private float speed;
    private float movementOnX;

    //-------------------------------------------

    private void Awake()
    {
        transform.position = Vector3.zero;
        speed = 0;
        horizontalDirection = 0;
        movementOnX = 0;
    }

    //-------------------------------------------

    public void Init(Vector3 initialPosition, int horizontalDirection, float speed)
    {
        transform.position = initialPosition;
        this.horizontalDirection = horizontalDirection;
        this.speed = speed;
    }

    //-------------------------------------------

    private void Update()
    {
        movementOnX = horizontalDirection * speed * Time.deltaTime;
        transform.position += new Vector3(movementOnX, 0, 0);

        if (horizontalDirection == -1 && transform.position.x < -8f - gameObject.GetComponent<SpriteRenderer>().localBounds.size.x)
        {
            transform.position = new Vector3(8 + gameObject.GetComponent<SpriteRenderer>().localBounds.size.x, transform.position.y, transform.position.z);
        }
        else if (horizontalDirection == 1 && transform.position.x > 8f + gameObject.GetComponent<SpriteRenderer>().localBounds.size.x)
        {
            transform.position = new Vector3(-8 - gameObject.GetComponent<SpriteRenderer>().localBounds.size.x, transform.position.y, transform.position.z);
        }
    }

    //-------------------------------------------

    public int GetHorizontalDirection()
    {
        return horizontalDirection;
    }

    //-------------------------------------------
}
