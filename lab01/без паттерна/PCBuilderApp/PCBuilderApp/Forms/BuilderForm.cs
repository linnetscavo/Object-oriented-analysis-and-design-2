using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using PCBuilderApp.Models;
using PCBuilderApp.Data;

namespace PCBuilderApp.Forms
{
    public partial class BuilderForm : Form
    {
        private Computer _computer;
        private int _currentStep = 0;
        private string _buildType;

        private string[] _components = { "CPU", "GPU", "RAM", "Storage", "Motherboard", "Case", "Cooling" };
        private string[] _componentNames = { "Процессор", "Видеокарта", "Оперативная память",
                                             "Накопитель", "Материнская плата", "Корпус", "Охлаждение" };

        public BuilderForm(string buildType)
        {
            InitializeComponent();
            _buildType = buildType;
            _computer = new Computer();
            _computer.BuildType = buildType + " ПК";
        }

        private void BuilderForm_Load(object sender, EventArgs e)
        {
            LoadStep();
        }

        private void LoadStep()
        {
            lblStep.Text = "Шаг " + (_currentStep + 1).ToString() + " из " + _components.Length.ToString();
            lblComponent.Text = "Выберите " + _componentNames[_currentStep] + ":";

            cboComponent.Items.Clear();
            List<string> components = ComponentData.GetComponents(_buildType, _components[_currentStep]);
            foreach (string item in components)
            {
                cboComponent.Items.Add(item);
            }
            cboComponent.SelectedIndex = 0;

            btnBack.Enabled = _currentStep > 0;
            btnNext.Visible = _currentStep < _components.Length - 1;
            btnFinish.Visible = _currentStep == _components.Length - 1;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            SaveCurrentSelection();
            _currentStep++;
            LoadStep();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            _currentStep--;
            LoadStep();
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            SaveCurrentSelection();
            ShowSummary();
        }

        private void SaveCurrentSelection()
        {
            if (cboComponent.SelectedItem != null)
            {
                string selected = cboComponent.SelectedItem.ToString();
                if (_components[_currentStep] == "CPU") _computer.Cpu = selected;
                else if (_components[_currentStep] == "GPU") _computer.Gpu = selected;
                else if (_components[_currentStep] == "RAM") _computer.Ram = selected;
                else if (_components[_currentStep] == "Storage") _computer.Storage = selected;
                else if (_components[_currentStep] == "Motherboard") _computer.Motherboard = selected;
                else if (_components[_currentStep] == "Case") _computer.Case = selected;
                else if (_components[_currentStep] == "Cooling") _computer.Cooling = selected;
            }
        }

        private void ShowSummary()
        {
            pnlSummary.Visible = true;
            cboComponent.Visible = false;
            lblComponent.Visible = false;
            btnNext.Visible = false;
            btnBack.Visible = false;
            btnFinish.Visible = false;
            lblStep.Text = "Сборка завершена!";

            Button btnExportHTML = new Button();
            btnExportHTML.Text = "Экспорт в HTML";
            btnExportHTML.Location = new Point(20, 20);
            btnExportHTML.Size = new Size(150, 35);
            btnExportHTML.BackColor = Color.FromArgb(52, 152, 219);
            btnExportHTML.ForeColor = Color.White;
            btnExportHTML.Click += BtnExportHTML_Click;
            pnlSummary.Controls.Add(btnExportHTML);

            Label lblConfig = new Label();
            lblConfig.Text = _computer.ToString();
            lblConfig.Location = new Point(20, 70);
            lblConfig.Size = new Size(360, 250);
            lblConfig.Font = new Font("Consolas", 9F);
            pnlSummary.Controls.Add(lblConfig);
        }

        private void BtnExportHTML_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "HTML файлы|*.html";
            sfd.FileName = "PC_Config_" + _buildType + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".html";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(sfd.FileName, _computer.ToHTML());
                MessageBox.Show("Конфигурация сохранена в:\n" + sfd.FileName,
                              "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}