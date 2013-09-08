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
    public class ThingControl: TreeNode
    {
        /// <summary>
        /// time between moves (in seconds)
        /// </summary>
        public double MoveDeltaTime = 0.2;

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

        public ThingControl()
        {
        }

        public override IEnumerable<RunStatus> Execute(object context)
        {
            BTAIContext ctx = context as BTAIContext;            
            wTime -= ctx.Dt;

            if (wTime <= 0 )   // timer triggers
            {
                wTime += MoveDeltaTime;

                // do my move                
                TargetMove = Vector2.Zero;
                OnNextMove(ctx);
                if (IsTargetMoveDefined)
                {
                    ctx.Entity.GetComponent<VelocityComp>().SteeringDirection = TargetMove;
                    yield return RunStatus.Success;
                }
                else
                    yield return RunStatus.Failure;
            }

            yield return (RunStatus) LastStatus;
        }
        
        /// <summary>
        /// Custom code should override this method. It will (in case of a move)
        /// set TargetMove and IsTargetMoveDefined := true.
        /// </summary>
        /// <param name="ctx">BTAIContext object passed from TreeSharp engine.</param>
        protected virtual void OnNextMove(BTAIContext ctx)
        {
        }

    }
}
