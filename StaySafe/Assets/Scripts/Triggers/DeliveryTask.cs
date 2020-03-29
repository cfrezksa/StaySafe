using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DeliveryState
{
    Arriving,
    Delivering,
}

public class DeliveryTask : Trigger
{

    public GameObject DeliveryGuy;
    public GameObject PacketPrefab;

    public DeliveryState State = DeliveryState.Arriving;
    public float InitialTimeToDelivery = 90.0f;
    public float MinTimeToDelivery = 45.0f;
    public float MaxTimeToDelivery = 120.0f;
    public float MinWaitingTime = 20.0f;
    public float MaxWaitingTime = 40.0f;

    public float TimeToDelivery = 0.0f;
    public float WaitingTime = 0.0f;

    public override Vector2 Position => this.transform.position.ToPlane();

    public override bool IsAvailable(GameObject obj) {

        if (State != DeliveryState.Delivering) return false;

        var ci = obj.GetComponent<CarriesItem>();
        if (ci == null) return false;

        var item = ci.Item;
        if (item == null) return false;
        return (item.ItemType == ItemType.ToiletPaper);

    }

    public override bool IsTaskFor(PlayerTypes type) {
        return (State == DeliveryState.Delivering);
    }

    public override void TriggerEvent(GameObject obj) {

        var ci = obj.GetComponent<CarriesItem>();
        var item = ci.TakeItem();
        item.gameObject.SetActive(false);
        Destroy(item.gameObject);
        var packet = GameObject.Instantiate(PacketPrefab);
        ci.Item = packet.GetComponent<CarryItem>();
        EnterState(DeliveryState.Arriving);

    }

    // Start is called before the first frame update
    void Start()
    {
        DeliveryGuy.SetActive(false);
        TimeToDelivery = InitialTimeToDelivery;
        WaitingTime = Random.Range(MinWaitingTime, MaxWaitingTime);
    }

    public AudioClip RingingSound;
    public float elapsedTime = 0.0f;
    void Update()
    {
        elapsedTime += Time.deltaTime;

        switch (State) {
            case DeliveryState.Arriving:
                if (elapsedTime > TimeToDelivery) {
                    elapsedTime = 0.0f;
                    EnterState(DeliveryState.Delivering);
                }
                break;
            case DeliveryState.Delivering:
                

                if (elapsedTime > WaitingTime) {
                    elapsedTime = 0.0f;
                    EnterState(DeliveryState.Arriving);
                }
                break;
        }
    }

    private void EnterState(DeliveryState newState) {

        State = newState;
        switch (newState) {
            case DeliveryState.Arriving:
                DeliveryGuy.SetActive(false);
                break;
            case DeliveryState.Delivering:
                if (GetComponent<AudioSource>() is AudioSource audio) {
                    audio.PlayOneShot(RingingSound);
                }
                DeliveryGuy.SetActive(true);
                break;
        }
    }
}
