using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMotion : MonoBehaviour

{

    public string PlayerName = "Player1";
    public Waypoint WP0 = null;
    public Waypoint WP1 = null;
    public float speed = 0.1f;
    private float Z;

        // Start is called before the first frame update
    void Start() {
        Z = this.transform.position.z;
        SetToClosestPointOnEdge();
    }

    public void EnterElevator(Waypoint wp0) {
        this.transform.SetParent(wp0.transform, true);
        WP0 = wp0;
        WP1 = wp0;
    }
    
    public void ReleaseFromElevator(Waypoint wp0, Waypoint wp1) {
        this.transform.SetParent(null, true);
        WP0 = wp0;
        WP1 = wp1;
    }
    // Update is called once per frame
    void Update()
    {
        if (WP0 == WP1) {
            return;
        }

        float x = Input.GetAxis($"{PlayerName}_Horizontal");
        float y = Input.GetAxis($"{PlayerName}_Vertical");
        Vector2 move = new Vector2(x, y);

        var P = this.transform.position.ToPlane();
        var P0 = WP0.transform.position.ToPlane();
        var P1 = WP1.transform.position.ToPlane();


        float d0 = (P - P0).magnitude;
        float d1 = (P - P1).magnitude;

        float reduceMoveDist = 0.0f;
        if (d0 < Waypoint.Radius) {
            var possibleDirs = WP0.OutgoingEdges.Where(e => e != null).Select(e => (wp: e, dir: (e.transform.position.ToPlane() - P).normalized));
            var mainDir = possibleDirs.OrderBy(pd => -Vector2.Dot(pd.dir, move)).First();
            WP1 = mainDir.wp;
            P1 = WP1.transform.position.ToPlane();

        } else if (d1 < Waypoint.Radius) {
            var possibleDirs = WP1.OutgoingEdges.Where(e => e != null).Select(e => (wp: e, dir: (e.transform.position.ToPlane() - P).normalized));
            var mainDir = possibleDirs.OrderBy(pd => -Vector2.Dot(pd.dir, move)).First();
            WP0 = WP1;
            WP1 = mainDir.wp;
            P0 = WP0.transform.position.ToPlane();
            P1 = WP1.transform.position.ToPlane();

        }

        var e0 = (P - P0);
        var e1 = (P1 - P);
        var m0 = e0.sqrMagnitude;
        var m1 = e1.sqrMagnitude;

        Vector3 edgeDir = (m0 > m1) ? e0.normalized : e1.normalized;
        move = Vector3.Dot(move, edgeDir) * edgeDir;
        //Debug.Log($"move = {move}");
        move *= speed * Time.deltaTime;
        if (reduceMoveDist > 0.0f) {
            var moveDir = move.normalized;
            var modeD = Mathf.Max(0.0f, move.magnitude - reduceMoveDist);
            move = modeD * moveDir;
        }
        MovePosition(move);

        //Vector3 pos = this.transform.position;
        //Debug.DrawLine(pos - Vector3.right, pos + Vector3.right, Color.magenta);
        //Debug.DrawLine(pos - Vector3.up, pos + Vector3.up, Color.magenta);
    }

    public Vector2 Position => this.transform.position.ToPlane();

    private void SetPosition(Vector2 p) {
        var P = this.transform.position;
        this.transform.position = new Vector3(p.x, p.y, Z);
    }

    private void MovePosition(Vector2 dir) {
        var P = this.transform.position.ToPlane();
        Vector2 dest = P + dir;
        Vector3 newPos = Util.ProjectPointToLine(dest, WP0.transform.position.ToPlane(), WP1.transform.position.ToPlane());
        newPos.z = Z;
        this.transform.position = newPos;
    }

    private void SetToClosestPointOnEdge() {
        var P = this.transform.position.ToPlane();
        var wps = FindObjectsOfType<Waypoint>();
        var edges = wps.SelectMany(wp1 => wp1.OutgoingEdges.Where(wp2 => wp2 != null).Select(wp2 => (wp1, wp2)));
        var cps = edges.Select(x => (x.wp1, x.wp2, d: Util.ClosestPointFromLine(this.gameObject, x.wp1, x.wp2))).OrderBy(x => (x.d - P).sqrMagnitude).First();
        var Q = cps.d;

        SetPosition(Q);
        WP0 = cps.wp1;
        WP1 = cps.wp2;
    }

}

public static class Util
{

    public static Vector2 ToPlane(this Vector3 p) {
        return new Vector2(p.x, p.y);
    }
    public static Vector2 ClosestPointFromLine(GameObject a, Waypoint q1, Waypoint q2) =>
        ClosestPointFromLine(a.transform.position.ToPlane(), q1.transform.position.ToPlane(), q2.transform.position.ToPlane());

    public static Vector2 ClosestPointFromLine(Vector2 P, Vector2 Q1, Vector2 Q2) {
        Vector2 d = (Q1 - Q2).normalized;
        Vector2 B = Q1 + Vector2.Dot(P-Q1, d) * d;
        Vector2 q1 = Q1 - B;
        Vector2 q2 = B - Q2;
        if (Vector2.Dot(q1, q2) > 0.0) {
            return B;
        } else {
            var d1 = (P - Q1).sqrMagnitude;
            var d2 = (P - Q2).sqrMagnitude;
            return (d1 > d2) ? Q2 : Q1;
        }
    }

    public static Vector2 ProjectPointToLine(Vector2 P, Vector2 Q1, Vector2 Q2) {
        Vector2 d = (Q1 - Q2).normalized;
        Vector2 B = Q1 + Vector2.Dot(P - Q1, d) * d;
        Vector2 q1 = Q1 - B;
        Vector2 q2 = B - Q2;
        if (Vector2.Dot(q1, q2) > 0.0) {
            return B;
        } else {
            var d1 = (P - Q1).sqrMagnitude;
            var d2 = (P - Q2).sqrMagnitude;
            if (d1 > d2) {
                if ((Q2 - B).magnitude < Waypoint.Radius) {
                    // within tolerance
                    return B;
                } else {
                    var moveDir = (B - Q2).normalized;
                    return Q2 + moveDir * Waypoint.Radius;

                }
            } else {
                if ((Q1 - B).magnitude < Waypoint.Radius) {
                    // within tolerance
                    return B;
                } else {
                    var moveDir = (B - Q1).normalized;
                    return Q1 + moveDir * Waypoint.Radius;

                }
            }
        }
    }
}
