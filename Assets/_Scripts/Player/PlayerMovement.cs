using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveTime = 0.25f;

    bool isMoving;


    // Start is called before the first frame update
    void Start()
    {
        ValidatePosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
            return;


        if(Input.GetKey(KeyCode.W))
        {
            StartCoroutine(Move(MoveDirection.Up));
        }
        else if (Input.GetKey(KeyCode.S))
        {
            StartCoroutine (Move(MoveDirection.Down));
        }
        else if(Input.GetKey(KeyCode.D)) 
        {
            StartCoroutine(Move(MoveDirection.Right));
        }
        else if(Input.GetKey(KeyCode.A))
        {
            StartCoroutine(Move(MoveDirection.Left));
        }
    }

    void ValidatePosition()
    {
        Vector3 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        transform.position = position;
    }

    IEnumerator Move(MoveDirection direction)
    {
        isMoving = true;
        //float distance = 1;
        float speed = 1 / moveTime;
        Vector3 dir = GetVectorFromDirection(direction);


        for (float i = 0; i < moveTime; i += Time.deltaTime)
        {
            transform.position += dir * speed * Time.deltaTime;
            yield return null;
        }

        ValidatePosition();

        isMoving = false;
    }

    Vector3 GetVectorFromDirection(MoveDirection direction)
    {
        switch(direction)
        {
            case MoveDirection.Down:
                return Vector3.down;
            case MoveDirection.Up:
                return Vector3.up;
            case MoveDirection.Left:
                return Vector3.left;
            case MoveDirection.Right:
                return Vector3.right;
        }
        return Vector3.zero;
    }

}

public enum MoveDirection
{
    Down,
    Up,
    Left,
    Right
}
