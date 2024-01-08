using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine_Body : MonoBehaviour
{
    [SerializeField] public List<GameObject> steams = new List<GameObject>();

    [SerializeField] public List<SpriteRenderer> sparks = new List<SpriteRenderer>();

    [SerializeField] public List<Sprite> sparksSprites = new List<Sprite>();

    [SerializeField] private AudioSource steamSound;

    private int nowIndex = 0; //스파크 위치 저장할 변수
    //private bool playerNearby = false;
    private bool isSteamCoroutineRunning = false;

    void Start()
    {
        foreach (var steam in steams)
        {
            StartCoroutine(RandomSteam(steam));
        }
        StartCoroutine(SparkEngine());
        
    }
    private IEnumerator RandomSteam(GameObject steam)
    {
        if (!isSteamCoroutineRunning)
        {
            isSteamCoroutineRunning = true;

            while (true)
            {
                float timer = Random.Range(0.5f, 1.5f);
                while (timer >= 0f)
                {
                    yield return null;
                    timer -= Time.deltaTime;
                }
                steam.SetActive(true);
               // steamSound.Play(); // 스팀이 나오면서 소리 재생
                timer = 0f;
                while (timer <= 0.6f)
                {
                    yield return null;
                    timer += Time.deltaTime;
                }
                steam.SetActive(false);
               // steamSound.Stop(); // 스팀이 사라지면서 소리 중지

                isSteamCoroutineRunning = false;
            }
        }
   
    }

    private IEnumerator SparkEngine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.05f);
        while (true)
        {
            float timer = Random.Range(0.2f, 1.5f);
            while (timer >= 0f)
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
            if (nowIndex >= sparks.Count)
            {
                nowIndex = 0;
            }
        }
    }
/*    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = true;
            foreach (var steam in steams)
            {
                StartCoroutine(RandomSteam(steam));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = false;
            steamSound.Stop();
        }
    }*/

}
    