using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping : MonoBehaviour
{

    public float EnvelopeFrequency = 0.1f;
    public float JumpingFrequency = 2.0f;
    public float Amplitude = 0.2f;
    public bool Active = false;
    // Start is called before the first frame update
    void Start()
    {
        startPos = this.transform.position;
    }

    Vector3 startPos;
    float elapsedTime = 0.0f;
    // Update is called once per frame
    void Update()
    {
        if (Active) {
            elapsedTime += Time.deltaTime;
            float Envelope = (EnvelopeFrequency == 0.0f)? 1.0f : (Mathf.Sin(EnvelopeFrequency * elapsedTime) + 1.0f) / 2.0f;
            float Jumping = Amplitude * Mathf.Abs(Mathf.Sin(JumpingFrequency * elapsedTime)) * Envelope;
            this.transform.position = startPos + Jumping * Vector3.up;
        } else {
            elapsedTime = 0.0f;
            this.transform.position = startPos;
        }
    }
}
