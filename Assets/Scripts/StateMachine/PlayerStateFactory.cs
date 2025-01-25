using System.Collections.Generic;

public enum PlayerStates
{
    Idle,
    Walk,
    Run,
    Grounded,
    Jump,
    Fall
}

public class PlayerStateFactory
{
    private PlayerStateMachine m_context;
    private Dictionary<PlayerStates, PlayerBaseState> m_states = new Dictionary<PlayerStates, PlayerBaseState>();

    public PlayerStateFactory(PlayerStateMachine context)
    {
        m_context = context;
        m_states[PlayerStates.Idle] = new PlayerIdleState(m_context, this);
        m_states[PlayerStates.Walk] = new PlayerWalkState(m_context, this);
        m_states[PlayerStates.Run] = new PlayerRunState(m_context, this);
        m_states[PlayerStates.Jump] = new PlayerJumpState(m_context, this);
        m_states[PlayerStates.Grounded] = new PlayerGroundedState(m_context, this);
        m_states[PlayerStates.Fall] = new PlayerFallState(m_context, this);
    }
    public PlayerBaseState Idle() => m_states[PlayerStates.Idle];

    public PlayerBaseState Walk() => m_states[PlayerStates.Walk];

    public PlayerBaseState Run() => m_states[PlayerStates.Run];

    public PlayerBaseState Jump() => m_states[PlayerStates.Jump];

    public PlayerBaseState Grounded() => m_states[PlayerStates.Grounded];

    public PlayerBaseState Fall() => m_states[PlayerStates.Fall];
}
