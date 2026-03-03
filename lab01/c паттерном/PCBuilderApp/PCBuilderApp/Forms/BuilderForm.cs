using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using PCBuilderApp.Models;
using PCBuilderApp.Data;
using PCBuilderApp.Builder;

namespace PCBuilderApp.Forms
{
    public partial class BuilderForm : Form
    {
        private Computer _computer;
        private int _currentStep = 0;
        private string _buildType;

        private IBuilder _builder;
        private Director _director;

        private string[] _components;

        public BuilderForm(string buildType)
        {
            InitializeComponent();
            _buildType = buildType;
            _computer = new Computer();
            _computer.BuildType = buildType + " ПК";
            _components = GetStepsForBuildType(buildType);
        }
        private string[] GetStepsForBuildType(string buildType)
        {
            if (buildType == "Игровой")
            {
                return new string[] { "CPU", "GPU", "RAM", "Storage", "Motherboard", "Case", "Cooling" };
            }
            else if (buildType == "Офисный")
            {
                return new string[] { "CPU", "RAM", "Storage" };
            }
            else if (buildType == "Учебный")
            {
                return new string[] { "CPU", "GPU", "RAM", "Storage", "Case" };
            }
            return new string[] { "CPU", "GPU", "RAM", "Storage", "Motherboard", "Case", "Cooling" };
        }

        private void BuilderForm_Load(object sender, EventArgs e)
        {
            LoadStep();
        }

        private void LoadStep()
        {
            lblStep.Text = "Шаг " + (_currentStep + 1).ToString() + " из " + _components.Length.ToString();
            lblComponent.Text = "Выберите " + GetComponentName(_components[_currentStep]) + ":";

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
        private string GetComponentName(string componentCode)
        {
            switch (componentCode)
            {
                case "CPU": return "Процессор";
                case "GPU": return "Видеокарту";
                case "RAM": return "Оперативную память";
                case "Storage": return "Накопитель";
                case "Motherboard": return "Материнскую плату";
                case "Case": return "Корпус";
                case "Cooling": return "Систему охлаждения";
                default: return "Компонент";
            }
        }



        //private IBuilder GetBuilderByType(string buildType)
        //{
        //    if (buildType == "Игровой")
        //        return new GamingPCBuilder();
        //    else if (buildType == "Офисный")
        //        return new OfficePCBuilder();
        //    else if (buildType == "Учебный")
        //        return new StudentPCBuilder();
        //    else
        //        return new StudentPCBuilder();
        //}

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
            ApplyDefaultValues();
            ShowSummary();
        }
        private void SaveCurrentSelection()
        {
            if (cboComponent.SelectedItem != null)
            {
                string selected = cboComponent.SelectedItem.ToString();
                switch (_components[_currentStep])
                {
                    case "CPU": _computer.Cpu = selected; break;
                    case "GPU": _computer.Gpu = selected; break;
                    case "RAM": _computer.Ram = selected; break;
                    case "Storage": _computer.Storage = selected; break;
                    case "Motherboard": _computer.Motherboard = selected; break;
                    case "Case": _computer.Case = selected; break;
                    case "Cooling": _computer.Cooling = selected; break;
                }
            }
        }
        private void ApplyDefaultValues()
        {
            if (_buildType == "Офисный")
            {
                if (string.IsNullOrEmpty(_computer.Gpu))
                    _computer.Gpu = ComponentData.GetDefaultComponent("Офисный", "GPU");
                if (string.IsNullOrEmpty(_computer.Motherboard))
                    _computer.Motherboard = ComponentData.GetDefaultComponent("Офисный", "Motherboard");
                if (string.IsNullOrEmpty(_computer.Case))
                    _computer.Case = ComponentData.GetDefaultComponent("Офисный", "Case");
                if (string.IsNullOrEmpty(_computer.Cooling))
                    _computer.Cooling = ComponentData.GetDefaultComponent("Офисный", "Cooling");
            }

            if (_buildType == "Учебный")
            {
                if (string.IsNullOrEmpty(_computer.Motherboard))
                    _computer.Motherboard = ComponentData.GetDefaultComponent("Учебный", "Motherboard");
                if (string.IsNullOrEmpty(_computer.Cooling))
                    _computer.Cooling = ComponentData.GetDefaultComponent("Учебный", "Cooling");
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
            btnExportHTML.Location = new Point(210, 20);
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