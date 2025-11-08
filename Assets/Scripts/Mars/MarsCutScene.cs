using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MarsCutScene : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text1;
    [SerializeField] TextMeshProUGUI text2;
    [SerializeField] TextMeshProUGUI text3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(CutSceneCoroutine());
    }

    IEnumerator CutSceneCoroutine()
    {
        text1.gameObject.SetActive(true);
        text1.color = new Color(text1.color.r, text1.color.g, text1.color.b, 0f);
        text1.DOFade(1f, 1f);
        yield return new WaitForSeconds(3f);
        text1.DOFade(0f, 1f).OnComplete(() => {
            text1.gameObject.SetActive(false);
        });
        yield return new WaitForSeconds(0.5f);
        text2.gameObject.SetActive(true);
        text2.color = new Color(text2.color.r, text2.color.g, text2.color.b, 0f);
        text2.DOFade(1f, 1f);
        yield return new WaitForSeconds(3f);
        text2.DOFade(0f, 1f).OnComplete(() => {
            text2.gameObject.SetActive(false);
        });
        yield return new WaitForSeconds(0.5f);
        text3.gameObject.SetActive(true);
        text3.color = new Color(text3.color.r, text3.color.g, text3.color.b, 0f);
        text3.DOFade(1f, 1f);
        yield return new WaitForSeconds(3f);
        text3.DOFade(0f, 1f).OnComplete(() => {
            text3.gameObject.SetActive(false);
        });
        yield return new WaitForSeconds(0.5f);
    }
}
