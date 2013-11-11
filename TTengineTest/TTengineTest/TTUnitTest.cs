using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

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
    /// This is the main type for your game
    /// </summary>
    public class TTUnitTest : TTGame
    {
        public UnitTestsFactory Factory;
        Channel titleChannel, gameChannel;

        public TTUnitTest()
        {
            GraphicsMgr.IsFullScreen = false;
            GraphicsMgr.PreferredBackBufferWidth = 1024; 
            GraphicsMgr.PreferredBackBufferHeight = 768;
            IsMusicEngine = false;
        }

        protected override void Initialize()
        {
            Factory = UnitTestsFactory.Instance;
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

}
