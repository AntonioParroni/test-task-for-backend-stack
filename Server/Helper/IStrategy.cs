namespace Server.Helper
{
    public interface IStrategy
    {
        object DoAlgorithm(params object[] data);
    }
}