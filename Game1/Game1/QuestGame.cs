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
using Game1.Levels;

namespace Game1
{
    /// <summary>
    /// Main game class, using TTGame template
    /// </summary>
    public class QuestGame : TTGame
    {
        Channel gameChannel;
        Level level;

        public QuestGame()
        {
            GraphicsMgr.IsFullScreen = false;
            GraphicsMgr.PreferredBackBufferWidth = 1024; 
            GraphicsMgr.PreferredBackBufferHeight = 768;
            IsMusicEngine = false;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            // game channel
            gameChannel = ChannelMgr.CreateChannel();
            ChannelMgr.ZapTo(gameChannel); 
            gameChannel.Screen.GetComponent<ScreenComp>().BackgroundColor = Color.Black;

            // add framerate counter
            FrameRateCounter.Create(Color.White);

            // create level
            level = new QuestLevel();
            Level.SetCurrentLevel(level);
            level.Init();

        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

    }
}
