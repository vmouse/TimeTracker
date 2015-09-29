using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.SharePoint.Client;
using System.Security;


namespace TimeTracker
{
    public partial class MainForm : System.Windows.Forms.Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (ClientContext cont = new ClientContext("https://inblago.sharepoint.com"))
            {
                SecureString pass = new SecureString();
                foreach (char c in "password".ToCharArray()) pass.AppendChar(c);
                cont.Credentials = new SharePointOnlineCredentials("login", pass);
                Web oWeb = cont.Web;
                cont.Load(oWeb);
                List tasks = oWeb.Lists.GetByTitle("Tasks");
                oWeb.Lists.RetrieveItems().Retrieve();

                CamlQuery caml = new Microsoft.SharePoint.Client.CamlQuery();
                caml.ViewXml = "<View Scope='RecursiveAll' />";
                ListItemCollection items = tasks.GetItems(caml);
                items.RetrieveItems().Retrieve();
                cont.ExecuteQuery();
                // foreach (List lst in oWeb.Lists)
                foreach (ListItem item in items)
                {
                    tbLog.AppendText(item.FieldValues["Title"]+"\n");
                }
            }
        }
    }
}
