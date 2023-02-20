using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlay : MonoBehaviour
{
    [SerializeField] Slider gaugeBar;
    [SerializeField] float gaugeTime = 1f;
    bool interacting = false;
    bool returning = false;
    GameObject slider;
    Ingridient ingridient;
    string haveingridient;

    [Header("FoodImage")]
    [SerializeField] Sprite pike;
    [SerializeField] Sprite shrimp;
    [SerializeField] Sprite meat;
    [SerializeField] Sprite pork;
    [SerializeField] Sprite seafood;
    [SerializeField] Sprite pan;
    [SerializeField] Sprite grill;
    [SerializeField] Sprite chop;
    [SerializeField] Sprite bluecheck;

    [SerializeField] GameObject havenow;
    Image havenowImage;

    List<string> checkList = new List<string>() {"Pike", "Shrimp", "Meat", "Pork", "Seafood", "Pan", "Grill", "Chop"};
    
    List<int> orderList;
    List<Image> foodImageList;

    public List<int> haveList = new List<int>();

    public bool isComplete = false;
    public bool isDelivering = false;
    

    void Awake()
    {
        gaugeBar.maxValue = gaugeTime;
        slider = GameObject.Find("Slider");
        slider.SetActive(false);
        havenowImage = havenow.GetComponent<Image>();

        Recipe recipe = FindObjectOfType<Recipe>();
        orderList = recipe.orderList;
        foodImageList = recipe.foodImageList;
    }

    void Update()
    {
        if (interacting)
        {
            if (Input.GetButton("Jump"))
            {
                slider.SetActive(true);
                gaugeBar.value += Time.deltaTime;
                DisableMovement();
                if (gaugeBar.value == gaugeBar.maxValue)
                {
                    slider.SetActive(false);
                    EnableMovement();
                    haveingridient = ingridient.GetIngridient();

                    ImageSetting();
                }
            }

            else if (!Input.GetButton("Jump"))
            {
                EnableMovement();
                slider.SetActive(false);
                gaugeBar.value = 0;
            }
        }

        if (returning && (haveingridient == "Pike" || haveingridient == "Shrimp" || haveingridient == "Meat" || haveingridient == "Pork" || haveingridient == "Seafood"))
        {
            if (Input.GetButton("Jump"))
            {
                slider.SetActive(true);
                gaugeBar.value += Time.deltaTime;
                DisableMovement();
                if (gaugeBar.value == gaugeBar.maxValue)
                {
                    slider.SetActive(false);
                    EnableMovement();
                    int idx = checkList.FindIndex(a => a.Contains(haveingridient));
                    
                    Debug.Log(idx.ToString());
                    
                    if (idx == orderList[0])
                    {
                        Debug.Log("Good!");
                        orderList.RemoveAt(0);
                        BlueCheck();
                        ImageReset();
                    
                        haveingridient = "default";
                    }
                    else if (idx != orderList[0])
                    {
                        Debug.Log("Wrong");
                        ImageReset();
                        Stun(3f);
                        haveingridient = "default";
                    }
                }
            }
        }
        else if (returning && (haveingridient == "Pan" || haveingridient == "Grill" || haveingridient == "Chop"))
        {
            if (Input.GetButton("Jump"))
            {
                slider.SetActive(true);
                gaugeBar.value += Time.deltaTime;
                DisableMovement();
                if (gaugeBar.value == gaugeBar.maxValue)
                {
                    slider.SetActive(false);
                    EnableMovement();
                    int idx = checkList.FindIndex(a => a.Contains(haveingridient));
                    
                    if (idx == orderList[0])
                    {
                        Debug.Log("Good!");
                        orderList.RemoveAt(0);
                        BlueCheck();
                        ImageReset();
                        haveingridient = "default";
                    }

                    else
                    {
                        Debug.Log("Wrong");
                        ImageReset();
                        Stun(3f);
                        haveingridient = "default";
                    }
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Object")
        {
            interacting = true;
            // 초기화를 안해줘서 nullreferenceexeption 떴었음. 여기서 초기화해주는 것으로 해결.
            ingridient = other.GetComponent<Ingridient>();
        }
        else if (other.tag == "Plate")
        {
            returning = true;
            Debug.Log(returning);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Object")
        {
            interacting = false;
        }
        else if (other.tag == "Plate")
        {
            returning = false;
            Debug.Log(returning);
        }
    }

    void ImageSetting()
    {
        if (haveingridient == "Pike")
        {
            havenowImage.sprite = pike;
            havenow.SetActive(true);
        }
        else if (haveingridient == "Shrimp")
        {
            havenowImage.sprite = shrimp;
            havenow.SetActive(true);
        }
        else if (haveingridient == "Meat")
        {
            havenowImage.sprite = meat;
            havenow.SetActive(true);
        }
        else if (haveingridient == "Pork")
        {
            havenowImage.sprite = pork;
            havenow.SetActive(true);
        }
        else if (haveingridient == "Seafood")
        {
            havenowImage.sprite = seafood;
            havenow.SetActive(true);
        }
        else if (haveingridient == "Pan")
        {
            havenowImage.sprite = pan;
            havenow.SetActive(true);
        }
        else if (haveingridient == "Grill")
        {
            havenowImage.sprite = grill;
            havenow.SetActive(true);
        }
        else if (haveingridient == "Chop")
        {
            havenowImage.sprite = chop;
            havenow.SetActive(true);
        }
    }

    void ImageReset()
    {
        havenow.SetActive(false);
    }

    void EnableMovement()
    {
        gameObject.GetComponent<PlayerMove>().enabled = true;
    }

    void DisableMovement()
    {
        gameObject.GetComponent<PlayerMove>().enabled = false;
    }

    void Stun(float stunTime)
    {
        DisableMovement();
        Invoke("EnableMovement", stunTime);
    }

    void BlueCheck()
    {
        int bluecheckcount = 0;
        for (int i = 0; i < foodImageList.Count; i++)
        {
            if (foodImageList[i].sprite == bluecheck)
            {
                bluecheckcount += 1;
            }
        }
        foodImageList[0 + bluecheckcount].sprite = bluecheck;
        Debug.Log("bluecheck count:" + bluecheckcount.ToString());
        Debug.Log("ListLength:" + foodImageList.Count.ToString());

        if (bluecheckcount == foodImageList.Count - 1)
        {
            isComplete = true;
            Debug.Log("Complete!!");
        }
    }
}
