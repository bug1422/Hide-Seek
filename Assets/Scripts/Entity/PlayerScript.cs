using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public delegate void OnChangingInventory(int i);
    public static event OnChangingInventory onChangingInventory;

    [SerializeField] private GameObject flashlight;
    private int index = -1;
    private List<GameObject> items = new();
    // Start is called before the first frame update
    void Start()
    {
        items.Add(flashlight);
    }

    // Update is called once per frame
    void Update()
    {
        CheckInventory();
    }
    void CheckInventory()
    {
        var newIndex = index;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            newIndex = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            newIndex = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            newIndex = 2;
        }
        if (newIndex != index)
        {
            index = newIndex;
            ChangeItem();
        }
    }
    void ChangeItem()
    {
        for (int i = 0; i < items.Count(); i++)
        {
            if (index == i)
            {
                items[i].SetActive(true);
            }
            else
            {
                items[i].SetActive(false);
            }
        }
        onChangingInventory.Invoke(index);
    }
}
