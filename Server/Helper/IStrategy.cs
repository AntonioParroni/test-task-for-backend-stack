namespace Server.Helper
{
    public interface IStrategy
    {
        object DoLogic(params object[] data);
    }
}