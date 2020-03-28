using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerTypes
{
    Cook = 1,
    Cleaner = 2,
    Repairer = 5,
    DogGuy = 8,
}
public abstract class Trigger : MonoBehaviour
{
    public string Combo = "A";
    public abstract void TriggerEvent(GameObject obj);
    public abstract bool IsAvailable(GameObject obj);
    public abstract Vector2 Position { get; }
    public abstract bool IsTaskFor(PlayerTypes type);
}
