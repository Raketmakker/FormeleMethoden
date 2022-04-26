using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week1
{
    public class Automata
    {
        private ISet<Transition<string>> transitions;

        private SortedSet<string> states;
        private SortedSet<string> startStates;
        private SortedSet<string> finalStates;
        public SortedSet<char> Symbols { get; private set; }

        public Automata() : this(new SortedSet<char>()) { }

        public Automata(char[] s) : this (new SortedSet<char>(s)) { }

        public Automata(SortedSet<char> symbols)
        {
            transitions = new SortedSet<Transition<string>>();
            states = new SortedSet<string>();
            startStates = new SortedSet<string>();
            finalStates = new SortedSet<string>();
            this.SetAlphabet(symbols);
        }

        public void SetAlphabet(char[] s)
        {
            this.SetAlphabet(new SortedSet<char>(s));
        }

        public void SetAlphabet(SortedSet<char> symbols)
        {
            this.Symbols = symbols;
        }

        public SortedSet<char> GetAlphabet()
        {
            return this.Symbols;
        }

        public void AddTransition(Transition<string> t)
        {
            transitions.Add(t);
            states.Add(t.FromState);
            states.Add(t.ToState);
        }

        public void DefineAsStartState(string t)
        {
            // if already in states no problem because a Set will remove duplicates.
            states.Add(t);
            startStates.Add(t);
        }

        public void DefineAsFinalState(string t)
        {
            // if already in states no problem because a Set will remove duplicates.
            states.Add(t);
            finalStates.Add(t);
        }

        public void PrintTransitions()
        {

            foreach (Transition<string> t in transitions)
            {
                Console.WriteLine(t);
            }
        }

        public bool IsDFA()
        {
            bool isDFA = true;

            foreach (string from in states)
            {
                foreach (char symbol in Symbols)
                {
                    //isDFA = isDFA && getToStates(from, symbol).size() == 1;
                    throw new NotImplementedException("Kaulo loser");
                }
            }

            return isDFA;
        }
    }
}
