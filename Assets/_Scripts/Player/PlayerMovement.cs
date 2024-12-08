using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveTime = 0.25f;

    bool isMoving;
    MoveDirection direction;
    bool moveInput;

    public AudioClip walkingSFX;
    public AudioSource audioSource;

    Animator animator;
    Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponentInChildren<AudioSource>();
        ValidatePosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
            return;

        bool moveThisFrame = true;

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
        else
        {
            moveThisFrame = false;
        }

        if(moveThisFrame != moveInput)
        {
            if(moveThisFrame)
                audioSource.Play();
            else 
                audioSource.Stop();
        }
        moveInput = moveThisFrame;
    }

    private void LateUpdate()
    {
        if(direction == MoveDirection.Down)
        {
            animator.Play(moveInput ? "Walk Front" : "Idle Front");
        }
        if(direction == MoveDirection.Up)
        {
            animator.Play(moveInput ? "Walk Back" : "Idle Back");
        }
        if (direction == MoveDirection.Left)
        {
            animator.Play(moveInput ? "Walk Side" : "Idle_Side");
            animator.transform.localScale = new Vector3(-1f, 1f, 1f);

        }
        if(direction == MoveDirection.Right)
        {
            animator.Play(moveInput ? "Walk Side" : "Idle_Side");
            animator.transform.localScale = new Vector3(1f, 1f, 1f);
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
        this.direction = direction;
        float speed = 1 / moveTime;
        Vector3 dir = GetVectorFromDirection(direction);


        for (float i = 0; i < moveTime; i += Time.fixedDeltaTime)
        {
            //transform.position += dir * speed * Time.deltaTime;
            yield return new WaitForFixedUpdate();

            rb.MovePosition(rb.transform.position + dir * speed * Time.fixedDeltaTime);
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
