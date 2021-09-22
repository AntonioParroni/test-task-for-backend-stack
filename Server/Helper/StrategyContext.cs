using System;
using System.Collections.Generic;

namespace Server.Helper
{
    class StrategyContext
    {
       
        private IStrategy _strategy;

        public StrategyContext()
        {
        }

        public StrategyContext(IStrategy strategy)
        {
            this._strategy = strategy;
        }

        public void SetStrategy(IStrategy strategy)
        {
            this._strategy = strategy;
        }

        public object DoSomeBusinessLogic(params object[] data)
        {
            var result = this._strategy.DoAlgorithm(data);
            return result;
        }
    }
}