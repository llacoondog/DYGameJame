using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Student : MonoBehaviour
{
    Vector2 moveDirection;
    [SerializeField] float speed;
    Rigidbody2D rigid;
    CircleCollider2D circleCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
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
        transform.DOKill();
        circleCollider.enabled = false;
        rigid.bodyType = RigidbodyType2D.Kinematic;
        rigid.linearVelocity = Vector2.zero;
        transform.DORotate(new Vector3(0, 0, -90f), 0.2f, RotateMode.Fast).OnComplete(() => {
            transform.DOShakeRotation(0.15f, 300f, 15, 90f, false ).SetLoops(-1);
        });
    }

    IEnumerator Move()
    {
        rigid.linearVelocity = moveDirection * speed;
        transform.DORotate(new Vector3(0, 0, Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 90f), 0.2f, RotateMode.Fast);
        yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        StartCoroutine(Move());
    }

    public void MoveToLab()
    {
        StopAllCoroutines();
        transform.DOKill();
        transform.DORotate(new Vector3(0, 0, 90f), 0.2f, RotateMode.Fast).OnComplete(()=>
        {
            transform.rotation = Quaternion.Euler(0, 0, 90f);
            transform.localScale = new Vector3(1f, 1f, 1f);
            transform.DOShakeScale(0.1f, Vector3.right * 0.2f, 20, 90f, false).SetRelative().SetLoops(-1);
            });
        rigid.linearVelocity = Vector2.left * 2f;
        Destroy(gameObject, 10f);
    }

    public void Confuse()
    {
        StartCoroutine(ConfuseCoroutine());
    }
    IEnumerator ConfuseCoroutine()
    {
        StopAllCoroutines();
        transform.DOKill();
        transform.DOShakePosition(2f, 0.1f, 15, 90f, false).SetRelative();
        transform.DOShakeRotation(2f, 300f, 15, 90f, false);
        yield return new WaitForSeconds(2f);
        transform.DOKill();
        StartCoroutine(Move());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Bait"))
        {
            StopAllCoroutines();
            transform.DOMove(other.transform.position, 2f);
            Vector3 direction = other.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.DORotate(new Vector3(0, 0, angle - 90f), 0.2f, RotateMode.Fast);
        }
        if(other.gameObject.CompareTag("KillZone"))
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(circleCollider.enabled == false) return;
        if(other.gameObject.CompareTag("Bait"))
        {
            StartCoroutine(Move());
        }
    }
    void OnDestroy()
    {
        transform.DOKill();
    }

}
