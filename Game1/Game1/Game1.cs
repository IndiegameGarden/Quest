// (c) 2010-2013 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TTengine.Core;
using TTengine.Comps;
using TTengine.Behaviors;
using TTengine.Modifiers;
using TTengine.Util;

using Artemis;
using Artemis.Interface;
using TreeSharp;

namespace Game1
{
    /// <summary>
    /// Main game class, using TTGame template
    /// </summary>
    public class Game1 : TTGame
    {
        public Game1Factory Factory;
        Channel titleChannel, gameChannel;

        public Game1()
        {
            GraphicsMgr.IsFullScreen = false;
            GraphicsMgr.PreferredBackBufferWidth = 1024; 
            GraphicsMgr.PreferredBackBufferHeight = 768;
            IsMusicEngine = false;
        }

        protected override void Initialize()
        {
            Factory = Game1Factory.Instance;
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            KeyboardState kb = Keyboard.GetState();
            if (kb.IsKeyDown(Keys.Space))
            {
                ChannelMgr.ZapTo(titleChannel);
            }
            else
            {
                ChannelMgr.ZapTo(gameChannel);
            }
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            // title channel
            titleChannel = ChannelMgr.CreateChannel();
            ChannelMgr.ZapTo(titleChannel); // TODO function to create on it without seeing it.
            titleChannel.Screen.GetComponent<ScreenComp>().BackgroundColor = Color.Black;

            // add framerate counter
            FrameRateCounter.Create(Color.White);

            var t = Factory.CreateMovingTextlet(new Vector2(0.5f, 0.5f), "Title Screen");
            t.GetComponent<DrawComp>().DrawColor = Color.LightGoldenrodYellow;
            t.GetComponent<ScaleComp>().Scale = 4;


            // game channel
            gameChannel = ChannelMgr.CreateChannel();
            ChannelMgr.ZapTo(gameChannel); 
            gameChannel.Screen.GetComponent<ScreenComp>().BackgroundColor = Color.White;

            // add framerate counter
            FrameRateCounter.Create(Color.Black);

            // add several sprites             
            for (float x = 0.1f; x < 1.6f; x += 0.1f)
            {
                for (float y = 0.1f; y < 1f; y += 0.1f)
                {
                    Factory.CreateHyperActiveBall(new Vector2(x,y));
                    Factory.CreateMovingTextlet(new Vector2(x,y),"This is the\nTTengine test. !@#$1234");
                    //break;
                }
                //break;
            }
        }       

    }

    /// <summary>
    /// Factory to create new game-specific entities
    /// </summary>
    public class Game1Factory
    {
        private static Game1Factory _instance = null;
        private Game1 _game;

        private Game1Factory(Game1 game)
        {
            _game = game;
        }

        public static Game1Factory Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Game1Factory(TTGame.Instance as Game1);
                return _instance as Game1Factory;
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
        public Entity CreateHyperActiveBall(Vector2 pos)
        {
            var ball = CreateBall(0.08f + 0.07f * (float)rnd.NextDouble());

            // position and velocity set
            ball.GetComponent<PositionComp>().Position = pos;
            ball.GetComponent<VelocityComp>().Velocity = 0.2f * new Vector2((float)rnd.NextDouble() - 0.5f, (float)rnd.NextDouble() - 0.5f);
            //ball.Motion.Rotate = (float)(Math.PI * 2 * rnd.NextDouble());                    
            //ball.Timing.StartTime = 10f * (float)rnd.NextDouble();

            // duration of entity
            ball.AddComponent(new ExpiresComp(4 + 500 * rnd.NextDouble()));

            // blink                    
            //ball.AddComponent(new BlinkComp(0.3+5*rnd.NextDouble(),0.4+0.4*rnd.NextDouble()));

            // Behavior Tree AI
            BTAIComp ai = new BTAIComp();
            var randomWanderBehavior = new RandomWanderBehavior(1, 6);
            ai.rootNode = new PrioritySelector(randomWanderBehavior);
            ball.AddComponent(ai);

            // Modifier to adapt scale
            var m = new Modifier(MyScaleModifier);
            //delegate(Entity entity){ entity.GetComponent<ScaleComp>().Scale = 0.5 + entity.GetComponent<PositionComp>().Position.X; }
            //);
            m.AttachTo(ball);

            // another adapting scale with sine rhythm
            var s = new SineModifier(MyScaleModifier2);
            s.Frequency = 0.5;
            s.Amplitude = 0.25;
            s.Offset = 1;
            s.AttachTo(ball);

            // modifier to adapt rotation
            var r = new Modifier(MyRotateModifier, ball.GetComponent<DrawComp>());
            r.AttachTo(ball);

            // set different time offset initially, per ball (for the modifiers)
            ball.GetComponent<ModifierComp>().SimTime = 10 * rnd.NextDouble();

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

        public void MyScaleModifier(Entity entity, double value)
        {
            entity.GetComponent<ScaleComp>().ScaleModifier *= 0.5 + entity.GetComponent<PositionComp>().Position.X;
        }

        public void MyScaleModifier2(Entity entity, double value)
        {
            entity.GetComponent<ScaleComp>().ScaleModifier *= value;
        }

        public void MyRotateModifier(IComponent drawComp, double value)
        {
            ((DrawComp)drawComp).DrawRotation = (float)value;
        }
    }

}
