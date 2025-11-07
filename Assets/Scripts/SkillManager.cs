using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    List<SkillData> SkillDatas => UpgradeManager.Instance.skillUpgradeDatas;
    List<int> SkillHavingList => UpgradeManager.Instance.skillHavingList;
    [SerializeField] GameObject baitPrefab;

    
    public void UseSkill_bait()
    {
        if(UpgradeManager.Instance.CheckSkillHaving(0) == false) return;
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(mousePosition);

        GameObject baitOB = Instantiate(baitPrefab, mousePosition, Quaternion.identity);
        baitOB.transform.localScale = Vector3.one * 7f;
        baitOB.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack).OnComplete(
            () => baitOB.GetComponent<CircleCollider2D>().enabled = true
        );

        Destroy(baitOB, 5f);
    }


}
