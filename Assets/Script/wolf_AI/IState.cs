public interface IState//状态机接口
{
    void OnEnter();
    
    void OnUpdate();

    void OnExit();
}
