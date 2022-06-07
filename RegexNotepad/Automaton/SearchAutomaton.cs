using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexNotepad.Automaton
{
    public class SearchAutomaton<T> : AutomatonBase<T> where T : IComparable<T>
    {
        protected SortedSet<T> errorStates;
        protected SortedSet<T> enterStates;
        protected SortedSet<T> exitStates;
        protected SortedSet<T> sequenceStates;

        public SearchAutomaton() : base()
        {
            errorStates = new SortedSet<T>();
            enterStates = new SortedSet<T>();
            exitStates = new SortedSet<T>();
            sequenceStates = new SortedSet<T>();
        }

        public void DefineAsErrorState(T t)
        {
            states.Add(t);
            errorStates.Add(t);
        }

        public void DefineAsEnterState(T t)
        {
            states.Add(t);
            enterStates.Add(t);
        }

        public void DefineAsExitState(T t)
        {
            states.Add(t);
            exitStates.Add(t);
        }

        public void DefineAsSequenceState(T t)
        {
            states.Add(t);
            sequenceStates.Add(t);
        }

        /// <summary>
        /// Return true if a given sequence is accepted by the automata object provided that the automata is a DFA
        /// The sequence is accepted if it puts the automata object in one of its final states
        /// </summary>
        /// <param name="sequence">The sequence to be accepted (or not)</param>
        /// <returns>True if the sequence is accepted, false if it is not accepted</returns>
        public async Task<Tuple<string, int>> AcceptDFAOnly(Tuple<string, int> sequence)
        {
            List<Tuple<string, bool>> occurences = new List<Tuple<string, bool>>();

            if (!IsDFA())
            {
                Console.WriteLine($"The automata is not a DFA!");
                return null;
            }

            T currentState = startStates.First<T>();

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
                    return null;

                currentState = nextState;

                if (i == sequence.Item1.Length - 1)
                {
                    if (this.finalStates.Contains(currentState))
                    {
                        return sequence;
                    }
                }
            }
            return null;
        }
    }
}
