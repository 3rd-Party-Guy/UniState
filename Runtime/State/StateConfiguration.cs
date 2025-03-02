using System;
using System.Collections.Generic;

namespace UniState {
    public class StateConfiguration<TState, TTrigger> {
        List<Func<object>> OnEntryFunctions = new();
        List<Func<object>> OnExitFunctions = new();

        Dictionary<TTrigger, TState> DefinedTransitions = new();
        
    }
}
