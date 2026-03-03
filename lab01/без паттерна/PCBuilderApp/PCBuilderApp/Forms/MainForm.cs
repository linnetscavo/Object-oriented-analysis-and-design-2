using System;
using System.Windows.Forms;

namespace PCBuilderApp.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            cboBuildType.Items.Add("Игровой");
            cboBuildType.Items.Add("Офисный");
            cboBuildType.Items.Add("Учебный");
            cboBuildType.SelectedIndex = 0;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            string buildType = cboBuildType.SelectedItem.ToString();
            BuilderForm builderForm = new BuilderForm(buildType);
            builderForm.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}