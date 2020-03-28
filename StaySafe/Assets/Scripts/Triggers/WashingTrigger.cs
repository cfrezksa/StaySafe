using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WashingTrigger : Trigger
{

    void Start() {
        Combo = "Y";
    }
    public override bool IsAvailable(GameObject obj) {
        var h = obj.GetComponent<HealthState>();
        return h.Health < 0.95f;
    }

    public override Vector2 Position => this.transform.position.ToPlane();

    public override bool IsTaskFor(PlayerTypes type) => true;

    public override void TriggerEvent(GameObject obj) {
        var h = obj.GetComponent<HealthState>();
        h.Health = Mathf.Min(1.0f, h.Health + 0.2f);
        Combo = combos[Random.Range(0, combos.Length)];
    }

    private string[] combos = new string[] { "A", "B", "X", "Y" };
}
