using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Profecor : MonoBehaviour
{
    [SerializeField] GameObject arrowOB;
    Arrow arrow;
    Rigidbody2D rigid;
    [SerializeField] LineRenderer line;

    [SerializeField] GameObject fakeArrowOB;
    [SerializeField] LineRenderer fakeLine;

    [SerializeField] int score;
    
    bool isShooting = false;
    public Action onArrowEnd;
    bool isInLab = false;

    [SerializeField] TextMeshProUGUI scoreText;

    float charge;
    Camera mainCamera;

    void Start()
    {
        arrow = arrowOB.GetComponent<Arrow>();
        rigid = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }


    // Update is called once per frame
    void Update()
    {
        line.SetPosition(0, transform.position);
        line.SetPosition(1, arrowOB.transform.position);

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
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        fakeArrowOB.transform.localPosition = new Vector3(arrow.BaseReach + arrow.BaseReach * charge ,0f,0f);
        fakeLine.SetPosition(0, transform.position);
        fakeLine.SetPosition(1, fakeArrowOB.transform.position);
    }
    void MouseInputAction()
    {   
        if(isInLab) return;
        if(Input.GetMouseButton(0))
        {
            charge = Mathf.Min(charge + Time.deltaTime, 1f);
        }
        if(Input.GetMouseButtonUp(0))
        {
            arrow.Shoot(charge);
            charge = 0f;
        }
    }

    void KeyboardInputAction()
    {
        rigid.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * 3f, Input.GetAxis("Vertical") * 3f);
    }

    public void SetShooting(bool isShooting)
    {
        this.isShooting = isShooting;
        fakeArrowOB.SetActive(!isShooting);
        fakeLine.gameObject.SetActive(!isShooting);
    }

    public void AddScore(int score)
    {
        this.score += score;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = "학생수 : " + score.ToString();
    }

    void SetInLab(bool isInLab)
    {
        this.isInLab = isInLab;
        arrowOB.SetActive(!isInLab);
        line.gameObject.SetActive(!isInLab);
        fakeArrowOB.SetActive(isInLab);
        fakeLine.gameObject.SetActive(isInLab);
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
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
