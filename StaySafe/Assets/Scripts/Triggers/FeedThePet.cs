using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedThePet : Trigger
{
    public override Vector2 Position => this.transform.position.ToPlane();
    public override bool IsAvailable(GameObject obj) {
        var pet = GetComponent<Pet>();
        if (pet.State != PetState.Hungry) return false;

        CarriesItem ci = obj.GetComponent<CarriesItem>();
        return ((ci != null) && (ci.HasItem) && (ci.Item.ItemType == ItemType.DogFood));
    }

    public override bool IsTaskFor(PlayerTypes type) {
        return type == PlayerTypes.DogGuy;
    }

    public override void TriggerEvent(GameObject obj) {
        var pet = GetComponent<Pet>();
        CarriesItem ci = obj.GetComponent<CarriesItem>();
        var food = ci.TakeItem();
        pet.FeedPet(food);
        Score.ScoreNumber += 10;

    }

}
