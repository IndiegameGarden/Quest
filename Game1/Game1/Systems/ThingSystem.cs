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
    public class ThingControlSystem : EntityComponentProcessingSystem<ThingComp,ThingControlComp>
    {
        public override void Process(Entity entity, ThingComp tc, ThingControlComp tcc)
        {
            throw new NotImplementedException(); // here code to move a thing based on tcc
        }
    }
}
