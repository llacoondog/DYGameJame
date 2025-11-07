using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentSpawner : MonoBehaviour
{
    [SerializeField] GameObject studentPrefab;

    [SerializeField] float spawnInterval;

    [SerializeField] List<Sprite> hairs;
    [SerializeField] List<Sprite> accs;
    [SerializeField] List<Sprite> clothes;
    [SerializeField] List<Sprite> skins;

    private void Start()
    {
        StartCoroutine(SpawnStudent());
    }

    IEnumerator SpawnStudent()
    {
        float x = Random.Range(-3f, 10f);
        float y = (Random.value > 0.5f) ? -6f : 6f;
        float speed = Random.Range(3f, 7f);
        GameObject studentOB = Instantiate(studentPrefab, new Vector2(x, y), Quaternion.identity);
        studentOB.transform.Find("Hair").GetComponent<SpriteRenderer>().sprite = hairs[Random.Range(0, hairs.Count)];
        studentOB.transform.Find("Acc").GetComponent<SpriteRenderer>().sprite = accs[Random.Range(0, accs.Count)];
        studentOB.transform.Find("Clothes").GetComponent<SpriteRenderer>().sprite = clothes[Random.Range(0, clothes.Count)];
        studentOB.transform.Find("Skin").GetComponent<SpriteRenderer>().sprite = skins[Random.Range(0, skins.Count)];

        Vector2 fianlPosition = new Vector2(x + Random.Range(-0.4f,0.4f), -y);

        Student student = studentOB.GetComponent<Student>();
        student.SetFinalPosition(fianlPosition, speed);

        yield return new WaitForSeconds(spawnInterval);
        StartCoroutine(SpawnStudent());
    }
}
