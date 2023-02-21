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
    Recipe recipe;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gamePlay = FindObjectOfType<GamePlay>();
        recipe = FindObjectOfType<Recipe>();
    }

    void Update()
    {
        if (recipe.hasTimeOuted)
        {
            spriteRenderer.enabled = false;
        }
        else if (gamePlay.isComplete && gamePlay.isDelivering)
        {
            spriteRenderer.enabled = false;
        }
        else if (gamePlay.isComplete)
        {
            spriteRenderer.enabled = true;
        }
    }

    public string GetIngridient()
    {
        return ingridientName;
    }
}
