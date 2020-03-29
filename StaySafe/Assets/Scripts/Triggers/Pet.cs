using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PetState
{
    Satisfied,
    Hungry,
    Eating,
    NeedsAShit,
    OutForWalk,
    ReturningFromWalk,
};

public enum PetType
{
    Dog, // dogs are getting hungry and need a shit from time to time
    Cat, // cats are getting hungry only
}

public class Pet : MonoBehaviour
{

    public AudioClip PetSound;
    public PetType Type = PetType.Dog;

    public float InitialIdleTime = 10.0f;
    public float MinIdleTime = 10.0f;
    public float MaxIdleTime = 30.0f;
    public float MinEatingTime = 5.0f;
    public float MaxEatingTime = 15.0f;
    public float MinTimeToShit = 20.0f;
    public float MaxTimeToShit = 30.0f;

    public PetState State;
    public float IdleTime;
    public float EatingTime;
    public float TimeToShit;

    public GameObject RegularDumpItem;
    public GameObject EmergencyDumpItem;
    public Transform ShitSlot;
    public Transform FoodSlot;
    public SpriteRenderer DemandIcon;
    public Sprite HungrySprite;
    public Sprite NeedsShitSprite;

    public void TakeADump() {
        if (RegularDumpItem != null) {
            var shit = GameObject.Instantiate(RegularDumpItem);
            shit.transform.position = ShitSlot.position;
        }
    }

    private GameObject foodItem = null;
    public void FeedPet(CarryItem food) {
        food.transform.parent = null;
        food.transform.position = FoodSlot.position;
        foodItem = food.gameObject;
        EnterState(PetState.Eating);
    }

    public void GoWalking() {
        GetComponent<FeedThePet>().enabled = false;
        GetComponent<Jumping>().enabled = false;
        EnterState(PetState.OutForWalk);
        if (null != DemandIcon) DemandIcon.sprite = null;
    }

    public void ReturnFromWalk() {
        GetComponent<Jumping>().enabled = true;
        GetComponent<FeedThePet>().enabled = true;
        EnterState(PetState.Satisfied);
    }

    void Start()
    {
        TimeToShit = Random.Range(MinTimeToShit, MaxTimeToShit);
        EnterState(PetState.Satisfied);
        IdleTime = InitialIdleTime;
    }

    public float elapsedTime = 0.0f;
    void Update()
    {

        switch(State) {
            case PetState.Satisfied:
                elapsedTime += Time.deltaTime;
                if (elapsedTime > IdleTime) {
                    elapsedTime = 0.0f;
                    EnterState(PetState.Hungry);
                }
                if (foodItem != null) { foodItem.SetActive(false); Destroy(foodItem); foodItem = null; }
                break;
            case PetState.Hungry:
                elapsedTime = 0.0f;
                break;
            case PetState.Eating:
                elapsedTime += Time.deltaTime;
                if (elapsedTime > EatingTime) {
                    elapsedTime = 0.0f;
                    EnterState((Type == PetType.Dog)? PetState.NeedsAShit : PetState.Satisfied);
                }
                break;
            case PetState.NeedsAShit:
                if (foodItem != null) { foodItem.SetActive(false); Destroy(foodItem); foodItem = null; }
                elapsedTime += Time.deltaTime;
                if (elapsedTime > TimeToShit) {
                    elapsedTime = 0.0f;
                    EmergencyDump();
                    EnterState(PetState.Satisfied);
                }
                break;
        }
    }

    private void PlayPetSound() {
        if (null != PetSound) {
            if (GetComponent<AudioSource>() is AudioSource src) {
                src.PlayOneShot(PetSound);
            }
        }
    }

    private void EmergencyDump() {
        if (EmergencyDumpItem != null) {
            var shit = GameObject.Instantiate(EmergencyDumpItem);
            shit.transform.position = ShitSlot.position;
        }
    }

    private void EnterState(PetState newState) {

        State = newState;
        var js = GetComponent<Jumping>();
        switch (newState) {
            case PetState.Satisfied:
                IdleTime = Random.Range(MinIdleTime, MaxIdleTime);
                if (null != DemandIcon) DemandIcon.sprite = null;
                if (null != js) js.Active = false;
                break;
            case PetState.Hungry:
                PlayPetSound();
                if (null != DemandIcon) DemandIcon.sprite =  HungrySprite;
                if (null != js) js.Active = false;
                break;
            case PetState.Eating:
                EatingTime = Random.Range(MinEatingTime, MaxEatingTime);
                if (null != DemandIcon) DemandIcon.sprite = null;
                if (null != js) js.Active = false;
                break;
            case PetState.NeedsAShit:
                PlayPetSound();
                if (null != DemandIcon) DemandIcon.sprite = NeedsShitSprite;
                if (null != js) js.Active = true;
                break;
        }

    }
}
