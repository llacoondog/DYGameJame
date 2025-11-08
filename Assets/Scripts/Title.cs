using UnityEngine;
using DG.Tweening;
public class Title : MonoBehaviour
{
    bool isClick = false;
    [SerializeField] GameObject up;
    [SerializeField] GameObject down;
    [SerializeField] GameObject text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // text.transform.DOShakePosition(1f, Vector2.up).SetLoops(-1);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            if(isClick) return;
            isClick = true;
            ClickAction();
        }
    }

    void ClickAction()
    {
        up.transform.DOMoveY(up.transform.position.y - 10f, 1.5f).SetEase(Ease.InOutBounce);
        down.transform.DOMoveY(down.transform.position.y + 10f, 1.5f).SetEase(Ease.InOutBounce)
        .OnComplete(() => {
            Destroy(gameObject);
        });
    }
}
