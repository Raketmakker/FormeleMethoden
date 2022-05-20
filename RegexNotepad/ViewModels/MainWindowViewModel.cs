using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using RegexNotepad.Models;
using System.Windows.Input;
using static RegexNotepad.Models.DataModel;

namespace RegexNotepad.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        public DataModel DataModel { get; set; } = null;

        public MainWindowViewModel()
        {
            this.DataModel = new DataModel();
        }

        private void SetTextType(TextType textType)
        {
            this.DataModel.Type = textType;
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
    }
}
