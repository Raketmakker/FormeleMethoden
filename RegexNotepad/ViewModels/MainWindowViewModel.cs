using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using RegexNotepad.ApplicationLogic;
using RegexNotepad.Automaton;
using RegexNotepad.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using static RegexNotepad.Models.DataModel;

namespace RegexNotepad.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private RichTextBox MainTb { get; set; }
        private string MainText
        {
            get
            {
                TextRange range = new TextRange(MainTb.Document.ContentStart, MainTb.Document.ContentEnd);
                return range.Text;
            }
            set
            {
                MainTb.Document.Blocks.Clear();
                MainTb.Document.Blocks.Add(new Paragraph(new Run(value)));
            }
        }

        private TextBox StartTb { get; set; }
        private TextBox ContainsTb { get; set; }
        private TextBox EndTb { get; set; }
        public DataModel DataModel { get; set; } = null;

        public MainWindowViewModel(RichTextBox mainTb, TextBox startTextbox, TextBox containsTextbox, TextBox endTextbox)
        {
            this.MainTb = mainTb;
            this.StartTb = startTextbox;
            this.ContainsTb = containsTextbox;
            this.EndTb = endTextbox;
            this.DataModel = new DataModel();
        }
                
        public ICommand SelectWords
        {
            get { return new RelayCommand(() => { SetTextType(TextType.words); }); }
        }

        public ICommand SelectSentences
        {
            get { return new RelayCommand(() => { SetTextType(TextType.sentences); }); }
        }

        public ICommand SelectText
        {
            get { return new RelayCommand(() => { SetTextType(TextType.text); });}
        }

        public ICommand ClearCommand
        {
            get { return new RelayCommand(() => { Clear(); }); }
        }

        public ICommand FindCommand
        {
            get 
            { 
                return new RelayCommand(async () => 
                { 
                    var sf = await Find(); 
                    ColorResult(sf);
                }); 
            }
        }
        
        public ICommand ReplaceCommand
        {
            get { return new RelayCommand(() => { Replace(); }); }
        }

        private void SetTextType(TextType textType)
        {
            this.DataModel.Type = textType;
        }

        /// <summary>
        /// Verzamel text
        /// Bouw automata
        ///     TODO:   Make it work for sentences, text. Combine automatons to search
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private async Task<StringFinder> Find()
        {
            if (this.MainText == null)
                return null;

            if (!(this.DataModel.StartText != null && this.DataModel.StartBoxChecked ||
                this.DataModel.ContainsText != null && this.DataModel.ContainsBoxChecked ||
                this.DataModel.EndText != null && this.DataModel.EndBoxChecked))
            {
                return null;
            }

            StringFinder stringFinder = null;

            switch (this.DataModel.Type)
            {
                case TextType.words:
                    stringFinder = new WordFinder();
                    break;
                case TextType.sentences:
                    stringFinder = new SentenceFinder();
                    break;
                default:
                    stringFinder = new TextFinder();
                    break;
            }
            var searchablesTask = stringFinder.CreateSearchablesAsync(this.MainText);
            Task<SearchAutomaton<int>> searchTask = null;

            if (this.DataModel.StartBoxChecked)
                searchTask = stringFinder.GenerateStartWithAutomatonAsync(this.DataModel.StartText);
            else if (this.DataModel.ContainsBoxChecked)
                searchTask = stringFinder.GenerateContainsAutomatonAsync(this.DataModel.ContainsText);
            else if (this.DataModel.EndBoxChecked)
                searchTask = stringFinder.GenerateEndsWithAutomatonAsync(this.DataModel.EndText);

            await Task.WhenAll(searchablesTask, searchTask);
            stringFinder.Find(searchTask.Result);

            return stringFinder;
        }

        private void Clear()
        {
            this.StartTb.Clear();
            this.ContainsTb.Clear();
            this.EndTb.Clear();
        }

        private async void Replace()
        {
            if (this.DataModel.ReplaceText == null)
                return;

            StringFinder sf = Find().Result;

            if (sf == null)
                return;

            string edited = this.MainText;

            foreach(var occurrence in sf.Occurrences)
            {
                edited = edited.Replace(occurrence.Item1, this.DataModel.ReplaceText);
            }
            this.MainText = edited;
        }

        private void ColorResult(StringFinder sf)
        {
            if (sf == null)
                return;

            if (sf.Occurrences.Count <= 0)
                return;

            int idx = 0;
            string text = this.MainText;

            MainTb.Document.Blocks.Clear();

            foreach (var occ in sf.Occurrences)
            {
                if(occ.Item2 - idx - 1 > 0)
                {
                    TextRange startRange = new TextRange(MainTb.Document.ContentEnd, MainTb.Document.ContentEnd);
                    startRange.Text = text.Substring(idx, occ.Item2 - idx);
                    startRange.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.White);
                }

                TextRange colorRange = new TextRange(MainTb.Document.ContentEnd, MainTb.Document.ContentEnd);
                colorRange.Text = text.Substring(occ.Item2, occ.Item1.Length);
                colorRange.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Red);
                idx = occ.Item2 + occ.Item1.Length;
            }
            int finalStart = sf.Occurrences[sf.Occurrences.Count - 1].Item2 + sf.Occurrences[sf.Occurrences.Count - 1].Item1.Length;
            TextRange lastRange = new TextRange(MainTb.Document.ContentEnd, MainTb.Document.ContentEnd);
            lastRange.Text = text.Substring(finalStart, text.Length - finalStart);
            lastRange.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.White);
        }
    }
}
