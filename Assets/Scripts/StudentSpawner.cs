using System.Collections;
using UnityEngine;

public class StudentSpawner : MonoBehaviour
{
    [SerializeField] GameObject studentPrefab;

    [SerializeField] float spawnInterval;

    private void Start()
    {
        StartCoroutine(SpawnStudent());
    }

    IEnumerator SpawnStudent()
    {
        float x = Random.Range(-5.5f, 10f);
        float y = (Random.value > 0.5f) ? -6f : 6f;
        float speed = Random.Range(3f, 7f);
        GameObject studentOB = Instantiate(studentPrefab, new Vector2(x, y), Quaternion.identity);

        Vector2 fianlPosition = new Vector2(x + Random.Range(-0.4f,0.4f), -y);

        Student student = studentOB.GetComponent<Student>();
        student.SetFinalPosition(fianlPosition, speed);

        yield return new WaitForSeconds(spawnInterval);
        StartCoroutine(SpawnStudent());
    }
}
