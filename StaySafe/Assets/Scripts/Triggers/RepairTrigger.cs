using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairTrigger : Trigger
{

    public enum RepairState
    {
        Broken,
        Working
    }

    public RepairState State = RepairState.Working;

    public float MinTimeToStayWorking = 5.0f;
    public float TimeToStayWorking;
    public float MaxDamagePerSecond = 0.1f;
    public SpriteRenderer ItemToBreak;
    public ParticleSystem BrokenEffekt;
    public Sprite DamagedSprite;
    public Sprite WorkingSprite;
    public float RepairPerClick = 0.025f;

    // Start is called before the first frame update
    void Start() {
        if (BrokenEffekt != null) {
            BrokenEffekt.Stop();
        }
        if (ItemToBreak != null) {
            ItemToBreak.sprite = WorkingSprite;
        }
        TimeToStayWorking = Random.Range(MinTimeToStayWorking, 2.0f * MinTimeToStayWorking);
    }

    public float Damage = 0.0f;
    float elapsedTime = 0.0f;

    public override bool IsAvailable(GameObject obj) { return State == RepairState.Broken; }

    public override Vector2 Position => this.transform.position.ToPlane();

    // Update is called once per frame
    void Update()
    {
        if (State == RepairState.Working) {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > TimeToStayWorking) {
                float damage = Random.Range(0.0f, MaxDamagePerSecond);
                Damage += Time.deltaTime * damage;
            }

            if (Damage > 1.0f) {
                State = RepairState.Broken;
                if (ItemToBreak != null) {
                    ItemToBreak.sprite = DamagedSprite;
                }
                if (BrokenEffekt != null) {
                    BrokenEffekt.Play();
                }
            }
        } else if (Damage < 0.1f) {
            State = RepairState.Working;
            TimeToStayWorking = Random.Range(MinTimeToStayWorking, 2.0f * MinTimeToStayWorking);
            elapsedTime = 0.0f;
            if (ItemToBreak != null) {
                ItemToBreak.sprite = WorkingSprite;
            }
            if (BrokenEffekt != null) {
                BrokenEffekt.Stop();
            }
        }
    }

    public override void TriggerEvent(GameObject obj) {
        Damage -= RepairPerClick;
    }

    public override bool IsTaskFor(PlayerTypes type) {
        return type == PlayerTypes.Repairer;
    }
}
