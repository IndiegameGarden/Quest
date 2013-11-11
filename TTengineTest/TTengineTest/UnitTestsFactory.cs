using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

using TTengine.Core;
using TTengine.Comps;
using TTengine.Behaviors;
using TTengine.Modifiers;
using TTengine.Util;

using Artemis;
using Artemis.Interface;
using TreeSharp;

namespace TTengineTest
{
    /// <summary>
    /// Factory to create new game-specific entities
    /// </summary>
    public class UnitTestsFactory
    {
        private static UnitTestsFactory _instance = null;
        private TTUnitTest _game;

        private UnitTestsFactory(TTUnitTest game)
        {
            _game = game;
        }

        public static UnitTestsFactory Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UnitTestsFactory(TTGame.Instance as TTUnitTest);
                return _instance as UnitTestsFactory;
            }
        }

        protected Random rnd = new Random();

        /// <summary>
        /// create a ball Spritelet that can be scaled
        /// </summary>
        /// <param name="radius">the relative size scaling, 1 is normal</param>
        /// <returns></returns>
        public Entity CreateBall(double radius)
        {
            Entity e = TTFactory.CreateSpritelet("red-circle_frank-tschakert");
            e.AddComponent(new ScaleComp(radius));
            return e;
        }

        /// <summary>
        /// create an active ball with given position and random velocity and some weird (AI) behaviors
        /// </summary>
        /// <returns></returns>
        public Entity CreateMovingBall(Vector2 pos, Vector2 velo)
        {
            var ball = CreateBall(0.96f + 0.08f * (float)rnd.NextDouble());

            // position and velocity set
            ball.GetComponent<PositionComp>().Position = pos;
            ball.GetComponent<VelocityComp>().Velocity = velo;
            ball.Refresh();
            return ball;

        }

        public Entity CreateTextlet(Vector2 pos, string text, Color col)
        {
            var t = TTFactory.CreateTextlet(text);
            t.GetComponent<PositionComp>().Position = pos;
            t.GetComponent<DrawComp>().DrawColor = col;
            t.GetComponent<ScaleComp>().Scale = 0.8;
            return t;
        }

    }

}
