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
            Entity e = TTFactory.CreateSpritelet("paul-hardman_circle-four");
            e.AddComponent(new ScaleComp(radius));
            return e;
        }

        /// <summary>
        /// create an active ball with given position and random velocity and some weird (AI) behaviors
        /// </summary>
        /// <returns></returns>
        public Entity CreateMovingBall(Vector2 pos)
        {
            var ball = CreateBall(0.08f + 0.03f * (float)rnd.NextDouble());

            // position and velocity set
            ball.GetComponent<PositionComp>().Position = pos;
            ball.GetComponent<VelocityComp>().Velocity = 0.2f * new Vector2((float)rnd.NextDouble() - 0.5f, (float)rnd.NextDouble() - 0.5f);

            // duration of entity
            ball.AddComponent(new ExpiresComp(4 + 500 * rnd.NextDouble()));

            ball.Refresh();
            return ball;

        }

        public Entity CreateMovingTextlet(Vector2 pos, string text)
        {
            var t = TTFactory.CreateTextlet(text);
            t.GetComponent<PositionComp>().Position = pos;
            t.GetComponent<DrawComp>().DrawColor = Color.Black;
            t.GetComponent<VelocityComp>().Velocity = 0.2f * new Vector2((float)rnd.NextDouble() - 0.5f, (float)rnd.NextDouble() - 0.5f);
            t.GetComponent<ScaleComp>().Scale = 0.5;
            return t;
        }

    }

}
