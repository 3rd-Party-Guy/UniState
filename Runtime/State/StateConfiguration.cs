using System;
using System.Collections.Generic;

namespace UniState
{
    public class StateConfiguration
    {
        List<Func<object>> OnEntryFunctions = new List<Func<object>>();
        List<Func<object>> OnExitFunctions = new List<Func<object>>();

        
    }
}
