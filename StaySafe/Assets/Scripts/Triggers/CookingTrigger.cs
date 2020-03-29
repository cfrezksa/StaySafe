using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingTrigger: Trigger
{
    public GameObject MealItem;
    public override Vector2 Position => this.transform.position.ToPlane();

    public override bool IsAvailable(GameObject obj) {

        var rt = obj.GetComponent<RepairTrigger>();
        if ((rt != null) && (rt.State == RepairTrigger.RepairState.Broken)) return false;

        var ci = obj.GetComponent<CarriesItem>();
        if (HasFood) {
            return ci.HasItem == false;
        } else {

            if (null == ci) return false;

            var item = ci.Item;
            if (item == null) return false;
            return (item.ItemType == ItemType.Pasta);
        }
    }

    bool HasFood = false;
    public override bool IsTaskFor(PlayerTypes type) {
        return type == PlayerTypes.Cook;
    }

    public float mealReady = 0.0f;
    public override void TriggerEvent(GameObject obj) {

        var ci = obj.GetComponent<CarriesItem>();

        if (!HasFood) {
            var food = ci.TakeItem();
            food.gameObject.SetActive(false);
            GameObject.Destroy(food);
            HasFood = true;
        }

        mealReady += 0.1f;
        if (mealReady >= 1.0f) {
            HasFood = false;
            var mealItem = GameObject.Instantiate(MealItem);
            ci.Item = mealItem.GetComponent<CarryItem>();
            mealReady = 0.0f;
        }
    }

}
