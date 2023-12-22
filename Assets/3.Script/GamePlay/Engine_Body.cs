using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine_Body : MonoBehaviour
{
    [SerializeField] public List<GameObject> steams = new List<GameObject>();

    [SerializeField] public List<SpriteRenderer> sparks = new List<SpriteRenderer>();

    [SerializeField] public List<Sprite> sparksSprites = new List<Sprite>();

    private int nowIndex = 0; //����ũ ��ġ ������ ����


    void Start()
    {
        foreach(var steam in steams)
        {
            StartCoroutine(RandomSteam(steam));
        }
        StartCoroutine(SparkEngine());
    }
    private IEnumerator RandomSteam(GameObject steam)
    {
        while (true)
        {
            float timer = Random.Range(0.5f, 1.5f);
            while (timer>=0f)
            {
                yield return null;
                timer -= Time.deltaTime;
            }
            steam.SetActive(true);
            timer = 0f;
            while (timer <= 0.6f)
            {
                yield return null;
                timer += Time.deltaTime;
            }
            steam.SetActive(false);
        }
    }
    private IEnumerator SparkEngine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.05f);
        while (true)
        {
            float timer = Random.Range(0.2f, 1.5f);
            while (timer>=0f)
            {
                yield return null;
                timer -= Time.deltaTime;
            }
            int[] indices = new int[Random.Range(2, 7)];
            for (int i = 0; i < indices.Length; i++)
            {
                indices[i] = Random.Range(0, sparksSprites.Count);
            }
            for (int i = 0; i < indices.Length; i++)
            {
                yield return wait;
                sparks[nowIndex].sprite = sparksSprites[indices[i]];
            }
            sparks[nowIndex++].sprite = null;
            if (nowIndex>=sparks.Count)
            {
                nowIndex = 0;
            }
        }
    }
   
}
