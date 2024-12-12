using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadProcesses();
        }
        private void LoadProcesses()
        {
            processListView.Items.Clear();

            foreach (var process in Process.GetProcesses())
            {
                try
                {
                    var item = new ListViewItem(process.Id.ToString());
                    item.SubItems.Add(process.ProcessName);
                    item.SubItems.Add((process.WorkingSet64 / 1024 / 1024).ToString());
                    item.SubItems.Add(process.StartTime.ToString());
                    item.SubItems.Add(process.PriorityClass.ToString());
                    item.SubItems.Add(process.Threads.Count.ToString());
                    processListView.Items.Add(item);
                }
                catch
                {
                }
            }
        }

        private void StopSelectedProcess()
        {
            if (processListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Виберіть процес.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int pid = int.Parse(processListView.SelectedItems[0].Text);
            try
            {
                var process = Process.GetProcessById(pid);
                process.Kill();
                MessageBox.Show($"Процес {process.ProcessName} (PID {pid}) припинено.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadProcesses();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ChangePriority()
        {
            if (processListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Виберіть процес.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (priorityComboBox.SelectedItem == null)
            {
                MessageBox.Show("Виберіть пріоритет.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int pid = int.Parse(processListView.SelectedItems[0].Text);
            try
            {
                var process = Process.GetProcessById(pid);
                var newPriority = (ProcessPriorityClass)Enum.Parse(typeof(ProcessPriorityClass), priorityComboBox.SelectedItem.ToString());
                process.PriorityClass = newPriority;
                MessageBox.Show($"Пріоритет {process.ProcessName} (PID {pid}) змінено на {newPriority}.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadProcesses();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LaunchProgram(string programPath)
        {
            try
            {
                Process.Start(programPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}