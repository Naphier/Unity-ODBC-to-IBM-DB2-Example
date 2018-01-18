using System;
using System.Data.Odbc;
using UnityEngine;

public static class OdbcExtensions
{
    public static int GetOrdinalExt(this OdbcDataReader reader, string key)
    {
        int ord = -1;
        try
        {
            ord = reader.GetOrdinal(key);
        }
        catch (Exception) { }//Silent catch since column names are likely to be missing.

        //Debug.LogWarningFormat("key: {0} ordinal: {1}", key, ord);
        return ord;
    }

    public static string GetStringExt(this OdbcDataReader reader, int ordinal)
    {
        string s = "";

        if (ordinal > -1)
        {
            try
            {
                string temp = reader.GetString(ordinal);

                if (!string.IsNullOrEmpty(temp))
                    temp = temp.Trim();

                s = temp;
            }
            catch (Exception) { } //Silent catch since column names are likely to be missing.
        }

        return s;
    }
}

