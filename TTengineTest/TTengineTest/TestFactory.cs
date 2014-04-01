﻿using System;
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
    public class TestFactory
    {
        private static TestFactory _instance = null;
        private TestGame _game;

        private TestFactory(TestGame game)
        {
            _game = game;
        }

        public static TestFactory Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new TestFactory(TTGame.Instance as TestGame);
                return _instance as TestFactory;
            }
        }

        protected Random rnd = new Random();

        public string BallSprite = "red-circle_frank-tschakert";

        /// <summary>
        /// create a ball Spritelet that can be scaled
        /// </summary>
        /// <param name="radius">the relative size scaling, 1 is normal</param>
        /// <returns></returns>
        public Entity CreateBall(double radius)
        {
            Entity e = TTFactory.CreateSpritelet(this.BallSprite);
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
            ball.GetComponent<PositionComp>().Position2D = pos;
            ball.GetComponent<PositionComp>().Z = 0.5f + 0.1f * ((float)rnd.NextDouble()); // random Z position
            ball.GetComponent<VelocityComp>().Velocity2D = velo;
            ball.Refresh();
            return ball;

        }

        public Entity CreateTextlet(Vector2 pos, string text, Color col)
        {
            var txt = TTFactory.CreateTextlet(text);
            txt.GetComponent<PositionComp>().Position2D = pos;
            txt.GetComponent<PositionComp>().Z = 0f + 0.1f * ((float)rnd.NextDouble()); // random Z position
            txt.GetComponent<DrawComp>().DrawColor = col;
            txt.GetComponent<ScaleComp>().Scale = 0.8;
            return txt;
        }

    }

}
