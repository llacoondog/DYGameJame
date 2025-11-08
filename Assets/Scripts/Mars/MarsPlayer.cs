using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MarsPlayer : MonoBehaviour
{
    [SerializeField] GameObject arrowOB;
    [SerializeField] MarsArrow arrow;
    Rigidbody2D rigid;

    [SerializeField] GameObject fakeArrowOB;
    [SerializeField] LineRenderer fakeLine;
    SpriteRenderer spriteRenderer;


    public int hp = 100;
    
    bool isShooting = false;
    public System.Action onArrowEnd;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshPro upTextPrefab;
    [SerializeField] WeaponData firstWeapon;
    [SerializeField] AudioClip hitSound;

    float charge;
    float chargePower = 1f;
    Camera mainCamera;

    [SerializeField] Image hpBar;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        EquipWeapon(firstWeapon);
    }


    // Update is called once per frame
    void Update()
    {
        if(isShooting) rigid.linearVelocity = Vector2.zero;

        if(!isShooting)
        {
            ArrowDirectionAction();
            MouseInputAction();
        }
            KeyboardInputAction();
    }

    void ArrowDirectionAction()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = (Vector2)(Camera.main.ScreenToWorldPoint(mousePosition) - fakeArrowOB.transform.parent.localPosition);
        Vector3 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        fakeArrowOB.transform.localPosition = new Vector3(arrow.BaseReach * charge + 0.5f,0f,0f);
        fakeLine.SetPosition(0, fakeArrowOB.transform.parent.position);
        fakeLine.SetPosition(1, fakeArrowOB.transform.position);
    }
    void MouseInputAction()
    {   
        // if(Input.GetMouseButton(0))
        // {
        //     charge = Mathf.Min(charge + Time.deltaTime * chargePower, 1f);
        // }
        // if(Input.GetMouseButtonUp(0))
        // {
        //     arrow.Shoot(charge);
        //     rigid.linearVelocity = Vector2.zero;
        //     charge = 0f;
        // }

        if(Input.GetMouseButtonDown(0))
        {
            arrow.Shoot(0f);
            rigid.linearVelocity = Vector2.zero;
            charge = 0f;
        }
    }

    void KeyboardInputAction()
    {
        rigid.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * 6f, Input.GetAxis("Vertical") * 6f);

    }

    public void SetShooting(bool isShooting)
    {
        this.isShooting = isShooting;
        fakeArrowOB.SetActive(!isShooting);
        fakeLine.gameObject.SetActive(!isShooting);
        if(isShooting)
        {
            spriteRenderer.transform.DORotate(new Vector3(0, 0, 40f), 0.1f, RotateMode.Fast).SetRelative();
        }
        else
        {
            spriteRenderer.transform.DORotate(new Vector3(0, 0, -40f), 0.1f, RotateMode.Fast).SetRelative()
            .OnComplete(() => {
                spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, 0);
            });
        }

    }

    // public void AddScore(int score)
    // {
    //     if(score == 0) return;
    //     this.score += score;
    //     TextMeshPro upText = Instantiate(upTextPrefab.gameObject, upTextPrefab.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0f), Quaternion.identity).GetComponent<TextMeshPro>();
    //     upText.text = "+" + score.ToString();
    //     upText.transform.DOMoveY(upText.transform.position.y + 0.2f, 0.5f);
    //     upText.transform.DOScale(0.5f, 0.5f).SetEase(Ease.OutBack).OnComplete(() => {
    //         Destroy(upText.gameObject);
    //     });
    //     UpdateScoreText();
    // }

    // public void UseScore(int score)
    // {
    //     this.score -= score;
    //     UpdateScoreText();
    // }

    // void UpdateScoreText()
    // {
    //     scoreText.text = "대학원생수 : " + score.ToString();
    // }

    public void EquipWeapon(WeaponData weaponData)
    {
        chargePower = weaponData.charge;
        arrow.EquipWeapon(weaponData);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Student" || other.tag=="Danger")
        {
            hp -= 5;
            SoundManager.Instance.PlaySound(hitSound);
            Destroy(other.gameObject);
            hpBar.DOFillAmount((float)hp / 100f, 0.5f);
            if(hp <= 0)
            {
                SceneManager.LoadScene("Mars");
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
    }
}
