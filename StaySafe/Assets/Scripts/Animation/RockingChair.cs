using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockingChair : MonoBehaviour
{

    public float MaxAngle;
    public float Frequency;

    float phase;
    float elapsedTime = 0.0f;

    private void Start() {
        phase = Random.Range(0.0f, Mathf.PI);
    }
    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        float angle = MaxAngle * Mathf.Sin(Frequency * elapsedTime + phase);
        this.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
