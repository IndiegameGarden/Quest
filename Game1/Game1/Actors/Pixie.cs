using System;
using System.Collections.Generic;
using TTengine.Core;
using TTengine.Util;
using Microsoft.Xna.Framework;
using Game1;
using Game1.Behaviors;
using Game1.Comps;
using Artemis;

namespace Game1.Actors
{
    public class Pixie
    {

        public static Entity Create()
        {
            var e = GameFactory.CreateThing();
            var tc = e.GetComponent<ThingComp>();

            tc.IsCollisionFree = false;
            e.AddComponent(GameFactory.CreateColorCycling(4f, Color.DarkGoldenrod, new Color(230, 210, 10)));
            tc.Velocity = 1.5f;

            Pushing.Force = 10f; // force higher than companions.

        }

        /*
        public void LeadAttack()
        {
            foreach (Companion c in Companions)
                c.Attacking.TriggerAttack();
        }
         */

    }
}
