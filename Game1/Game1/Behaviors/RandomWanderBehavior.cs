using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TTengine.Core;
using TTengine.Util;
using TreeSharp;
using Game1.Comps;

namespace Game1.Behaviors
{
    public class RandomWanderBehavior: TreeNode
    {

        public override IEnumerable<RunStatus> Execute(object context)
        {
            BTAIContext ctx = context as BTAIContext;
            // enable the random wander comp. 
            ctx.Entity.GetComponent<ThingComp>().TargetMove = ctx.Entity.GetComponent<RandomWanderComp>().TargetMove;
            yield return RunStatus.Success;
        }

    }    
}
