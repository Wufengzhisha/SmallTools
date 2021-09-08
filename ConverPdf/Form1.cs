using PdfiumViewer;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace ConverPdf
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        private void butPDF_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                //FileName：显示文件路径+名字
                this.txtPDF.Text = dialog.FileName;
            }
        }

        private void butTUPIAM_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            if (folder.ShowDialog() == DialogResult.OK)
            {

                //SelectedPath:获取文件夹绝对路径,显示在textbox里面
                this.txtJPG.Text = folder.SelectedPath;
            }
        }

        private void butPDFtoTupian_Click(object sender, EventArgs e)
        {
            string strpdfPath = txtPDF.Text.ToString();
            var pdf = PdfiumViewer.PdfDocument.Load(strpdfPath);
            var pdfpage = pdf.PageCount;
            var pagesizes = pdf.PageSizes;


            string fileName = Path.GetFileNameWithoutExtension(txtPDF.Text.ToString());

            for (int i = 1; i <= pdfpage; i++)
            {
                Size size = new Size();
                size.Height = (int)pagesizes[(i - 1)].Height;
                size.Width = (int)pagesizes[(i - 1)].Width;
                //可以把".jpg"写成其他形式
                if (pdfpage == 1)
                {
                    RenderPage(strpdfPath, i, size, Path.Combine(txtJPG.Text.ToString(), fileName + "." + comboBox1.Text));
                }
                else
                {
                    RenderPage(strpdfPath, i, size, Path.Combine(txtJPG.Text.ToString(), fileName+i+ "." + comboBox1.Text));
                }
            }

            MessageBox.Show("转换成功！");
        }

        public void RenderPage(string pdfPath, int pageNumber, System.Drawing.Size size, string outputPath, int dpi = 300)
        {
            using (var document = PdfiumViewer.PdfDocument.Load(pdfPath))
            using (var stream = new FileStream(outputPath, FileMode.Create))
            using (var image = GetPageImage(pageNumber, size, document, dpi))
            {
                image.Save(stream, ImageFormat.Jpeg);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageNumber">pdf文件张数</param>
        /// <param name="size">pdf文件尺寸</param>
        /// <param name="document">pdf文件位置</param>
        /// <param name="dpi"></param>
        /// <returns></returns>
        private static System.Drawing.Image GetPageImage(int pageNumber, Size size, PdfiumViewer.PdfDocument document, int dpi)
        {
            return document.Render(pageNumber - 1, size.Width, size.Height, dpi, dpi, PdfRenderFlags.Annotations);
        }
    }
}
