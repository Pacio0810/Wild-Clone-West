
public abstract class State
{
    protected StateMachine fsm;

    public virtual void OnEnter() { }

    public virtual void OnExit() { }

    public abstract void Update();

    public void SetStateMachine(StateMachine sm)
    {
        fsm = sm;
    }
}

public enum StateType
{
    PATROL,
    CHASE,
    ATTACK
}
