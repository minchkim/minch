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
    bool serving = false;
    GameObject slider;
    Ingridient ingridient;
    string haveingridient;
    float blinkTime = 0.3f;

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
    [SerializeField] Sprite domeCover;

    [SerializeField] GameObject havenow;
    Image havenowImage;

    List<string> checkList = new List<string>() {"Pike", "Shrimp", "Meat", "Pork", "Seafood", "Pan", "Grill", "Chop"};
    
    List<int> orderList;
    List<Image> foodImageList;

    public List<int> haveList = new List<int>();

    public bool isComplete = false;
    public bool isDelivering = false;

    Complete complete;
    Table table;
    Recipe recipe;

    string tableName;

    public bool hasServed = false;

    SpriteRenderer spriter;
    

    void Awake()
    {
        gaugeBar.maxValue = gaugeTime;
        slider = GameObject.Find("Slider");
        slider.SetActive(false);
        havenowImage = havenow.GetComponent<Image>();

        recipe = FindObjectOfType<Recipe>();
        orderList = recipe.orderList;
        foodImageList = recipe.foodImageList;

        spriter = GetComponent<SpriteRenderer>();

        complete = FindObjectOfType<Complete>();        
        complete.gameObject.SetActive(false);
    }

    void Update()
    {
        if (recipe.hasTimeOuted)
        {
            Debug.Log("Timeout");
            TimeOut();
            recipe.hasTimeOuted = false;
        }
        
        if (interacting && !isDelivering)
        {
            if (Input.GetButton("Jump"))
            {
                slider.SetActive(true);
                gaugeBar.value += Time.deltaTime;
                DisableMovement();
                if (gaugeBar.value == gaugeBar.maxValue)
                {
                    gaugeBar.value = 0;
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

        if (returning && (haveingridient == "Pike" || haveingridient == "Shrimp" || haveingridient == "Meat" || haveingridient == "Pork" || haveingridient == "Seafood") && !isDelivering)
        {
            if (Input.GetButton("Jump"))
            {
                slider.SetActive(true);
                gaugeBar.value += Time.deltaTime;
                DisableMovement();
                if (gaugeBar.value == gaugeBar.maxValue)
                {
                    gaugeBar.value = 0;
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
            else if (!Input.GetButton("Jump"))
            {
                slider.SetActive(false);
                gaugeBar.value = 0;
                EnableMovement();
            }
        }
        else if (returning && (haveingridient == "Pan" || haveingridient == "Grill" || haveingridient == "Chop") && !isDelivering)
        {
            if (Input.GetButton("Jump"))
            {
                slider.SetActive(true);
                gaugeBar.value += Time.deltaTime;
                DisableMovement();
                if (gaugeBar.value == gaugeBar.maxValue)
                {
                    gaugeBar.value = 0;
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
            else if (!Input.GetButton("Jump"))
            {
                slider.SetActive(false);
                gaugeBar.value = 0;
                EnableMovement();
            }
        }

        if (isComplete && !isDelivering)
        {
           complete.gameObject.SetActive(true);
           
           if (Input.GetButton("Jump"))
            {
                slider.SetActive(true);
                gaugeBar.value += Time.deltaTime;
                DisableMovement();
                if (gaugeBar.value == gaugeBar.maxValue)
                {
                    isDelivering = true;
                    slider.SetActive(false);
                    EnableMovement();
                    
                    havenowImage.sprite = domeCover;
                    havenow.SetActive(true);
                }
            }

            else if (!Input.GetButton("Jump"))
            {
                EnableMovement();
                slider.SetActive(false);
                gaugeBar.value = 0;
            }
        }

        if (isDelivering && serving)
        {
            if (Input.GetButton("Jump"))
            {
                slider.SetActive(true);
                gaugeBar.value += Time.deltaTime;
                DisableMovement();
                if (gaugeBar.value == gaugeBar.maxValue)
                {
                    tableName = table.GetTableName();
                    Debug.Log(tableName);
                    EnableMovement();
                
                    if (TableCheck())
                    {
                        hasServed = true;
                        slider.SetActive(false);
                        ImageReset();
                        complete.gameObject.SetActive(false);
                        isDelivering = false;
                        serving = false;
                        isComplete = false;
                        Debug.Log("grats!!!");
                    }
                    else if (!TableCheck())
                    {
                        Stun(3f);
                        slider.SetActive(false);
                        Debug.Log("nonono!!");
                    }
                }
            }

            else if (!Input.GetButton("Jump"))
            {
                slider.SetActive(false);
                gaugeBar.value = 0;
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
        else if (other.tag == "Table")
        {
            serving = true;
            table = other.GetComponent<Table>();
        }

        else if (other.tag == "Enemy")
        {
            GetDamaged();
            Stun(2f);
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
        else if (other.tag == "Table")
        {
            serving = false;
            tableName = "None";
            Debug.Log(tableName);
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

    bool TableCheck()
    {
        if (recipe.customerno == 0 && tableName == "Yeti")
        {
            return true;
        }
        else if (recipe.customerno == 1 && tableName == "Pepe")
        {
            return true;
        }
        else if (recipe.customerno == 2 && tableName == "Pinkbean")
        {
            return true;
        }
        else if (recipe.customerno == 3 && tableName == "Pig")
        {
            return true;
        }
        else if (recipe.customerno == 4 && tableName == "Slime")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void TimeOut()
    {
        haveingridient = "None";
        ImageReset();
        complete.gameObject.SetActive(false);
        isDelivering = false;
        serving = false;
        isComplete = false;
    }

    void GetDamaged()
    {
        OnDamaged();
        Invoke("OffDamaged", blinkTime);
        Invoke("OnDamaged", blinkTime * 2);
        Invoke("OffDamaged", blinkTime * 3);
        Invoke("OnDamaged", blinkTime * 4);
        Invoke("OffDamaged", blinkTime * 5);
    }

    void OnDamaged()
    {
        spriter.color = new Color(1,1,1,0.4f);
    }

    void OffDamaged()
    {
        spriter.color = new Color(1,1,1,1);
    }
}
