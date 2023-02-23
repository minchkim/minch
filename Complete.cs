using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Complete : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI completeText;

    GamePlay gamePlay; 
    
    void Awake()
    {
        gamePlay = FindObjectOfType<GamePlay>();
    }

    void Update()
    {
        if (gamePlay.isComplete  && gamePlay.isDelivering)
        {
            completeText.text = "Delivering!";
        }
        else if (gamePlay.isComplete)
        {
            completeText.text = "Complete!";
        }
    }
}
