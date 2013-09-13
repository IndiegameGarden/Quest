using System;
using System.Collections.Generic;
using TreeSharp;

namespace Game1.Core
{
    public class Behavior: TreeNode
    {
        /// <summary>The time (seconds) between two successive moves for this Behavior</summary>
        public double MoveDeltaTime = 0.2;
    }
}
