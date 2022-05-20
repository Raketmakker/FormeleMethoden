using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using RegexNotepad.Models;
using System;
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

        private void Find()
        {
            throw new NotImplementedException();
        }

        private void Clear()
        {
            this.StartTb.Clear();
            this.ContainsTb.Clear();
            this.EndTb.Clear();
        }
    }
}
