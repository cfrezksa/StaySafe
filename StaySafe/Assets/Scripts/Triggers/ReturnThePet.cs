using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnThePet : Trigger
{
    public GameObject PetObject;
    public override Vector2 Position => this.transform.position.ToPlane();

    public override bool IsAvailable(GameObject obj) {

        var pet =  obj.GetComponentInChildren<Pet>();
        return (pet != null) && (pet.gameObject == PetObject) &&
            ((pet.State == PetState.OutForWalk) || (pet.State == PetState.ReturningFromWalk));

    }

    public void Start() {
        PetPosition = PetObject.transform.position;
    }
    public override bool IsTaskFor(PlayerTypes type) {
        return (type == PlayerTypes.DogGuy);
    }

    public override void TriggerEvent(GameObject obj) {
        if (PetObject.GetComponent<Pet>() is Pet pet) {
            pet.ReturnFromWalk();
            PetObject.transform.position = PetPosition;
            PetObject.transform.SetParent(null);
        }
    }
    Vector3 PetPosition;
}
