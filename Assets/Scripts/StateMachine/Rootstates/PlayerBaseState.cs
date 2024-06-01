using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState
{
    private bool _isRootState = false;
    private PlayerStateMachine _ctx;
    private PlayerStateFactory _factory;
    private PlayerBaseState _currentSubState;
    private PlayerBaseState _currentSuperState;
    protected PlayerBaseState CurrentSubState {get {return _currentSubState;}}

    protected bool IsRootState {get {return _isRootState;} set {_isRootState = value;}}
    protected PlayerStateMachine Ctx { get { return _ctx;}}
    protected PlayerStateFactory Factory { get { return _factory;}}

    public PlayerBaseState(PlayerStateMachine currentContext,PlayerStateFactory playerStateFactory) {
        _ctx = currentContext;
        _factory = playerStateFactory;
    }

    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void ExitState();

    public abstract void CheckSwitchState();

    public abstract void InitializeSubState();

    public void UpdateStates(){
        UpdateState();
        if (_currentSubState != null) {
            // Debug.Log(_currentSubState);
            _currentSubState.UpdateStates();
        }
    }

    /*
    public void ExitStates() {
        ExitState();
        if (_currentSubState != null) {
            _currentSubState.ExitStates();
        }
    }
    */

    protected void SwitchState(PlayerBaseState newState) {
        // current state exits state
        ExitState();

        // new state enters state
        newState.EnterState();

        // switch current state of context
        if (_isRootState) {
            _ctx.CurrentState = newState;
        } else if (_currentSuperState != null) {
            _currentSuperState.SetSubState(newState);
        }
    }

    protected void SetSuperState(PlayerBaseState newSuperState){
        _currentSuperState = newSuperState;
    }

    protected void SetSubState(PlayerBaseState newSubState){
        if (_currentSubState != null) {
            _currentSubState.ExitState();
        }
        _currentSubState = newSubState;
        newSubState.EnterState();
        newSubState.SetSuperState(this);
    }
}
