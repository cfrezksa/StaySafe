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

    public AudioClip BrokenClip;
    public RepairState State = RepairState.Working;

    public float InitialTimeToBreak;
    public float MinTimeToBreak = 5.0f;
    public float MaxTimeToBreak = 125.0f;
    public float TimeToBreak;

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
        EnterWorkingState();
        TimeToBreak = InitialTimeToBreak;
    }

    float elapsedTime = 0.0f;

    public override bool IsAvailable(GameObject obj) { return State == RepairState.Broken; }

    public override Vector2 Position => this.transform.position.ToPlane();

    // Update is called once per frame
    void Update()
    {
        if (State == RepairState.Working) {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > TimeToBreak) {
                EnterBrokenState();
            }
        }
    }

    private void EnterWorkingState() {
        State = RepairState.Working;
        TimeToBreak = Random.Range(MinTimeToBreak, MaxTimeToBreak);
        elapsedTime = 0.0f;
        if (ItemToBreak != null) {
            ItemToBreak.sprite = WorkingSprite;
        }
        if (BrokenEffekt != null) {
            BrokenEffekt.Stop();
        }
    }

    float Damage;
    private void EnterBrokenState() {
        State = RepairState.Broken;
        Damage = 1.0f;
        if ((BrokenClip != null) && (GetComponent<AudioSource>() is AudioSource src)) {
            src.PlayOneShot(BrokenClip);
        }
        if (ItemToBreak != null) {
            ItemToBreak.sprite = DamagedSprite;
        }
        if (BrokenEffekt != null) {
            BrokenEffekt.Play();
        }
    }

    public override void TriggerEvent(GameObject obj) {
        Damage -= RepairPerClick;
        if (Damage <= 0.0f) {
            EnterWorkingState();
            Score.ScoreNumber += 20;
        }
    }

    public override bool IsTaskFor(PlayerTypes type) {
        return type == PlayerTypes.Repairer;
    }
}
