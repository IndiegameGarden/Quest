using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TTengine.Core;
using TTengine.Util;

namespace Game1.Behaviors
{
    public class RandomWanderBehavior: ThingControl
    {
        /**
         * current random direction of wandering - may be changed by an external object
         */
        public Vector2 CurrentDirection = Vector2.Zero;

        /**
         * the random interval during which the direction changes at a random time. Can be
         * tweaked during operation.
         */
        public float MaxDirectionChangeTime, MinDirectionChangeTime;

        public RandomWanderBehavior(float minDirectionChangeTime, float maxDirectionChangeTime)
        {
            this.MinDirectionChangeTime = minDirectionChangeTime;
            this.MaxDirectionChangeTime = maxDirectionChangeTime;
        }

        float dirChangeTime = 0f;
        float timeSinceLastChange = 0f;

        protected override void OnNextMove()
        {
            base.OnNextMove();

            Vector2 dir = CurrentDirection;
            if (dir.Length() < 0.1f)
                dir = Vector2.Zero;
            else
            {
                // TODO choose direction here or constrain later (in VelocityComp)
                // choose one direction randomly, if diagonals would be required
                if (dir.X != 0f && dir.Y != 0f)
                {
                    float r = RandomMath.RandomUnit();
                    if (r > 0.5f)
                        dir.X = 0f;
                    else
                        dir.Y = 0f;
                }
                dir.Normalize();
            }
            TargetMove = dir;
            IsTargetMoveDefined = true;

            // direction changing after some random time
            if (timeSinceLastChange >= dirChangeTime)
            {
                timeSinceLastChange = 0f;
                dirChangeTime = RandomMath.RandomBetween(MinDirectionChangeTime, MaxDirectionChangeTime);
                CurrentDirection = RandomMath.RandomDirection();
            }

        }
    }    
}
