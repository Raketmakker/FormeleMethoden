using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatonAssignment1
{
    /// <summary>
    /// Class that represents an NDFA or a DFA
    /// This is a template class to allow for different state types, e.g. tuples when two (N)DFA's are combined into one
    /// </summary>
    /// <typeparam name="T">Substitute a suitable type for a state in the (N)DFA as T (such as char, string, or a tuple)</typeparam>
    class Automaton<T> where T : IComparable<T>
    {
        private ISet<Transition<T>> transitions;
        private SortedSet<T> states;
        private SortedSet<T> startStates;
        private SortedSet<T> finalStates;
        private SortedSet<char> symbols; 

        /// <summary>
        /// Create an empty DFA (no alfabet, no states, no transitions)
        /// </summary>
        public Automaton()
            : this(new SortedSet<char>())
        {
        }

        /// <summary>
        /// Create a DFA for a given alfabet with no states or transitions
        /// </summary>
        /// <param name="s">The alfabet (i.e. the symbols that the DFA can handle)</param>
        public Automaton(char[] s)
            : this(new SortedSet<char>(s.ToList<char>()))
        {
        }

        /// <summary>
        /// Create a DFA for a given alfabet with no states or transitions
        /// </summary>
        /// <param name="symbols">The alfabet (i.e. the symbols that the DFA can handle)</param>
        public Automaton(SortedSet<char> symbols)
        {
            this.transitions = new SortedSet<Transition<T>>();
            this.states = new SortedSet<T>();
            this.startStates = new SortedSet<T>();
            this.finalStates = new SortedSet<T>();
            SetAlphabet(symbols);
        }

        public override string ToString()
        {
            // TODO Write ToString() body that produces this output:
            // ({ ...all states... }, {...alphabet chars...}, delta, {...start state...}, {...final states...})

            string s = "({";
            var setList = this.states.ToList();

            for (int i = 0; i < setList.Count(); i++)
            {
                s += setList[i].ToString();

                if (i != setList.Count() - 1)
                {
                    s += ", ";
                }
            }

            s += "}, {";
            setList.Clear();

            var symbolList = this.symbols.ToList();
            for (int i = 0; i < symbolList.Count(); i++)
            {
                s += symbolList[i].ToString();

                if (i != symbolList.Count() - 1)
                {
                    s += ", ";
                }
            }

            s += "}, delta {";
            setList = this.startStates.ToList();

            for (int i = 0; i < setList.Count(); i++)
            {
                s += setList[i].ToString();

                if (i != setList.Count() - 1)
                {
                    s += ", ";
                }
            }
            
            s += "}, {";
            setList = this.finalStates.ToList();

            for (int i = 0; i < setList.Count(); i++)
            {
                s += setList[i].ToString();

                if (i != setList.Count() - 1)
                {
                    s += ", ";
                }
            }
            s += "})";
            return s;
        }

        /// <summary>
        /// Assign an alfabet to the DFA
        /// </summary>
        /// <param name="s"></param>
        public void SetAlphabet(char[] s)
        {
            this.SetAlphabet(new SortedSet<char>(s));
        }

        /// <summary>
        /// Assign an alfabet to the DFA
        /// </summary>
        /// <param name="symbols"></param>
        public void SetAlphabet(SortedSet<char> symbols)
        {
            this.symbols = new SortedSet<char>(symbols);
        }

        /// <summary>
        /// Add a transition to the DFA
        /// </summary>
        /// <param name="t">The transition to be added</param>
        public void AddTransition(Transition<T> t)
        {
            transitions.Add(t);
            states.Add(t.GetFromState());
            states.Add(t.GetToState());
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

        private void PrintStates(ISet<T> states)
        {
            Console.Write("{");
            foreach (T state in states)
            {
                Console.Write($"{state} ");
            }
            Console.Write("}");
        }

        public void PrintAutomaton()
        {
            // TODO Write code that produces this output:
            // ({ ...all states... }, {...alphabet chars...}, delta, {...start state...}, {...final states...})
            Console.WriteLine(this.ToString());
        }

        public void PrintAlphabet()
        {
            Console.Write("{");
            foreach (char c in symbols)
            {
                Console.Write($"{c} ");
            }
            Console.Write("}");
        }

        public void PrintTransitions()
        {
            foreach (Transition<T> t in transitions)
            {
                Console.WriteLine(t);
            }
        }

        public void PrintStartStates()
        {
            PrintStates(startStates);
        }

        public void PrintFinalStates()
        {
            PrintStates(finalStates);
        }

        public void PrintAllStates()
        {
            PrintStates(states);
        }

        /// <summary>
        /// Determine if the automata object represents a deterministic finite automaton (DFA)
        /// </summary>
        /// <returns>True if the automata object is deterministic (DFA)</returns>
        public bool IsDFA()
        {
            bool isDFA = true;
            // TODO Write code that returns true if the Automaton is deterministic, false if it is non-deterministic
            isDFA &= this.startStates.Count == 1;   //DFA's have exact 1 start state

            foreach (T state in this.states)
            {
                //Skip final states
                if (this.finalStates.Contains(state))
                    continue;

                //Count the amount of transitions (for this state) and check if it already contains this symbol
                var stateTransitions = new List<char>();
                foreach (var t in this.transitions)
                {
                    if (t.GetFromState().Equals(state))
                    {
                        if (!stateTransitions.Contains(t.GetSymbol()))
                        {
                            stateTransitions.Add(t.GetSymbol());
                        }
                    }
                }
                isDFA &= stateTransitions.Count == this.symbols.Count;
            }
            return isDFA;
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
            // TODO Write code that returns the correct set of states

            return states;
        }

        /// <summary>
        /// Return true if a given sequence is accepted by the automata object provided that the automata is a DFA
        /// The sequence is accepted if it puts the automata object in one of its final states
        /// </summary>
        /// <param name="sequence">The sequence to be accepted (or not)</param>
        /// <returns>True if the sequence is accepted, false if it is not accepted</returns>
        public bool AcceptDFAOnly(string sequence)
        {
            if (!IsDFA())
            {
                Console.WriteLine($"The automata is not a DFA!");
                return false;
            }

            // TODO HARDCODED
            return true;

            // This implementation is only for DFAs, throws an exception as soon as
            // multiple to states are encountered
            T currentState = startStates.First<T>(); // Assume DFA, so only one start state
            Console.WriteLine($"Accept sequence {sequence}, start at state {currentState}");
            // TODO Write the code that follows the transitions according to the symbols in the sequence
            // and returns true if the last state is one of the final states
        }

        ///// <summary>
        ///// Return the set of states that can be reached from a given state using an epsilon transition (i.e. no symbol received)
        ///// Note: this is basically the epsilon closure of the given state
        ///// </summary>
        ///// <param name="from">The state to start from</param>
        ///// <returns>The set of destination states (not including the from state)</returns>
        //public ISet<T> GetToStates(T from)
        //{
        //    // Follow all epsilon transitions starting in the from state
        //    SortedSet<T> states = new SortedSet<T>();
        //    ...  
        //    return states;
        //}

        ///// <summary>
        ///// Return true if a given sequence is accepted by the automata object (even if it is an NDFA)
        ///// Multiple start states are allowed (and used) and epsilon transitions are used as well
        ///// THe sequence is accepted if one of the possible transition paths through the automaton
        ///// ends in one of the final states
        ///// </summary>
        ///// <param name="sequence">The sequence to be accepted (or not)</param>
        ///// <returns>True if the sequence is accepted, false if it is not accepted</returns>
        //public bool Accept(string sequence)
        //{
        //    bool accept = false;
        //    // Assume multiple start states, try each one in turn
        //    ...
        //    return accept;
        //}

        ///// <summary>
        ///// Return true if a given sequence is accepted by the automaton (even if it is an NDFA)
        ///// from the given state, including any epsilon transitions that might occur
        ///// The sequence is accepted if one of the possible transitions paths through the automaton
        ///// ends in one of the final states
        ///// </summary>
        ///// <param name="from">The state to start the search from</param>
        ///// <param name="sequence">The sequence to be accepted (or not)</param>
        ///// <returns>True if the sequence is accepted, false if it is not accepted</returns>
        //private bool AcceptFromState(T from, string sequence)
        //{
        //    bool accepted = false;
        //    ...
        //    return accepted;
        //}

        //// TODO Add functionality to return the epsilon closure --> This is GetToStates(T from)
        //// TODO Add functionality to return delta epsilon for a given set of state and symbol, based on the epsilon closure --> This is GetToStates(T from, char symbol)

        //// TODO Add functionality for writing an object to a file and for reading an object from a file, using a human readable
        //// TODO Maybe add a derived class for this, or a decorator class?!

        //// TODO Add functionality for constructing an object that accepts only sequences that start with a given sequence of symbols
        //// TODO Add functionality for constructing an object that accepts only sequences that end with a given sequence of symbols
        //// TODO Add functionality for constructing an object that accepts only sequences that contain a given sequence of symbols
        //// TODO Maybe construct a factory class for this?!

        //// TODO Add functionality for combining two objects into a new object using the AND operator
        //// TODO Add functionality for combining two objects into a new object using the OR operator
        //// TODO Add functionality for creating a new object from an existing object using the NOT operator (the complement)

        //// TODO Add functionality to return a sorted set of string containing all words of given length that can be formed using the given symbols

        //// TODO Add functionality for listing all accepted sequences of symbols up to a given length, sorted by length

        //// TODO Add functionality for listing all non-accepted sequences of symbols up to a given length, sorted by length

        //// TODO Add functionality to convert an NDFA into a DFA
        //// TODO Add functionality to reverse a DFA (resulting in an NDFA)
        //// TODO Add functionality to minimise a DFA (using either of the two given algorithms)
        //// TODO Add functionality to perform an equality check on two automata
    }
}
