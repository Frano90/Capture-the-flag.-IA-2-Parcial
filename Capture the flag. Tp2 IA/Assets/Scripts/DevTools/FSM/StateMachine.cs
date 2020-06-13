using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateMachine
{
    
    IState previous;

    public string DebugState() => _currentState.ToString();  
    
    List<IState> stateList = new List<IState>();
    
    //TODO NUEVA STATE MACHINE CON TRANSICIONES
    IState _currentState;
    Transition _currentTransition;
    private Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type, List<Transition>>();
    private List<Transition> _currentTransitions = new List<Transition>();
    private List<Transition> _anyTransition = new List<Transition>();
    private static List<Transition> EmptyTransitions = new List<Transition>(0);


    public IState CurrentState
    {
        get => _currentState;
        private set => _currentState = value;
    }

    public void Tick()
    {
        var transition = GetTransition();
        if (transition != null)
        {
            _currentTransition = transition;
            SetState(transition.To);
        }
        

        _currentState?.Tick();
    }

    public void SetState(IState state)
    {
        if (state == _currentState)
            return;
        
        _currentState?.OnExit();
        _currentTransition?.OnTransition?.Invoke();
        _currentState = state;

        _transitions.TryGetValue(_currentState.GetType(), out _currentTransitions);
        if (_currentTransitions == null)
            _currentTransitions = EmptyTransitions;
        
        _currentState.OnEnter();
    }

    private Transition GetTransition()
    {
        foreach (var transition in _anyTransition)
            if (transition.Condition())
                return transition;
        
        foreach (var transition in _currentTransitions)
            if (transition.Condition())
                return transition;

        return null;
    }
    public Transition AddTransition(IState from, IState to, Func<bool> predicate)
    {
        if (_transitions.TryGetValue(from.GetType(), out var transitions) == false)
        {
            transitions = new List<Transition>();
            _transitions[from.GetType()] = transitions;
        }

        Transition newT = new Transition(to, predicate);
        transitions.Add(newT);
        return newT;
    }

    public void AddAnyTransition(IState state, Func<bool> predicate)
    {
        _anyTransition.Add(new Transition(state, predicate));
    }

    public class Transition
    {
        public Func<bool> Condition { get; } 
        public IState To { get; }

        public Action OnTransition;

        public Transition(IState to, Func<bool> predicate)
        {
            To = to;
            Condition = predicate;
        }
    }
}

