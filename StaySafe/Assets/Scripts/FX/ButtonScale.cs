using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScale : MonoBehaviour
{
    Vector3 InitialScale;

    // Start is called before the first frame update
    void Start()
    {
        InitialScale = this.transform.localScale;
    }

    public bool IsActive = true;
    public float timeScale = 1.0f;
    public AnimationCurve Curve;
    public float minScaleFactor = 1.0f;
    public float maxScaleFactor = 1.2f;
    float elapsedTime = 0.0f;
    // Update is called once per frame
    void Update()
    {
        if (IsActive) {
            elapsedTime += Time.deltaTime;
            float alpha = elapsedTime * timeScale;
            alpha = alpha - Mathf.Floor(alpha);
            float s = Mathf.Lerp(minScaleFactor, maxScaleFactor, Curve.Evaluate(alpha));
            this.transform.localScale = InitialScale * s;
        }
    }
}
