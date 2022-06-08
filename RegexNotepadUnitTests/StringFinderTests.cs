using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegexNotepad.ApplicationLogic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RegexNotepadUnitTests
{
    [TestClass]
    public class StringFinderTests
    {
        [TestMethod]
        public void TestGenerateStartWithAutomatonAsync()
        {
            var sf = new WordFinder();
            var task = sf.GenerateStartWithAutomatonAsync("Hello");
            var automaton = task.Result;
            Assert.IsTrue(automaton.states.Count == 7);
            Assert.IsTrue(automaton.transitions.Count == 12);
            Assert.IsTrue(automaton.startStates.Count == 1);
            Assert.IsTrue(automaton.errorStates.Count == 1);
            Assert.IsTrue(automaton.finalStates.Count == 1);
        }

        [TestMethod]
        public void TestGenerateContainsAutomatonAsync()
        {
            var sf = new WordFinder();
            var task = sf.GenerateContainsAutomatonAsync("Hello");
            var automaton = task.Result;
            Assert.IsTrue(automaton.states.Count == 6);
            Assert.IsTrue(automaton.transitions.Count == 15);
            Assert.IsTrue(automaton.startStates.Count == 1);
            Assert.IsTrue(automaton.finalStates.Count == 1);
        }

        [TestMethod]
        public void TestGenerateEndsWithAutomatonAsync()
        {
            var sf = new WordFinder();
            var task = sf.GenerateEndsWithAutomatonAsync("Hello");
            var automaton = task.Result;
            Assert.IsTrue(automaton.states.Count == 6);
            Assert.IsTrue(automaton.transitions.Count == 16);
            Assert.IsTrue(automaton.startStates.Count == 1);
            Assert.IsTrue(automaton.finalStates.Count == 1);
        }

        [TestMethod]
        public async Task TestFind()
        {
            var sf = new WordFinder();
            await sf.CreateSearchablesAsync("Hello world");
            var automaton = await sf.GenerateContainsAutomatonAsync("Hello");
            sf.Find(automaton);
            Assert.IsTrue(sf.Occurrences.Contains(Tuple.Create("Hello", 0)));
        }

        [TestMethod]
        public async Task TestCreateSearchableWords()
        {
            var sf = new WordFinder();
            await sf.CreateSearchablesAsync("Hello test world");
            Assert.AreEqual(sf.Searchables.Count, 3);
        }

        [TestMethod]
        public async Task TestCreateSearchableSentences()
        {
            var sf = new WordFinder();
            await sf.CreateSearchablesAsync("Hello. Test. World.");
            Assert.AreEqual(sf.Searchables.Count, 3);
        }

        [TestMethod]
        public async Task TestCreateSearchableText()
        {
            var sf = new TextFinder();
            string teststring = "Hello. Test. World.";
            await sf.CreateSearchablesAsync(teststring);
            Assert.AreEqual(sf.Searchables.Count, 1);
            Assert.AreEqual(teststring, sf.Searchables[0].Item1);
        }
    }
}
