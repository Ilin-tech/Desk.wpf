using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataWpf.Model
{
    public class Person : INotifyPropertyChanged, INotifyDataErrorInfo
    {

        #region Fields

        private int _id;
        private string _firstName;
        private string _lastName;
        private DateTime? _dateOfBirth;
        private Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }



        #endregion

        #region Properties

        public int Id
        {
            get { return _id; }
            set
            {
                if (_id == value)
                {
                    return;
                }
                _id = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Id"));

            }
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (_firstName == value)
                {
                    return;
                }
                _firstName = value;

                List<string> errors = new List<string>();
                bool valid = true;

                if (value == null || value == "")
                {
                    errors.Add("First name can't be empty.");
                    SetErrors("FirstName", errors);
                    valid = false;
                }


                if (!Regex.Match(value, @"^[a-zA-Z]+$").Success)
                {
                    errors.Add("First Name can only contain letters.");
                    SetErrors("FirstName", errors);
                    valid = false;
                }

                if (valid)
                {
                    ClearErrors("FirstName");
                }


                OnPropertyChanged(new PropertyChangedEventArgs("FirstName"));
            }
        }
        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (_lastName == value)
                {
                    return;
                }
                _lastName = value;

                List<string> errors = new List<string>();
                bool valid = true;

                if (value == null || value == "")
                {
                    errors.Add("Last name can't be empty.");
                    SetErrors("LastName", errors);
                    valid = false;
                }


                if (!Regex.Match(value, @"^[a-zA-Z]+$").Success)
                {
                    errors.Add("Last Name can only contain letters.");
                    SetErrors("LastName", errors);
                    valid = false;
                }

                if (valid)
                {
                    ClearErrors("LastName");
                }


                OnPropertyChanged(new PropertyChangedEventArgs("LastName"));
            }
        }

        public DateTime? DateOfBirth
        {
            get { return _dateOfBirth; }
            set
            {
                _dateOfBirth = value;

                List<string> errors = new List<string>();
                bool valid = true;


                if(value == null)
                {
                    errors.Add("Date Of Birth can be empty.");
                    SetErrors("DateOfBirth", errors);
                    valid = false;
                }


                if (value < new DateTime(1880, 12, 12))
                {
                    errors.Add("Date Of Birth can not be before December 12th of 1880.");
                    SetErrors("DateOfBirth", errors);
                    valid = false;
                }
                if (value > DateTime.Now)
                {
                    errors.Add("Date is in future.");
                    SetErrors("DateOfBirth", errors);
                    valid = false;
                }

                if (valid)
                {
                    ClearErrors("DateOfBirth");
                }



                OnPropertyChanged(new PropertyChangedEventArgs("DateOfBirth"));
            }
        }

        public bool HasErrors
        {
            get
            {
                return (errors.Count > 0);
            }
        }


        #endregion

        #region Constructor

        public Person(string FirstName, string LastName, DateTime DateOfBirth)
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.DateOfBirth = DateOfBirth;
        }

        public Person(int Id, string FirstName, string LastName, DateTime DateOfBirth)
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.DateOfBirth = DateOfBirth;
            this.Id = Id;
        }

        public Person()
        {
            FirstName = "";
            LastName = "";
            DateOfBirth = null;
        }


        #endregion

        #region Data Access


        public static Person GetPersonFromResultSet(SqlDataReader reader)
        {
            Person person = new Person((int)reader["id"], (string)reader["first_name"], (string)reader["last_name"], (DateTime)reader["date_of_birth"]);
            return person;
        }



        public void DeletePerson()
        {
            using (SqlConnection conn = new SqlConnection())
            {

                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ToString();
                conn.Open();

                SqlCommand command = new SqlCommand("UPDATE Person SET is_deleted=1 WHERE id=@Id", conn);

                SqlParameter myParam = new SqlParameter("@Id", SqlDbType.Int, 11);
                myParam.Value = this.Id;

                command.Parameters.Add(myParam);

                int rows = command.ExecuteNonQuery();

            }
        }

        public void UpdatePerson()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ToString();
                conn.Open();

                SqlCommand command = new SqlCommand("UPDATE Person SET first_name=@FirstName, last_name=@LastName, date_of_birth=@DateOfBirth WHERE id=@Id", conn);

                SqlParameter firstNameParam = new SqlParameter("@FirstName", SqlDbType.NVarChar);
                firstNameParam.Value = this.FirstName;

                SqlParameter lastNameParam = new SqlParameter("@LastName", SqlDbType.NVarChar);
                lastNameParam.Value = this.LastName;

                SqlParameter dateOfBirthParam = new SqlParameter("@DateOfBirth", SqlDbType.Date);
                dateOfBirthParam.Value = this.DateOfBirth;

                SqlParameter myParam = new SqlParameter("@Id", SqlDbType.Int, 11);
                myParam.Value = this.Id;

                command.Parameters.Add(firstNameParam);
                command.Parameters.Add(lastNameParam);
                command.Parameters.Add(dateOfBirthParam);
                command.Parameters.Add(myParam);

                int rows = command.ExecuteNonQuery();

            }
        }


        public void Insert()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ToString();
                conn.Open();

                SqlCommand command = new SqlCommand("INSERT INTO Person(first_name, last_name, date_of_birth, is_deleted) VALUES(@FirstName, @LastName, @DateOfBirth, 0); SELECT IDENT_CURRENT('Person');", conn);

                SqlParameter firstNameParam = new SqlParameter("@FirstName", SqlDbType.NVarChar);
                firstNameParam.Value = this.FirstName;

                SqlParameter lastNameParam = new SqlParameter("@LastName", SqlDbType.NVarChar);
                lastNameParam.Value = this.LastName;

                SqlParameter dateOfBirthParam = new SqlParameter("@DateOfBirth", SqlDbType.Date);
                dateOfBirthParam.Value = this.DateOfBirth;

                command.Parameters.Add(firstNameParam);
                command.Parameters.Add(lastNameParam);
                command.Parameters.Add(dateOfBirthParam);

                var id = command.ExecuteScalar();

                if (id != null)
                {
                    this.Id = Convert.ToInt32(id);
                }

            }
        }

        public void Save()
        {
            if(Id == 0)
            {
                Insert();
            }
            else
            {
                UpdatePerson();
            }
        }

        private void SetErrors(string propertyName, List<string> propertyErrors)
        {
            // Clear any errors that already exist for this property.
            errors.Remove(propertyName);
            // Add the list collection for the specified property.
            errors.Add(propertyName, propertyErrors);
            // Raise the error-notification event.
            if (ErrorsChanged != null)
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }
        private void ClearErrors(string propertyName)
        {
            // Remove the error list for this property.
            errors.Remove(propertyName);
            // Raise the error-notification event.
            if (ErrorsChanged != null)
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }



        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                // Provide all the error collections.
                return (errors.Values);
            }
            else
            {
                // Provice the error collection for the requested property
                // (if it has errors).
                if (errors.ContainsKey(propertyName))
                {
                    return (errors[propertyName]);
                }
                else
                {
                    return null;
                }
            }

        }

        public Person Clone()
        {
            Person clonedPerson = new Person();
            clonedPerson.FirstName = this.FirstName;
            clonedPerson.LastName = this.LastName;
            clonedPerson.DateOfBirth = this.DateOfBirth;
            clonedPerson.Id = this.Id;

            return clonedPerson;
        }

        // override object.Equals
        public override bool Equals(object obj)
        {

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Person objPerson = (Person)obj;

            if (objPerson.Id == this.Id) return true;

            return false;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }



        #endregion

    }

}
