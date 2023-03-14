using EventBusSystem;

public interface IBlocksChangeHandler : IGlobalSubscriber
{
    void HandleBlocksChange(int count);
}