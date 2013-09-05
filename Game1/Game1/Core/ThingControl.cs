using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TTengine.Core;
using Microsoft.Xna.Framework;
using TTengine.Util;
using TreeSharp;

namespace Game1
{
    /**
     * A script or behavior that controls a Thing
     */
    public class ThingControl: TreeNode
    {
        public Thing ParentThing;

        /// <summary>
        /// relative speed of execution (1.0 is normal)
        /// </summary>
        public float MoveSpeed = 1.0f;

        /// <summary>
        /// flag indicating whether TargetMove and TargetMoveMultiplier are valid (true) for this round or undefined (false)
        /// </summary>
        public bool IsTargetMoveDefined = false;

        /// <summary>
        /// flag indicating whether a previously generated TargetMove is still active, or not (active until a next move calculation)
        /// </summary>
        public bool IsMoveActive = false;

        /// <summary>
        /// Every update cycle the move produced by this control, if any, is written to TargetMove
        /// </summary>
        public Vector2 TargetMove = new Vector2();
        public Vector2 TargetMoveMultiplier = new Vector2(1f, 1f);

        /// <summary>
        /// the TargetMove of the previous round
        /// </summary>
        public Vector2 LastTargetMove = new Vector2();

        // timer that fires when next move is to be computed
        protected double wTime = RandomMath.RandomBetween(0f,0.2f);

        public ThingControl(Thing parentThing)
        {
            this.ParentThing = parentThing;
        }

        public override IEnumerable<RunStatus> Execute(object context)
        {
            BTAIContext ctx = context as BTAIContext;
            TargetMove = Vector2.Zero;
            wTime -= ctx.Dt;

            if (wTime <= 0f )   // timer triggers
            {
                wTime += 0.2f / MoveSpeed;

                // do my move                
                OnNextMove();
                if (IsTargetMoveDefined)
                    yield return RunStatus.Success;
                else
                    yield return RunStatus.Failure;
            }

            yield return (RunStatus) LastStatus;
        }
        
        protected virtual void OnNextMove()
        {
        }

    }
}
