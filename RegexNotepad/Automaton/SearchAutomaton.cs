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
            bool inSequence = true;

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

                //The current state is an enter state (the beginning of the sequence)
                if (enterStates.Contains(currentState))
                {
                    enterSuccesStateIndex = i;
                    inSequence = true;
                }

                //If current state is in the sequence, add the char to the substring
                if (inSequence)
                    foundSubstring += sequence.Item1[i];

                //Exiting the substring. Reset values
                if (exitStates.Contains(currentState))
                {
                    inSequence = false;
                    occurences.Add(new Tuple<string, int>(foundSubstring, enterSuccesStateIndex + sequence.Item2));
                    foundSubstring = "";
                    enterSuccesStateIndex = -1;
                }

                currentState = nextState;
            }
            return occurences;
        }
    }
}
