using System;
using TTengine.Core;
using TTengine.Comps;
using TTengine.Util;
using Microsoft.Xna.Framework;
using Game1;
using Game1.Comps;
using Game1.Behaviors;
using TreeSharp;
using Artemis;
using Artemis.Interface;
using Game1.Factories;

namespace Game1.Actors
{
    public class RedGuard
    {
        protected static string[] attackString = new string[] { "Take this, golden villain!", "We hurt him!", "He bleeds!", "Our swords struck true!",
            "He bleeds!", "To the grave, golden traitor!", "Die, golden scum!" , "He stumbles!"};

        /// <summary>
        /// Factory method to create new Red Guard
        /// </summary>
        /// <returns></returns>
        public static Entity Create()
        {
            ChaseBehavior  ChasingHero;
            ChaseBehavior ChasingCompanions;
            AlwaysTurnRightBehavior Turning;
            RandomWanderBehavior Wandering;
            AttackEnemyBehavior Attacking;

            var e = GameFactory.CreateThing(true);
            var ai = new BTAIComp();
            e.AddComponent(ai);
            var sub = new PrioritySelector();
            ai.rootNode = sub;

            var tc = e.GetComponent<ThingComp>();
            tc.IsCollisionFree = false;
            tc.Color = new Color(255, 10, 4);
            tc.Faction = Faction.EVIL;

            // attack hero or companions
            Attacking = new AttackEnemyBehavior(attackString);

            // chase companions that are very close
            /*
            ChasingCompanions = new ChaseBehavior(typeof(Companion));
            ChasingCompanions.MoveDeltaTime = RandomMath.RandomBetween(0.43f, 0.65f);
            ChasingCompanions.ChaseRange = 1.5f; // RandomMath.RandomBetween(12f, 40f);
            sub.AddChild(ChasingCompanions);
            */

            // chase hero
            ChasingHero = new ChaseBehavior(Level.Current.pixie);
            ChasingHero.MoveDeltaTime = RandomMath.RandomBetween(0.47f, 0.75f);
            ChasingHero.ChaseRange = 15f; // RandomMath.RandomBetween(12f, 40f);
            sub.AddChild(ChasingHero);

            Turning = new AlwaysTurnRightBehavior(); // patrolling
            Turning.MoveDeltaTime = ChasingHero.MoveDeltaTime; //RandomMath.RandomBetween(0.57f, 1.05f);
            Turning.MoveDeltaTime = 0.7f;
            sub.AddChild(Turning);

            Wandering = new RandomWanderBehavior(2.7f, 11.3f);
            Wandering.MoveDeltaTime = 0.7f;
            sub.AddChild(Wandering);

            e.Refresh();
            return e;
        }

        protected void OnUpdate(ScriptContext p)
        {
            if (TargetMove.LengthSquared() > 0)
            {
                if (CollidesWhenThisMoves(Level.Current.pixie, TargetMove))
                {
                    if (Level.Current.Subtitles.Children.Count <= 4)
                    {
                        Level.Current.Subtitles.Show(3, attackString[RandomMath.RandomIntBetween(0, attackString.Length - 1)], 3.5f, Color.IndianRed);
                        Level.Current.pixie.Health -= RandomMath.RandomBetween(1f, 3f);
                    }
                }
            }
        }
    }
}
