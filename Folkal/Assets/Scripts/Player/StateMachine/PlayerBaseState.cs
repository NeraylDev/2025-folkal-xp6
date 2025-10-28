public abstract class PlayerBaseState
{
    private PlayerManager _playerManager;
    private PlayerStateMachine _playerStateMachine;

    protected PlayerManager GetPlayerManager => _playerManager;
    protected PlayerStateMachine GetPlayerStateMachine => _playerStateMachine;


    public PlayerBaseState(PlayerStateMachine playerStateMachine, PlayerManager playerManager)
    {
        _playerManager = playerManager;
        _playerStateMachine = playerStateMachine;
    }

    public virtual void Enter() { }
    public virtual void Execute() { }
    public virtual void FixedExecute() { }
    public virtual void Exit() { }
}
