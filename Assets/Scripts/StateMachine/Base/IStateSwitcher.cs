
public interface IStateSwitcher
{
    public void SwitchState<State>() where State : IState;
}
