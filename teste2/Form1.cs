using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace teste2
{
    public partial class Form1 : Form
    {
        private List<Image> LoadedImages { get; set; }
        private FilterInfoCollection CaptureDevice;
        private VideoCaptureDevice VideoDevices;
        public Form1()
        {
            InitializeComponent();
            getallcameralist();
        }
        void getallcameralist() 
        {
            CaptureDevice = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo Devices in CaptureDevice) 
            {
                cbCamera.Items.Add(Devices.Name);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                VideoDevices = new VideoCaptureDevice(CaptureDevice[cbCamera.SelectedIndex].MonikerString);
                VideoDevices.NewFrame += new NewFrameEventHandler(NewVideoFrame);
                VideoDevices.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void NewVideoFrame(object sender, NewFrameEventArgs eventArgs) 
        {
            pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = (Bitmap)pictureBox1.Image.Clone();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "save image as";
            sfd.Filter = "image files(*.jpg, *.png) | *.jpg; *.png";
            ImageFormat imageFormat = ImageFormat.Png;
            if (sfd.ShowDialog() == DialogResult.OK) 
            {
                string ext = System.IO.Path.GetExtension(sfd.FileName);
                switch (ext)
                {
                    case ".jpg":
                        imageFormat = ImageFormat.Jpeg;
                        break;
                    case ".bmp":
                        imageFormat = ImageFormat.Bmp;
                        break;
                }
                pictureBox2.Image.Save(sfd.FileName,imageFormat);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (VideoDevices.IsRunning == true) 
            {
                VideoDevices.Stop();
            }
            Application.Exit(null);
        }
        private void LoadImagesFromFolder()
        {
            string[] arquivos = Directory.GetFiles(@"C:\Users\fabio\Desktop\imagens\");
            LoadedImages = new List<Image>();
            foreach (var item in arquivos)
            {
                var tempImage = Image.FromFile(item);
                LoadedImages.Add(tempImage);
            }
        }
        private void imageList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
        }

        private void btnImagem_Click(object sender, EventArgs e)
        {
            imageList.Clear();
            //carregando imagens da pasta
            LoadImagesFromFolder();

            //inicializando lista de imagens
            ImageList images = new ImageList();
            images.ImageSize = new Size(130, 40);

            foreach (var image in LoadedImages)
            {
                images.Images.Add(image);
            }

            //configurando nossa listview com imageList
            imageList.LargeImageList = images;

            for (int itemIndex = 1; itemIndex <= LoadedImages.Count; itemIndex++)
            {
                imageList.Items.Add(new ListViewItem($"Imagem {itemIndex}", itemIndex - 1));
            }
        }

        private void imageList_ItemSelectionChanged_1(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (imageList.SelectedIndices.Count > 0)
            {
                var selectedIndex = imageList.SelectedIndices[0];
                Image selectedImg = LoadedImages[selectedIndex];
                pictureBox2.Image = selectedImg;
            }
        }
    }
}
