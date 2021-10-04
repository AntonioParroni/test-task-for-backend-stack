namespace Infrastructure
{
    public interface IStrategy
    {
        object DoLogic(params object[] data);
    }
}