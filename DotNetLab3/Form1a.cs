using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        private string selectedFilePath;

        public Form1()
        {
            InitializeComponent();
            Text = "Шифрувальник файлів";

            selectFileButton.Click += SelectFile;
            encryptButton.Click += (s, e) => ProcessFile(true);
            decryptButton.Click += (s, e) => ProcessFile(false);
        }

        private void SelectFile(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    selectedFilePath = dialog.FileName;
                    statusLabel.Text = "Вибраний файл: " + Path.GetFileName(selectedFilePath);
                }
            }
        }
        private void ProcessFile(bool encrypt)
        {
            if (string.IsNullOrEmpty(selectedFilePath) || string.IsNullOrEmpty(keyTextBox.Text))
            {
                MessageBox.Show("Виберіть файл і введіть ключ.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            progressBar.Value = 0;
            statusLabel.Text = "Обробка...";
            string key = keyTextBox.Text;
            DateTime startTime = DateTime.Now;

            BackgroundWorker worker = new BackgroundWorker { WorkerReportsProgress = true };

            worker.DoWork += (s, e) =>
            {
                try
                {
                    using (FileStream inputFile = new FileStream(selectedFilePath, FileMode.Open))
                    using (FileStream outputFile = new FileStream(selectedFilePath + (encrypt ? ".enc" : ".dec"), FileMode.Create))
                    using (CryptoStream cryptoStream = new CryptoStream(outputFile, GetCryptoTransform(key, encrypt), CryptoStreamMode.Write))
                    {
                        byte[] buffer = new byte[4096];
                        int bytesRead;
                        long totalBytes = inputFile.Length;
                        long processedBytes = 0;

                        while ((bytesRead = inputFile.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            cryptoStream.Write(buffer, 0, bytesRead);
                            processedBytes += bytesRead;
                            worker.ReportProgress((int)((double)processedBytes / totalBytes * 100));
                        }
                    }
                }
                catch (Exception ex)
                {
                    e.Result = ex.Message;
                }
            };

            worker.ProgressChanged += (s, e) =>
            {
                progressBar.Value = e.ProgressPercentage;
            };

            worker.RunWorkerCompleted += (s, e) =>
            {
                if (e.Result is string error)
                {
                    MessageBox.Show("Error: " + error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    DateTime endTime = DateTime.Now;
                    TimeSpan duration = endTime - startTime;
                    statusLabel.Text = $"Готово: {Path.GetFileName(selectedFilePath)} {(encrypt ? "encrypted" : "decrypted")}";
                    MessageBox.Show($"Операцію успішно завершено. \nТривалість: {duration.TotalSeconds:F2} seconds.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            worker.RunWorkerAsync();
        }

        private ICryptoTransform GetCryptoTransform(string key, bool encrypt)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] keyBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(key));

                using (Aes aes = Aes.Create())
                {
                    aes.Key = keyBytes;
                    aes.IV = new byte[16];
                    return encrypt ? aes.CreateEncryptor() : aes.CreateDecryptor();
                }
            }
        }
    }
}