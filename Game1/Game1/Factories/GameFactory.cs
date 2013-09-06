using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Artemis;
using Artemis.Interface;
using TreeSharp;
using TTengine.Core;
using TTengine.Comps;
using TTengine.Behaviors;
using TTengine.Modifiers;

namespace Game1.Factories
{
    /// <summary>
    /// Factory to create new game-specific entities
    /// </summary>
    public class GameFactory
    {
        private static GameFactory _instance = null;
        private Game1 _game;

        private GameFactory(Game1 game)
        {
            _game = game;
        }

        public static GameFactory Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameFactory(TTGame.Instance as Game1);
                return _instance as GameFactory;
            }
        }

        protected Random rnd = new Random();

        public Entity CreateThing(string bitmapFile)
        {
            Entity t = TTFactory.CreateSpritelet(bitmapFile);
            Thing tc = new Thing();
            SpriteComp sc = t.GetComponent<SpriteComp>();

            tc.PassableIntensityThreshold = Level.Current.DefaultPassableIntensityThreshold;
            tc.SetBoundingRectangleWidthHeight(sc.Texture.Width, sc.Texture.Height);
            var textureData = new Color[BoundingRectangle.Width * BoundingRectangle.Height];
            sc.Texture.GetData(textureData);
            sc.Center = Vector2.Zero;

            t.AddComponent(tc);

            Pushing = new PushBehavior();
            Add(Pushing);     

        }
    }
}
