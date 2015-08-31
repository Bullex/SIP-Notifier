using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SIP_Notifier
{
    public partial class HistoryForm : Form
    {
        public HistoryForm()
        {
            InitializeComponent();

            InitializeList();
        }

        private void InitializeList()
        {
            listView1.Items.Clear();
            HistoryDatabase db = HistoryDatabase.getInstance();
            List<HistoryRow> list =  db.getAll();
            foreach (var item in list)
            {
                string[] arr = new string[4];
                arr[0] = item.getId().ToString();
                arr[1] = item.getDate();
                arr[2] = item.getPhone();
                arr[3] = item.getText();
                listView1.Items.Add(new ListViewItem(arr));
            }
        }
    }
}
