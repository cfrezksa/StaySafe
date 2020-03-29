using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DropTrigger : Trigger
{

    public GameObject[] DeliveredItemPrefabs;
    public ItemType[] AcceptedItems;
    public Transform Regal;
    public override Vector2 Position => this.transform.position.ToPlane();

    public override bool IsAvailable(GameObject obj) {

        var ci = obj.GetComponent<CarriesItem>();
        if (null == ci) return false;

        if (ci.Item is CarryItem item) {
            return AcceptedItems.Contains(item.ItemType);
        }
        return false;
        
    }

    public override bool IsTaskFor(PlayerTypes type) {
        return true;
    }

    public override void TriggerEvent(GameObject obj) {

        var ci = obj.GetComponent<CarriesItem>();
        var item = ci.TakeItem();

        if (item.ItemType == ItemType.Delivery) {

            int numItemsInDelivery = Random.Range(2, 5);
            numItemsInDelivery = Mathf.Max(EmptySlots.Count(), numItemsInDelivery);
            for(int i = 0; i < numItemsInDelivery; i++) {
                int idx = Random.Range(0, DeliveredItemPrefabs.Length);
                var g = DeliveredItemPrefabs[idx];
                var delivery = Instantiate(g).GetComponent<CarryItem>();
                StoreItem(delivery);
            }
            item.gameObject.SetActive(false);
            Destroy(item.gameObject);

        } else {
            StoreItem(item);
        }

    }

    private void StoreItem(CarryItem item) {
        var slot = EmptySlots.FirstOrDefault();
        if (slot == null) return;
        item.transform.position = slot.transform.position;
        item.transform.parent = slot.transform;
    }

    public IEnumerable<GameObject> EmptySlots =>
        Enumerable.Range(0, Regal.childCount).Select(i => Regal.GetChild(i))
            .Select(t => t.gameObject).Where(g => g.transform.childCount == 0);
    
}
