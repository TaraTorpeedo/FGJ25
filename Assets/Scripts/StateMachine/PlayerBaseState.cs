public abstract class PlayerBaseState
{
    private bool m_isRootState = false;
    private PlayerStateMachine m_context;
    private PlayerStateFactory m_stateFactory;
    private PlayerBaseState m_curreSuperState;
    private PlayerBaseState m_currentSubState;

    protected bool IsRootState { set { m_isRootState = value; } }
    protected PlayerStateMachine Context => m_context;
    protected PlayerStateFactory StateFactory => m_stateFactory;

    public PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    {
        m_context = currentContext;
        m_stateFactory = playerStateFactory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();
    public abstract void InitializeSubState();

    public void UpdateStates() 
    {
        UpdateState();
        if(m_currentSubState != null)
        {
            m_currentSubState.UpdateStates();
        }
    }

    public void SwitchState(PlayerBaseState newState)
    {
        ExitState();

        newState.EnterState();

        if (m_isRootState)
        {
            m_context.CurrentState = newState;
        }
        else if(m_curreSuperState != null)
        {
            m_curreSuperState.SetSubState(newState);
        }
    }

    public void SetSuperState(PlayerBaseState newSuperState) => m_curreSuperState = newSuperState;

    public void SetSubState(PlayerBaseState newSubState) 
    {
        m_currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
}