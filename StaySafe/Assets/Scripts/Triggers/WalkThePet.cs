using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkThePet : Trigger
{
    public GameObject PetObject;
    public override Vector2 Position => this.transform.position.ToPlane();

    public override bool IsAvailable(GameObject obj) {
        if (PetObject.GetComponent<Pet>() is Pet pet) {
            return pet.State == PetState.NeedsAShit;
        } else {
            return false;
        }
    }

    public override bool IsTaskFor(PlayerTypes type) {
        return (type == PlayerTypes.DogGuy);
    }

    public override void TriggerEvent(GameObject obj) {
        if (PetObject.GetComponent<Pet>() is Pet pet) {
            pet.GoWalking();
            PetObject.transform.position = obj.transform.position;
            PetObject.transform.parent = obj.transform;
        }
    }

}
