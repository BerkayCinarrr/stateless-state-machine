namespace stateless_state_machine
{
    public class Car
    {
        public enum State
        {
            Stopped,
            Started,
            Running
        }
        public enum Action
        {
            Start,
            Stop,
            Move
        }
        private State state = State.Stopped;
        public State currentState
        {
            get { return state; }
        }

        public void TakeAction(Action action)
        {
            state = (state, action) switch
            {
                (State.Stopped, Action.Start) => State.Started,
                (State.Started, Action.Stop) => State.Stopped,
                (State.Started, Action.Move) => State.Running,
                (State.Running, Action.Stop) => State.Stopped,
                _ => state
            };
        }
    }

}
