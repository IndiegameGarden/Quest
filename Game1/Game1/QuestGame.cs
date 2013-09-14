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
using TTengine.Util;

using Artemis;
using TreeSharp;

using Game1.Core;

namespace Game1
{
    /// <summary>
    /// Main game class, using TTGame template
    /// </summary>
    public class QuestGame : TTGame
    {
        public GameFactory Factory;
        Channel titleChannel, gameChannel;

        public QuestGame()
        {
            GraphicsMgr.IsFullScreen = false;
            GraphicsMgr.PreferredBackBufferWidth = 1024; 
            GraphicsMgr.PreferredBackBufferHeight = 768;
            IsMusicEngine = false;
        }

        protected override void Initialize()
        {
            Factory = GameFactory.Instance;
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

            // game channel
            gameChannel = ChannelMgr.CreateChannel();
            ChannelMgr.ZapTo(gameChannel); 
            gameChannel.Screen.GetComponent<ScreenComp>().BackgroundColor = Color.White;

            // add framerate counter
            FrameRateCounter.Create(Color.Black);

            // FIXME create level

        }       

    }
}
