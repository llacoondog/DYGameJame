using TMPro;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    [SerializeField] Arrow arrow;
    [SerializeField] Profecor profecor;

    [SerializeField] TextMeshProUGUI StatText;

    void Update()
    {
        if(!profecor.IsInLab) return;
        StatText.text = $"{arrow.BaseSpeed}\n{arrow.Limit}\n{arrow.WeaponData.charge}\n{arrow.transform.localScale.x:F1}\n\n{(1/StudentSpawner.Instance.SpawnInterval).ToString("F1")}/s";
    }
}
