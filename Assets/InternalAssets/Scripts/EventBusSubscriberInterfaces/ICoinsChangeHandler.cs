using EventBusSystem;

public interface ICoinsChangeHandler : IGlobalSubscriber
{
    void HandleCoinsChange(int count);
}