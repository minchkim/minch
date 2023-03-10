using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    GameObject endScreen;
    GameObject mainScreen;
    Recipe recipe;

    [SerializeField] TextMeshProUGUI finalScoreText;

    void Awake()
    {
        recipe = FindObjectOfType<Recipe>();
        endScreen = GameObject.Find("EndScreen");
        endScreen.SetActive(false);
        mainScreen = transform.Find("MainCanvas").gameObject;
    }

    void Update()
    {
        if (recipe.gameOver)
        {
            mainScreen.SetActive(false);
            endScreen.SetActive(true);
            ShowFinalScore();
        }
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ShowFinalScore()
    {
        finalScoreText.text = "Your Final Score is" + recipe.newPoint + " !";
    }
}
