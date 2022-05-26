using RegexNotepad.ApplicationLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexNotepad.Automaton
{
    public abstract class AutomatonBase<T> where T : IComparable<T>
    {
        protected ISet<AdvancedTransition<T>> transitions;
        protected SortedSet<T> states;
        protected SortedSet<T> startStates;
        protected SortedSet<T> finalStates;

        public AutomatonBase()
        {
            this.transitions = new SortedSet<AdvancedTransition<T>>();
            this.states = new SortedSet<T>();
            this.startStates = new SortedSet<T>();
            this.finalStates = new SortedSet<T>();
        }

        /// <summary>
        /// Add a transition to the DFA
        /// </summary>
        /// <param name="t">The transition to be added</param>
        public void AddTransition(AdvancedTransition<T> t)
        {
            transitions.Add(t);
            states.Add(t.FromState);
            states.Add(t.ToState);
        }

        /// <summary>
        /// Set a given state to be the start state
        /// Note that multiple states can be set as a start state in an NDFA (not in a DFA though)
        /// </summary>
        /// <param name="t">The state that is to be a start state</param>
        public void DefineAsStartState(T t)
        {
            // TODO Write DefineAsStartState() body
            // if already in states no problem because a Set will remove duplicates.
            states.Add(t);
            startStates.Add(t);
        }

        /// <summary>
        /// Set a given state to be one of the end states
        /// </summary>
        /// <param name="t">The state that is to be an end state</param>
        public void DefineAsFinalState(T t)
        {
            // TODO Write DefineAsFinalState() body
            // if already in states no problem because a Set will remove duplicates.
            states.Add(t);
            finalStates.Add(t);
        }

        /// <summary>
        /// Return the set of states that can be reached from a given state when a given symbol is received
        /// </summary>
        /// <param name="from">The state to start from</param>
        /// <param name="symbol">The symbol that is received</param>
        /// <returns>The set of destination states</returns>
        public ISet<T> GetToStates(T from, char symbol)
        {
            SortedSet<T> states = new SortedSet<T>();

            foreach (var t in this.transitions)
            {
                if (t.FromState.Equals(from) && t.Symbol.Equals(symbol) && t.InvertedTransition == false)
                {
                    states.Add(t.ToState);
                }
            }

            //The given symbol is an inverted transition and is not mentioned in the created transitions
            if(states.Count == 0)
            {
                foreach (var t in this.transitions)
                {
                    if (t.FromState.Equals(from) && t.InvertedTransition)
                    {
                        states.Add(t.ToState);
                    }
                }
            }
            return states;
        }

        /// <summary>
        /// Determine if the automata object represents a deterministic finite automaton (DFA)
        /// Algorithm: check for every state if:
        /// it has exactly one inverted transition (the not alphabet)
        /// </summary>
        /// <returns>True if the automata object is deterministic (DFA)</returns>
        public bool IsDFA()
        {
            bool isDFA = true;
            //DFA's have exact 1 start state
            isDFA &= this.startStates.Count == 1;   
            
            foreach (T state in this.states)
            {
                //Skip final states
                if (this.finalStates.Contains(state))
                    continue;

                //Count the amount of transitions (for this state)
                var uniqueSymbols = new List<char>();
                //Count the symbols that go over state x to transition y
                int totalSymbolCount = 0;
                //Every transition has exactly one inverted transition (everything except the given symbols)
                int invertedStates = 0;
                foreach (var t in this.transitions)
                {
                    if (t.FromState.Equals(state))
                    {
                        if (t.InvertedTransition)
                        {
                            invertedStates++;
                        }
                        else
                        {
                            totalSymbolCount++;

                            if (!uniqueSymbols.Contains(t.Symbol))
                            {
                                uniqueSymbols.Add(t.Symbol);
                            }
                        }                        
                    }
                }
                isDFA = isDFA && (uniqueSymbols.Count == totalSymbolCount) && (invertedStates == 1);
            }
            return isDFA;
        }        
    }
}
