using System.Collections;
using UnityEngine;

public class MarsArrow : MonoBehaviour
{
    [SerializeField] WeaponData weaponData;
    public WeaponData WeaponData => weaponData;

    [SerializeField] float baseReach;
    [SerializeField] float reach;
    [SerializeField] float baseSpeed;
    [SerializeField] float speed;
    [SerializeField] int limit;

    [SerializeField] SpriteRenderer lineSprite;
     SpriteRenderer spriteRenderer;
    [SerializeField] AudioClip hitSound;

    MarsPlayer profecor;
    int captureCount;
    public int CaptureCount => captureCount;
    public float BaseReach => baseReach;
    public float Charge => charge;
    public float BaseSpeed => baseSpeed;
    public int Limit => limit;
    public float Speed => speed;
    float charge;
    CircleCollider2D circleCollider;
    [SerializeField] GameObject lightsaberPrefab;

    void Start()
    {
        profecor = transform.parent.parent.GetComponent<MarsPlayer>();
        // profecor.onArrowEnd += OnArrowEnd;
        circleCollider = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        if(weaponData?.shootSound != null)
        {
            SoundManager.Instance.PlaySound(weaponData.shootSound);
        }
        reach = baseReach * charge+0.5f;
        
        if(weaponData.name == "광선검" && lightsaberPrefab != null)
        {
            GameObject lightsaber = Instantiate(lightsaberPrefab, transform.position, transform.rotation, transform.parent);
            MarsLightSaberArrow lightSaberArrowComponent = lightsaber.GetComponentInChildren<MarsLightSaberArrow>();
            if(lightSaberArrowComponent != null)
            {
                lightSaberArrowComponent.Initialize(limit, baseSpeed, charge, hitSound);
            }
            yield return new WaitForSeconds(0.9f);
            // OnArrowEnd();
            profecor.SetShooting(false);
            circleCollider.enabled = false;
            yield break;
        }
        // // 앞으로 간다
        // float distance = 0;
        // for(distance = 0f; distance < reach && captureCount < limit; )
        // {
        //     transform.localPosition += new Vector3(baseSpeed + baseSpeed * charge,0,0) * Time.deltaTime;
        //     if(weaponData.isChaining)
        //         lineSprite.size = new Vector2(transform.localPosition.x, 1f);
        //     distance = transform.localPosition.x;
        //     yield return null;
        // }

        // // 뒤로 간다
        // for(distance = reach; distance > 0f; )
        // {
        //     transform.localPosition -= new Vector3(speed,0,0) * Time.deltaTime;
        //     if(weaponData.isChaining)
        //         lineSprite.size = new Vector2(transform.localPosition.x, 1f);
        //     distance = transform.localPosition.x;
        //     yield return null;
        // }
        // if(weaponData.isChaining)
        //     lineSprite.size = new Vector2(0f, 1f);
        // // OnArrowEnd();
        // profecor.SetShooting(false);
        // circleCollider.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(captureCount >= limit) return;
        
        if(other.gameObject.CompareTag("Student"))
        {
            other.GetComponent<MarsStudent>().OnCapture();
            other.transform.SetParent(transform);
            captureCount++;
            SetSpeed(this.charge);
            SoundManager.Instance.PlaySound(hitSound);
        }
    }
    void SetSpeed(float charge)
    {
        this.speed = Mathf.Max(baseSpeed - (captureCount) + (baseSpeed * charge), 2f);
    }

    // void OnArrowEnd()
    // {
    //     // profecor.AddScore(captureCount);
    //     captureCount = 0;
    //     SetSpeed(0f);
    //     transform.localPosition = Vector2.zero;

    //     for(int i = transform.childCount; i  > 0; i--)
    //     {
    //         transform.GetChild(i-1).GetComponent<MarsStudent>().MoveToBoss(direction);
    //         transform.GetChild(i-1).SetParent(null);
    //         // Destroy(transform.GetChild(i-1).gameObject);
    //     }
    // }

    public void EquipWeapon(WeaponData weaponData)
    {
        this.weaponData = weaponData;
        baseSpeed = weaponData.velocity;
        limit = weaponData.limit;
        if(spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        spriteRenderer.sprite = weaponData.icon;
        lineSprite.sprite = weaponData.chainSprite;
        transform.localScale = new Vector3(weaponData.size, weaponData.size, 0f);
    }


}
