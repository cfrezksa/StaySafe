using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    
    public float HealthDecreasePerPerson = 0.03f;
    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("Enter Room:" + collision.gameObject.name);
        var h = collision.gameObject.GetComponent<HealthState>();
        if (h != null) PlayersInRoom.Add(h);
        SetHealthDescreaseForAllInmates();
    }

    private void OnTriggerExit2D(Collider2D collision) {
        Debug.Log("Exit Room:" + collision.gameObject.name);
        var h = collision.gameObject.GetComponent<HealthState>();
        if (h != null) {
            h.HealthDecreasePerSecond = HealthState.BaseHealthDecreasePerSecond;
            PlayersInRoom.Remove(h);
        }
        SetHealthDescreaseForAllInmates();
    }

    private void SetHealthDescreaseForAllInmates() {
        float healthDecrease = totalHealthDecrease;
        foreach (var x in PlayersInRoom) {
            x.HealthDecreasePerSecond = healthDecrease;
        }
    }

    float totalHealthDecrease => Mathf.Max(PlayersInRoom.Count-1, 0) * HealthDecreasePerPerson + HealthState.BaseHealthDecreasePerSecond;
    List<HealthState> PlayersInRoom = new List<HealthState>();
}
