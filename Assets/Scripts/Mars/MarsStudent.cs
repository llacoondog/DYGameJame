using System.Collections;
using DG.Tweening;
using UnityEngine;

public class MarsStudent : MonoBehaviour
{
    
    Vector2 moveDirection;
    [SerializeField] float speed;
    Rigidbody2D rigid;
    CircleCollider2D circleCollider;
    
    public void OnCapture()
    {
        transform.DOKill();
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.enabled = false;
        transform.SetParent(null);
        gameObject.tag = "Alien";
        rigid = GetComponent<Rigidbody2D>();
        // rigid.bodyType = RigidbodyType2D.Kinematic;
        rigid.linearVelocity = Vector2.zero;
        transform.DORotate(new Vector3(0, 0, -90f), 0.2f, RotateMode.Fast).OnComplete(() => {
            transform.DOShakeRotation(0.15f, 300f, 15, 90f, false ).SetLoops(-1);
        });
        moveDirection = FindAnyObjectByType<Boss>().transform.position - transform.position;
        MoveToBoss(moveDirection);
    }


    public void MoveToBoss(Vector2 direction)
    {
        StopAllCoroutines();
        transform.DOKill();
        circleCollider.enabled = true;
        transform.DORotate(new Vector3(0, 0, 90f), 0.2f, RotateMode.Fast).OnComplete(()=>
        {
            transform.rotation = Quaternion.Euler(0, 0, 90f);
            transform.localScale = new Vector3(1f, 1f, 1f);
            transform.DOShakeScale(0.1f, Vector3.right * 0.2f, 20, 90f, false).SetRelative().SetLoops(-1);
            });
        rigid.linearVelocity = direction.normalized * 10f;
        Destroy(gameObject, 10f);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("KillZone"))
        {
            Destroy(gameObject);
        }
    }
}
