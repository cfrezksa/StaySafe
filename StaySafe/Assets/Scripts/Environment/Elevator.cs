using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Elevator : MonoBehaviour
{

    public Waypoint MovingWaypoint;
    public Waypoint UpWaypoint;
    public Waypoint DownWaypoint;

    public float MinY = 0.0f;
    public float MaxY = 10.0f;
    public float MovementTime = 1.5f;
    public AnimationCurve EasingCurve;
    // Start is called before the first frame update
    void Start()
    {
        MovementState = ElevatorState.MovingUp;
    }

    float elapsedTime = 0.0f;
    // Update is called once per frame
    void Update()
    {
        switch (MovementState) {
            case ElevatorState.Idle:
                break;
            case ElevatorState.MovingUp:
                UpdateMovingUp();
                break;
            case ElevatorState.MovingDown:
                UpdateMovingDown();
                break;
        }
    }

    private void UpdateMovingDown() {
        elapsedTime += Time.deltaTime;
        float ystate = Mathf.Clamp01(EasingCurve.Evaluate(elapsedTime / MovementTime));
        Vector3 pos = this.transform.position;
        pos.y = MaxY + ystate * (MinY - MaxY);
        this.transform.position = pos;
        if (ystate >= 1.0f) {
            MovementState = ElevatorState.Idle;
            AddDownPaths();
            ReleasePlayers(DownWaypoint);
            //MoveUp();
            //elapsedTime = -2.0f;
        }
    }

    private void UpdateMovingUp() {
        elapsedTime += Time.deltaTime;
        float ystate = Mathf.Clamp01(EasingCurve.Evaluate(elapsedTime / MovementTime));
        Vector3 pos = this.transform.position;
        pos.y = MinY + ystate * (MaxY - MinY);
        this.transform.position = pos;
        if (ystate >= 1.0f) {
            AddUpPaths();
            MovementState = ElevatorState.Idle;
            ReleasePlayers(UpWaypoint);
            //MoveDown();
            //elapsedTime = -2.0f;
        }
    }

    private void AddUpPaths() {
        int idxUpOutA = Enumerable.Range(0, UpWaypoint.OutgoingEdges.Length).Where(x => UpWaypoint.OutgoingEdges[x] == null).FirstOrDefault();
        if (UpWaypoint.OutgoingEdges[idxUpOutA]  == null) UpWaypoint.OutgoingEdges[idxUpOutA] = MovingWaypoint;
        int idxUpOutB = Enumerable.Range(0, MovingWaypoint.OutgoingEdges.Length).Where(x => MovingWaypoint.OutgoingEdges[x] == null).FirstOrDefault();
        if (MovingWaypoint.OutgoingEdges[idxUpOutB] == null) MovingWaypoint.OutgoingEdges[idxUpOutB] = UpWaypoint;
    }

    private void AddDownPaths() {
        int idxDownOutA = Enumerable.Range(0, DownWaypoint.OutgoingEdges.Length).Where(x => DownWaypoint.OutgoingEdges[x] == null).FirstOrDefault();
        if (DownWaypoint.OutgoingEdges[idxDownOutA]  == null) DownWaypoint.OutgoingEdges[idxDownOutA] = MovingWaypoint;
        int idxDownOutB = Enumerable.Range(0, MovingWaypoint.OutgoingEdges.Length).Where(x => MovingWaypoint.OutgoingEdges[x] == null).FirstOrDefault();
        if (MovingWaypoint.OutgoingEdges[idxDownOutB]  == null) MovingWaypoint.OutgoingEdges[idxDownOutB] = DownWaypoint;
    }

    private void RemoveDownPaths() {
        int idxStatic = Enumerable.Range(0, DownWaypoint.OutgoingEdges.Length).Where(x => DownWaypoint.OutgoingEdges[x] == MovingWaypoint).FirstOrDefault();
        if (DownWaypoint.OutgoingEdges[idxStatic] == MovingWaypoint) DownWaypoint.OutgoingEdges[idxStatic] = null; 
        int idxMoving = Enumerable.Range(0, MovingWaypoint.OutgoingEdges.Length).Where(x => MovingWaypoint.OutgoingEdges[x] == DownWaypoint).FirstOrDefault();
        if (MovingWaypoint.OutgoingEdges[idxMoving] == DownWaypoint) MovingWaypoint.OutgoingEdges[idxMoving] = null;
    }

    private void RemoveUpPaths() {
        int idxStatic = Enumerable.Range(0, UpWaypoint.OutgoingEdges.Length).Where(x => UpWaypoint.OutgoingEdges[x] == MovingWaypoint).FirstOrDefault();
        if (UpWaypoint.OutgoingEdges[idxStatic] == MovingWaypoint) UpWaypoint.OutgoingEdges[idxStatic] = null;

        int idxMoving = Enumerable.Range(0, MovingWaypoint.OutgoingEdges.Length).Where(x => MovingWaypoint.OutgoingEdges[x] == UpWaypoint).FirstOrDefault();
        if (MovingWaypoint.OutgoingEdges[idxMoving] == UpWaypoint) MovingWaypoint.OutgoingEdges[idxMoving] = null;
    }

    public void MoveUp() {
        if (IsUp) return;
        CapturePlayers();
        RemoveDownPaths();
        MovementState = ElevatorState.MovingUp;
        elapsedTime = 0.0f;
    }

    public void MoveDown() {
        if (IsDown) return;
        CapturePlayers();
        RemoveUpPaths();
        MovementState = ElevatorState.MovingDown;
        elapsedTime = 0.0f;
    }

    private void CapturePlayers() {
        CapturedPlayers = FindObjectsOfType<PlayerMotion>().Where(x => (x.Position - MovingWaypoint.Position).magnitude < Waypoint.InteractionRadius);
        foreach (var p in CapturedPlayers) {
            p.EnterElevator(MovingWaypoint);
        }
    }

    private void ReleasePlayers(Waypoint target) {
        foreach (var p in CapturedPlayers) {
            p.ReleaseFromElevator(MovingWaypoint, target);
        }
    }

    public bool IsUp => (MovementState == ElevatorState.Idle) && (Mathf.Abs(this.transform.position.y - MaxY) < TOLERANCE); 
    public bool IsDown => (MovementState == ElevatorState.Idle) && (Mathf.Abs(this.transform.position.y - MinY) < TOLERANCE);

    private IEnumerable<PlayerMotion> CapturedPlayers = Enumerable.Empty<PlayerMotion>();
    public enum ElevatorState
    {
        Idle,
        MovingUp,
        MovingDown,
    };
    public ElevatorState MovementState;
    public const float TOLERANCE = 0.2f;
}
