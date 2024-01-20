using ForecastDesign.Commands;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using WpfApp1.Services;

namespace WpfApp1.viewmodel
{
    internal class MainWindowViewModel
    {
        public ObservableCollection<string> CategoriName { get; set; } = new();
        public ObservableCollection<string> AuthorName { get; set; } = new();
        public ICommand CategoriCBselect { get; set; }
        public ICommand BooksSearchBtn { get; set; }
        public ICommand BooksDeleteBtn { get; set; }
        public ICommand CategoriInsert { get; set; }
        public ICommand CategoriUpdate { get; set; }
        public MainWindowViewModel()
        {
            startdataCategori();
            startdataAutors();
            CategoriCBselect = new Command(execCategori, canexecCategori);
            BooksSearchBtn = new Command(execBooksSearch, canexecBooksSearch);
            BooksDeleteBtn = new Command(execBooksDelete, canexecBooksDelete);
            CategoriInsert = new Command(execCategoriInsert, canexecCategoriInsert);
            CategoriUpdate = new Command(execCategoriUpdate, canexecCategoriUpdate);

        }


        private bool canexecCategoriUpdate(object obj)
        {
            return true;
        }

        private void execCategoriUpdate(object obj)
        {
            Grid gg = ((Grid)obj);
            string name = ((TextBox)gg.FindName("CategoriUpdeteNameSaveTb")).Text;
            int Id = int.Parse(((TextBox)gg.FindName("CategoriUpdeteIdTb")).Text);
            SqlConnection sqlConnection = new SqlConnection(Conficuration.getConficration("ConnectionStrings", "connectionServis"));
            sqlConnection.Open();
            string selectQuery = @"Update Categories Set Name=@Name WHERE Id=@Id";
            SqlCommand sqlCommand = new SqlCommand(selectQuery, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@Id", Id);
            sqlCommand.Parameters.AddWithValue("@Name", name);
            sqlCommand.ExecuteNonQuery();
            MessageBox.Show("UPDATE");
        }



        private bool canexecCategoriInsert(object obj)
        {
            return true;
        }

        private void execCategoriInsert(object obj)
        {
            Grid gg = ((Grid)obj);
            string name = ((TextBox)gg.FindName("booksInsertNameTb")).Text;
            int Id = int.Parse(((TextBox)gg.FindName("booksInsertIdTb")).Text);
            SqlConnection sqlConnection = new SqlConnection(Conficuration.getConficration("ConnectionStrings", "connectionServis"));
            sqlConnection.Open();
            string selectQuery = @"insert into Categories(Id,Name) values (@Id,@Name)";
            SqlCommand sqlCommand = new SqlCommand(selectQuery, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@Id", Id);
            sqlCommand.Parameters.AddWithValue("@Name", name);
            sqlCommand.ExecuteNonQuery();
            MessageBox.Show("Insert");
            
        }


        private bool canexecBooksDelete(object obj)
        {
            return true;
        }

        private void execBooksDelete(object obj)
        {
            try
            {
                int Id = int.Parse(obj.ToString());
                SqlConnection sqlConnection = new SqlConnection(Conficuration.getConficration("ConnectionStrings", "connectionServis"));
                sqlConnection.Open();
                string selectQuery = @"delete from Categories where id=@Name";
                SqlCommand sqlCommand = new SqlCommand(selectQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Name", Id);
                sqlCommand.ExecuteNonQuery();

                MessageBox.Show("Delete");


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }







        private bool canexecCategori(object obj)
        {
            return true;
        }

        private void execCategori(object obj)
        {
            ((ComboBox)obj).IsEnabled = true;
        }




        private bool canexecBooksSearch(object obj)
        {
            return true;
        }

        private void execBooksSearch(object obj)
        {
            string aa = ((string)obj);
            SqlConnection sqlConnection = new SqlConnection(Conficuration.getConficration("ConnectionStrings", "connectionServis"));
            sqlConnection.Open();
            string selectQuery = @"Select * from Categories where name=@Name";
            SqlCommand sqlCommand = new SqlCommand(selectQuery, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@Name", aa);
            SqlDataReader reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {

                MessageBox.Show("Id " + reader[0].ToString() + "\nName " + reader[1].ToString());
            }

        }










        public void startdataCategori()
        {
            SqlConnection sqlConnection = new SqlConnection(Conficuration.getConficration("ConnectionStrings", "connectionServis"));
            sqlConnection.Open();
            string selectQuery = "select name from Categories;";
            SqlCommand sqlCommand = new SqlCommand(selectQuery, sqlConnection);
            SqlDataReader reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                CategoriName.Add(reader[0].ToString());
            }



        }

        public void startdataAutors()
        {
            SqlConnection sqlConnection = new SqlConnection(Conficuration.getConficration("ConnectionStrings", "connectionServis"));
            sqlConnection.Open();
            string selectQuery = "select FirstName from Authors";
            SqlCommand sqlCommand = new SqlCommand(selectQuery, sqlConnection);
            SqlDataReader reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                AuthorName.Add(reader[0].ToString());
            }

        }

        /*
        
         using System.Data.Common;
        using Microsoft.Data.SqlClient;
        // ConnectionString
        
        var conStr = 
            "Data Source=STHQ0118-01;Initial Catalog=Library;User ID=admin;Password=admin;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;";
        
        // way 1
        //SqlConnection sqlConnection = new SqlConnection(conStr);
        
        
        //// way 2
        //SqlConnection sqlConnection = new SqlConnection();
        //sqlConnection.ConnectionString = conStr;
        
        
        // examples
        // InsertDataToLiblaryWithADO();
        // ReadDataFromLiblaryWithADO();
        // MultiReadDataFromLiblaryWithADO();
        ReadWithParamDataFromLiblaryWithADO("Cavid");
        
        
        // Inser
        void InsertDataToLiblaryWithADO()
        {
            SqlConnection sqlConnection = new SqlConnection(conStr);
        	try
        	{
        		sqlConnection.Open();
        		string insertQuery = "INSERT INTO Authors (Id, FirstName, LastName) VALUES (155, 'Cavid', 'Atamoghlanov')";
        		SqlCommand sqlCommand = new SqlCommand(insertQuery, sqlConnection);
        
        		sqlCommand.ExecuteNonQuery();  // Bize data qaytarmir. sadece ishi icra edir. Insert, Update, Delete ucun istifade edilir.
        
        		//sqlCommand.ExecuteReader(); // Bize data qaytarir. ishi icra edir ve data qaytarir. Table qaytarir. Birden cox data qaytarir. select query ucun istifade edilir.
        
        		//sqlCommand.ExecuteScalar(); // eyer sadece bir cavab bir netice gelirse bunu istifade edirik
        	}
        	catch (Exception ex)
        	{
                Console.WriteLine(ex.Message);
            }
        	finally
        	{
        		sqlConnection.Close();
        	}
        }

// read data from db
void ReadDataFromLiblaryWithADO()
{
    SqlConnection sqlConnection = new SqlConnection(conStr);
    try
    {
        sqlConnection.Open();
        string selectQuery = "SELECT * FROM books";
        SqlCommand sqlCommand = new SqlCommand(selectQuery, sqlConnection);
        SqlDataReader reader =  sqlCommand.ExecuteReader();
        {
            //bool line = false;
            //while (reader.Read())
            //{
            //    if (line == false)
            //    {
            //        Console.WriteLine($"{reader.GetName(0)}     {reader.GetName(1)}    {reader.GetName(2)}");
            //        line = true;
            //    }
            //    //Console.WriteLine($"{reader[0]}     {reader[1]}    {reader[2]}");
            //    Console.WriteLine($"{reader["Id"]}     {reader["LastName"]}    {reader["FirstName"]}");
            //}
        }
        bool line = false;
        while (reader.Read())
        {
            if (line == false)
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write(reader.GetName(i) + "");
                }
                Console.WriteLine();
                line = true;
            }

            for (int i = 0; i < reader.FieldCount; i++)
            {
                Console.Write($"{reader[i]}");
            }
            Console.WriteLine();
            
        }

    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
    finally
    {
        sqlConnection.Close();
    }
}

// Multi select query
void MultiReadDataFromLiblaryWithADO()
{
    SqlConnection sqlConnection = new SqlConnection(conStr);
    try
    {
        sqlConnection.Open();
        string selectQuery = "SELECT * FROM Authors; SELECT * FROM Books";
        SqlCommand sqlCommand = new SqlCommand(selectQuery, sqlConnection);
        SqlDataReader reader = sqlCommand.ExecuteReader();

        do
        {
        bool line = false;
            while (reader.Read())
            {
                if (!line)
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write($"{reader.GetName(i)}   ");
                    }
                    Console.WriteLine();
                    line = !line;
                }

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write($"{reader[i]}   ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }
        while (reader.NextResult());



    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
    finally
    {
        sqlConnection.Close();
    }
}

// sql  injection
// read data from db  with param
void ReadWithParamDataFromLiblaryWithADO(string name)
{
    SqlConnection sqlConnection = new SqlConnection(conStr);
    try
    {
        sqlConnection.Open();
        string selectQuery = $"SELECT * FROM Authors WHERE FirstName = @cavid";
        SqlCommand sqlCommand = new SqlCommand(selectQuery, sqlConnection);
        // way 1
        sqlCommand.Parameters.AddWithValue("@cavid", name);

        SqlDataReader reader = sqlCommand.ExecuteReader();
       
        bool line = false;
        while (reader.Read())
        {
            if (line == false)
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write(reader.GetName(i) + "  ");
                }
                Console.WriteLine();
                line = true;
            }

            for (int i = 0; i < reader.FieldCount; i++)
            {
                Console.Write($"{reader[i]}  ");
            }
            Console.WriteLine();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
    finally
    {
        sqlConnection.Close();
    }
}


/*
 ** TASK **
    Verilenler Bazamiz ucun kitablar cedvelindeki Butun Kitablarin qiymetlerinin cemini ve butun kitablarin
    sehifelerinin cemini hesablayan bir console application yazin.
    HELP: ExecuteNonQuery() -den istifade edin. (Insert, Update, Delete ucun istifade edilir.)
          ExecuteReader()   -den istifade edin. (Select ucun istifade edilir.)
          ExecuteScalar()   -den istifade edin. (1 deyer qaytarirsa istifade edilir.)

    SELECT SUM(Pages) AS TotalPages, SUM(Quantity) AS TotalQuantity FROM Books
    SELECT SUM(Pages) FROM Books
 */

    }
}
