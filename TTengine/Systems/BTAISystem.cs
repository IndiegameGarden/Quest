using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;
using Artemis.System;
using Artemis.Attributes;
using Artemis.Manager;
using TTengine.Core;
using TTengine.Comps;
using TreeSharp;

namespace TTengine.Systems
{
    /// <summary>
    /// A Behavior Tree (BT) based AI system for Entities
    /// </summary>
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = 2)]
    public class BTAISystem : EntityComponentProcessingSystem<BTAIComp>
    {
        private BTAIContext ctx = new BTAIContext();

        protected override void Begin()
        {
            // once per update-cycle, set timing in updParams
            ctx.Dt = TimeSpan.FromTicks(this.EntityWorld.Delta).TotalSeconds;
            ctx.SimTime += ctx.Dt;
        }

        public override void Process(Entity entity, BTAIComp btComp)
        {
            ctx.Entity = entity;

            if (btComp.rootNode.LastStatus == null)
                btComp.rootNode.Start(ctx);
            else if (btComp.rootNode.LastStatus == RunStatus.Success)
                btComp.rootNode.Start(ctx);
            btComp.rootNode.Tick(ctx);

        }

    }
}
