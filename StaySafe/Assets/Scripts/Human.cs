using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HumanState
{
    Satisfied,
    Hungry,
    Eating,
}

public class Human : MonoBehaviour
{

    public float Depression = 0.0f;
    public float DepressionDecreaseOnFood = 0.5f;
    public float DepressionIncreaseOnHungry = 0.03f;
    public float DepressionIncreaseOnBored = 0.02f;
    public float DepressionDecreaseOnSatisfied = 0.01f;

    public RepairTrigger TV;
    public float InitialIdleTime = 10.0f;
    public float MinIdleTime = 10.0f;
    public float MaxIdleTime = 30.0f;
    public float MinEatingTime = 5.0f;
    public float MaxEatingTime = 15.0f;

    public HumanState State;
    public float IdleTime;
    public float EatingTime;
    public float TimeToShit;

    public SpriteRenderer DemandIcon;
    public Renderer DepressionBar;
    public Sprite HungrySprite;
    public Sprite NeedsRepairSprite;

    public void FeedHuman(CarryItem food) {
        food.transform.parent = null;
        GameObject.Destroy(food.gameObject);
        Depression -= DepressionDecreaseOnFood;
        EnterState(HumanState.Eating);
    }

    void Start() {
        EatingTime = Random.Range(MinEatingTime, MaxEatingTime);
        EnterState(HumanState.Satisfied);
        IdleTime = InitialIdleTime;
    }

    public float elapsedTime = 0.0f;
    void Update() {

        switch (State) {
            case HumanState.Satisfied:
                elapsedTime += Time.deltaTime;
                if (elapsedTime > IdleTime) {
                    elapsedTime = 0.0f;
                    EnterState(HumanState.Hungry);
                }
                Depression -= Time.deltaTime * DepressionDecreaseOnSatisfied;
                break;
            case HumanState.Hungry:
                Depression += Time.deltaTime * DepressionIncreaseOnHungry;
                elapsedTime = 0.0f;
                break;
            case HumanState.Eating:
                
                elapsedTime += Time.deltaTime;
                if (elapsedTime > EatingTime) {
                    elapsedTime = 0.0f;
                    EnterState(HumanState.Satisfied);
                }
                break;
        }

        if ((TV != null) && (TV.State == RepairTrigger.RepairState.Broken)) {
            Depression += Time.deltaTime * DepressionIncreaseOnBored;
            if ((null != DemandIcon) && (DemandIcon.sprite == null)) DemandIcon.sprite = NeedsRepairSprite;
        }
        Depression = Mathf.Clamp01(Depression);

        if (DepressionBar != null) {
            DepressionBar.material.SetFloat("_Health", 1.0f-Depression);
        }
    }

    private void EnterState(HumanState newState) {

        State = newState;
        var js = GetComponent<Jumping>();
        switch (newState) {
            case HumanState.Satisfied:
                IdleTime = Random.Range(MinIdleTime, MaxIdleTime);
                if (null != DemandIcon) DemandIcon.sprite = ((TV != null) && (TV.State == RepairTrigger.RepairState.Broken))? NeedsRepairSprite : null;
                if (null != js) js.Active = false;
                break;
            case HumanState.Hungry:
                if (null != DemandIcon) DemandIcon.sprite = HungrySprite;
                if (null != js) js.Active = false;
                break;
            case HumanState.Eating:
                EatingTime = Random.Range(MinEatingTime, MaxEatingTime);
                if (null != DemandIcon) DemandIcon.sprite = ((TV != null) && (TV.State == RepairTrigger.RepairState.Broken)) ? NeedsRepairSprite : null;
                if (null != js) js.Active = false;
                break;
        }

    }
}
