using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }

    public event EventHandler OnStateChange;

    private enum State {
        WaitingKeyPress,
        Gameplaying,
        GameOver,
    }

    private State state;
    private bool isObstacleHit;

    private void Awake() {
        Instance = this;
        state = State.WaitingKeyPress;
    }

    private void Start() {
        GameInput.Instance.OnSpacebarAction += GameInput_OnSpacebarAction;
    }

    private void GameInput_OnSpacebarAction(object sender, System.EventArgs e) {
        if (state == State.WaitingKeyPress) {
            state = State.Gameplaying;
        }
    }

    private void Update() {
        switch (state) {
            case State.WaitingKeyPress:
                OnStateChange?.Invoke(this, EventArgs.Empty);
                break;

            case State.Gameplaying:
                isObstacleHit = Player.Instance.IsObstacleHit();

                if (isObstacleHit) {
                    state = State.GameOver;
                    OnStateChange?.Invoke(this, EventArgs.Empty);
                }

                break;

            case State.GameOver:
                OnStateChange?.Invoke(this, EventArgs.Empty);
                break;
        }
    }

    public bool IsWaitingKeyPress() {
        return state == State.WaitingKeyPress;
    }

    public bool IsGamePlaying() {
        return state == State.Gameplaying;
    }

    public bool IsGameOver() {
        return state == State.GameOver;
    }

    private void OnDestroy() {
        GameInput.Instance.OnSpacebarAction -= GameInput_OnSpacebarAction;
    }
}
