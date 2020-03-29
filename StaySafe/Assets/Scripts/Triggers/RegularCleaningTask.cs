using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum DirtState
{
    Clean,
    Dirty
};

public class RegularCleaningTask : Trigger
{
    public AudioClip DirtSound;
    public DirtState State = DirtState.Clean;
    public GameObject DirtObject;

    public float InitialTimeToGetDirty = 30.0f;
    public float MinTimeToGetDirty = 90.0f;
    public float MaxTimeToGetDirty = 190.0f;
    public float TimeToGetDirty;

    private void Start() {
        EnterState(DirtState.Clean);
        TimeToGetDirty = InitialTimeToGetDirty;
    }

    float elapsedTime = 0.0f;
    private void Update() {

        switch (State) {
            case DirtState.Clean:
                elapsedTime += Time.deltaTime;
                if (elapsedTime > TimeToGetDirty) {
                    elapsedTime = 0.0f;
                    EnterState(DirtState.Dirty);
                }
                break;
            case DirtState.Dirty:
                break;
        }
    }
    private void EnterState(DirtState state) {
        State = state;
        switch (state) {
            case DirtState.Clean:
                TimeToGetDirty = Random.Range(MinTimeToGetDirty, MaxTimeToGetDirty);
                DirtObject.SetActive(false);
                break;
            case DirtState.Dirty:
                if ((GetComponent<AudioSource>() is AudioSource src) && (DirtSound != null)) {
                    src.PlayOneShot(DirtSound);
                }
                DirtObject.SetActive(true);
                break;
        }
    }

    public float UsageDecreasePreUse = 0.025f;
    public override Vector2 Position => this.transform.position.ToPlane();

    public override bool IsAvailable(GameObject obj) {

        if (State == DirtState.Clean) return false;
        var ci = obj.GetComponent<CarriesItem>();
        if (ci == null) return false;

        var item = ci.Item;
        if (item == null) return false;
        return ((item.ItemType == ItemType.CleaningStuff) && (item.Usage > 0.0f));
    }

    public override bool IsTaskFor(PlayerTypes type) {
        return (State == DirtState.Dirty) && (type == PlayerTypes.Cleaner);
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
            EnterState(DirtState.Clean);
            Score.ScoreNumber += 10;
        }
    }
}
