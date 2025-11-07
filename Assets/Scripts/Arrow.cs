using UnityEngine;

public class Arrow : MonoBehaviour
{
    Profecor profecor;
    int captureCount;

    void Start()
    {
        profecor = transform.parent.GetComponent<Profecor>();
        profecor.onArrowEnd += OnArrowEnd;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Student"))
        {
            other.GetComponent<Student>().OnCapture();
            other.transform.SetParent(transform);
            captureCount++;
        }
    }

    void OnArrowEnd()
    {
        profecor.AddScore(captureCount);
        captureCount = 0;
        
        for(int i = transform.childCount; i  > 0; i--)
        {
            Destroy(transform.GetChild(i-1).gameObject);
        }
    }


}
