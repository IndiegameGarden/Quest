using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis.Interface;
using Microsoft.Xna.Framework;

namespace Game1.Comps
{
    public class ThingComp: IComponent
    {
        public Vector2 TargetMove = Vector2.Zero;
    }
}
