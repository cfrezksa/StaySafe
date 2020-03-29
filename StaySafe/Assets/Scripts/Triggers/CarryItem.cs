using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    CleaningStuff,
    DogFood,
    Pasta,
    ToiletPaper,
    Meal,
    Delivery,
}

public class CarryItem : MonoBehaviour
{
    public ItemType ItemType;
    public float Usage = 0.0f;
}
