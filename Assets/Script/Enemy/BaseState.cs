
public abstract class BaseState<T> where T : Enemy 
{
    protected T currentEnemy;
    public abstract void OnEnter(T Enemy);
    public abstract void LogicUpdate();
    public abstract void PhysicUpdate();
    public abstract void OnExit();
    
}
