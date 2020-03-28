using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthState : MonoBehaviour
{
    // Start is called before the first frame update

    public float Health;
    public const float BaseHealthDecreasePerSecond = 0.005f;
    public float HealthDecreasePerSecond = BaseHealthDecreasePerSecond;
    public Renderer Renderer;
    void Start()
    {
        Health = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Health -= Time.deltaTime * HealthDecreasePerSecond;
        Renderer.material.SetFloat("_Health", Health);
    }
}
