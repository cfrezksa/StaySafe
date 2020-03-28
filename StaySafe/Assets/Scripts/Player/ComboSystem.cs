using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboSystem : MonoBehaviour
{

    public delegate void ComboEventHandler(object sender, string combo);

    public event ComboEventHandler OnCombo;
    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 0.0f;
        Combo = "";
    }

    float elapsedTime = 0.0f;
    // Update is called once per frame
    void Update()
    {
        var cmbo = Combo;
        if (Input.GetButtonDown($"{PlayerName}_Fire1")) {
            elapsedTime = 0.0f;
            Combo += "A";
        } if (Input.GetButtonDown($"{PlayerName}_Fire2")) {
            elapsedTime = 0.0f;
            Combo += "B";
        } else if (Input.GetButtonDown($"{PlayerName}_Fire3")) {
            elapsedTime = 0.0f;
            Combo += "X";
        } else if (Input.GetButtonDown($"{PlayerName}_Fire4")) {
            elapsedTime = 0.0f;
            Combo += "Y";
        } else {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > comboInterval) {
                Combo = "";
            }
        }

        if ((cmbo != Combo) && (!string.IsNullOrEmpty(Combo))){
            OnCombo?.Invoke(this, Combo);
        }
    }

    public float comboInterval = 0.2f;
    public string PlayerName = "Player1";
    public string Combo = "";
}
