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
        public StateConfiguration<TState, TTrigger> CurrentConfiguration => stateConfigurations[State];

        readonly Dictionary<TState, StateConfiguration<TState, TTrigger>> stateConfigurations = new();

        public void Signal(TTrigger trigger)
        {
            Debug.Assert(CurrentConfiguration != null, $"No configuration found for state {State}");

            if (CurrentConfiguration.IgnoredTriggers.Contains(trigger))
                return;
            if (!CurrentConfiguration.DefinedTransitions.TryGetValue(trigger, out var newState))
                throw new InvalidOperationException($"No defined transition for {trigger} from {State}.");

            var oldState = State;

            CurrentConfiguration.ExitFunctions.ForEach(e => _ = e());
            if (CurrentConfiguration.OnExitToFunctions.TryGetValue(newState, out var exitFunctions)) {
                exitFunctions.ForEach(e => _ = e());
            }

            stateConfigurations[newState] ??= new();
            State = newState;

            CurrentConfiguration.EntryFunctions.ForEach(e => _ = e());
            if (CurrentConfiguration.OnEntryFromFunctions.TryGetValue(oldState, out var entryTransitions)) {
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