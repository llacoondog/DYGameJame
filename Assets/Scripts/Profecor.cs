using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Profecor : MonoBehaviour
{
    [SerializeField] GameObject arrowOB;
    [SerializeField] Arrow arrow;
    Rigidbody2D rigid;

    [SerializeField] GameObject fakeArrowOB;
    [SerializeField] LineRenderer fakeLine;
    SkillManager skillManager;
    SpriteRenderer spriteRenderer;


    public int score;
    
    bool isShooting = false;
    public Action onArrowEnd;
    bool isInLab = false;

    [SerializeField] TextMeshProUGUI scoreText;

    float charge;
    float chargePower = 1f;
    Camera mainCamera;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        skillManager = GetComponent<SkillManager>();
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
    }


    // Update is called once per frame
    void Update()
    {
        if(isShooting) rigid.linearVelocity = Vector2.zero;

        if(!isShooting)
        {
            ArrowDirectionAction();
            MouseInputAction();
            KeyboardInputAction();
        }
    }

    void ArrowDirectionAction()
    {
        if(isInLab) return;
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
        if(isInLab) return;
        if(Input.GetMouseButton(0))
        {
            charge = Mathf.Min(charge + Time.deltaTime * chargePower, 1f);
        }
        if(Input.GetMouseButtonUp(0))
        {
            arrow.Shoot(charge);
            rigid.linearVelocity = Vector2.zero;
            charge = 0f;
        }
    }

    void KeyboardInputAction()
    {
        rigid.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * 3f, Input.GetAxis("Vertical") * 3f);

        if(isInLab) return; 
        if(Input.GetKeyDown(KeyCode.Q))
        {
            skillManager.UseSkill_bait();
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            skillManager.UseSkill_confuse();
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            skillManager.UseSkill_faster();
        }
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

    public void AddScore(int score)
    {
        this.score += score;
        UpdateScoreText();
    }

    public void UseScore(int score)
    {
        this.score -= score;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = "대학원생수 : " + score.ToString();
    }

    void SetInLab(bool isInLab)
    {
        this.isInLab = isInLab;
        arrowOB.SetActive(!isInLab);
        fakeArrowOB.SetActive(!isInLab);
        fakeLine.gameObject.SetActive(!isInLab);

    }

    public void EquipWeapon(WeaponData weaponData)
    {
        chargePower = weaponData.charge;
        arrow.EquipWeapon(weaponData);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(isShooting) return;
        if(other.gameObject.CompareTag("Lab"))
        {
            SetInLab(true);
            mainCamera.transform.DOMoveX(-18f, 0.5f);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(Application.isPlaying == false) return;
        if(other.gameObject.CompareTag("Lab"))
        {
            SetInLab(false);
            mainCamera.transform.DOMoveX(0f, 0.5f);
        }
    }
}
