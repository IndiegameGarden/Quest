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
    /// A script or behavior that controls a ThingComp, a TreeNode part of a TreeSharp BT AI tree structure
    /// </summary>
    public abstract class ThingControlComp: Comp
    {
        /// <summary>
        /// Every update cycle the move produced by this control, if any, is written to TargetMove
        /// </summary>
        public Vector2 TargetMove = new Vector2();

        public Vector2 TargetMoveMultiplier = new Vector2(1f, 1f);

        public ThingControl()
        {
        }

    }
}
