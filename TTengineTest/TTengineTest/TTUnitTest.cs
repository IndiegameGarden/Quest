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
    /// Visual "unit" tests of various aspects of the TTengine. Press keys to cycle through tests.
    /// </summary>
    public class TTUnitTest : TTGame
    {
        public UnitTestsFactory Factory;
        KeyboardState kbOld = Keyboard.GetState();
        int channel = 0;
        List<Channel> channels = new List<Channel>();

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
            if (kb.IsKeyDown(Keys.Space) && !kbOld.IsKeyDown(Keys.Space))
            {
                channel++;
                if (channel >= channels.Count)
                    Exit();
                else
                    ChannelMgr.ZapTo(channels[channel]);
            }

            kbOld = kb;
        }

        private void DoTest(Test t)
        {
            var c = ChannelMgr.CreateChannel();
            c.Screen.GetComponent<ScreenComp>().BackgroundColor = t.BackgroundColor;

            channels.Add(c);
            t.Initialize(Factory);
            t.Create();

            // add framerate counter
            var col = TTutil.InvertColor(t.BackgroundColor);
            FrameRateCounter.Create(col);

            Factory.CreateTextlet(new Vector2(2f, 750f), t.Name, col);

        }

        protected override void LoadContent()
        {
            base.LoadContent();

            // Here all the tests are listed
            DoTest(new TestLinearMotion());


            ChannelMgr.ZapTo(channels[0]);

        }       

    }

}
