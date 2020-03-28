using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TriggerInteraction : MonoBehaviour
{
    public PlayerTypes PlayerType;
    public SpriteRenderer sprite;
    Trigger[] AllTriggers;

    public Sprite[] ComboSprites;
    public List<string> TextureCombos;

    // Start is called before the first frame update
    void Start()
    {
        if (sprite != null) sprite.enabled = false;
        ComboSystem cs = GetComponent<ComboSystem>();
        if (cs != null) cs.OnCombo += OnCombo;
        AllTriggers = FindObjectsOfType<Trigger>();
    }

    private void OnCombo(object sender, string combo) {
        if ((ActiveTrigger != null) && (combo.EndsWith(ActiveTrigger.Combo))) {
            ActiveTrigger.TriggerEvent(this.gameObject);
        }
    }

    private Trigger ActiveTrigger = null;
    // Update is called once per frame
    void Update()
    {
        Vector2 P = this.transform.position.ToPlane();
        ActiveTrigger = AllTriggers
            .Where(x => x.IsTaskFor(PlayerType))
            .Where(x => (x.Position - P).magnitude < Waypoint.InteractionRadius)
            .Where(x => x.IsAvailable(gameObject)).FirstOrDefault();
        if (null != ActiveTrigger) {
            
            int idx = TextureCombos.IndexOf(ActiveTrigger.Combo);
            if ((idx >= 0) && (idx < ComboSprites.Length)) {
                sprite.sprite = ComboSprites[idx];
                sprite.enabled = true;
            }
        } else {
            sprite.enabled = false;
        }
    }
}
