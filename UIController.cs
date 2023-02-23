using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    Transform playerTransform;
    [SerializeField]
    Vector3 uiPosition;

    void Start()
    {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UIMove();
    }

    void UIMove()
    {
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + 0.5f, playerTransform.position.z);
    }
}
