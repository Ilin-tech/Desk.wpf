using DataWpf.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace DataWpf.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {

        #region Fields

        private Person currentPerson;
        private PersonCollection personList;
        private ListCollectionView personListView;

        private string filteringText;

        private Mediator mediator;


        #endregion

        #region Properties

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

        public PersonCollection PersonList
        {
            get { return personList; }
            set
            {
                if (personList == value)
                {
                    return;
                }
                personList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PersonList"));
            }
        }

        public ListCollectionView PersonListView
        {
            get { return personListView; }
            set
            {
                if (personListView == value)
                {
                    return;
                }
                personListView = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PersonListView"));
            }
        }

        public String FilteringText
        {
            get { return filteringText; }
            set
            {
                if (filteringText == value)
                {
                    return;
                }
                filteringText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilteringText"));
            }
        }


        #endregion


        private ICommand deleteCommand;

        public ICommand DeleteCommand
        {
            get { return deleteCommand; }
            set
            {
                if (deleteCommand == value)
                {
                    return;
                }
                deleteCommand = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeleteCommand"));
            }
        }

        void DeleteExecute(object obj)
        {
            CurrentPerson.DeletePerson();
            PersonList.Remove(CurrentPerson);
        }

        bool CanDelete(object obj)
        {

            if (CurrentPerson == null) return false;

            return true;
        }



        #region Constructors

        public MainWindowViewModel(Mediator mediator)
        {

            this.mediator = mediator;

            DeleteCommand = new RelayCommand(DeleteExecute, CanDelete);

            this.PropertyChanged += MainWindowViewModel_PropertyChanged;

            PersonList = PersonCollection.GetAllPersons();

            PersonListView = new ListCollectionView(PersonList);
            PersonListView.Filter = PersonFiler;

            CurrentPerson = new Person();

            mediator.Register("PersonChange", PersonChanged);

        }

        private void PersonChanged(object obj)
        {
            Person person = (Person)obj;

            int index = PersonList.IndexOf(person);

            if(index != -1)
            {
                PersonList.RemoveAt(index);
                PersonList.Insert(index, person);
            }
            else
            {
                PersonList.Add(person);
            }


        }

        private void MainWindowViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName.Equals("FilteringText"))
            {
                PersonListView.Refresh();
            }
        }

        private bool PersonFiler(object obj)
        {
            if (FilteringText == null) return true;
            if (FilteringText.Equals("")) return true;

            Person person = obj as Person;
            return (person.FirstName.ToLower().StartsWith(FilteringText.ToLower()) || person.LastName.ToLower().StartsWith(FilteringText.ToLower()));
        }


        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
    }
}
