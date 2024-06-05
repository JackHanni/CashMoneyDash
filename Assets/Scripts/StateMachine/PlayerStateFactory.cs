using System.Collections.Generic;

enum PlayerStates {
    idle,
    walk,
    run,
    grounded,
    jump,
    fall,
    crouched,
    backflip,
    longjump,
    jumpsub,
    victory,
    defeated,
    //floated
}

public class PlayerStateFactory
{
    PlayerStateMachine _context;
    Dictionary<PlayerStates, PlayerBaseState> _states = new Dictionary<PlayerStates,PlayerBaseState>();

    public PlayerStateFactory(PlayerStateMachine currentContext) {
        _context = currentContext;
        _states[PlayerStates.idle]      = new PlayerIdleState(_context,this);
        _states[PlayerStates.walk]      = new PlayerWalkState(_context,this);
        _states[PlayerStates.run]       = new PlayerRunState(_context,this);
        _states[PlayerStates.grounded]  = new PlayerGroundedState(_context,this);
        _states[PlayerStates.jump]      = new PlayerJumpState(_context,this);
        _states[PlayerStates.fall]      = new PlayerFallState(_context,this);
        _states[PlayerStates.crouched]  = new PlayerCrouchedState(_context,this);
        _states[PlayerStates.backflip]  = new PlayerBackflipState(_context,this);
        _states[PlayerStates.longjump]  = new PlayerLongjumpState(_context,this);
        _states[PlayerStates.jumpsub]   = new PlayerJumpSubstate(_context,this);
        _states[PlayerStates.victory]   = new PlayerVictory(_context, this);
        _states[PlayerStates.defeated]  = new PlayerDefeated(_context, this);
        //_states[PlayerStates.floated]     = new PlayerFloatState(_context, this);
    }

    public PlayerBaseState Idle() {
        return _states[PlayerStates.idle];
    }
    public PlayerBaseState Walk() {
        return _states[PlayerStates.walk];
    }
    public PlayerBaseState Run() {
        return _states[PlayerStates.run];
    }
    public PlayerBaseState Jump() {
        return _states[PlayerStates.jump];
    }
    public PlayerBaseState Grounded() {
        return _states[PlayerStates.grounded];
    }
    public PlayerBaseState Fall() {
        return _states[PlayerStates.fall];
    }
    public PlayerBaseState Crouched() {
        return _states[PlayerStates.crouched];
    }
    public PlayerBaseState Backflip() {
        return _states[PlayerStates.backflip];
    }
    public PlayerBaseState Longjump() {
        return _states[PlayerStates.longjump];
    }
    public PlayerBaseState Jumpsub() {
        return _states[PlayerStates.jumpsub];
    }
    public PlayerBaseState Victory()
    {
        return _states[PlayerStates.victory];
    }
    public PlayerBaseState Defeated()
    {
        return _states[PlayerStates.defeated];
    }

    //public PlayerBaseState Float()
    //{
    //    return _states[PlayerStates.floated];
    //}
}

