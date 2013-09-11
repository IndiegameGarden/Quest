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
using Game1.Comps;

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

        public static Entity CreateThing(bool hasControls, string bitmap)
        {
            var e = TTFactory.CreateSpritelet(bitmap);
            var thing = new ThingComp();
            e.AddComponent(thing);

            SpriteComp sc = e.GetComponent<SpriteComp>();

            thing.PassableIntensityThreshold = Level.Current.DefaultPassableIntensityThreshold;
            thing.SetBoundingRectangleWidthHeight(sc.Texture.Width, sc.Texture.Height);
            var textureData = new Color[BoundingRectangle.Width * BoundingRectangle.Height];
            sc.Texture.GetData(textureData);
            sc.Center = Vector2.Zero;

            thing.Pushing = new PushBehavior(thing);
            (e.GetComponent<BTAIComp>().rootNode as PrioritySelector).AddChild(thing.Pushing);

            return e;
        }

        public static Entity CreateThing(bool hasControls)
        {
            return CreateThing(hasControls,"pixie");
        }

        public static ColorCycleComp CreateColorCycling(float cyclePeriod, Color minColor, Color maxColor)
        {
            ColorCycleComp cycl = new ColorCycleComp(cyclePeriod);
            cycl.minColor = minColor;
            cycl.maxColor = maxColor;
            return cycl;
        }

    }
}
