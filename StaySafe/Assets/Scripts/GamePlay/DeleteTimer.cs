using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteTimer : MonoBehaviour
{

    public float TimeToDelete = 6.0f;
    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 0.0f;
    }


    float elapsedTime = 0.0f;
    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > TimeToDelete) {
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
    }
}
