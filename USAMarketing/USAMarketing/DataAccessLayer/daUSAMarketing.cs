using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;


namespace USAMarketing.DataAccessLayer
{
    public class daUSAMarketing
    {
        //Be sure to add using for System.Data.SqlClient
        //Constructor method for Data Access Layer
        public daUSAMarketing()
        {
            //Initialize properties
            pTransactionSuccessful = true;
            pErrorMessage = "";
        }

        #region "Properties"
        //These properties are for bubbling up error messages from the Data Tier
        private bool pTransactionSuccessful;
        public bool TransactionSuccessful
        {
            get { return pTransactionSuccessful; }
            set { pTransactionSuccessful = value; }
        }
        private string pErrorMessage;
        public string ErrorMessage
        {
            get { return pErrorMessage; }
        }
        #endregion

        #region "Get Methods"

        public DataTable GetItemModel(string ConnectionString)
        {
            /*
             * There are many, many ways to do the following. This is perhaps the most
             * common and is consistent with Microsoft Patterns and Practices.
             * It is good practice to put your Connection Object inside a using(){}
             * statement. This ensures that the connection is properly disposed.
             * It is possible to exhaust the connection pool and create a DOS
             * situation if connection objects are not properly disposed.
             * We explictly open the connection but the using statement closes it.
             */

            /*
             * Create a DataTable to return. 
             * This has to be outside the using(){} because the return statement is outside.
             * We could just return the entire DataSet but there is no
             * reason to send more than is needed. a DataSet can be very large and if only a single
             * DataTable is required, send only what is required.
             */

            DataTable dtItem = new DataTable("dtItem");

            // Create & open a SqlConnection, and dispose of it after we are done
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Create a command and prepare it for execution by the data tier
                SqlCommand command = new SqlCommand();

                // Associate the connection with the command. There could be more than one connection open.
                command.Connection = connection;

                // Set the command type: sproc, return an entire table, supply SQL string
                command.CommandType = CommandType.StoredProcedure;

                //In this case, the Command Text, meaning the command to be executed by the data tier
                //is the name of the sproc. We know this because the command type is storedprocedure
                //If the type were "Text" we would put the SQL expression here.
                command.CommandText = "GetItem";

                // Create the DataAdapter & DataSet
                using (SqlDataAdapter da = new SqlDataAdapter(command))
                {
                    DataSet ds = new DataSet();

                    //This try catch block handles errors where the sproc (data tier) does not return a result
                    try
                    {
                        // Fill the DataSet using default values for DataTable names, etc
                        da.Fill(ds);

                        // Extract the first table (and only table) from the dataset
                        dtItem = ds.Tables[0];
                    }
                    catch (SqlException ReadError)
                    {
                        //If the sproc didn't return a result, put the error message in dtItem
                        DataRow ErrorRow = dtItem.NewRow();
                        dtItem.Columns.Add("ErrorMessage");
                        ErrorRow["ErrorMessage"] = ReadError.Message.ToString();
                        dtItem.Columns.Add("ErrorLineNumber");
                        ErrorRow["ErrorLineNumber"] = ReadError.Message.ToString();
                        dtItem.Rows.Add(ErrorRow);

                        pTransactionSuccessful = false;
                    }
                }
            }
            return dtItem;
        }

        public DataTable GetLineItemModel(string ConnectionString)
        {
            DataTable dtLineItem = new DataTable("dtLineItem");

            // Create & open a SqlConnection, and dispose of it after we are done
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Create a command and prepare it for execution by the data tier
                SqlCommand command = new SqlCommand();

                // Associate the connection with the command. There could be more than one connection open.
                command.Connection = connection;

                // Set the command type: sproc, return an entire table, supply SQL string
                command.CommandType = CommandType.StoredProcedure;

                //In this case, the Command Text, meaning the command to be executed by the data tier
                //is the name of the sproc. We know this because the command type is storedprocedure
                //If the type were "Text" we would put the SQL expression here.
                command.CommandText = "GetLineItem";

                // Create the DataAdapter & DataSet
                using (SqlDataAdapter da = new SqlDataAdapter(command))
                {
                    DataSet ds = new DataSet();

                    //This try catch block handles errors where the sproc (data tier) does not return a result
                    try
                    {
                        // Fill the DataSet using default values for DataTable names, etc
                        da.Fill(ds);

                        // Extract the first table (and only table) from the dataset
                        dtLineItem = ds.Tables[0];
                    }
                    catch (SqlException ReadError)
                    {
                        //If the sproc didn't return a result, put the error message in dtSubCategory
                        DataRow ErrorRow = dtLineItem.NewRow();
                        dtLineItem.Columns.Add("ErrorMessage");
                        ErrorRow["ErrorMessage"] = ReadError.Message.ToString();
                        dtLineItem.Columns.Add("ErrorLineNumber");
                        ErrorRow["ErrorLineNumber"] = ReadError.Message.ToString();
                        dtLineItem.Rows.Add(ErrorRow);

                        pTransactionSuccessful = false;
                    }
                }
            }
            return dtLineItem;
        }

        #endregion

        #region "Insert Methods"

        public void AddItem(string ItemNumber, string Description, string UnitPrice, string ConnectionString)
        {
            // Create & open a SqlConnection, and dispose of it after we are done
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Create a command and prepare it for execution by the data tier
                SqlCommand command = new SqlCommand();

                // Associate the connection with the command. There could be more than one connection open.
                command.Connection = connection;

                // Set the command type: sproc, return an entire table, supply SQL string
                command.CommandType = CommandType.StoredProcedure;

                //In this case, the Command Text, meaning the command to be executed by the data tier
                //is the name of the sproc. We know this because the command type is storedprocedure
                //If the type were "Text" we would put the SQL expression here.
                command.CommandText = "AddItem";

                command.Parameters.Add("@ItemNumber", SqlDbType.Char).Value = ItemNumber;
                command.Parameters["@ItemNumber"].Direction = ParameterDirection.Input;
                command.Parameters.Add("@Description", SqlDbType.VarChar).Value = Description;
                command.Parameters["@Description"].Direction = ParameterDirection.Input;
                command.Parameters.Add("@UnitPrice", SqlDbType.Money).Value = UnitPrice;
                command.Parameters["@UnitPrice"].Direction = ParameterDirection.Input;

                //This try catch block handles errors where the sproc (data tier) does not return a result
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException InsertError)
                {
                    //If the sproc didn't return a result, put the error message in dtJob
                    pErrorMessage = InsertError.Message.ToString();
                    pTransactionSuccessful = false;
                }

            }
        }

        public void AddLineItem(int Quantity, string UnitPrice, string Amount, int InvoiceID, int ItemID, string ConnectionString)
        {
            // Create & open a SqlConnection, and dispose of it after we are done
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Create a command and prepare it for execution by the data tier
                SqlCommand command = new SqlCommand();

                // Associate the connection with the command. There could be more than one connection open.
                command.Connection = connection;

                // Set the command type: sproc, return an entire table, supply SQL string
                command.CommandType = CommandType.StoredProcedure;

                //In this case, the Command Text, meaning the command to be executed by the data tier
                //is the name of the sproc. We know this because the command type is storedprocedure
                //If the type were "Text" we would put the SQL expression here.
                command.CommandText = "AddLineItem";

                command.Parameters.Add("@Quantity", SqlDbType.Int).Value = Quantity;
                command.Parameters["@Quantity"].Direction = ParameterDirection.Input;
                command.Parameters.Add("@UnitPrice", SqlDbType.Money).Value = UnitPrice;
                command.Parameters["@UnitPrice"].Direction = ParameterDirection.Input;
                command.Parameters.Add("@Amount", SqlDbType.Money).Value = Amount;
                command.Parameters["@Amount"].Direction = ParameterDirection.Input;
                command.Parameters.Add("@InvoiceID", SqlDbType.Int).Value = InvoiceID;
                command.Parameters["@InvoiceID"].Direction = ParameterDirection.Input;
                command.Parameters.Add("@ItemID", SqlDbType.Int).Value = ItemID;
                command.Parameters["@ItemID"].Direction = ParameterDirection.Input;

                //This try catch block handles errors where the sproc (data tier) does not return a result
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException InsertError)
                {
                    //If the sproc didn't return a result, put the error message in dtJob
                    pErrorMessage = InsertError.Message.ToString();
                    pTransactionSuccessful = false;
                }

            }
        }

        #endregion

        #region "Update Methods"

        public void UpdateItem(int ItemID, string ItemNumber, string Description, string UnitPrice, string ConnectionString)
        {
            // Create & open a SqlConnection, and dispose of it after we are done
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Create a command and prepare it for execution by the data tier
                SqlCommand command = new SqlCommand();

                // Associate the connection with the command. There could be more than one connection open.
                command.Connection = connection;

                // Set the command type: sproc, return an entire table, supply SQL string
                command.CommandType = CommandType.StoredProcedure;

                //In this case, the Command Text, meaning the command to be executed by the data tier
                //is the name of the sproc. We know this because the command type is storedprocedure
                //If the type were "Text" we would put the SQL expression here.
                command.CommandText = "UpdateItem";

                command.Parameters.Add("@ItemID", SqlDbType.Int).Value = ItemID;
                command.Parameters["@ItemID"].Direction = ParameterDirection.Input;
                command.Parameters.Add("@ItemNumber", SqlDbType.Char).Value = ItemNumber;
                command.Parameters["@ItemNumber"].Direction = ParameterDirection.Input;
                command.Parameters.Add("@Description", SqlDbType.VarChar).Value = Description;
                command.Parameters["@Description"].Direction = ParameterDirection.Input;
                command.Parameters.Add("@UnitPrice", SqlDbType.Money).Value = UnitPrice;
                command.Parameters["@UnitPrice"].Direction = ParameterDirection.Input; 

                //This try catch block handles errors where the sproc (data tier) does not return a result
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException UpdateError)
                {
                    //If the sproc didn't return a result, put the error message in dtJob
                    pErrorMessage = UpdateError.Message.ToString();
                    pTransactionSuccessful = false;
                }

            }
        }

        public void UpdateLineItem(int LineItemID, int Quantity, string UnitPrice, string Amount, int InvoiceID, int ItemID, string ConnectionString)
        {
            // Create & open a SqlConnection, and dispose of it after we are done
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Create a command and prepare it for execution by the data tier
                SqlCommand command = new SqlCommand();

                // Associate the connection with the command. There could be more than one connection open.
                command.Connection = connection;

                // Set the command type: sproc, return an entire table, supply SQL string
                command.CommandType = CommandType.StoredProcedure;

                //In this case, the Command Text, meaning the command to be executed by the data tier
                //is the name of the sproc. We know this because the command type is storedprocedure
                //If the type were "Text" we would put the SQL expression here.
                command.CommandText = "UpdateLineItem";

                command.Parameters.Add("@LineItemID", SqlDbType.Int).Value = LineItemID;
                command.Parameters["@LineItemID"].Direction = ParameterDirection.Input;
                command.Parameters.Add("@Quantity", SqlDbType.Int).Value = Quantity;
                command.Parameters["@Quantity"].Direction = ParameterDirection.Input;
                command.Parameters.Add("@UnitPrice", SqlDbType.Money).Value = UnitPrice;
                command.Parameters["@UnitPrice"].Direction = ParameterDirection.Input;
                command.Parameters.Add("@Amount", SqlDbType.Money).Value = Amount;
                command.Parameters["@Amount"].Direction = ParameterDirection.Input;
                command.Parameters.Add("@InvoiceID", SqlDbType.Int).Value = InvoiceID;
                command.Parameters["@InvoiceID"].Direction = ParameterDirection.Input;
                command.Parameters.Add("@ItemID", SqlDbType.Int).Value = ItemID;
                command.Parameters["@ItemID"].Direction = ParameterDirection.Input;


                //This try catch block handles errors where the sproc (data tier) does not return a result
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException UpdateError)
                {
                    //If the sproc didn't return a result, put the error message in dtJob
                    pErrorMessage = UpdateError.Message.ToString();
                    pTransactionSuccessful = false;
                }

            }
        }

        #endregion

        #region "Delete Methods"

        public void DeleteItem(int ItemID, string ConnectionString)
        {

            // Create & open a SqlConnection, and dispose of it after we are done
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Create a command and prepare it for execution by the data tier
                SqlCommand command = new SqlCommand();

                // Associate the connection with the command. There could be more than one connection open.
                command.Connection = connection;

                // Set the command type: sproc, return an entire table, supply SQL string
                command.CommandType = CommandType.StoredProcedure;

                //In this case, the Command Text, meaning the command to be executed by the data tier
                //is the name of the sproc. We know this because the command type is storedprocedure
                //If the type were "Text" we would put the SQL expression here.
                command.CommandText = "DeleteItem";

                //Add parameter(s) to the command object. These must match the parameters in the sproc signature
                command.Parameters.Add("@ItemID", SqlDbType.Int).Value = ItemID;

                //Set parameter direction: input, input/output, or output (must match sproc signature)
                command.Parameters["@ItemID"].Direction = ParameterDirection.Input;

                //This try catch block handles errors where the sproc (data tier) does not return a result
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException DeleteError)
                {
                    //If the sproc didn't return a result, put the error message in dtJob
                    pErrorMessage = DeleteError.Message.ToString();
                    pTransactionSuccessful = false;
                }
            }
        }

        public void DeleteLineItem(int LineItemID, string ConnectionString)
        {

            // Create & open a SqlConnection, and dispose of it after we are done
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Create a command and prepare it for execution by the data tier
                SqlCommand command = new SqlCommand();

                // Associate the connection with the command. There could be more than one connection open.
                command.Connection = connection;

                // Set the command type: sproc, return an entire table, supply SQL string
                command.CommandType = CommandType.StoredProcedure;

                //In this case, the Command Text, meaning the command to be executed by the data tier
                //is the name of the sproc. We know this because the command type is storedprocedure
                //If the type were "Text" we would put the SQL expression here.
                command.CommandText = "DeleteLineItem";

                //Add parameter(s) to the command object. These must match the parameters in the sproc signature
                command.Parameters.Add("@LineItemID", SqlDbType.Int).Value = LineItemID;

                //Set parameter direction: input, input/output, or output (must match sproc signature)
                command.Parameters["@LineItemID"].Direction = ParameterDirection.Input;

                //This try catch block handles errors where the sproc (data tier) does not return a result
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException DeleteError)
                {
                    //If the sproc didn't return a result, put the error message in dtJob
                    pErrorMessage = DeleteError.Message.ToString();
                    pTransactionSuccessful = false;
                }
            }
        }
        #endregion
    }
}