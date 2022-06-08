using RegexNotepad.ApplicationLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegexNotepad.Automaton
{
    public class SplitAutomaton<T> : AutomatonBase<T> where T : IComparable<T>
    {
        public SortedSet<T> succesStates;

        public SplitAutomaton() : base()
        {
            this.succesStates = new SortedSet<T>();
        }

        public void DefineAsSuccesState(T t)
        {
            states.Add(t);
            succesStates.Add(t);
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

                //In succes state, add character to the string
                if (succesStates.Contains(nextState))
                {
                    foundSubstring += sequence[i];

                    // Entered a successtate from not successtate
                    if (!succesStates.Contains(currentState))
                        enterSuccesStateIndex = i;

                    //Save last occurence if it is in, or enters a successtate
                    if (i == sequence.Length - 1)
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
