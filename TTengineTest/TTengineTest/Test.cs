using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTengineTest
{
    public abstract class Test
    {
        protected UnitTestsFactory Factory;

        public void Initialize(UnitTestsFactory factory)
        {
            this.Factory = factory;
        }

        public abstract void Create();

    }
}
