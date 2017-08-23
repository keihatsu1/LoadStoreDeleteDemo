using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Configuration;

using Persistence;

/*
 * todo: add sys function and sys SP to a new database automatically
 * */

namespace CodeGenerator
{
	public partial class frmCodeGenerator : Form
	{
		public frmCodeGenerator()
		{
			InitializeComponent();
			prefix = ConfigurationManager.AppSettings["PersistenceSpPrefix"];
		}

		DataTable codeTable = null;
		string prefix = "";

		private void btnSQLCode_Click(object sender, EventArgs e)
		{
			try
			{
				codeTable = Database.ExecuteDataTable("sys_CodeForTable", new { TableName = txtTableName.Text });

				if (codeTable.Rows.Count == 0)
					throw new ApplicationException("Table not found: " + txtTableName.Text);

				string code = "";
				code += DeleteSP();
				code += "\r\nGO\r\n\r\n";
				code += ListSP();
				code += "\r\nGO\r\n\r\n";
				code += LoadSP();
				code += "\r\nGO\r\n\r\n";
				code += StoreSP();
				code += "\r\nGO\r\n";

				txtCode.Text = code;
				Clipboard.SetText(code);
			}
			catch (System.Data.SqlClient.SqlException se)
			{
				MessageBox.Show(se.Message + "\r\n\r\nThe script to create the database is located in the solution root.");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void btnCSCode_Click(object sender, EventArgs e)
		{
			try
			{
				codeTable = Database.ExecuteDataTable("sys_CodeForTable", new { TableName = txtTableName.Text });

				if (codeTable.Rows.Count == 0)
					throw new ApplicationException("Table not found: " + txtTableName.Text);

				string code = CSCode();

				txtCode.Text = code;
				Clipboard.SetText(code);
			} 
			catch (System.Data.SqlClient.SqlException se)
			{
				MessageBox.Show(se.Message + "\r\n\r\nThe script to create the database is located in the solution root.");
			} 
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private string DeleteSP()
		{
			string code = "";
			code += "create procedure " + prefix + txtTableName.Text + "_Delete" + "\r\n";
			code += "\t" + Convert.ToString(codeTable.Rows[0]["SQLParms"]);

			code = code.Substring(0, code.Length - 1) + "\r\n";	//remove the last comma

			code += "as\r\n";
			code += "\tdelete from\r\n";
			code += "\t\t" + txtTableName.Text + "\r\n";
			code += "\twhere\r\n";
			code += "\t\t" + Convert.ToString(codeTable.Rows[0]["UpdateList"]);

			code = code.Substring(0, code.Length - 1) + "\r\n";	//remove the last comma

			return code;
		}

		private string ListSP()
		{
			string code = "";
			code += "create procedure " + prefix + txtTableName.Text + "_List" + "\r\n";
			code += "as\r\n";
			code += "\tselect\r\n";

            foreach (DataRow row in codeTable.Rows)
                code += "\t\t" + row["FieldList"] + "\r\n";
            code = code.Substring(0, code.Length - 3) + "\r\n";	//remove the last comma

            code += "\tfrom\r\n";
			code += "\t\t" + txtTableName.Text + "\r\n";
			
			return code;
		}

		private string LoadSP()
		{
			string code = "";
			code += "create procedure " + prefix + txtTableName.Text + "_Load" + "\r\n";
			code += "\t" + Convert.ToString(codeTable.Rows[0]["SQLParms"]);

			code = code.Substring(0, code.Length - 1) + "\r\n";	//remove the last comma

			code += "as\r\n";
			code += "\tselect\r\n";

            foreach (DataRow row in codeTable.Rows)
                code += "\t\t" + row["FieldList"] + "\r\n";
            code = code.Substring(0, code.Length - 3) + "\r\n";	//remove the last comma
            
            code += "\tfrom\r\n";
			code += "\t\t" + txtTableName.Text + "\r\n";
			code += "\twhere\r\n";
			code += "\t\t" + Convert.ToString(codeTable.Rows[0]["UpdateList"]);

			code = code.Substring(0, code.Length - 1) + "\r\n";	//remove the last comma

			return code;
		}

		private string StoreSP()
		{
			string code = "";
			code += "create procedure " + prefix + txtTableName.Text + "_Store" + "\r\n";

			foreach (DataRow row in codeTable.Rows)
			{
				code += "\t" + Convert.ToString(row["SQLParms"]) + "\r\n";
				if (row == codeTable.Rows[0])
					code = code.Insert(code.Length - 3, " output");
			}
			code = code.Substring(0, code.Length - 3) + "\r\n";	//remove the last comma

			code += "as\r\n";
			code += "\tif exists(select 1 from " + txtTableName.Text + " ";
			code += "where " + codeTable.Rows[0]["ColumnName"] + " = @" + codeTable.Rows[0]["ColumnName"] + ") begin\r\n";
			code += "\t\tupdate " + txtTableName.Text + " set\r\n";

			foreach (DataRow row in codeTable.Rows)
				if (row != codeTable.Rows[0] && !Convert.ToBoolean(row["AutoIncrement"]))	//skip the first row and nonupdateable
					code += "\t\t\t" + Convert.ToString(row["UpdateList"]) + "\r\n";
			code = code.Substring(0, code.Length - 3) + "\r\n";	//remove the last comma

			code += "\t\twhere\r\n";
			code += "\t\t\t" + codeTable.Rows[0]["ColumnName"] + " = @" + codeTable.Rows[0]["ColumnName"] + "\r\n";
			code += "\tend else begin\r\n";
			code += "\t\tinsert into " + txtTableName.Text + " (\r\n";

			foreach (DataRow row in codeTable.Rows)
				if (row != codeTable.Rows[0] && !Convert.ToBoolean(row["AutoIncrement"]))	//skip the first row and nonupdateable
					code += "\t\t\t" + row["FieldList"] + "\r\n";
			code = code.Substring(0, code.Length - 3) + "\r\n";	//remove the last comma

			code += "\t\t) values (\r\n";

			foreach (DataRow row in codeTable.Rows)
				if (row != codeTable.Rows[0] && !Convert.ToBoolean(row["AutoIncrement"]))	//skip the first row and nonupdateable
					code += "\t\t\t" + row["ParmList"] + "\r\n";
			code = code.Substring(0, code.Length - 3) + "\r\n";	//remove the last comma

			code += "\t\t)\r\n";
			code += "\r\n";
			code += "\t\tset @" + codeTable.Rows[0]["ColumnName"] + " = scope_identity()\r\n";
			code += "\tend\r\n";
			code += "\r\n";
			code += "return @" + codeTable.Rows[0]["ColumnName"] + "\r\n";

			return code;
		}

		private void btnCopyToClipboard_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(txtCode.Text);
		}

		private string CSCode()
		{
			string code = "";

			code += "using System;\r\n";
			code += "using System.Collections.Generic;\r\n";
			code += "using System.Linq;\r\n";
			code += "using System.Text;\r\n";
			code += "\r\n";
			code += "using Persistence;\r\n";
			code += "\r\n";
			code += "namespace doLogic\r\n";
			code += "{\r\n";
            code += "\tpublic class " + txtTableName.Text + "s : PersistentList<" + txtTableName.Text + ">\r\n";
            code += "\t{\r\n";
            code += "\t}\r\n";
            code += "\r\n";
			code += "\t[Persistable(\"" + txtTableName.Text + "\", \"" + codeTable.Rows[0]["ColumnName"] + "\", false)]\r\n";
			code += "\tpublic class " + txtTableName.Text + "\r\n";
			code += "\t{\r\n";
			code += "\t\t//constructors\r\n";
			code += "\t\tpublic " + txtTableName.Text + "() { }\r\n";
			code += "\r\n";
			code += "\t\tpublic " + txtTableName.Text + "(int id)\r\n";
			code += "\t\t{\r\n";
			code += "\t\t\t" + codeTable.Rows[0]["ColumnName"] + " = id;\r\n";
			code += "\t\t\tthis.Load();\r\n";
			code += "\t\t}\r\n";
			code += "\r\n";
			code += "\t\t//persistent properties\r\n";
			foreach (DataRow row in codeTable.Rows)
				code += "\t\t" + row["CSharp"] + "\r\n";

			code += "\r\n";
			code += "\t\tpublic void Load() { Database.Load(this); }\r\n";
			code += "\t\tpublic int Store() { return Database.Store(this); }\r\n";
			code += "\t\tpublic bool Delete() { return Database.Delete(this); }\r\n";
			code += "\t}\r\n";
			code += "}\r\n";

			return code;
		}

        private void frmCodeGenerator_Load(object sender, EventArgs e)
        {

        }
	}
}
