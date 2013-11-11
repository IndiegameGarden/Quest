using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace TTengineTest
{
    /// <summary>Testing the linear motion of objects on screen</summary>
    class TestLinearMotion : Test
    {
        public TestLinearMotion()
            : base()
        { }

        public override void Create()
        {
            for (float x = 10f; x < 1024f; x += 100f)
            {
                for (float y = 10f; y < 768f; y += 100f)
                {
                    Factory.CreateMovingBall(new Vector2(x, y));
                }
            }
        }

    }
}
