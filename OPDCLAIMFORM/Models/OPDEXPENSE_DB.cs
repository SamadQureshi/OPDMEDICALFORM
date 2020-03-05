using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OPDCLAIMFORM.Models
{
    public class OPDEXPENSE_DB
    {
        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["MedicalDBConnection"].ToString();
            con = new SqlConnection(constring);
        }

        // **************** ADD OPD EXPENSE IMAGE *********************
        public bool ADDOPDEXPENSE_IMAGES(OPDEXPENSE_IMAGE smodel)
        {
            connection();
            SqlCommand cmd = new SqlCommand("ADDOPDEXPENSE_IMAGE", con);
            cmd.CommandType = CommandType.StoredProcedure;



            cmd.Parameters.AddWithValue("@IMAGE_NAME", smodel.IMAGE_NAME);
            cmd.Parameters.AddWithValue("@IMAGE", smodel.IMAGE);
            cmd.Parameters.AddWithValue("@OPDEXPENSE_ID", smodel.OPDEXPENSE_ID);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }


        public bool sp_insert_file(int OPDEXPENSE_ID ,string IMAGE_NAME, string IMAGE_EXT, string IMAGE_BASE64, string NAME_EXPENSES, Nullable<decimal> EXPENSE_AMOUNT)
        {
            connection();
            SqlCommand cmd = new SqlCommand("ADDOPDEXPENSE_IMAGE", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@OPDEXPENSE_ID", OPDEXPENSE_ID);
            cmd.Parameters.AddWithValue("@IMAGE_NAME", IMAGE_NAME);
            cmd.Parameters.AddWithValue("@IMAGE", IMAGE_EXT);
            cmd.Parameters.AddWithValue("@IMAGE", IMAGE_BASE64);
            cmd.Parameters.AddWithValue("@NAME_EXPENSES", NAME_EXPENSES);
            cmd.Parameters.AddWithValue("@EXPENSE_AMOUNT", EXPENSE_AMOUNT);
             con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            return true;
        }     

    }
}