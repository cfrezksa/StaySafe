using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TakeTrigger : Trigger
{
    public ItemType ItemType;
    public GameObject Board;
    public PlayerTypes[] PlayerTypes;

    public override Vector2 Position => this.transform.position.ToPlane();

    public override bool IsAvailable(GameObject obj) {

        var items = Board.GetComponentsInChildren<CarryItem>().ToArray();
        items = items.Where(x => x.ItemType == ItemType).ToArray();
        if (items.Length == 0) return false;

        var pm = obj.GetComponent<CarriesItem>();
        return !pm.HasItem;
    }

    public override bool IsTaskFor(PlayerTypes type) {
        return ((IEnumerable<PlayerTypes>)PlayerTypes).Contains(type);
    }

    public override void TriggerEvent(GameObject obj) {
        var items = Board.GetComponentsInChildren<CarryItem>().Where(x => x.ItemType == ItemType);
        var item = items.First();

        var ci = obj.GetComponent<CarriesItem>();
        item.transform.position = ci.ItemSlot.transform.position;
        if (ci != null) ci.Item = item;
    }
}
