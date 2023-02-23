using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField] string tableName;

    public string GetTableName()
    {
        return tableName;
    }
}
