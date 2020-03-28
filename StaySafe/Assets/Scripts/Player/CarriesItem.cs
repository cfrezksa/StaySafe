using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarriesItem : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ItemSlot;
    public bool HasItem {
        get {
            return (ItemSlot.GetComponentsInChildren<CarryItem>().Length > 0);
        }
    }
    
    public CarryItem Item {
        get {
            return ItemSlot?.GetComponentsInChildren<CarryItem>().FirstOrDefault();
        }
        set {
            value.transform.parent = ItemSlot.transform;
        }
    }
}
