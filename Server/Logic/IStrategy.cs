namespace Server.Logic
{
    public interface IStrategy
    {
        object DoLogic(params object[] data);
    }
}