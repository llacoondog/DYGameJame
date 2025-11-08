using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    int hp = 100;
    [SerializeField] GameObject babyAlienPrefab;
    [SerializeField] GameObject dangerAlienPrefab;
    [SerializeField] GameObject rollingPrefab;
    [SerializeField] Image hpBar;
    void Start()
    {
        StartCoroutine(PlayRandomAttack());
    }
    IEnumerator PlayRandomAttack()
    {
        int rand = Random.Range(0,4 + 1);
        switch(rand)
        {
            case 0:
                yield return StartCoroutine(SimplyThrowThirdByThird());
                break;
            case 1:
                yield return StartCoroutine(WaveShoot());
                break;
            case 2:
                yield return StartCoroutine(PanningShoot());
                break;
            case 3:
                yield return StartCoroutine(RollingShoot());
                break;
            case 4:
                yield return StartCoroutine(RainShoot());
                break;
            default:
                break;
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(PlayRandomAttack());
    }

    IEnumerator SimplyThrowThirdByThird()
    {
        List<Vector2> poses = new List<Vector2>(){ new Vector2(6f,2f), new Vector2(6f,0f), new Vector2(6f,-2f)};
        
        for(int i = 0; i < 3; i++)
        {
            int rand = Random.Range(0,poses.Count);
            transform.DOMove(poses[rand],0.5f);
            yield return new WaitForSeconds(1f);
            poses.RemoveAt(rand);

            for(int j = 0; j < 3 ; j++)
            {
                Shoot(transform.position,Vector2.left*10f);
                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    IEnumerator WaveShoot()
    {
        float randY = Random.value > 0.5f ? 5f : -5f;
        transform.DOMoveY(randY,1f);
        yield return new WaitForSeconds(1f);
        
        // randY에서 -randY로 2초간 이동하며 0.25초 간격으로 발사
        float targetY = -randY;
        float duration = 2f;
        float shootInterval = 0.25f;
        int shootCount = Mathf.FloorToInt(duration / shootInterval);
        
        transform.DOMoveY(targetY, duration);
        
        int rand = Random.Range(0,shootCount);
        for(int i = 0; i < shootCount; i++)
        {
            if(i == rand)
                ShootParriable(transform.position, Vector2.left * 10f);
            else
                Shoot(transform.position, Vector2.left * 10f);
            yield return new WaitForSeconds(shootInterval);
        }
        
        yield return new WaitForSeconds(0.5f);
        
        transform.DOMoveY(randY, duration);
        rand = Random.Range(0,shootCount);
        for(int i = 0; i < shootCount; i++)
        {
            if(i == rand)
                ShootParriable(transform.position, Vector2.left * 10f);
            else
                Shoot(transform.position, Vector2.left * 10f);
            yield return new WaitForSeconds(shootInterval);
        }

        // StartCoroutine(WaveShoot());
    }

    IEnumerator PanningShoot()
    {
        // -1~1 사이의 Y좌표로 이동
        float randY = Random.Range(-1f, 1f);
        transform.DOMoveY(randY, 1f);
        yield return new WaitForSeconds(1f);
        
        // 4공격: 왼쪽을 기준으로 -30, -6, 6, 30도 각도로 발사
        float[] angles4 = new float[] { -30f, -10f, 10f, 30f };
        foreach(float angle in angles4)
        {
            Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.left;
            Shoot(transform.position, direction * 10f);
            yield return new WaitForSeconds(0.02f);
        }
        
        yield return new WaitForSeconds(0.5f);
        
        // 3공격: 왼쪽을 기준으로 -20, 0, 20도 각도로 발사
        float[] angles3 = new float[] { -20f, 0f, 20f };
        foreach(float angle in angles3)
        {
            Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.left;
            Shoot(transform.position, direction * 10f);
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(0.5f);
        
        // 4공격: 왼쪽을 기준으로 -30, -6, 6, 30도 각도로 발사
        foreach(float angle in angles4)
        {
            Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.left;
            Shoot(transform.position, direction * 10f);
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(0.5f);


    }

    IEnumerator RollingShoot()
    {
        transform.DOMoveY(0f,1f);
        yield return new WaitForSeconds(1f);
        for(int i = 0; i < 3; i++)
        {
            GameObject rolling = Instantiate(rollingPrefab, transform.position, Quaternion.identity);
            rolling.transform.eulerAngles = new Vector3(0, 0, Random.Range(0f, 360f));
            rolling.transform.DOMoveX(rolling.transform.position.x - 30f, 5f).SetEase(Ease.Linear);
            rolling.transform.DOLocalRotate(new Vector3(0, 0, i % 2 == 0 ? 360f : -360f), 2.5f, RotateMode.Fast).SetRelative().SetEase(Ease.Linear).SetLoops(-1);
            yield return new WaitForSeconds(3f);
            Destroy(rolling,10f);
        }
        // StartCoroutine(RollingShoot());

    }
    IEnumerator RainShoot()
    {
        transform.DOMoveY(4f,1f);
        yield return new WaitForSeconds(1f);
        transform.DOShakePosition(0.5f, Vector3.up * 0.1f, 15, 90f, false).SetRelative();
        yield return new WaitForSeconds(0.5f);
        for(int i = 0; i < 40; i++)
        {
            Shoot(new Vector2(Random.Range(-10f, 10f), 7f), Vector2.down * 10f + Vector2.right * Random.Range(-1, 0f));
            yield return new WaitForSeconds(0.12f);
            // Destroy(alien,10f);
        }
        // StartCoroutine(RainShoot());
        transform.DOMoveY(0f,1f);
    }   

    GameObject Shoot(Vector2 pos, Vector2 vel)
    {
        GameObject shoot;
        if(Random.value <0.2f)
            shoot = Instantiate(babyAlienPrefab,pos, Quaternion.identity);
        else
            shoot = Instantiate(dangerAlienPrefab,pos,Quaternion.identity);

        shoot.GetComponent<Rigidbody2D>().linearVelocity = vel;
        return shoot;

    }
    GameObject ShootParriable(Vector2 pos, Vector2 vel)
    {
        GameObject shoot = Instantiate(babyAlienPrefab,pos, Quaternion.identity);
        shoot.GetComponent<Rigidbody2D>().linearVelocity = vel;
        return shoot;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Alien"))
        {
            hp -= 5;
            Destroy(other.gameObject);
            hpBar.DOFillAmount((float)hp / 100f, 0.5f);
            if(hp <= 0)
            {
                StopAllCoroutines();
                // transform.DOMoveY(-10f, 1f);
                SceneManager.LoadScene("End");
                Destroy(gameObject);
            }
        }
    }
    
}