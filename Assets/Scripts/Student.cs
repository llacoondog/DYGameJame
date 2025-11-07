using UnityEngine;

public class Student : MonoBehaviour
{
    Vector2 moveDirection;
    [SerializeField] float speed;
    Rigidbody2D rigid;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    public void SetFinalPosition(Vector2 finalPosition, float speed)
    {
        if(rigid == null)
        {
            rigid = GetComponent<Rigidbody2D>();
        }

        this.speed = speed;
        Vector2 direction = finalPosition - (Vector2)transform.position;
        moveDirection = direction.normalized;
        rigid.linearVelocity = moveDirection * speed;
    }

    public void OnCapture()
    {
        rigid.linearVelocity = Vector2.zero;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("KillZone"))
        {
            Destroy(gameObject);
        }
    }

}
