using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week1
{
    public class TestAutomata
    {
        static public Automata GetExampleSlide8Lesson2()
        {
            char[] alphabet = { 'a', 'b' };
            Automata m = new Automata(alphabet);

            m.AddTransition(new Transition<String>("q0", 'a', "q1"));
            m.AddTransition(new Transition<String>("q0", 'b', "q4"));

            m.AddTransition(new Transition<String>("q1", 'a', "q4"));
            m.AddTransition(new Transition<String>("q1", 'b', "q2"));

            m.AddTransition(new Transition<String>("q2", 'a', "q3"));
            m.AddTransition(new Transition<String>("q2", 'b', "q4"));

            m.AddTransition(new Transition<String>("q3", 'a', "q1"));
            m.AddTransition(new Transition<String>("q3", 'b', "q2"));

            // the error state, loops for a and b:
            m.AddTransition(new Transition<String>("q4", 'a'));
            m.AddTransition(new Transition<String>("q4", 'b'));

            // only on start state in a dfa:
            m.DefineAsStartState("q0");

            // two final states:
            m.DefineAsFinalState("q2");
            m.DefineAsFinalState("q3");

            return m;
        }

        static public Automata GetExampleSlide14Lesson2()
        {
            char[] alphabet = { 'a', 'b' };
            Automata m = new Automata(alphabet);

            m.AddTransition(new Transition<String>("A", 'a', "C"));
            m.AddTransition(new Transition<String>("A", 'b', "B"));
            m.AddTransition(new Transition<String>("A", 'b', "C"));

            m.AddTransition(new Transition<String>("B", 'b', "C"));
            m.AddTransition(new Transition<String>("B", "C"));

            m.AddTransition(new Transition<String>("C", 'a', "D"));
            m.AddTransition(new Transition<String>("C", 'a', "E"));
            m.AddTransition(new Transition<String>("C", 'b', "D"));

            m.AddTransition(new Transition<String>("D", 'a', "B"));
            m.AddTransition(new Transition<String>("D", 'a', "C"));

            m.AddTransition(new Transition<String>("E", 'a'));
            m.AddTransition(new Transition<String>("E", "D"));

            // only on start state in a dfa:
            m.DefineAsStartState("A");

            // two final states:
            m.DefineAsFinalState("C");
            m.DefineAsFinalState("E");

            return m;
        }

        /// <summary>
        /// Begint met ABB of eindigd met BAAB
        /// </summary>
        /// <returns></returns>
        public static Automata Opdracht1()
        {
            char[] alphabet = { 'a', 'b' };
            Automata m = new Automata(alphabet);

            m.AddTransition(new Transition<string>("S", 'a', "A"));
            m.AddTransition(new Transition<string>("S", 'b', "E"));

            m.AddTransition(new Transition<string>("A", 'a', "E"));
            m.AddTransition(new Transition<string>("A", 'b', "B"));

            m.AddTransition(new Transition<string>("B", 'a', "E"));
            m.AddTransition(new Transition<string>("B", 'b', "C"));

            m.AddTransition(new Transition<string>("E", 'a', "E"));
            m.AddTransition(new Transition<string>("E", 'b', "F"));

            m.AddTransition(new Transition<string>("F", 'a', "G"));
            m.AddTransition(new Transition<string>("F", 'b', "E"));
            
            m.AddTransition(new Transition<string>("G", 'a', "H"));
            m.AddTransition(new Transition<string>("G", 'b', "E"));

            m.AddTransition(new Transition<string>("H", 'a', "E"));
            m.AddTransition(new Transition<string>("H", 'b', "I"));

            m.DefineAsStartState("S");
            
            m.DefineAsFinalState("C");
            m.DefineAsFinalState("I");

            return m;
        }

        /// <summary>
        /// Oneven aantal A's of even aantal B's
        /// </summary>
        /// <returns></returns>
        public static Automata Opdracht2()
        {
            char[] alphabet = { 'a', 'b' };
            Automata m = new Automata(alphabet);

            m.AddTransition(new Transition<string>("A", 'a', "C"));
            m.AddTransition(new Transition<string>("A", 'b', "B"));

            m.AddTransition(new Transition<string>("B", 'a', "D"));
            m.AddTransition(new Transition<string>("B", 'b', "A"));

            m.AddTransition(new Transition<string>("C", 'a', "A"));
            m.AddTransition(new Transition<string>("C", 'b', "D"));

            m.AddTransition(new Transition<string>("D", 'a', "B"));
            m.AddTransition(new Transition<string>("D", 'b', "C"));

            m.DefineAsStartState("A");

            m.DefineAsFinalState("A");
            m.DefineAsFinalState("C");
            m.DefineAsFinalState("D");

            return m;
        }

        /// <summary>
        /// Even aantal B's en eindigd op AAB
        /// </summary>
        /// <returns></returns>
        public static Automata Opdracht3()
        {
            char[] alphabet = { 'a', 'b' };
            Automata m = new Automata(alphabet);

            m.AddTransition(new Transition<string>("A", 'a', "A"));
            m.AddTransition(new Transition<string>("A", 'b', "B"));

            m.AddTransition(new Transition<string>("B", 'a', "C"));
            m.AddTransition(new Transition<string>("B", 'b', "A"));

            m.AddTransition(new Transition<string>("C", 'a', "D"));
            m.AddTransition(new Transition<string>("C", 'b', "A"));

            m.AddTransition(new Transition<string>("D", 'a', "A"));
            m.AddTransition(new Transition<string>("D", 'b', "E"));

            m.DefineAsStartState("A");

            m.DefineAsFinalState("E");

            return m;
        }

        /// <summary>
        /// Begint met ABB en bevat baab
        /// </summary>
        /// <returns></returns>
        public static Automata Opdracht4()
        {
            char[] alphabet = { 'a', 'b' };
            Automata m = new Automata(alphabet);

            m.AddTransition(new Transition<string>("S", 'a', "A"));
            m.AddTransition(new Transition<string>("S", 'b', "X"));

            m.AddTransition(new Transition<string>("A", 'a', "X"));
            m.AddTransition(new Transition<string>("A", 'b', "B"));

            m.AddTransition(new Transition<string>("B", 'a', "X"));
            m.AddTransition(new Transition<string>("B", 'b', "C"));

            m.AddTransition(new Transition<string>("C", 'a', "D"));
            m.AddTransition(new Transition<string>("C", 'b', "C"));

            m.AddTransition(new Transition<string>("D", 'a', "E"));
            m.AddTransition(new Transition<string>("D", 'b', "C"));

            m.AddTransition(new Transition<string>("E", 'a', "B"));
            m.AddTransition(new Transition<string>("E", 'b', "F"));

            m.AddTransition(new Transition<string>("F", 'a', "F"));
            m.AddTransition(new Transition<string>("F", 'b', "F"));

            m.AddTransition(new Transition<string>("X", 'a', "X"));
            m.AddTransition(new Transition<string>("X", 'b', "X"));

            m.DefineAsStartState("S");

            m.DefineAsFinalState("F");
            m.DefineAsFinalState("X");

            return m;
        }
    }
}
