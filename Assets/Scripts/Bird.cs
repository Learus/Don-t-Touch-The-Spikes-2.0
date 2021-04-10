using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public Vector3 startingPosition = new Vector3(0, .6f, -0.1f);
    public float speed = 4f;
    public float jumpForce = 8f;
    public Commander.Direction Direction = Commander.Direction.Right;
    public bool start = false;

    Collider2D cl;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        cl = this.GetComponent<Collider2D>();

        rb.velocity = Vector3.zero;
        rb.bodyType = RigidbodyType2D.Static;
    }

    // Update is called once per frame
    void Update()
    {
        if (start) this.transform.Translate(Time.deltaTime * speed, 0, 0);

        if (start && Input.GetKeyDown(KeyCode.Mouse0))
        {
            Jump();
        }
    }

    public void Reset() {
        start = false;
        this.transform.position = startingPosition;

        rb.velocity = Vector3.zero;
        rb.bodyType = RigidbodyType2D.Static;
    }

    public void Jump()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        

        Vector2 velocity = rb.velocity;
        velocity.y = jumpForce;
        rb.velocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Wall")
        {
            this.transform.Rotate(0, 180, 0);
            if (Direction == Commander.Direction.Right) Direction = Commander.Direction.Left;
            else Direction = Commander.Direction.Right;

            Commander.Instance.GenerateSpikes(Direction);
        }
        else if (other.gameObject.tag == "Spike")
        {
            Commander.Instance.Lose();
        }
    }
}
