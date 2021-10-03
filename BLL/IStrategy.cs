namespace BLL
{
    public interface IStrategy
    {
        object DoLogic(params object[] data);
    }
}