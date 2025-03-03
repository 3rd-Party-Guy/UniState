using System;
using System.Collections.Generic;
using System.Linq;
using UniHelper;

namespace UniState {
    public class StateConfiguration<TState, TTrigger> {
        public IEnumerable<Action> EntryFunctions => onEntryFunctions;
        public IEnumerable<Action> ExitFunctions => onExitFunctions;
        
        public IReadOnlyDictionary<TState, IEnumerable<Action>> OnEntryFromFunctions => onEntryFromFunctions.ReadOnlyDictionary;
        public IReadOnlyDictionary<TState, IEnumerable<Action>> OnExitToFunctions => onExitToFunctions.ReadOnlyDictionary;

        public IReadOnlyDictionary<TTrigger, TState> DefinedTransitions => definedTransitions;
        public IEnumerable<TTrigger> IgnoredTriggers => ignoredTriggers;

        readonly List<Action> onEntryFunctions = new();
        readonly List<Action> onExitFunctions = new();

        readonly ObservableListDictionary<TState, Action> onEntryFromFunctions = new();
        readonly ObservableListDictionary<TState, Action> onExitToFunctions = new();

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

        public StateConfiguration<TState, TTrigger> OnEntry(Action action) {
            onEntryFunctions.Add(action);
            return this;
        }

        public StateConfiguration<TState, TTrigger> OnExit(Action action) {
            onExitFunctions.Add(action);
            return this;
        }

        public StateConfiguration<TState, TTrigger> OnEntryFrom(TState state, Action action) {
            onEntryFromFunctions.Add(state, action);
            return this;
        }

        public StateConfiguration<TState, TTrigger> OnExitTo(TState state, Action action) {
            onExitToFunctions.Add(state, action);
            return this;
        }
    }
}