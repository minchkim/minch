using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Recipe : MonoBehaviour
{
    [Header("raw food")]
    [SerializeField] List<Sprite> rawFoodList = new List<Sprite>();

    [Header("cuisine")]
    [SerializeField] List<Sprite> cuisineList = new List<Sprite>();

    [Header("foodtomake")]
    [SerializeField] List<Image> foodToMakeList = new List<Image>();
    [SerializeField] List<Image> cuisineToMakeList = new List<Image>();

    [Header("Customer")]
    [SerializeField] List<Sprite> customerList = new List<Sprite>();
    [SerializeField] Image currentCustomer;

    [SerializeField] GameObject[] foodMaker;
    Image foodImage;

    float shortTime = 60f;
    float longTime = 120f;
    float settedTime;
    float remainTime;
    [SerializeField] TextMeshProUGUI minuteTimer;
    [SerializeField] TextMeshProUGUI secondTimer;

    [SerializeField] TextMeshProUGUI timeLimit;
    float totalTime = 300f;
    float remainMinute;
    float remainSecond;

    [SerializeField] TextMeshProUGUI getPoint;
    [SerializeField] TextMeshProUGUI totalPoint;
    int newPoint;

    List<int> rawfood = new List<int>(){0, 1, 2, 3, 4};
    List<int> maker = new List<int>(){0, 1, 2};
    
    List<int> newrawfood = new List<int>();
    List<int> newmaker = new List<int>();

    List<int> rawfoodOrderList = new List<int>();
    List<int> cuisineOrderList = new List<int>();
    
    public List<int> orderList = new List<int>();
    public List<Image> foodImageList = new List<Image>();

    int difficulty;
    int point;

    public int customerno;
    public bool hasTimeOuted;

    GamePlay gamePlay;

    void Awake()
    {
        gamePlay = FindObjectOfType<GamePlay>();
        CustomerReset();
        DifficultySetting();
        Debug.Log(string.Join(",", orderList));
        totalPoint.text = newPoint.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        TimerSetting();
        if (remainTime < 0)
        {
            CustomerReset();
            DifficultySetting();
            TimerSetting();
            Debug.Log(string.Join(",", orderList));
            hasTimeOuted = true;
        }

        if (gamePlay.hasServed)
        {
            newPoint += point;
            totalPoint.text = newPoint.ToString();
            CustomerReset();
            DifficultySetting();
            TimerSetting();
            gamePlay.hasServed = false;
        }

        if (timeLimit != null)
        {
            totalTime -= Time.deltaTime;
            remainMinute = Mathf.FloorToInt(totalTime / 60f);
            remainSecond = Mathf.FloorToInt(totalTime % 60f);
            timeLimit.text = remainMinute + " : " + remainSecond;
        
            if (totalTime <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }   
        }
    }

    public void DifficultySetting()
    {
        newrawfood.Clear();
        newmaker.Clear();

        rawfoodOrderList.Clear();
        cuisineOrderList.Clear();

        orderList.Clear();

        foodImageList.Clear();

        difficulty = Random.Range(1, 5);
        int easyrawfoodno = Random.Range(2, 4);
        int easymakerno = Random.Range(1, 3);
        int hardrawfoodno = 4;
        int hardmakerno = Random.Range(2, 4);
        
        
        
        Image foodImage;
        
        List<Image> foodImageRawFoodList = new List<Image>();
        List<Image> foodImageMakerList = new List<Image>();

        if (difficulty == 1)
        {
            settedTime = longTime;
            remainTime = settedTime;
            newrawfood = GetRandomNumbers(rawfood, easyrawfoodno);
            newmaker = GetRandomNumbers(maker, easymakerno);
            point = 3000;
        }
        else if (difficulty == 2)
        {
            settedTime = shortTime;
            remainTime = settedTime;
            newrawfood = GetRandomNumbers(rawfood, easyrawfoodno);
            newmaker = GetRandomNumbers(maker, easymakerno);
            point = 4500;
        }
        else if (difficulty == 3)
        {
            settedTime = longTime;
            remainTime = settedTime;
            newrawfood = GetRandomNumbers(rawfood, hardrawfoodno);
            newmaker = GetRandomNumbers(maker, hardmakerno);
            point = 6000;
        }
        else
        {
            settedTime = shortTime;
            remainTime = settedTime;
            newrawfood = GetRandomNumbers(rawfood, hardrawfoodno);
            newmaker = GetRandomNumbers(maker, hardmakerno);
            point = 10000;
        }

        getPoint.text = point.ToString();


        for (int i = 0; i < newrawfood.Count; i++)
        {
            int randno = Random.Range(0, rawFoodList.Count);
            foodImage = foodToMakeList[i].GetComponent<Image>();
            foodImage.sprite = rawFoodList[randno];
            foodImageRawFoodList.Add(foodImage);
            rawfoodOrderList.Add(randno);
        }

        if (newrawfood.Count != 4)
        {
            for (int i = 0; i < 4 - newrawfood.Count; i++)
            {
                foodImage = foodToMakeList[newrawfood.Count + i].GetComponent<Image>();
                foodImage.sprite = cuisineList[cuisineList.Count - 1];
            }
        }

        for (int i = 0; i < newmaker.Count; i++)
        {
            int randno = Random.Range(0, cuisineList.Count - 1);
            foodImage = cuisineToMakeList[i].GetComponent<Image>();
            foodImage.sprite = cuisineList[randno];
            foodImageMakerList.Add(foodImage);
            cuisineOrderList.Add(randno);
        }

        if (newmaker.Count != 4)
        {
            for (int i = 0; i < 4 - newmaker.Count; i++)
            {
                foodImage = cuisineToMakeList[newmaker.Count + i].GetComponent<Image>();
                foodImage.sprite = cuisineList[cuisineList.Count - 1];
            }
        }

        for (int i = 0; i < rawfoodOrderList.Count; i++)
        {
            orderList.Add(rawfoodOrderList[i]);
            foodImageList.Add(foodImageRawFoodList[i]);
            if (cuisineOrderList.Count > 0)
            {
                orderList.Add(cuisineOrderList[0] + 5);
                cuisineOrderList.RemoveAt(0);
                foodImageList.Add(foodImageMakerList[i]);
            }
        }
        
        currentCustomer.sprite = customerList[customerno];
    }

    void TimerSetting()
    {
        remainTime -= Time.deltaTime;
        minuteTimer.text = Mathf.FloorToInt(remainTime / 60f).ToString();
        secondTimer.text = Mathf.FloorToInt(remainTime % 60f).ToString();
    }

    void CustomerReset()
    {
        customerno = Random.Range(0, 5);
    }

    List<int> GetRandomNumbers(List<int> numbers, int count)
    {
        List<int> result = new List<int>();

        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, numbers.Count);
            int selectedNumber = numbers[index];
        
            while (result.Contains(selectedNumber))
            {
                index = Random.Range(0, numbers.Count);
                selectedNumber = numbers[index];
            }
            result.Add(selectedNumber);
        }
    return result;
    }
}
