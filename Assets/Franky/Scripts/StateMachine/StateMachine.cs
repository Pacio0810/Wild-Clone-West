using System.Collections.Generic;

public class StateMachine
{
    private Dictionary<StateType, State> states;
    private State currentState;

    public StateMachine()
    {
        states = new Dictionary<StateType, State>();
        currentState = null;
    }

    public void AddState(StateType key, State value)
    {
        states[key] = value;
        value.SetStateMachine(this);
    }

    public void GoTo(StateType key)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }

        currentState = states[key];
        currentState.OnEnter();
    }

    public void Update()
    {
        currentState.Update();
    }
}
