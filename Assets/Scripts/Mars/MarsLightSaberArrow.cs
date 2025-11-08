using System.Collections;
using DG.Tweening;
using UnityEngine;

public class MarsLightSaberArrow : MonoBehaviour
{
    int _captureCount;
    int _limit;
    float _charge;
    float _baseSpeed;
    AudioClip _hitSound;
    CircleCollider2D _circleCollider;

    void Start()
    {
        _circleCollider = GetComponent<CircleCollider2D>();
    }

    public void Initialize(int limit, float baseSpeed, float charge, AudioClip hitSound)
    {
        _limit = limit;
        _baseSpeed = baseSpeed;
        _charge = charge;
        _hitSound = hitSound;
        
        if (_circleCollider == null)
        {
            _circleCollider = GetComponent<CircleCollider2D>();
        }
        
        if (_circleCollider != null)
        {
            _circleCollider.enabled = true;
        }
        transform.localScale = new Vector3(1f, Random.value > 0.5f ? 1f : -1f, 1f) * (1+charge*2f);
        StartCoroutine(Cool());
    }

    IEnumerator Cool()
    {
        GetComponent<SpriteRenderer>().DOFade(0f, 0.2f);
        yield return new WaitForSeconds(0.2f);
        OnArrowEnd();
    }
    void OnArrowEnd()
    {
        SetSpeed(0f);
        transform.localPosition = Vector2.zero;

        for(int i = transform.childCount; i  > 0; i--)
        {
            // transform.GetChild(i-1).GetComponent<MarsStudent>().MoveToBoss();
            transform.GetChild(i-1).SetParent(null);
            // Destroy(transform.GetChild(i-1).gameObject);
        }
        Destroy(gameObject.transform.parent.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_captureCount >= _limit) return;

        if (other.gameObject.CompareTag("Student"))
        {
            other.GetComponent<MarsStudent>().OnCapture();
            // other.transform.SetParent(transform);
            // _captureCount++;
            SetSpeed(_charge);
            if (_hitSound != null)
            {
                SoundManager.Instance.PlaySound(_hitSound);
            }
        }
    }

    void SetSpeed(float charge)
    {
        // Arrow의 SetSpeed 로직과 동일
        // 실제로는 사용되지 않을 수 있지만 Arrow와 동일한 기능을 위해 포함
    }

}

