using System;
using System.Collections.Generic;
using TTengine.Core;
using Microsoft.Xna.Framework;
using Game1.Actors;
using Game1.Comps;
using TTengine.Util;

namespace Game1.Behaviors
{
    /// <summary>
    /// Attack any Thing that is in another faction than mine.
    /// </summary>
    public class AttackEnemyBehavior: ThingControl
    {
        public bool IsAttacking = false;

        public string[] AttackStrings;

        public AttackEnemyBehavior(string[] attackStrings)
        {
            this.AttackStrings = attackStrings;
        }

        protected override void OnNextMove(BTAIContext ctx) 
        {
            ThingComp tc = ctx.Entity.GetComponent<ThingComp>();

            // only attack if not blocked there.
            Vector2 dir = tc.FacingDirection;
            List<ThingComp> col = tc.DetectCollisions(tc.FacingDirection);
            foreach(var t in col) 
            {
                if (t.Faction != tc.Faction)
                {
                    IsAttacking = true;
                    TargetMove = dir;
                    IsTargetMoveDefined = true;

                    // TODO color set! and health decrease values parameterize.
                    Level.Current.Subtitles.Show(3, AttackStrings[RandomMath.RandomIntBetween(0, AttackStrings.Length - 1)], 3.5f, Color.IndianRed);
                    t.Health -= RandomMath.RandomBetween(1f, 3f);
                }
            }
        }

    }
}
