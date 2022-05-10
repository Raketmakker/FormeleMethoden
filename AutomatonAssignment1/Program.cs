using System;
using System.Diagnostics;

namespace AutomatonAssignment1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Automaton assignment 1");

            char[] alfabet = { 'a', 'b' };
            Automaton<string> m = new Automaton<string>(alfabet);

            // Create a DFA for the language L: starts with abb and ends with ab
            m.AddTransition(new Transition<string>("S", 'a', "A"));
            m.AddTransition(new Transition<string>("S", 'b', "F"));
            m.AddTransition(new Transition<string>("A", 'a', "F"));
            m.AddTransition(new Transition<string>("A", 'b', "B"));
            m.AddTransition(new Transition<string>("B", 'a', "F"));
            m.AddTransition(new Transition<string>("B", 'b', "C"));
            m.AddTransition(new Transition<string>("C", 'a', "D"));
            m.AddTransition(new Transition<string>("C", 'b', "C"));
            m.AddTransition(new Transition<string>("D", 'a', "D"));
            m.AddTransition(new Transition<string>("D", 'b', "E"));
            m.AddTransition(new Transition<string>("E", 'a', "D"));
            m.AddTransition(new Transition<string>("E", 'b', "C"));
            m.AddTransition(new Transition<string>("F", 'a', "F"));
            m.AddTransition(new Transition<string>("F", 'b', "F"));
            m.DefineAsStartState("S");
            m.DefineAsFinalState("E");

            // Verify that m is indeed a DFA
            Debug.Assert(m.IsDFA());

            // Test the automaton with various words, some in L, others not in L
            // Note: alternatively, use Debug.Assert() on method AcceptDFAOnly()
            TestWithString(m, "abbab", true);
            TestWithString(m, "abbaab", true);
            TestWithString(m, "abbbab", true);
            TestWithString(m, "abbababab", true);
            TestWithString(m, "abbbb", false);
            TestWithString(m, "ababab", false);
            TestWithString(m, "abbba", false);
            TestWithString(m, "aabbab", false);
            TestWithString(m, "ab", false);
            TestWithString(m, "abbbbbbbbab", true);
            TestWithString(m, "abbababa", false);


            // TODO Add your own DFAs and test them with your own sequences

        }

        static void TestWithString(Automaton<string> m, string s, bool expectedResult)
        {
            Console.Write($"\nTesting ");
            m.PrintAutomaton();
            Console.WriteLine($"with string '{s}'");
            bool accepted = m.AcceptDFAOnly(s);
            string result = accepted == expectedResult ? "Passed" : "Failed";
            Console.WriteLine($"Result : {result}");
        }

    }
}
