using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    List<SkillData> SkillDatas => UpgradeManager.Instance.skillUpgradeDatas;
    List<int> SkillHavingList => UpgradeManager.Instance.skillHavingList;
    [SerializeField] GameObject baitPrefab;



    [SerializeField] Image skill1Icon;
    [SerializeField] Image skill2Icon;
    [SerializeField] Image skill3Icon;

    [SerializeField] AudioClip skill1Sound;
    [SerializeField] AudioClip skill2Sound;
    [SerializeField] AudioClip skill3Sound;

    bool isSkill1Ready = false;
    bool isSkill2Ready = false;
    bool isSkill3Ready = false;

    public void SkillPurchased(int index)
    {
        switch(index)
        {
            case 0:
                isSkill1Ready = true;
                Destroy(skill1Icon.transform.GetChild(0).gameObject);
                break;
            case 1:
                isSkill2Ready = true;
                Destroy(skill2Icon.transform.GetChild(0).gameObject);
                break;
            case 2:
                isSkill3Ready = true;
                Destroy(skill3Icon.transform.GetChild(0).gameObject);
                break;
            default:
                break;
        }
    }

    public void UseSkill_bait()
    {
        if(isSkill1Ready == false) return;
        if(UpgradeManager.Instance.CheckSkillHaving(0) == false) return;
        SoundManager.Instance.PlaySound(skill1Sound);
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(mousePosition);

        GameObject baitOB = Instantiate(baitPrefab, mousePosition, Quaternion.identity);
        baitOB.transform.localScale = Vector3.one * 7f;
        baitOB.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack).OnComplete(
            () => baitOB.GetComponent<CircleCollider2D>().enabled = true
        );
        StartCoroutine(Skill1Coroutine());
        Destroy(baitOB, 5f);
    }

    public void UseSkill_confuse()
    {
        if(isSkill2Ready == false) return;
        if(UpgradeManager.Instance.CheckSkillHaving(1) == false) return;
        SoundManager.Instance.PlaySound(skill2Sound);
        GameObject[] students = GameObject.FindGameObjectsWithTag("Student");
        foreach(GameObject student in students)
        {
            student.GetComponent<Student>().Confuse();
        }
        StartCoroutine(Skill2Coroutine());
    }

    public void UseSkill_faster()
    {
        if(isSkill3Ready == false) return;
        if(UpgradeManager.Instance.CheckSkillHaving(2) == false) return;
        StudentSpawner.Instance.MakeFaster();
        SoundManager.Instance.PlaySound(skill3Sound);
        StartCoroutine(Skill3Coroutine());
    }

    IEnumerator Skill1Coroutine()
    {
        isSkill1Ready = false;
        for(float time = SkillDatas[0].cooldown; time > 0f; time -= Time.deltaTime)
        {
            skill1Icon.fillAmount = time / SkillDatas[0].cooldown;
            yield return null;
        }
        skill1Icon.fillAmount = 0f;
        isSkill1Ready = true;
    }
    IEnumerator Skill2Coroutine()
    {
        isSkill2Ready = false;
        for(float time = SkillDatas[1].cooldown; time > 0f; time -= Time.deltaTime)
        {
            skill2Icon.fillAmount = time / SkillDatas[1].cooldown;
            yield return null;
        }
        skill2Icon.fillAmount = 0f;
        isSkill2Ready = true;
    }

    IEnumerator Skill3Coroutine()
    {
        isSkill3Ready = false;
        for(float time = SkillDatas[2].cooldown; time > 0f; time -= Time.deltaTime)
        {
            skill3Icon.fillAmount = time / SkillDatas[2].cooldown;
            yield return null;
        }
        skill3Icon.fillAmount = 0f;
        isSkill3Ready = true;
    }
}
