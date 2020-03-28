using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Waypoint : MonoBehaviour
{
    public const float Radius = 0.2f;
    public const float InteractionRadius = 0.5f;
    public Waypoint[] OutgoingEdges;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
    }

    private void OnDrawGizmos() {

        var P0 = this.transform.position;
        Gizmos.DrawIcon(P0, "Waypoint.png", true);

        Gizmos.color = Color.red;
        if (OutgoingEdges != null) {
            foreach (var x in OutgoingEdges) {
                if (x == null) continue;
                Vector3 P1 = x.transform.position;
                Vector3 d = (P1 - P0).normalized;
                Vector3 r = 0.05f * new Vector3(d.y, d.x, -d.z);
                Gizmos.DrawLine(P0 + r, P1 + r);
            }
        }
       
        DrawCircle(P0, Radius);
        DrawCircle(P0, 2.0f * Radius);
        //Gizmos.DrawLine(p + Vector3.forward,    p + Vector3.back);
    }
    public Vector2 Position => this.transform.position.ToPlane();

    private static void DrawCircle(Vector3 P0, float radius) {
        Gizmos.color = Color.green;
        Vector3 p0 = P0 + radius * Vector3.right;
        Vector3 p1;
        int N = 32;
        for (int i = 0; i < N; i++) {
            float a = i * Mathf.PI * 2.0f / (N - 1);
            p1 = P0 + radius * (Mathf.Sin(a) * Vector3.right + Mathf.Cos(a) * Vector3.up);
            Gizmos.DrawLine(p0, p1);
            p0 = p1;
        }
    }
}
