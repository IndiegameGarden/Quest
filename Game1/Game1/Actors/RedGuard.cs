using System;
using TTengine.Core;
using TTengine.Comps;
using TTengine.Util;
using Microsoft.Xna.Framework;
using Game1;
using Game1.Behaviors;
using TreeSharp;
using Artemis;
using Game1.Factories;

namespace Game1.Actors
{
    public class RedGuard
    {
        // behaviors - the things that red guards do 
        public BlinkBehavior Blinking;
        public ChaseBehavior  Chasing;
        public ChaseBehavior ChasingComp;
        public AlwaysTurnRightBehavior Turning;
        public RandomWanderBehavior Wandering;

        protected string[] attackString = new string[] { "Take this, golden villain!", "We hurt him!", "He bleeds!", "Our swords struck true!",
            "He bleeds!", "To the grave, golden traitor!", "Die, golden scum!" , "He stumbles!"};

        public static Entity Create()
        {
            var e = GameFactory.CreateThing();
            var redGuard = e.GetComponent<ThingComp>();
            redGuard.IsCollisionFree = false;
            // FIXME make this spritecomp or ....
            e.GetComponent<DrawComp>().DrawColor = new Color(255, 10, 4);

            var ai = new BTAIComp();
            e.AddComponent(ai);
            var sub = new PrioritySelector();
            ai.rootNode = sub;

            // chase companions that are very close
            redGuard.ChasingComp = new ChaseBehavior(typeof(Companion));
            redGuard.ChasingComp.MoveSpeed = RandomMath.RandomBetween(0.43f, 0.65f);
            redGuard.ChasingComp.ChaseRange = 1.5f; // RandomMath.RandomBetween(12f, 40f);
            sub.AddChild(redGuard.ChasingComp);

            // chase hero
            redGuard.Chasing = new ChaseBehavior(chaseTarget);
            redGuard.Chasing.MoveSpeed = RandomMath.RandomBetween(0.47f, 0.75f);
            redGuard.Chasing.ChaseRange = 15f; // RandomMath.RandomBetween(12f, 40f);
            sub.AddChild(redGuard.Chasing);

            redGuard.Turning = new AlwaysTurnRightBehavior(); // patrolling
            redGuard.Turning.MoveSpeed = redGuard.Chasing.MoveSpeed; //RandomMath.RandomBetween(0.57f, 1.05f);
            redGuard.Turning.MoveSpeed = 0.7f;
            sub.AddChild(redGuard.Turning);

            redGuard.Wandering = new RandomWanderBehavior(redGuard,2.7f, 11.3f);
            redGuard.Wandering.MoveSpeed = 0.7f;
            sub.AddChild(redGuard.Wandering);

            return e;
        }

        protected override void OnUpdate(ScriptContext p)
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
