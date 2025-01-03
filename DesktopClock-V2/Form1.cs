using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace DesktopClock_V2
{



    public partial class Form1 : Form
    {

#if DEBUG
        private readonly bool isDebug = true;
#else
        private readonly bool isDebug = false;
#endif

        public Form1()
        {
            InitializeComponent();
        }

        private void apdatetime()
        {
            DateTime dt = DateTime.Now;
            // timetxt.Text = (dt.Hour.ToString() + ":" + dt.Minute.ToString());
            timetxt.Text = dt.ToString("HH:mm");
            datetxt.Text = (dt.ToString("M") + " (" + dt.ToString("ddd") + ")");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            string PCN = Environment.MachineName;
            dev1.Text = PCN;

            apdatetime();
            if (PCN.Contains("IKA004"))
            {
                try
                {
                    string ikapicpath = "C:\\Users\\ika004\\Pictures\\GEKIKAWANENECHANWALLPAPERFORIKACLOCK.png";
                    Image iimage = Image.FromFile(ikapicpath);
                    this.BackgroundImage = iimage;
                }
                catch (FileNotFoundException)
                {

                }
                if (isDebug)
                {
                    dev1.Visible = true;
                    this.Text = this.Text + " -DEBUG";
                }
            }
        }
        private void uptime_Tick(object sender, EventArgs e)
        {
            apdatetime();
        }

        private void Strip_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }




        private void 壁紙変更CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //9:16の縦横比の写真しか行けないデメリットはあるけど、とりあえずはできる

            var ofd = new OpenFileDialog();
            ofd.Filter = "Image File(*.bmp,*.jpg,*.png,*.tif)|*.bmp;*.jpg;*.png;*.tif|Bitmap(*.bmp)|*.bmp|Jpeg(*.jpg)|*.jpg|PNG(*.png)|*.png";


            if (ofd.ShowDialog() == DialogResult.OK)

            {
                string FilePath = ofd.FileName;
                try
                {
                    Image SelectedImage = Image.FromFile(FilePath);
                    string filePath = ofd.FileName;
                    float aspe = 9f / 16f;
                    float imgaspe = (float)SelectedImage.Width / SelectedImage.Height;
                    if (Math.Abs(imgaspe - aspe) < 0.01f)
                    {
                        this.BackgroundImage = SelectedImage;
                        this.BackgroundImageLayout = ImageLayout.Zoom;
                        MessageBox.Show("壁紙を変更しました");
                    }
                    else
                    {
                        MessageBox.Show("壁紙を変更できませんでした。\n 画像ファイルの縦横比が9:16ではありません。\n クロップ機能をお試しください。", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"画像の読み込み中にエラーが発生しました: {ex.Message}");
                }


            }
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isDebug)
            {
                var ofd = new OpenFileDialog();
                ofd.Filter = "Image File(*.bmp,*.jpg,*.png,*.tif)|*.bmp;*.jpg;*.png;*.tif|Bitmap(*.bmp)|*.bmp|Jpeg(*.jpg)|*.jpg|PNG(*.png)|*.png";
                if (ofd.ShowDialog() == DialogResult.OK)

                {
                    string FilePath = ofd.FileName;
                    try
                    {
                        Image SelectedImage = Image.FromFile(FilePath);
                        string filePath = ofd.FileName;

                        float aspe = 9f / 16f;
                        float imgaspe = (float)SelectedImage.Width / SelectedImage.Height;
                        if (Math.Abs(imgaspe - aspe) < 0.01f)
                        {
                            this.BackgroundImage = SelectedImage;
                            this.BackgroundImageLayout = ImageLayout.Zoom;
                            MessageBox.Show("壁紙を変更しました");
                        }
                        else
                        {
                            Image CropIMG = Crop(SelectedImage);
                            this.BackgroundImage = CropIMG;
                        }


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"画像の読み込み中にエラーが発生しました: {ex.Message}");
                    }
                }
            }
        }


        public static Image Crop(Image SelectedImage, double targetAspectRatio = 9.0 / 16.0)
        {
            Rectangle ClopArea;
            float imgaspe = (float)SelectedImage.Width / SelectedImage.Height;
            double targetAspe = 9.0 / 16.0;
            if (imgaspe > targetAspe)
            {
                int NewW = (int)(SelectedImage.Height * targetAspe);
                int XOffset = (SelectedImage.Width - NewW) / 2;
                ClopArea = new Rectangle(XOffset, 0, NewW, SelectedImage.Height);
            }
            else
            {
                int newH = (int)(SelectedImage.Width / targetAspe);
                int YOffset = (SelectedImage.Height - newH) / 2;
                ClopArea = new Rectangle(0, YOffset, SelectedImage.Width, newH);
            }

            Bitmap CroppedIMG = new Bitmap(ClopArea.Width, ClopArea.Height);
            using (Graphics g = Graphics.FromImage(CroppedIMG))
            {
                g.DrawImage(SelectedImage, new Rectangle(0, 0, CroppedIMG.Width, CroppedIMG.Height), ClopArea, GraphicsUnit.Pixel);
            }
            return CroppedIMG;
        }

        private void ChangeWP_crop_Click(object sender, EventArgs e)
        {
            //Crop

            var ofd = new OpenFileDialog();
            ofd.Filter = "Image File(*.bmp,*.jpg,*.png,*.tif)|*.bmp;*.jpg;*.png;*.tif|Bitmap(*.bmp)|*.bmp|Jpeg(*.jpg)|*.jpg|PNG(*.png)|*.png";
            if (ofd.ShowDialog() == DialogResult.OK)

            {
                string FilePath = ofd.FileName;
                try
                {
                    Image SelectedImage = Image.FromFile(FilePath);
                    string filePath = ofd.FileName;

                    float aspe = 9f / 16f;
                    float imgaspe = (float)SelectedImage.Width / SelectedImage.Height;
                    if (Math.Abs(imgaspe - aspe) < 0.01f)
                    {
                        this.BackgroundImage = SelectedImage;
                        this.BackgroundImageLayout = ImageLayout.Zoom;
                        MessageBox.Show("壁紙を変更しました");
                    }
                    else
                    {
                        Image CropIMG = Crop(SelectedImage);
                        this.BackgroundImage = CropIMG;
                        MessageBox.Show("壁紙をクロップして変更しました");
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show($"画像の読み込み中にエラーが発生しました: {ex.Message}");
                }
            }
        }
    }
}
