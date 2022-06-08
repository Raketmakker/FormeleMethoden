using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegexNotepad.ApplicationLogic;
using RegexNotepad.Automaton;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RegexNotepadUnitTests
{
    [TestClass]
    public class AutomatonTests
    {
        [TestMethod]
        public void TestAddFunctionsSplitAutomaton()
        {
            var auto = new SplitAutomaton<int>();
            var trans = new AdvancedTransition<int>(0, ' ', 1);
            auto.AddTransition(trans);
            Assert.AreEqual(2, auto.states.Count);
            auto.DefineAsStartState(0);
            Assert.AreEqual(1, auto.startStates.Count);
            auto.DefineAsFinalState(1);
            Assert.AreEqual(1, auto.finalStates.Count);
            auto.DefineAsSuccesState(0);
            Assert.AreEqual(1, auto.succesStates.Count);
        }

        [TestMethod]
        public void TestAddFunctionsSearchAutomaton()
        {
            var auto = new SearchAutomaton<int>();
            var trans = new AdvancedTransition<int>(0, ' ', 1);
            auto.AddTransition(trans);
            Assert.AreEqual(2, auto.states.Count);
            auto.DefineAsStartState(0);
            Assert.AreEqual(1, auto.startStates.Count);
            auto.DefineAsFinalState(1);
            Assert.AreEqual(1, auto.finalStates.Count);
            auto.DefineAsEnterState(0);
            Assert.AreEqual(1, auto.enterStates.Count);
            auto.DefineAsErrorState(0);
            Assert.AreEqual(1, auto.errorStates.Count);
            auto.DefineAsExitState(0);
            Assert.AreEqual(1, auto.exitStates.Count);
        }

        [TestMethod]
        public void TestGetToStates()
        {
            var auto = new SearchAutomaton<int>();
            var t1 = new AdvancedTransition<int>(0, ' ', 1);
            var t2 = new AdvancedTransition<int>(0, ' ', 2);
            var t3 = new AdvancedTransition<int>(0, 'E', 3);
            auto.AddTransition(t1);
            auto.AddTransition(t2);
            var states = auto.GetToStates(0, ' ');
            Assert.AreEqual(2, states.Count);
            Assert.IsTrue(states.Contains(t1.ToState));
            Assert.IsTrue(states.Contains(t2.ToState));
            Assert.IsFalse(states.Contains(t3.ToState));
        }

        [TestMethod]
        public void TestDFACorrect()
        {
            var auto = new SearchAutomaton<int>();
            var t1 = new AdvancedTransition<int>(0, ' ', 1);
            var t2 = new AdvancedTransition<int>(0, ' ', 2, true);
            auto.AddTransition(t1);
            auto.AddTransition(t2);
            auto.DefineAsStartState(0);
            auto.DefineAsFinalState(1);
            auto.DefineAsFinalState(2);
            Assert.IsTrue(auto.IsDFA());
        }

        [TestMethod]
        public void TestDFAMultipleStarts()
        {
            var auto = new SearchAutomaton<int>();
            var t1 = new AdvancedTransition<int>(0, ' ', 1);
            var t2 = new AdvancedTransition<int>(0, ' ', 2, true);
            auto.AddTransition(t1);
            auto.AddTransition(t2);
            auto.DefineAsStartState(0);
            auto.DefineAsStartState(1);
            auto.DefineAsFinalState(2);
            Assert.IsFalse(auto.IsDFA());
        }

        [TestMethod]
        public void TestDFADuplicateCharacter()
        {
            var auto = new SearchAutomaton<int>();
            var t1 = new AdvancedTransition<int>(0, ' ', 1);
            var t2 = new AdvancedTransition<int>(0, ' ', 2);
            auto.AddTransition(t1);
            auto.AddTransition(t2);
            auto.DefineAsStartState(0);
            auto.DefineAsFinalState(1);
            auto.DefineAsFinalState(2);
            Assert.IsFalse(auto.IsDFA());
        }

        [TestMethod]
        public void TestDFADuplicateInvertedTransition()
        {
            var auto = new SearchAutomaton<int>();
            var t1 = new AdvancedTransition<int>(0, ' ', 1);
            var t2 = new AdvancedTransition<int>(0, ' ', 1, true);
            var t3 = new AdvancedTransition<int>(0, ' ', 2, true);
            auto.AddTransition(t1);
            auto.AddTransition(t2);
            auto.AddTransition(t3);
            auto.DefineAsStartState(0);
            auto.DefineAsFinalState(1);
            auto.DefineAsFinalState(2);
            Assert.IsFalse(auto.IsDFA());
        }
        
        //Split function is used in CreateSearchablesAsync
        [TestMethod]
        public async Task TestSplit()
        {
            string testFruit = "Apple banana strawberry";
            var sf = new WordFinder();
            await sf.CreateSearchablesAsync(testFruit);
            
            Assert.AreEqual(3, sf.Searchables.Count);
            Assert.AreEqual(Tuple.Create("Apple", 0), sf.Searchables[0]);
            Assert.AreEqual(Tuple.Create("banana", 6), sf.Searchables[1]);
            Assert.AreEqual(Tuple.Create("strawberry", 13), sf.Searchables[2]);
        }

        [TestMethod]
        public async Task TestAcceptDFAOnlyStartWith()
        {
            string testText = "Hello test world ten ette";
            string search = "te";

            var stringFinder = new WordFinder();
            var searchablesTask = stringFinder.CreateSearchablesAsync(testText);                        
            var automataTask = stringFinder.GenerateStartWithAutomatonAsync(search);
            await Task.WhenAll(searchablesTask, automataTask);
            stringFinder.Find(automataTask.Result);

            Assert.AreEqual(2, stringFinder.Occurrences.Count);
            Assert.AreEqual("test", stringFinder.Occurrences[0].Item1);
            Assert.AreEqual("ten", stringFinder.Occurrences[1].Item1);
        }

        [TestMethod]
        public async Task TestAcceptDFAOnlyContains()
        {
            string testText = "Hello test world ten ette";
            string search = "te";

            var stringFinder = new WordFinder();
            var searchablesTask = stringFinder.CreateSearchablesAsync(testText);
            var automataTask = stringFinder.GenerateContainsAutomatonAsync(search);
            await Task.WhenAll(searchablesTask, automataTask);
            stringFinder.Find(automataTask.Result);

            Assert.AreEqual(3, stringFinder.Occurrences.Count);
            Assert.AreEqual("test", stringFinder.Occurrences[0].Item1);
            Assert.AreEqual("ten", stringFinder.Occurrences[1].Item1);
            Assert.AreEqual("ette", stringFinder.Occurrences[2].Item1);
        }

        [TestMethod]
        public async Task TestAcceptDFAOnlyEndsWith()
        {
            string testText = "Hello test world ten ette";
            string search = "te";

            var stringFinder = new WordFinder();
            var searchablesTask = stringFinder.CreateSearchablesAsync(testText);
            var automataTask = stringFinder.GenerateEndsWithAutomatonAsync(search);
            await Task.WhenAll(searchablesTask, automataTask);
            stringFinder.Find(automataTask.Result);

            Assert.AreEqual(1, stringFinder.Occurrences.Count);
            Assert.AreEqual("ette", stringFinder.Occurrences[0].Item1);
        }
    }
}
