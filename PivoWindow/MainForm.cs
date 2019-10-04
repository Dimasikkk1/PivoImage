using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using PivoLib;

namespace PivoWindow
{
    public partial class MainForm : Form
    {
        public MainForm() => InitializeComponent();

        private void BOpen_Click(object sender, EventArgs e)
        {
            bOpen.Enabled = false;
            bOpenPivo.Enabled = false;
            bSavePivo.Enabled = false;
            using (OpenFileDialog dialog = new OpenFileDialog() { Filter = "Картинки (*.bmp;*.gif;*.jpg;*.jpeg;*.png;*.tiff;*.ico)|*.bmp;*.gif;*.jpg;*.jpeg;*.png;*.tiff;*.ico|Все файлы (*.*)|*.*" })
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (dialog.FileName.IndexOf(".pivo") != -1)
                    {
                        MessageBox.Show("Здесь нельзя открыть pivo, иначе программа сломается.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    try
                    {
                        pictureBox1.Image = Image.FromFile(dialog.FileName);
                    }
                    catch (OutOfMemoryException)
                    {
                        MessageBox.Show("И чего ты этим хотел добиться?", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw;
                    }

                    Text = dialog.FileName;
                    lSize.Text = $"{pictureBox1.Image.Width}x{pictureBox1.Image.Height}";
                    bSavePivo.Enabled = true;
                }
            bOpen.Enabled = true;
            bOpenPivo.Enabled = true;
        }

        private async void BOpenPivo_Click(object sender, EventArgs e)
        {
            bOpen.Enabled = false;
            bOpenPivo.Enabled = false;
            bSavePivo.Enabled = false;
            using (OpenFileDialog dialog = new OpenFileDialog() { Filter = "pivo файлы (*.pivo)|*.pivo" })
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = await Pivo.OpenAsync(dialog.FileName);
                    Text = dialog.FileName;
                    lSize.Text = $"{pictureBox1.Image.Width}x{pictureBox1.Image.Height}";
                    bSavePivo.Enabled = true;
                }
            bOpen.Enabled = true;
            bOpenPivo.Enabled = true;
        }

        private async void BSavePivo_Click(object sender, EventArgs e)
        {
            bOpen.Enabled = false;
            bOpenPivo.Enabled = false;
            bSavePivo.Enabled = false;
            using (SaveFileDialog dialog = new SaveFileDialog() { Filter = "pivo файлы (*.pivo)|*.pivo" })
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    await Pivo.SaveAsync(dialog.FileName, (Bitmap)pictureBox1.Image);
                    Text = dialog.FileName;
                }
            bOpen.Enabled = true;
            bOpenPivo.Enabled = true;
            bSavePivo.Enabled = true;
        }
    }
}