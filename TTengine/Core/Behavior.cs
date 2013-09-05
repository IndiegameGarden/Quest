﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Artemis;
using Artemis.Interface;

namespace TTengine.Core
{
    /// <summary>An atomic piece of AI script defining a specific aspect of Entity behavior</summary>
    public class Behavior: IComponent
    {
        public Behavior()
        {
        }

        /// <summary>Flag indicating whether the Behavior is 'active', i.e. 'triggers'. May change dynamically 
        /// depending on conditions calculated by the specific Behavior during OnUpdate().</summary>
        public virtual bool IsActive {get; set;}

        /// <summary>called every update cycle of the BTAISystem</summary>
        /// <param name="ctx">Informative update parameters that may be used by the Behavior</param>
        public virtual void OnUpdate(BTAIContext p)
        {
        }

        /// <summary>Called when the BTAISystem selects this Behavior for execution.</summary>
        public virtual void OnExecute(BTAIContext p)
        {
        }

    }
}
