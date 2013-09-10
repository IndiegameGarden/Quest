using System;
using System.Collections.Generic;
using TTengine.Core;
using TTengine.Comps;
using TTengine.Util;
using Microsoft.Xna.Framework;
using TreeSharp;
using Game1.Comps;

namespace Game1
{
    /// <summary>
    /// The ability to control moves of a ThingComp, either by user input or from AI
    /// </summary>
    public abstract class ThingControlComp: Comp
    {
        public Vector2 TargetMove = new Vector2();

        public Vector2 TargetMoveMultiplier = new Vector2(1f, 1f);

        public ThingControlComp()
        {
        }

    }
}
