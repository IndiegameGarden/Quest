using System;
using System.Collections.Generic;
using TTengine.Core;
using Microsoft.Xna.Framework;
using TreeSharp;
using Game1.Comps;

namespace Game1.Behaviors
{
    /**
     * always go forward and turn right when I can
     */
    public class AlwaysTurnRightBehavior: TreeNode
    {
        /// <summary>
        /// current direction of motion (e.g. along wall turning right). May be modified on the fly for sazzy effect.
        /// </summary>
        public Vector2 CurrentDirection = new Vector2(1f, 0f);

        // keep track of wall last seen
        protected bool didSeeWall = false;

        public override IEnumerable<RunStatus> Execute(object context)
        {
            var ctx = context as BTAIContext;
            var tc = ctx.Entity.GetComponent<ThingComp>();
            var tcc = ctx.Entity.GetComponent<ThingControlComp>();

            Vector2 rightHandDirection = RotateVector2(CurrentDirection, MathHelper.PiOver2);
            Vector2 leftHandDirection = RotateVector2(CurrentDirection, -MathHelper.PiOver2);
            bool isRightHandFree = !tc.CollidesWithSomething(rightHandDirection);
            bool isLeftHandFree = !tc.CollidesWithSomething(leftHandDirection);
            bool isFrontFree = !tc.CollidesWithSomething(CurrentDirection);
            bool isSuccess = false;

            // change direction to righthand if that's free
            if (didSeeWall && isRightHandFree)
            {
                CurrentDirection = rightHandDirection;
                didSeeWall = false;
                tcc.TargetMove = CurrentDirection;
                isSuccess = true;
            }

            else if (!isFrontFree)
            {
                // turn left if the way is blocked
                CurrentDirection = leftHandDirection;
                didSeeWall = true;
            }
            else if (didSeeWall || !isRightHandFree || !isLeftHandFree || !isFrontFree)
            {
                tcc.TargetMove = CurrentDirection;
                isSuccess = true;
            }

            if (!isRightHandFree)
                didSeeWall = true;

            if (isSuccess)
            {
                tcc.MoveTimeDelta = this.MoveTimeDelta;
                yield return RunStatus.Success;
            }
            else
                yield return RunStatus.Failure;
        }

        public static Vector2 RotateVector2(Vector2 point, float radians)
        {
            float cosRadians = (float)Math.Cos(radians);
            float sinRadians = (float)Math.Sin(radians);

            return new Vector2(
                point.X * cosRadians - point.Y * sinRadians,
                point.X * sinRadians + point.Y * cosRadians);
        }

    }
}
