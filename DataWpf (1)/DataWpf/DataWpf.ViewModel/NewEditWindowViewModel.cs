using DataWpf.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DataWpf.ViewModel
{
    public class NewEditWindowViewModel : INotifyPropertyChanged
    {

        private Person currentPerson;
        private string windowTitle;

        private Mediator mediator;

        public Person CurrentPerson
        {
            get { return currentPerson; }
            set
            {
                if (currentPerson == value)
                {
                    return;
                }
                currentPerson = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrentPerson"));
            }
        }

        public string WindowTitle
        {
            get { return windowTitle; }
            set
            {
                if (windowTitle == value)
                {
                    return;
                }
                windowTitle = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowTitle"));
            }
        }



        public NewEditWindowViewModel(Person person, Mediator mediator)
        {

            this.mediator = mediator;

            SaveCommand = new RelayCommand(SaveExecute, CanSave);

            CurrentPerson = person;
            WindowTitle = "Edit Person"; 
        }

        public NewEditWindowViewModel(Mediator mediator)
        {

            this.mediator = mediator;
            SaveCommand = new RelayCommand(SaveExecute, CanSave);

            CurrentPerson = new Person();
            WindowTitle = "New Person";
        }

        private ICommand saveCommand;

        public ICommand SaveCommand
        {
            get { return saveCommand; }
            set
            {
                if (saveCommand == value)
                {
                    return;
                }
                saveCommand = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SaveCommand"));
            }
        }

        void SaveExecute(object obj)
        {

            if (CurrentPerson != null && !CurrentPerson.HasErrors)
            {
                CurrentPerson.Save();
                OnDone(new DoneEventArgs("Person Saved."));

                mediator.Notify("PersonChange", CurrentPerson);
            }
            else
            {
                OnDone(new DoneEventArgs("Check your input."));
            }
        }

        bool CanSave(object obj)
        {
            return true;
        }

        public delegate void DoneEventHandler(object sender, DoneEventArgs e);

        public class DoneEventArgs : EventArgs
        {
            private string message;

            public string Message
            {
                get { return message; }
                set
                {
                    if (message == value)
                    {
                        return;
                    }
                    message = value;
                }
            }

            public DoneEventArgs(string message)
            {
                this.message = message;
            }
        }


        public event DoneEventHandler Done;

        public void OnDone(DoneEventArgs e)
        {
            if(Done != null)
            {
                Done(this, e);
            }
        }





        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
    }
}
