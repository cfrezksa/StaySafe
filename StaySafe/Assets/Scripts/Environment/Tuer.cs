using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tuer : MonoBehaviour
{
    enum DoorState
    {
        Opening,
        Closing,
    };

    DoorState state;
    public SpriteRenderer Renderer;
    public Sprite[] States;
    private float elapsedTime = 100.0f;
    // Start is called before the first frame update
    void Start()
    {
        state = DoorState.Closing;
        Renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        switch(state) {
            case DoorState.Closing:
                UpdateClosing();
                return;
            case DoorState.Opening:
                UpdateOpening();
                return;
        }
    }

    public float speed = 0.1f;

    void UpdateClosing() {

        for(int i = 0; i < States.Length; i++) {
            if (elapsedTime < i * speed) {
                Renderer.sprite = States[States.Length-1-i];
                return;
            }
        }
        Renderer.sprite = States[0];

    }

    void UpdateOpening() {

        for (int i = 0; i < States.Length; i++) {
            if (elapsedTime < i * speed) {
                Renderer.sprite = States[i];
                return;
            }
        }
        Renderer.sprite = States[States.Length - 1];
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        var pm = collision.gameObject.GetComponent<PlayerMotion>();
        if (pm == null) return;
        
        Debug.Log($"Enter Door:{pm.name} colliders = {colliders.Count}");
        colliders.Add(pm);
        if (colliders.Count == 1) {
            state = DoorState.Opening;
            elapsedTime = 0.0f;
        }

    }


    private void OnTriggerExit2D(Collider2D collision) {
        var pm = collision.gameObject.GetComponent<PlayerMotion>();
        if (pm == null) return;

        Debug.Log($"Exit Door:{pm.name} colliders = {colliders.Count}");

        colliders.Remove(pm);
        if (colliders.Count == 0) {
            state = DoorState.Closing;
            elapsedTime = 0.0f;
        }

    }

    List<PlayerMotion> colliders = new List<PlayerMotion>();
}
