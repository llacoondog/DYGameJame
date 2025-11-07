using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Profecor : MonoBehaviour
{
    [SerializeField] float reach;
    [SerializeField] float speed;
    [SerializeField] int limit;

    [SerializeField] GameObject arrow;
    [SerializeField] LineRenderer line;

    [SerializeField] int score;
    
    Coroutine shootCoroutine;
    public Action onArrowEnd;

    [SerializeField] TextMeshProUGUI scoreText;

    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        line.SetPosition(0, transform.position);
        line.SetPosition(1, arrow.transform.position);

        if(shootCoroutine == null)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Vector3 direction = mousePosition - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            
            if(Input.GetMouseButtonDown(0))
            {
                shootCoroutine = StartCoroutine(Shoot());
            }

            transform.position += new Vector3(0, Input.GetAxis("Vertical") ,0) * Time.deltaTime;
        }
    }

    IEnumerator Shoot()
    {
        // 1초 동안 앞으로 간다
        float time = 0;
        for(time = 0f; time < 1f; time += Time.deltaTime)
        {
            arrow.transform.localPosition += new Vector3(speed,0,0) * Time.deltaTime;
            yield return null;
        }

        // 1초 동안 뒤로 간다
        for(time = 0f; time < 1f; time += Time.deltaTime)
        {
            arrow.transform.localPosition -= new Vector3(speed,0,0) * Time.deltaTime;
            yield return null;
        }
        onArrowEnd?.Invoke();
        shootCoroutine = null;
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
}
