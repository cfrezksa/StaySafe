using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaningTask : Trigger
{

    public float UsageDecreasePreUse = 0.025f;
    public override Vector2 Position => this.transform.position.ToPlane();

    public override bool IsAvailable(GameObject obj) {
        var ci = obj.GetComponent<CarriesItem>();
        if (ci == null) return false;

        var item = ci.Item;
        if (item == null) return false;
        return ((item.ItemType == ItemType.CleaningStuff) && (item.Usage > 0.0f));
    }

    public override bool IsTaskFor(PlayerTypes type) {
        return type == PlayerTypes.Cleaner;
    }

    public float CleaningAmount = 1.0f;
    public override void TriggerEvent(GameObject obj) {

        var ci = obj.GetComponent<CarriesItem>();
        var item = ci.Item;
        item.Usage -= UsageDecreasePreUse;
        if (item.Usage <= 0.0f) {
            item = ci.TakeItem();
            item.gameObject.SetActive(false);
            Destroy(item.gameObject);
        }
        CleaningAmount -= 0.1f;

        if (CleaningAmount <= 0.0f) {
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
            Score.ScoreNumber += 10;
        }
    }
}
