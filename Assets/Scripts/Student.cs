using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Student : MonoBehaviour
{
    Vector2 moveDirection;
    [SerializeField] float speed;
    Rigidbody2D rigid;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 60f);
    }

    public void SetFinalPosition(Vector2 finalPosition, float speed)
    {
        if(rigid == null)
        {
            rigid = GetComponent<Rigidbody2D>();
            rigid.mass = Random.Range(1f, 2f);
        }

        this.speed = speed;
        Vector2 direction = finalPosition - (Vector2)transform.position;
        moveDirection = direction.normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        
        StartCoroutine(Move());
    }

    public void OnCapture()
    {
        rigid.bodyType = RigidbodyType2D.Kinematic;
        rigid.linearVelocity = Vector2.zero;
        transform.DORotate(new Vector3(0, 0, -90f), 0.2f, RotateMode.Fast);
    }

    IEnumerator Move()
    {
        rigid.linearVelocity = moveDirection * speed;
        yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        StartCoroutine(Move());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("KillZone"))
        {
            Destroy(gameObject);
        }
    }

}
