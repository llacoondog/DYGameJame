using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    [SerializeField] float baseReach;
    [SerializeField] float reach;
    [SerializeField] float baseSpeed;
    [SerializeField] float speed;
    [SerializeField] int limit;
    
    [SerializeField] AudioClip hitSound;

    Profecor profecor;
    int captureCount;
    public int CaptureCount => captureCount;
    public float BaseReach => baseReach;
    float charge;
    CircleCollider2D circleCollider;

    void Start()
    {
        profecor = transform.parent.parent.GetComponent<Profecor>();
        profecor.onArrowEnd += OnArrowEnd;
        circleCollider = GetComponent<CircleCollider2D>();
        SetSpeed(0f);
    }

    public void Shoot(float charge)
    {
        StartCoroutine(ShootCoroutine(charge));
    }
    IEnumerator ShootCoroutine(float charge)
    {
        this.charge = charge;
        circleCollider.enabled = true;
        profecor.SetShooting(true);
        reach = baseReach * charge+0.5f;
        // 앞으로 간다
        float distance = 0;
        for(distance = 0f; distance < reach; )
        {
            transform.localPosition += new Vector3(baseSpeed + baseSpeed * charge,0,0) * Time.deltaTime;
            distance = transform.localPosition.x;
            yield return null;
        }

        // 뒤로 간다
        for(distance = reach; distance > 0f; )
        {
            transform.localPosition -= new Vector3(speed,0,0) * Time.deltaTime;
            distance = transform.localPosition.x;
            yield return null;
        }
        OnArrowEnd();
        profecor.SetShooting(false);
        circleCollider.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(captureCount >= limit) return;
        
        if(other.gameObject.CompareTag("Student"))
        {
            other.GetComponent<Student>().OnCapture();
            other.transform.SetParent(transform);
            captureCount++;
            SetSpeed(this.charge);
            SoundManager.Instance.PlaySound(hitSound);
        }
    }
    void SetSpeed(float charge)
    {
        this.speed = Mathf.Max(baseSpeed - (captureCount) + (baseSpeed * charge), 1f);
    }

    void OnArrowEnd()
    {
        profecor.AddScore(captureCount);
        captureCount = 0;
        SetSpeed(0f);
        transform.localPosition = Vector2.zero;

        for(int i = transform.childCount; i  > 0; i--)
        {
            transform.GetChild(i-1).GetComponent<Student>().MoveToLab();
            transform.GetChild(i-1).SetParent(null);
            // Destroy(transform.GetChild(i-1).gameObject);
        }
    }

    public void EquipWeapon(WeaponData weaponData)
    {
        baseSpeed = weaponData.velocity;
        limit = weaponData.limit;
        transform.localScale = new Vector3(weaponData.size, weaponData.size, 0f);
    }


}
