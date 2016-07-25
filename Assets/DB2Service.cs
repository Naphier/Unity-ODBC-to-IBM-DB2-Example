using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
//C:\Program Files\IBM\SQLLIB\BIN may need to be in environment path.
using System.Data.Odbc;
public class DB2Service : MonoBehaviour
{
    public InputField conStringInput;
    public InputField queryInput;
    public Text resultsText;
    public GameObject resultsPanel;

    void Start()
    {
        //Test();
        resultsPanel.SetActive(false);
    }

    void Test()
    {       
        // Set up a connection string. The format is pretty specific, just change the YOUR...HERE to real values.
        string connectionStringODBC =
            "Driver={IBM DB2 ODBC DRIVER};Database=YOURDATABASENAMEHERE;Hostname=localhost;Port=50000;Protocol=TCPIP;Uid=YOURUSERNAMEHERE;Pwd=YOURPASSWORDHERE;";
        // Make the connection and open it
        OdbcConnection odbcCon = new OdbcConnection(connectionStringODBC);
        odbcCon.Open();

        // Try out a simple command/query - make sure to change SOMEREALTABLENAME to your table's name
        OdbcCommand command = new OdbcCommand("SELECT COUNT(*) FROM SOMEREALTABLENAME", odbcCon);
        int count = Convert.ToInt32(command.ExecuteScalar());
        Debug.Log("count: " + count);

        // Try a full select query
        OdbcCommand command2 = new OdbcCommand("SELECT * FROM SOMEREALTABLENAME", odbcCon);
        StringBuilder sb = new StringBuilder();
        using (OdbcDataReader reader = command2.ExecuteReader())
        {
            // Add the column names to the string builder
            for (int i = 0; i < reader.FieldCount; i++)
            {
                sb.Append(reader.GetName(i));
                if (i < reader.FieldCount - 1)
                    sb.Append(",");
            }

            sb.AppendLine();

            // Step through the query's results and add those to the string builder.
            while (reader.Read())
            {
                // Separate each column with a comma 
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    sb.Append(reader.GetString(i).Trim());
                    if (i < reader.FieldCount - 1)
                        sb.Append(",");
                }
                sb.AppendLine();

            }

            // Output the results to the console
            Debug.Log(sb.ToString());
        }

        // Close up that connection!
        odbcCon.Close();
    }

    public void MakeQueryButton()
    {
        Debug.LogFormat("conStringInput: {0}\nqueryInput: {1}", conStringInput.text, queryInput.text);
        MakeQuery(conStringInput.text, queryInput.text);
    }

    public void MakeQuery(string connectionString, string query)
    {
        bool failed = false;
        string errorMessage = "";
        if (string.IsNullOrEmpty(connectionString))
        {
            errorMessage = "Connection string cannot be empty\n";
            failed = true;
        }

        if (string.IsNullOrEmpty(query))
        {
            errorMessage += "Query cannot be empty";
            failed = true;
        }

        if (failed)
        {
            ShowResults(errorMessage);
            Debug.LogError(errorMessage);
            return;
        }
        try
        {
            OdbcConnection odbcCon = new OdbcConnection(connectionString);
            odbcCon.Open();

            OdbcCommand command = new OdbcCommand(query, odbcCon);
            StringBuilder sb = new StringBuilder();
            using (OdbcDataReader reader = command.ExecuteReader())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    sb.Append(reader.GetName(i));
                    if (i < reader.FieldCount - 1)
                        sb.Append(",");
                }

                sb.AppendLine();

                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        sb.Append(reader.GetString(i).Trim());
                        if (i < reader.FieldCount - 1)
                            sb.Append(",");
                    }
                    sb.AppendLine();

                }

                Debug.Log(sb.ToString());
                ShowResults(sb.ToString());
            }
            odbcCon.Close();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            ShowResults(
                string.Format(
                    "EXCEPTION: {0}\nconnectionString: {1}\nquery: {2}", 
                    e.Message,
                    connectionString, 
                    query)
                );

        }
    }

    void ShowResults(string message)
    {
        resultsText.text = message;
        resultsPanel.SetActive(true);
    }
}
