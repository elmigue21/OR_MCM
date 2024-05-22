using MinimumCost;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tabControl1.Location = new System.Drawing.Point(tabControl1.Location.X - 10, tabControl1.Location.Y);
            tabControl1.Width += 10;
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void NumberLimitation(System.Windows.Forms.TextBox checker)
        {
            if (!int.TryParse(checker.Text, out int value))
            {
                checker.Text = "";
            }
            else if (value > 10)
            {
                checker.Text = "10";
            }
            else if (value <= 0)
            {
                checker.Text = "1";
            }
        }
        // city count 
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            NumberLimitation(textBox1);
        }
        // plant count 
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            NumberLimitation(textBox2);
        }
        private void minimumCost()
        {
            int minCost = int.MaxValue;
            int minCostRowIndex = -1;
            int minCostColumnIndex = -1;
                for (int i = 0; i < dataGridView2.ColumnCount-1; i++)
                {
                    for (int j = 0; j < dataGridView2.RowCount-1; j++)
                    {
                        if (dataGridView2.Rows[j].Cells[i].Value != null && dataGridView2.Rows[j].Cells[i].Value.ToString() != "x")
                        {
                            int cost = Int32.Parse(dataGridView2.Rows[j].Cells[i].Value.ToString());
                            if (cost < minCost && dataGridView2.Rows[j].Cells[i].Style.BackColor != Color.LightGoldenrodYellow)
                            {
                                minCost = cost;
                                minCostRowIndex = j;
                                minCostColumnIndex = i;
                            }
                        }
                    }
                }
                if (minCostRowIndex != -1 && minCostColumnIndex != -1)
                {
                    DataGridViewCell minCostCell = dataGridView2.Rows[minCostRowIndex].Cells[minCostColumnIndex];
                    minCostCell.Style.BackColor = Color.LightGoldenrodYellow;
                    subtractLowest(minCost, minCostRowIndex, minCostColumnIndex);
                }
        }
        private void generateTable(DataGridView datagridview)
        {
            datagridview.Rows.Clear();
            datagridview.Columns.Clear();
            datagridview.Refresh();
            datagridview.RowCount = Int32.Parse(textBox2.Text) + 1;
            datagridview.ColumnCount = Int32.Parse(textBox1.Text) + 1;
            datagridview.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            for (int i = 0; i < Int32.Parse(textBox1.Text) + 1; i++)
            {
                datagridview.Columns[i].Width = 100;
                datagridview.Columns[i].HeaderText = "C" + (i + 1);
                if (i == Int32.Parse(textBox1.Text))
                {
                    datagridview.Columns[Int32.Parse(textBox1.Text)].HeaderText = "Supply";
                }
            }
            for (int j = 0; j < Int32.Parse(textBox2.Text) + 1; j++)
            {
                datagridview.Rows[j].Height = 30;
                datagridview.Rows[j].HeaderCell.Value = "P" + (j + 1);
                if (j == Int32.Parse(textBox2.Text))
                {
                    datagridview.Rows[Int32.Parse(textBox2.Text)].HeaderCell.Value = "Demand";
                }
            }
        }
        // generate button
        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView2.DefaultCellStyle.SelectionBackColor = Color.White;
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Please enter a number for city and plant!");
                return;
            }
            else
            {
                generateTable(dataGridView1);
            }
            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
        }
        private void CheckTotalMethod(DataGridView datagridview)
        {
            int supplyTotal = 0;
            int demandTotal = 0;

            for (int j = 0; j < datagridview.RowCount - 1; j++)
            {
                if (datagridview.Rows[j].Cells[datagridview.ColumnCount - 1].Value != null)
                {
                    if (int.TryParse(datagridview.Rows[j].Cells[datagridview.ColumnCount - 1].Value.ToString(), out int supply))
                    {
                        supplyTotal += supply;
                    }
                }
            }

            for (int i = 0; i < datagridview.ColumnCount - 1; i++)
            {
                if (datagridview.Rows[datagridview.RowCount - 1].Cells[i].Value != null)
                {
                    if (int.TryParse(datagridview.Rows[datagridview.RowCount - 1].Cells[i].Value.ToString(), out int demand))
                    {
                        demandTotal += demand;
                    }
                }
            }

            int total = Math.Max(supplyTotal, demandTotal);

            datagridview.Rows[datagridview.RowCount - 1].Cells[datagridview.ColumnCount - 1].Value = total;

            if (demandTotal < total)
            {
                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn
                {
                    Name = "DummySupply",
                    HeaderText = "Dummy",
                    Width = 100
                };
                datagridview.Columns.Insert(datagridview.Columns.Count - 1, column);
                foreach (DataGridViewRow row in datagridview.Rows)
                {
                    row.Cells["DummySupply"].Value = 0;
                }
                datagridview.Rows[datagridview.RowCount - 1].Cells["DummySupply"].Value = total - demandTotal;
            }

            if (supplyTotal < total)
            {
                DataGridViewRow row = new DataGridViewRow
                {
                    Height = 30,
                    Tag = "DummyDemand",
                    HeaderCell = { Value = "Dummy" }
                };
                datagridview.Rows.Insert(datagridview.Rows.Count - 1, row);
                foreach (DataGridViewRow rw in datagridview.Rows)
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
                int ind = datagridview.RowCount - 2;
                int ind2 = datagridview.ColumnCount - 1;
                datagridview.Rows[ind].Cells[ind2].Value = total - supplyTotal;
            }
        }
        private void CheckTotal()
        {
            CheckTotalMethod(dataGridView1);
            CheckTotalMethod(dataGridView2);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell.Value != null)
                {
                    if (!int.TryParse(cell.Value.ToString(), out int _))
                    {
                        cell.Value = ""; 
                    }
                }
            }
        }
        private void subtractLowest(int minCost, int minCostRowIndex, int minCostColumnIndex)
        {
            DataGridViewCell test = dataGridView2.Rows[minCostRowIndex].Cells[dataGridView2.ColumnCount-1];
            DataGridViewCell test1 = dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[minCostColumnIndex];

            test.Style.BackColor = Color.LightBlue;
            test1.Style.BackColor = Color.LightGreen;

            int minCostSupply = Int32.Parse(dataGridView2.Rows[minCostRowIndex].Cells[dataGridView2.ColumnCount - 1].Value.ToString());
            int minCostDemand = Int32.Parse(dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[minCostColumnIndex].Value.ToString());

            int lowest = minCostSupply < minCostDemand ? minCostSupply : minCostDemand;

            minCostSupply -= lowest;
            minCostDemand -= lowest;


            dataGridView2.Rows[minCostRowIndex].Cells[dataGridView2.ColumnCount - 1].Value = minCostSupply;
            dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[minCostColumnIndex].Value = minCostDemand;


            if (minCostSupply == minCostDemand)
            {
                for (int i = 0; i < dataGridView2.Rows.Count - 1; i++)
                {

                    if (dataGridView2.Rows[i].Cells[minCostColumnIndex].Style.BackColor == Color.LightGoldenrodYellow)
                    {
                        continue;
                    }
                    dataGridView2.Rows[i].Cells[minCostColumnIndex].Value = "x";
                }
                for (int j = 0; j < dataGridView2.ColumnCount - 1; j++)
                {
                    if (dataGridView2.Rows[minCostRowIndex].Cells[j].Style.BackColor == Color.LightGoldenrodYellow)
                    {
                        continue;
                    }
                    dataGridView2.Rows[minCostRowIndex].Cells[j].Value = "x";
                }


            }
            else if (minCostSupply > minCostDemand)
            {
                for (int i = 0; i < dataGridView2.Rows.Count - 1; i++)
                {
                    if (dataGridView2.Rows[i].Cells[minCostColumnIndex].Style.BackColor == Color.LightGoldenrodYellow)
                    {
                        continue;
                    }
                    dataGridView2.Rows[i].Cells[minCostColumnIndex].Value = "x";
                }
            }
            else
            {
                for (int i = 0; i < dataGridView2.ColumnCount - 1; i++)
                {
                    if (dataGridView2.Rows[minCostRowIndex].Cells[i].Style.BackColor == Color.LightGoldenrodYellow)
                    {
                        continue;
                    }
                    dataGridView2.Rows[minCostRowIndex].Cells[i].Value = "x";
                }
            }
            dataGridView2.Rows[minCostRowIndex].Cells[minCostColumnIndex].Value = lowest;
        }
        // solve button
        private void button1_Click(object sender, EventArgs e)
        {
                dataGridView2.Rows.Clear();
                dataGridView2.Columns.Clear();
                dataGridView2.Refresh();
                for (int i = 0; i < dataGridView2.RowCount - 1; i++)
                {
                    for (int j = 0; j < dataGridView2.ColumnCount - 1; j++)
                    {
                        if (dataGridView1.Rows[j].Cells[i].Value == null)
                        {
                            MessageBox.Show("Please fill up every cell!");
                            return;
                        }
                        if (!int.TryParse(dataGridView1.Rows[j].Cells[i].Value.ToString(), out int value))
                        {
                            dataGridView1.Rows[j].Cells[i].Value = "";
                        }
                    }
                }
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    dataGridView2.Columns.Add(column.Clone() as DataGridViewColumn);
                }
                dataGridView2.RowHeadersVisible = true;
                dataGridView2.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
                dataGridView2.RowHeadersDefaultCellStyle = dataGridView1.RowHeadersDefaultCellStyle.Clone() as DataGridViewCellStyle;

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    int index = dataGridView2.Rows.Add();
                    dataGridView2.Rows[index].Height = row.Height;
                    for (int i = 0; i < dataGridView1.Columns.Count; i++)
                    {
                        dataGridView2.Rows[index].HeaderCell.Value = row.HeaderCell.Value;
                        dataGridView2.Rows[index].Cells[i].Value = row.Cells[i].Value;
                    }
                }
                dataGridView2.Refresh();
                CheckTotal();
                if (tabControl1 != null) 
                {
                    tabControl1.SelectedIndex = 1;
                }
        }
        // next step button
        private void button3_Click(object sender, EventArgs e)
        {
            int[] arrayColumn = new int[dataGridView2.ColumnCount];
            int[] arrayRow = new int[dataGridView2.RowCount];
            for (int j = 0; j < dataGridView2.RowCount - 1; j++)
            {
                if (dataGridView2.Rows[j].Cells[dataGridView2.ColumnCount - 1].Value != null)
                {
                    int cellValue;
                    if (int.TryParse(dataGridView2.Rows[j].Cells[dataGridView2.ColumnCount - 1].Value.ToString(), out cellValue))
                    {
                        arrayColumn[j] = cellValue;
                    }
                }
            }
            for (int j = 0; j < dataGridView2.ColumnCount - 1; j++)
            {
                if (dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[j].Value != null)
                {
                    int cellValue;
                    if (int.TryParse(dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[j].Value.ToString(), out cellValue))
                    {
                        arrayRow[j] = cellValue;
                    }
                }
            }
            bool allZeroColumn = true;
            bool allZeroRow = true;
            foreach (int value in arrayColumn)
            {
                if (value != 0)
                {
                    allZeroColumn = false;
                    break;
                }
            }
            foreach (int value in arrayRow)
            {
                if (value != 0)
                {
                    allZeroRow = false;
                    break;
                }
            }
            if (allZeroColumn || (allZeroRow && arrayColumn.Length == 0))
            {
                dataGridView3.Rows.Clear();
                dataGridView3.Columns.Clear();
                dataGridView3.Refresh();
                foreach (DataGridViewColumn column in dataGridView2.Columns)
                {
                    dataGridView3.Columns.Add(column.Clone() as DataGridViewColumn);
                }
                dataGridView3.RowHeadersVisible = true;
                dataGridView3.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
                dataGridView3.RowHeadersDefaultCellStyle = dataGridView1.RowHeadersDefaultCellStyle.Clone() as DataGridViewCellStyle;

                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    int index = dataGridView3.Rows.Add();
                    dataGridView3.Rows[index].Height = row.Height;
                    for (int i = 0; i < dataGridView2.Columns.Count; i++)
                    {
                        dataGridView3.Rows[index].HeaderCell.Value = row.HeaderCell.Value;
                        dataGridView3.Rows[index].Cells[i].Value = row.Cells[i].Value;
                    }
                }
                dataGridView3.Refresh();
                if (tabControl1 != null)
                {
                    tabControl1.SelectedIndex = 2;
                }
            }
            minimumCost(); 
            if (dataGridView2.Rows[0].Cells[0].Style.BackColor == Color.LightGoldenrodYellow)
            {
                dataGridView2.DefaultCellStyle.SelectionBackColor = Color.LightGoldenrodYellow;
            }
        }
        private void calculateTotalCost()
        {
            StringBuilder calculation = new StringBuilder();
            int totalCost = 0;

            for (int i = 0; i < dataGridView2.Columns.Count - 1; i++)
            {
                for (int j = 0; j < dataGridView2.Rows.Count - 1; j++)
                {
                    if (dataGridView2.Rows[j].Cells[i].Value != null && i < dataGridView1.Columns.Count && j < dataGridView1.Rows.Count)
                    {
                        int value1 = 0;
                        int value2 = 0;

                        if (dataGridView1.Rows[j].Cells[i].Value != null)
                        {
                            value1 = Int32.Parse(dataGridView1.Rows[j].Cells[i].Value.ToString());
                        }

                        if (dataGridView2.Rows[j].Cells[i].Value.ToString() != "x")
                        {
                            value2 = Int32.Parse(dataGridView2.Rows[j].Cells[i].Value.ToString());
                        }

                        totalCost += value1 * value2;

                        if (calculation.Length > 0)
                        {
                            calculation.Append(" + ");
                        }

                        calculation.Append(value1.ToString() + "(" + value2.ToString() + ")");
                    }
                }
            }

            calculation.Append(" = " + totalCost.ToString());

            MessageBox.Show("Total Cost: " + calculation.ToString(), "Total Cost");
        }
        // show total cost button
        private void button5_Click(object sender, EventArgs e)
        {
            calculateTotalCost();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.ReadOnly = false;
            textBox2.ReadOnly = false;
        }
    }
}
