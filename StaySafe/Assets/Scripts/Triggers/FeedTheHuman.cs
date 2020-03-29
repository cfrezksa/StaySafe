using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedTheHuman : Trigger
{
    public GameObject ToliletPaperPrefab;
    public override Vector2 Position => this.transform.position.ToPlane();
    public override bool IsAvailable(GameObject obj) {
        var human = GetComponent<Human>();
        if (human.State != HumanState.Hungry) return false;

        CarriesItem ci = obj.GetComponent<CarriesItem>();
        return ((ci != null) && (ci.HasItem) && (ci.Item.ItemType == ItemType.Meal));
    }

    public override bool IsTaskFor(PlayerTypes type) {
        return type == PlayerTypes.Cook;
    }

    public override void TriggerEvent(GameObject obj) {
        var human = GetComponent<Human>();
        CarriesItem ci = obj.GetComponent<CarriesItem>();
        var food = ci.TakeItem();
        human.FeedHuman(food);
        Score.ScoreNumber += 20;

        var tp = GameObject.Instantiate(ToliletPaperPrefab);
        ci.Item = tp.GetComponent<CarryItem>();

    }

}
