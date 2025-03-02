using System;
using System.Collections.Generic;
using System.Linq;
using UniHelper;
using UnityEngine;

namespace UniState {
    public class StateMachine<TState, TTrigger>
        where TState : struct, IConvertible
        where TTrigger : struct, IConvertible {
        public TState State { get; private set; }

        readonly Dictionary<TState, StateConfiguration<TState, TTrigger>> stateConfigurations = new();

        public void Signal(TTrigger trigger)
        {
            var currentConfig = stateConfigurations[State];
            Debug.Assert(currentConfig != null, $"No configuration found for state {State}");

            if (currentConfig.IgnoredTriggers.Contains(trigger))
                return;
            if (!currentConfig.DefinedTransitions.TryGetValue(trigger, out var newState))
                throw new InvalidOperationException($"No defined transition for {trigger} from {State}.");

            var oldState = State;

            currentConfig.ExitFunctions.ForEach(e => _ = e());
            if (currentConfig.OnExitToFunctions.TryGetValue(newState, out var exitFunctions)) {
                exitFunctions.ForEach(e => _ = e());
            }

            State = newState;
            stateConfigurations[State] ??= new();

            stateConfigurations[State].EntryFunctions.ForEach(e => _ = e());
            if (stateConfigurations[State].OnEntryFromFunctions.TryGetValue(oldState, out var entryTransitions)) {
                entryTransitions.ForEach(e => _ = e());
            }
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