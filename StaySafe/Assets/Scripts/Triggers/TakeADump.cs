using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeADump : Trigger
{
    public override Vector2 Position => this.transform.position.ToPlane();

    public override bool IsAvailable(GameObject obj) {

        var pet = obj.GetComponentInChildren<Pet>();
        if (pet == null) return false;
        return (pet.State == PetState.OutForWalk);
    }

    public override bool IsTaskFor(PlayerTypes type) {
        return (type == PlayerTypes.DogGuy);
    }

    float dumpFinished = 0.0f;
   
    public override void TriggerEvent(GameObject obj) {
        var pet = obj.GetComponentInChildren<Pet>();
        dumpFinished += 0.1f;
        if (dumpFinished >= 1.0f) {
            dumpFinished = 0.0f;
            pet.TakeADump();
            pet.State = PetState.ReturningFromWalk;
        }
    }
}
