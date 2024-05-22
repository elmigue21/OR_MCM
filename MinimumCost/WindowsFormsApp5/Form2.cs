using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MinimumCost
{
    public partial class Form2 : Form
    {
        Form opener;
        public int txtbx1txt;
        public int txtbx2txt;
        public DataGridView passedDataGridView1;
        DataGridView passedDataGridView2;
 
        public Form2(Form parentForm)
        {
            InitializeComponent();
            opener = parentForm;
            //dataGridView1 = passedDataGridView1;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            panel1.Controls.Add(passedDataGridView1);
            
            passedDataGridView1.Show();

            passedDataGridView2 = CopyDataGridView(passedDataGridView1);
            panel2.Controls.Add(passedDataGridView2);
            
            passedDataGridView2.Show();
            
        }
        private DataGridView CopyDataGridView(DataGridView original)
        {
            DataGridView copy = new DataGridView();

            // Copy the column structure

            for (int i = 0; i < original.Columns.Count; i++)
            {
                DataGridViewColumn col = original.Columns[i];
                copy.Columns.Add((DataGridViewColumn)col.Clone());
            }

            // Copy the data
            foreach (DataGridViewRow row in original.Rows)
            {
                int rowIndex = copy.Rows.Add();
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    copy.Rows[rowIndex].Cells[i].Value = row.Cells[i].Value;
                }
            }

            return copy;
        }
        private void CheckTotal()
        {
            int total = Int32.Parse(passedDataGridView2.Rows[txtbx1txt].Cells[txtbx2txt].Value.ToString());


            int columnTotal = 0;
            for (int j = 0; j < passedDataGridView2.RowCount - 1; j++)
            {
                if (passedDataGridView2.Rows[j].Cells[passedDataGridView2.ColumnCount - 1].Value != null)
                {
                    int cellValue;
                    if (int.TryParse(passedDataGridView2.Rows[j].Cells[passedDataGridView2.ColumnCount - 1].Value.ToString(), out cellValue))
                    {
                        columnTotal += cellValue;
                    }
                }
            }
            int rowTotal = 0;
            for (int j = 0; j < passedDataGridView2.ColumnCount - 1; j++)
            {
                if (passedDataGridView2.Rows[passedDataGridView2.RowCount - 1].Cells[j].Value != null)
                {
                    int cellValue;
                    if (int.TryParse(passedDataGridView2.Rows[passedDataGridView2.RowCount - 1].Cells[j].Value.ToString(), out cellValue))
                    {
                        rowTotal += cellValue;
                    }
                }
            }

            if (rowTotal < total)
            {
                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();

                // Set properties for the new column
                column.Name = "DummySupply";
                column.HeaderText = "Dummy Supply";
                column.Width = 50;
                // Add the new column to the DataGridView
                passedDataGridView2.Columns.Insert(passedDataGridView2.Columns.Count - 1, column);

                // Set all cells in the "DummySupply" column to 0
                foreach (DataGridViewRow row in passedDataGridView2.Rows)
                {
                    row.Cells["DummySupply"].Value = 0;
                }
                passedDataGridView2.Rows[passedDataGridView2.RowCount - 1].Cells["DummySupply"].Value = total - rowTotal;
            }
            else if (rowTotal == total)
            {
                MessageBox.Show("rowTotal == total");
            }
            else
            {
                MessageBox.Show("rowTotal > total");
            }
            if (columnTotal < total)
            {
                DataGridViewRow row = new DataGridViewRow();

                row.Tag = "DummyDemand";

                passedDataGridView2.Rows.Insert(passedDataGridView2.Columns.Count - 1, row);
                foreach (DataGridViewRow rw in passedDataGridView2.Rows)
                {
                    if (rw.Tag != null && rw.Tag.ToString() == "DummyDemand")
                    {
                        foreach (DataGridViewCell cell in rw.Cells)
                        {
                            cell.Value = 0;
                        }
                        break;
                    }
                }
                int ind = passedDataGridView2.RowCount - 2;
                int ind2 = passedDataGridView2.ColumnCount - 1;
                MessageBox.Show(ind.ToString());

                passedDataGridView2.Rows[ind].Cells[ind2].Value = total - columnTotal;
            }
            else if (columnTotal == total)
            {
                MessageBox.Show("columnTotal == total");
            }
            else
            {
                MessageBox.Show("columnTotal > total");
            }


        }

    }
}
