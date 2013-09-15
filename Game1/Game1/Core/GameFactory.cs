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

namespace Game1.Core
{
    /// <summary>
    /// Factory to create new game-specific entities
    /// </summary>
    public class GameFactory
    {
        protected static Random rnd = new Random();

        public static Entity CreateThing(ThingType tp, bool hasControls, string bitmap)
        {
            var e = TTFactory.CreateSpritelet(bitmap);
            var tc = new ThingComp(tp);
            e.AddComponent(tc);

            SpriteComp sc = e.GetComponent<SpriteComp>();
            tc.PassableIntensityThreshold = Level.Current.DefaultPassableIntensityThreshold;
            tc.SetBoundingRectangleWidthHeight(sc.Texture.Width, sc.Texture.Height);
            var textureData = new Color[tc.BoundingRectangle.Width * tc.BoundingRectangle.Height];
            sc.Texture.GetData(textureData);
            sc.Center = Vector2.Zero;

            return e;
        }

        public static Entity CreateThing(ThingType tp, bool hasControls)
        {
            return CreateThing(tp,hasControls,"pixie");
        }

        public static ColorCycleComp CreateColorCycling(float cyclePeriod, Color minColor, Color maxColor)
        {
            ColorCycleComp cycl = new ColorCycleComp(cyclePeriod);
            cycl.minColor = minColor;
            cycl.maxColor = maxColor;
            return cycl;
        }

        public static Entity CreateSubtitle(string text, Color color)
        {
            var e = CreateSubtitle(new SubtitleText(text));
            e.GetComponent<DrawComp>().DrawColor = color;
            return e;
        }

        public static Entity CreateSubtitle(SubtitleText stComp)
        {
            var e = TTFactory.CreateDrawlet();
            e.AddComponent(stComp);
            e.Refresh();
            return e;
        }

    }
}
