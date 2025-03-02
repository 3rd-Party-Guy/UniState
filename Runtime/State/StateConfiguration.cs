using System;
using System.Collections.Generic;

namespace UniState {
    public class StateConfiguration<TState, TTrigger> {
        public IEnumerable<Func<object>> EntryFunctions => onEntryFunctions;
        public IEnumerable<Func<object>> ExitFunctions => onExitFunctions;
        
        public IReadOnlyDictionary<TTrigger, TState> DefinedTransitions => definedTransitions;
        public IEnumerable<TTrigger> IgnoredTriggers => ignoredTriggers;

        readonly List<Func<object>> onEntryFunctions = new();
        readonly List<Func<object>> onExitFunctions = new();

        readonly Dictionary<TTrigger, TState> definedTransitions = new();
        readonly List<TTrigger> ignoredTriggers = new();

        public StateConfiguration<TState, TTrigger> Permit(TTrigger trigger, TState state) {
            definedTransitions[trigger] = state;
            return this;
        }

        public StateConfiguration<TState, TTrigger> Ignore(TTrigger trigger) {
            ignoredTriggers.Add(trigger);
            return this;
        }

        public StateConfiguration<TState, TTrigger> OnEntry(Func<object> action) {
            onEntryFunctions.Add(action);
            return this;
        }

        public StateConfiguration<TState, TTrigger> OnExit(Func<object> action) {
            onExitFunctions.Add(action);
            return this;
        }
    }
}
