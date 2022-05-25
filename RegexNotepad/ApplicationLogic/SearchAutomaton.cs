using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexNotepad.ApplicationLogic
{
    public class SearchAutomaton<T> where T : IComparable<T>
    {
        private ISet<AdvancedTransition<T>> transitions;
        private SortedSet<T> states;
        private SortedSet<T> startStates;
        private SortedSet<T> errorStates;
        private SortedSet<T> succesStates;
        private SortedSet<T> finalStates;

        public SearchAutomaton()
        {
            this.transitions = new SortedSet<AdvancedTransition<T>>();
            this.states = new SortedSet<T>();
            this.startStates = new SortedSet<T>();
            this.errorStates = new SortedSet<T>();
            this.succesStates = new SortedSet<T>();
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

        public void DefineAsSuccesState(T t)
        {
            states.Add(t);
            succesStates.Add(t);
        }

        public void DefineAsErrorState(T t)
        {
            states.Add(t);
            errorStates.Add(t);
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
        /// Return true if a given sequence is accepted by the automata object provided that the automata is a DFA
        /// The sequence is accepted if it puts the automata object in one of its final states
        /// </summary>
        /// <param name="sequence">The sequence to be accepted (or not)</param>
        /// <returns>True if the sequence is accepted, false if it is not accepted</returns>
        public async Task<List<Tuple<string, int>>> AcceptDFAOnly(Tuple<string, int> sequence)
        {
            List<Tuple<string, int>> occurences = new List<Tuple<string, int>>();

            if (!IsDFA())
            {
                Console.WriteLine($"The automata is not a DFA!");
                return occurences;
            }

            T currentState = startStates.First<T>();            
            string foundSubstring = "";
            int enterSuccesStateIndex = -1;

            for (int i = 0; i < sequence.Item1.Length; i++)
            {
                ISet<T> nextStates = GetToStates(currentState, sequence.Item1[i]);

                if (nextStates.Count > 1 || nextStates.Count == 0)
                {
                    throw new Exception($"The DFA acceptor failed! Zero or more than one transitions for character {sequence.Item1[i]} in state {currentState}.");
                }

                T nextState = nextStates.First<T>();

                //Ran into an error state. Return result
                if (errorStates.Contains(nextState))
                    return occurences;

                //In succes state, add character to the string
                if (succesStates.Contains(nextState))
                {
                    foundSubstring += sequence.Item1[i];

                    // Entered a successtate from not successtate
                    if (!succesStates.Contains(currentState))
                        enterSuccesStateIndex = i;

                    //Save last occurence if it is in, or enters a successtate
                    if (i == sequence.Item1.Length - 1)
                        occurences.Add(new Tuple<string, int>(foundSubstring, enterSuccesStateIndex + sequence.Item2));
                }
                else
                {
                    //Transition from successtate to not successtate. Add the substring
                    if (succesStates.Contains(currentState))
                    {
                        occurences.Add(new Tuple<string, int>(foundSubstring, enterSuccesStateIndex + sequence.Item2));
                        foundSubstring = "";
                    }
                }
                currentState = nextState;
            }
            return occurences;
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

        /// <summary>
        /// When a succes state is reached, a piece of text has been found.
        /// Keep adding characters to the substring until it leaves the successtate(s).
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<Tuple<string, int>> Split(string sequence)
        {
            if (!IsDFA())
            {
                Console.WriteLine($"The automata is not a DFA!");
                return null;
            }

            T currentState = startStates.First<T>();
            List<Tuple<string, int>> occurences = new List<Tuple<string, int>>();
            string foundSubstring = "";
            int enterSuccesStateIndex = -1;

            for (int i = 0; i < sequence.Length; i++)
            {
                ISet<T> nextStates = GetToStates(currentState, sequence[i]);

                if (nextStates.Count > 1 || nextStates.Count == 0)
                {
                    throw new Exception($"The DFA acceptor failed! Zero or more than one transitions for character {sequence[i]} in state {currentState}.");
                }

                T nextState = nextStates.First<T>();

                //Ran into an error state. Return result
                if (errorStates.Contains(nextState))
                    return occurences;
                
                //In succes state, add character to the string
                if (succesStates.Contains(nextState))
                {
                    foundSubstring += sequence[i];
                    
                    // Entered a successtate from not successtate
                    if (!succesStates.Contains(currentState))
                        enterSuccesStateIndex = i;

                    //Save last occurence if it is in, or enters a successtate
                    if(i == sequence.Length - 1)
                        occurences.Add(new Tuple<string, int>(foundSubstring, enterSuccesStateIndex));
                }
                else
                {
                    //Transition from successtate to not successtate. Add the substring
                    if (succesStates.Contains(currentState))
                    {
                        occurences.Add(new Tuple<string, int>(foundSubstring, enterSuccesStateIndex));
                        foundSubstring = "";
                    }
                }                

                currentState = nextState;
            }
            return occurences;
        }
    }
}
