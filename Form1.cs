using System;
using System.IO;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.ComponentModel;

namespace PdfRenamer
{
    public partial class Form1 : Form
    {
        private Button btnSelectFolder = null!;
        private ProgressBar progressBar = null!;
        private Label lblStatus = null!;
        private System.ComponentModel.IContainer components = null!;

        public Form1()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            
            this.btnSelectFolder = new Button();
            this.progressBar = new ProgressBar();
            this.lblStatus = new Label();

            // btnSelectFolder
            this.btnSelectFolder.Location = new System.Drawing.Point(12, 12);
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.Size = new System.Drawing.Size(120, 30);
            this.btnSelectFolder.Text = "Select Folder";
            this.btnSelectFolder.Click += new EventHandler(OnBtnSelectFolderClick);

            // progressBar
            this.progressBar.Location = new System.Drawing.Point(12, 50);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(460, 23);

            // lblStatus
            this.lblStatus.Location = new System.Drawing.Point(12, 80);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(460, 23);
            this.lblStatus.Text = "Plz select folder...";

            // Form1
            this.SuspendLayout();
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 120);
            this.Controls.Add(this.btnSelectFolder);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblStatus);
            this.Name = "Form1";
            this.Text = "PDF Renamer";
            this.ResumeLayout(false);
        }

        private async void OnBtnSelectFolderClick(object? sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    btnSelectFolder.Enabled = false;
                    await ProcessPdfFiles(folderDialog.SelectedPath);
                    btnSelectFolder.Enabled = true;
                }
            }
        }

        private async Task ProcessPdfFiles(string folderPath)
        {
            string[] pdfFiles = Directory.GetFiles(folderPath, "*.pdf");
            progressBar.Maximum = pdfFiles.Length;
            progressBar.Value = 0;

            foreach (string pdfFile in pdfFiles)
            {
                progressBar.Value++;
                try
                {
                    string? code = await ExtractCodeFromPdf(pdfFile);
                    if (!string.IsNullOrEmpty(code))
                    {
                        string newFileName = System.IO.Path.Combine(
                            System.IO.Path.GetDirectoryName(pdfFile) ?? string.Empty, 
                            code + ".pdf");
                            
                        if (File.Exists(newFileName) && !pdfFile.Equals(newFileName, StringComparison.OrdinalIgnoreCase))
                        {
                            lblStatus.Text = $"Warning: {code}.pdf already have. Skipping...";
                            continue;
                        }
                        
                        if (!pdfFile.Equals(newFileName, StringComparison.OrdinalIgnoreCase))
                        {
                            File.Move(pdfFile, newFileName);
                            lblStatus.Text = $"Renamed: {System.IO.Path.GetFileName(newFileName)}";
                        }
                    }
                    else
                    {
                        lblStatus.Text = $"Kod not found: {System.IO.Path.GetFileName(pdfFile)}";
                    }
                }
                catch (Exception ex)
                {
                    lblStatus.Text = $"Eror: {System.IO.Path.GetFileName(pdfFile)} - {ex.Message}";
                }

                await Task.Delay(100);
            }

            MessageBox.Show("Operation completed!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async Task<string?> ExtractCodeFromPdf(string pdfPath)
        {
            return await Task.Run(() =>
            {
                using (PdfReader reader = new PdfReader(pdfPath))
                {
                    string text = "";
                    for (int page = 1; page <= reader.NumberOfPages; page++)
                    {
                        text += PdfTextExtractor.GetTextFromPage(reader, page);
                    }

                    Regex regex = new Regex(@"KOD:\s*([A-Z0-9-]+)", RegexOptions.IgnoreCase);
                    Match match = regex.Match(text);
                    
                    if (match.Success)
                    {
                        return match.Groups[1].Value.Trim();
                    }
                }
                return null;
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}