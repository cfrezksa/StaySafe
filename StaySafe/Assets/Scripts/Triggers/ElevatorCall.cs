using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorCall : Trigger
{
    public enum TriggerMode
    {
        Auto,
        CallUp,
        CallDown,
    }

    public TriggerMode Mode = TriggerMode.CallUp;

    Elevator Elevator {
        get {
           if (foundElevator == null) {
                foundElevator = FindObjectOfType<Elevator>();
            }
            return foundElevator;
        }
    }
    Elevator foundElevator = null;

    public override void TriggerEvent(GameObject obj) {

        if (null != Elevator) {
            switch (Mode) {
                case TriggerMode.CallUp:
                    Elevator.MoveUp();
                    break;
                case TriggerMode.CallDown:
                    Elevator.MoveDown();
                    break;
                case TriggerMode.Auto:
                    if (Elevator.IsUp) Elevator.MoveDown();
                    if (Elevator.IsDown) Elevator.MoveUp();
                    break;
            }
        }
    }

    public override bool IsAvailable(GameObject obj) {
       
            if (Elevator != null) {
                switch (Mode) {
                    case TriggerMode.CallUp:
                        return !Elevator.IsUp;
                    case TriggerMode.CallDown:
                        return !Elevator.IsDown;
                    case TriggerMode.Auto:
                        return (Elevator.IsUp || Elevator.IsDown);
                }
            }
            return false;

    }

    public override Vector2 Position => this.transform.position.ToPlane();

    public override bool IsTaskFor(PlayerTypes type) => true;
}
