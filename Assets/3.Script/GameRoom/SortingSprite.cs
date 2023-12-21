using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ESortingType
{
    Static,Update
}

public class SortingSprite : MonoBehaviour
{

    [SerializeField] private ESortingType sortingType;

    private SpriteSorter sorter;
    private SpriteRenderer spriterenderer;

    private void Start()
    {
        sorter = FindObjectOfType<SpriteSorter>();
        spriterenderer = GetComponent<SpriteRenderer>();

        spriterenderer.sortingOrder = sorter.GetSortingOrder(gameObject);
    }
    private void Update()
    {
        if (sortingType == ESortingType.Update)
        {
            spriterenderer.sortingOrder = sorter.GetSortingOrder(gameObject);
        }
    }

}
