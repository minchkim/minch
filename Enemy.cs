using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;

    void Update()
    {
        transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Wall")
        {
            moveSpeed = -moveSpeed;
        }
    }
}
