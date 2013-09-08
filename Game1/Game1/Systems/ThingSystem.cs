using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;
using Artemis.System;
using Artemis.Manager;
using Artemis.Attributes;
using Game1.Comps;
using TTengine.Comps;

namespace Game1.Systems
{
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = 3)]
    public class ThingSystem : EntityComponentProcessingSystem<ThingComp,PositionComp>
    {
        public override void Process(Entity entity, ThingComp comp, PositionComp Motion)
        {
            // update position of the smooth motion of this ThingComp in the TTengine
            // update position when attached to a parent ThingComp
            if (comp.Parent != null)
            {
                comp.Target = comp.Parent.Target + comp.AttachmentPosition;
                Motion.Position = /*Scale.ScaleAbs * */ comp.FromPixels(comp.AttachmentPosition);
            }
            else
            {   // not attached to a parent ThingComp
                Motion.Position = Screen.Center + Motion.ScaleAbs * (FromPixels(Position - ViewPos)); // TODO ViewPos smoothing using Draw cache?
                //Motion.Position = Position - ViewPos; // alternative to above
            }

            // compute new facingDirection from final TargetMove
            if (comp.TargetMove.LengthSquared() > 0f)
            {
                comp.FacingDirection = comp.TargetMove;
                comp.FacingDirection.Normalize();
            }

            // take steering inputs if any, and move ThingComp, applying collision detection
            if (comp.TargetMove.LengthSquared() > 0f)
            {
                // check if passable...
                List<ThingComp> cols = DetectCollisions(TargetMove);

                if (!IsCollisionFree && Pushing != null && !IsCollisionFree && cols.Count > 0 && Pushing.Force > 0f)
                {
                    // no - so try to push neighbouring things away
                    foreach (ThingComp t in cols)
                    {
                        if (t.Pushing != null)
                            t.Pushing.BePushed(TargetMove);
                    }
                }

                if (IsCollisionFree || (!CollidesWithBackground(TargetMove) && cols.Count==0 ) )
                {
                	  // yes - passable
                    bool ok = true;
                    if (!IsCollisionFree)
                    {
                        // check all attached Things too                        
                        foreach (ThingComp g in Children)
                        {
                            if (g.Visible)
                            {
                                ThingComp t = g as ThingComp;
                                if (t.IsCollisionFree) continue;

                                // first, test if hits background
                                if (t.CollidesWithBackground(TargetMove))
                                {
                                    ok = false;
                                    break;
                                }

                                // if not, test if it hits others
                                List<ThingComp> colsChild = t.DetectCollisions(TargetMove);
                                if (colsChild.Count > 0)
                                {
                                    ok = false;
                                    break;
                                }
                            }
                        }
                    }
                    // if there are no objections of main ThingComp (or its attachment) to the move, then move.
                    if (ok)
                    {
                        Target += TargetMove;
                        TTutil.Round(ref Target);
                    }
                }
                
            }            

            Vector2 vdif = Target - Position;
            if (vdif.LengthSquared() > 0f) // if target not reached yet
            {
                Vector2 vmove = vdif;
                vmove.Normalize();
                vmove *= TargetSpeed * Velocity ;
                // convert speed vector to move vector (x = v * t)
                vmove *= p.Dt;
                // check if target reached already (i.e. move would overshoot target)
                if (vmove.LengthSquared() >= vdif.LengthSquared())
                {
                    Position = Target;
                }
                else
                {
                    // apply move towards target
                    Position += vmove;
                }
            }

        }
    }
}
