using System;
using System.Collections.Generic;

namespace UniState {
    public class StateMachine<TState, TTrigger>
        where TState : struct, IConvertible
        where TTrigger : struct, IConvertible {
        public TState State { get; private set; }

        Dictionary<TState, StateConfiguration<TState, TTrigger>> stateConfigurations = new();

        public StateMachine(TState initialState) {
            if (!typeof(TState).IsEnum) {
                throw new ArgumentException("TState must be an enum");
            }
            if (!typeof(TTrigger).IsEnum) {
                throw new ArgumentException("TTrigger must be an enum");
            }

            stateConfigurations[initialState] = new();
        }

        public StateConfiguration<TState, TTrigger> Configure(TState state) {
            stateConfigurations[state] ??= new();
            return stateConfigurations[state];
        }
    }
}