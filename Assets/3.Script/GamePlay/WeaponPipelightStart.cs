using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPipelightStart : MonoBehaviour
{
    private WaitForSeconds wait = new WaitForSeconds(1f);

    private List<WeaponPipelight> lights = new List<WeaponPipelight>();
    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i).GetComponent<WeaponPipelight>();
            if (child)
            {
                lights.Add(child);
            }
        }
        StartCoroutine(TurnOnPipeLight());
    }
    private IEnumerator TurnOnPipeLight()
    {
        while (true)
        {
            yield return wait;

            foreach(var child in lights)
            {
                child.TurnOnLight();
            }
        }
    }
}
