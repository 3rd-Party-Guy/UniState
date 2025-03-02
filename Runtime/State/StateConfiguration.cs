using System;
using System.Collections.Generic;
using System.Linq;
using UniHelper;

namespace UniState {
    public class StateConfiguration<TState, TTrigger> {
        public IEnumerable<Func<object>> EntryFunctions => onEntryFunctions;
        public IEnumerable<Func<object>> ExitFunctions => onExitFunctions;
        
        public IReadOnlyDictionary<TState, IEnumerable<Func<object>>> OnEntryFromFunctions => onEntryFromFunctions.ReadOnlyDictionary;
        public IReadOnlyDictionary<TState, IEnumerable<Func<object>>> OnExitToFunctions => onExitToFunctions.ReadOnlyDictionary;

        public IReadOnlyDictionary<TTrigger, TState> DefinedTransitions => definedTransitions;
        public IEnumerable<TTrigger> IgnoredTriggers => ignoredTriggers;

        readonly List<Func<object>> onEntryFunctions = new();
        readonly List<Func<object>> onExitFunctions = new();

        readonly ObservableListDictionary<TState, Func<object>> onEntryFromFunctions = new();
        readonly ObservableListDictionary<TState, Func<object>> onExitToFunctions = new();

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

        public StateConfiguration<TState, TTrigger> OnEntryFrom(TState state, Func<object> action) {
            onEntryFromFunctions.Add(state, action);
            return this;
        }

        public StateConfiguration<TState, TTrigger> OnExitTo(TState state, Func<object> action) {
            onExitToFunctions.Add(state, action);
            return this;
        }
    }
}