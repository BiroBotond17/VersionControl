using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _7.ora
{
    public partial class Form1 : Form
    {
        PortfolioEntities context = new PortfolioEntities();
        List<Tick> Ticks;
        public Form1()
        {
            InitializeComponent();
            
            dataGridView1.DataSource = Ticks;
            Ticks = context.Tick.ToList();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
