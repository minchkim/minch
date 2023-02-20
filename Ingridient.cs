using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ingridient : MonoBehaviour
{
    [SerializeField] Image ingridientImage;
    [SerializeField] string ingridientName;
    public SpriteRenderer spriteRenderer;
    GamePlay gamePlay;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gamePlay = FindObjectOfType<GamePlay>();
    }

    void Update()
    {
        if (gamePlay.isComplete)
        {
            spriteRenderer.enabled = true;
        }
    }

    public string GetIngridient()
    {
        return ingridientName;
    }
}
