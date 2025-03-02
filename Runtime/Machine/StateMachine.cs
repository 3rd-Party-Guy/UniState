using System;
using System.Collections.Generic;
using System.Linq;
using UniHelper;

namespace UniState {
    public class StateMachine<TState, TTrigger>
        where TState : struct, IConvertible
        where TTrigger : struct, IConvertible {
        public TState State { get; private set; }

        readonly Dictionary<TState, StateConfiguration<TState, TTrigger>> stateConfigurations = new();

        public void Signal(TTrigger trigger)
        {
            if (!stateConfigurations.TryGetValue(State, out var currentConfig))
                throw new InvalidOperationException($"No configuration found for state {State}.");
            if (currentConfig.IgnoredTriggers.Contains(trigger))
                return;
            if (!currentConfig.DefinedTransitions.TryGetValue(trigger, out var nextState))
                throw new InvalidOperationException($"No defined transition for {trigger} from {State}.");

            currentConfig.ExitFunctions.ForEach(e => e());
            State = nextState;
            stateConfigurations[State] ??= new();

            stateConfigurations[State].EntryFunctions.ForEach(e => e());
        }

        public StateMachine(TState initialState) {
            if (!typeof(TState).IsEnum)
                throw new ArgumentException("TState must be an enum");
            if (!typeof(TTrigger).IsEnum)
                throw new ArgumentException("TTrigger must be an enum");

            stateConfigurations[initialState] = new();
            State = initialState;
        }

        public StateConfiguration<TState, TTrigger> Configure(TState state) {
            stateConfigurations[state] ??= new();
            return stateConfigurations[state];
        }
    }
}