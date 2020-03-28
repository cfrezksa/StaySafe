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
}

public class CarryItem : MonoBehaviour
{
    public ItemType ItemType;
}
