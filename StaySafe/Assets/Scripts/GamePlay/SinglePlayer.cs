using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SinglePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Toggle")) {
            var players = FindObjectsOfType<PlayerMotion>();
            var controllerNames = players.Select(x => x.PlayerName);
            var newCtrlNames = controllerNames.Skip(1).Append(controllerNames.First()).ToArray();
            for(int i = 0; i < players.Length; i++) {
                players[i].PlayerName = newCtrlNames[i];
                var cs = players[i].GetComponent<ComboSystem>();
                if (null != cs) cs.PlayerName = newCtrlNames[i];
            }
        }
    }
}
