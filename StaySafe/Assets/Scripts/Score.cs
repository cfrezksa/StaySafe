using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Score : MonoBehaviour
{
    public float MaxGameTime = 10.0f * 60.0f;
    public TMPro.TextMeshProUGUI TextDisplay;
    public static float ScoreNumber = 0;

    public float LocalScore = 0;
    public float elapsedTime = 0.0f;

    void Update()
    {
        ScoreNumber += HygieneScore() * Time.deltaTime;
        ScoreNumber += DepressionScore() * Time.deltaTime;

        ScoreNumber = Mathf.Max(-10.0f, ScoreNumber);

        elapsedTime += Time.deltaTime;
        while (elapsedTime > 0.1f) {
            elapsedTime -= 0.1f;
            if (LocalScore > ScoreNumber) {
                LocalScore -= 1;
            }
            if (LocalScore < ScoreNumber) {
                LocalScore += 1;
            }

            int DisplayScore = (int)Mathf.Max(0, LocalScore);
            TextDisplay.text = $"{(int)DisplayScore}";
        }

        if (elapsedTime > MaxGameTime) {
            // TODO: Game Over: Display Highscore
        }

    }

    float HygieneScore() {
        return FindObjectsOfType<HealthState>().All(x => x.Health > 0.5f)? 0.2f : -0.3f;
    }

    float DepressionScore() {
        return FindObjectsOfType<Human>().All(x => x.Depression < 0.5f) ? 0.1f : -0.15f;
    }

    //float WeightDepression(float d) {
    //    return (d > 0.5f) ? -1.0f : 0.5f;
    //}

    //float HygieneScore(float h) {
    //    return (h > 0.5f) ? 2.0f : -3.0f;
    //}
}
