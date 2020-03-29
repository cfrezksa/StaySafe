using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waving : MonoBehaviour
{
    public float FrequencyX;
    public float FrequencyY;
    public float AmplitudeX;
    public float AmplitudeY;

    float phaseX;
    float phaseY;
    float elapsedTime = 0.0f;
    Vector3 initPos;

    private void Start() {
        initPos = this.transform.position;
        phaseX = Random.Range(0.0f, Mathf.PI);
        phaseY = Random.Range(0.0f, Mathf.PI);
    }
    // Update is called once per frame
    void Update() {
        elapsedTime += Time.deltaTime;
        float x = AmplitudeX * Mathf.Sin(FrequencyX * elapsedTime + phaseX);
        float y = AmplitudeY * Mathf.Sin(FrequencyY * elapsedTime + phaseY);
        this.transform.position = initPos + new Vector3(x,y,0.0f);
    }
}
