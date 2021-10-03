namespace BLL
{
    public class Strategy
    {
        private IStrategy _strategy;

        public Strategy()
        {
        }

        public Strategy(IStrategy strategy)
        {
            this._strategy = strategy;
        }

        public void SetStrategy(IStrategy strategy)
        {
            this._strategy = strategy;
        }

        public object Execute(params object[] data)
        {
            var result = this._strategy.DoLogic(data);
            return result;
        }
    }
}