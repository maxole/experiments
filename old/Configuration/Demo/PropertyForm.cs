using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ConfiguratorDecorator;

namespace Demo
{
    public partial class PropertyForm : Form
    {
        public PropertyForm()
        {
            InitializeComponent();
        }

        public Action SaveAction = () => { };

        public void AddDecorator(IConfiguratorDecorator decorator)
        {
            var page = new TabPage(decorator.Name);
            var propertyGrid = new PropertyGrid
            {
                Dock = DockStyle.Fill, SelectedObject = decorator
            };
            page.Controls.Add(propertyGrid);

            tabControl1.TabPages.Add(page);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveAction();
        }
    }
}
