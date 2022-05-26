using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using RegexNotepad.ApplicationLogic;
using RegexNotepad.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using static RegexNotepad.Models.DataModel;

namespace RegexNotepad.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private TextBox StartTb { get; set; }
        private TextBox ContainsTb { get; set; }
        private TextBox EndTb { get; set; }
        public DataModel DataModel { get; set; } = null;

        public MainWindowViewModel(TextBox startTextbox, TextBox containsTextbox, TextBox endTextbox)
        {
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
            get { return new RelayCommand(() => { Find(); }); }
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
        private async void Find()
        {
            if (this.DataModel.Text == null || 
                (this.DataModel.StartText == null && 
                this.DataModel.ContainsText == null && 
                this.DataModel.EndText == null))
            {
                return;
            }

            StringFinder stringFinder = null;

            switch (this.DataModel.Type)
            {
                case TextType.words:
                    stringFinder = new WordFinder();
                    break;
                //case TextType.sentences:
                //    stringFinder = new SenteceFinder();
                //    break;
                default:
                    stringFinder = new TextFinder();
                    break;
            }
            
            var searchablesTask = stringFinder.CreateSearchablesAsync(this.DataModel.Text);
            var startWithTask = stringFinder.GenerateStartWithAutomatonAsync(this.DataModel.StartText);
            await Task.WhenAll(searchablesTask, startWithTask);
            stringFinder.Find(startWithTask.Result);
            Console.WriteLine(stringFinder.Occurrences);
        }

        private void Clear()
        {
            this.StartTb.Clear();
            this.ContainsTb.Clear();
            this.EndTb.Clear();
        }
    }
}
