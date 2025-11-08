using System.Collections;
using UnityEngine;

public class ChacramArrow : MonoBehaviour
{
    [SerializeField] WeaponData _weaponData;
    [SerializeField] float _baseReach;
    [SerializeField] float _baseSpeed;
    [SerializeField] float _speed;
    [SerializeField] int _limit;
    [SerializeField] AudioClip _hitSound;

    float _reach;
    float _charge;
    int _captureCount;
    CircleCollider2D _circleCollider;

    void Start()
    {
        _circleCollider = GetComponent<CircleCollider2D>();
        SetSpeed(0f);
    }

    public void Initialize(WeaponData weaponData, float baseReach, float baseSpeed, int limit, AudioClip hitSound)
    {
        _weaponData = weaponData;
        _baseReach = baseReach;
        _baseSpeed = baseSpeed;
        _limit = limit;
        _hitSound = hitSound;
        
        if (_weaponData != null)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = _weaponData.icon;
            }
            transform.localScale = new Vector3(_weaponData.size, _weaponData.size, 0f);
        }
    }

    public void Shoot(float charge)
    {
        _circleCollider = GetComponent<CircleCollider2D>();
        StartCoroutine(ShootCoroutine(charge));
    }

    IEnumerator ShootCoroutine(float charge)
    {
        _charge = charge;
        _circleCollider.enabled = true;
        _reach = _baseReach * charge + 0.5f;

        // 앞으로 이동
        float distance = 0f;
        while (distance < _reach && _captureCount < _limit)
        {
            transform.localPosition += new Vector3(_baseSpeed + _baseSpeed * charge, 0, 0) * Time.deltaTime;
            distance = transform.localPosition.x;
            yield return null;
        }

        // 뒤로 이동
        while (distance > 0f)
        {
            transform.localPosition -= new Vector3(_speed, 0, 0) * Time.deltaTime;
            distance = transform.localPosition.x;
            yield return null;
        }

        OnArrowEnd();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_captureCount >= _limit) return;

        if (other.gameObject.CompareTag("Student"))
        {
            other.GetComponent<Student>().OnCapture();
            other.transform.SetParent(transform);
            _captureCount++;
            SetSpeed(_charge);
            if (_hitSound != null)
            {
                SoundManager.Instance.PlaySound(_hitSound);
            }
        }
    }

    void SetSpeed(float charge)
    {
        _speed = Mathf.Max(_baseSpeed - _captureCount + (_baseSpeed * charge), 2f);
    }

    void OnArrowEnd()
    {
        SetSpeed(0f);
        transform.localPosition = Vector2.zero;

        Profecor profecor = transform.GetComponentInParent<Profecor>(true);
        if(profecor != null)
        {
            profecor.AddScore(_captureCount);
        }
        // 포획한 Student들을 해제
        for (int i = transform.childCount; i > 0; i--)
        {
            transform.GetChild(i - 1).GetComponent<Student>().MoveToLab();
            transform.GetChild(i - 1).SetParent(null);
        }

        _circleCollider.enabled = false;
        Destroy(transform.parent.gameObject);
    }
}

