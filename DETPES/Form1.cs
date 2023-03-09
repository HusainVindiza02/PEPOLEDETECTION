using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Emgu;
using Emgu.Util;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.Structure;
using Emgu.CV;


namespace DETPES
{
    public partial class Form1 : Form
    {
        private VideoCapture capture = null;

        private double frames;

        private double framesCounter;

        private double fps;

        private bool play = false;
        public Form1()
        {
            InitializeComponent();
        }
        private Image<Bgr, byte> Find(Image<Bgr, Byte> image)
        {
            MCvObjectDetection[] regions;

            using (HOGDescriptor descriptor = new HOGDescriptor())
            {
                descriptor.SetSVMDetector(HOGDescriptor.GetDefaultPeopleDetector());

                regions = descriptor.DetectMultiScale(image);
            }
            foreach (MCvObjectDetection pesh in regions)
            {
                image.Draw(pesh.Rect, new Bgr(Color.Green), 3);

                CvInvoke.PutText(image, "peshehod", new Point(pesh.Rect.X, pesh.Rect.Y), Emgu.CV.CvEnum.FontFace.HersheyPlain, 1, new MCvScalar(255, 255, 255), 2);

            }
            return image;
        }
        private async void ReadFrames()
        {
            Mat m = new Mat();

            while(play && framesCounter < frames)
            {
                framesCounter += 1;
                capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, framesCounter);
                capture.Read(m);
                pictureBox1.Image = m.Bitmap;
                pictureBox2.Image = Find(m.ToImage<Bgr, byte>()).Bitmap;
                toolStripLabel1.Text = $"{framesCounter} / {frames}";
                await Task.Delay(1000 / Convert.ToInt16(fps));
            }
        }


        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult res = openFileDialog1.ShowDialog();
                if (res == DialogResult.OK)
                {
                    capture= new VideoCapture(openFileDialog1.FileName);

                    Mat m = new Mat();
                    capture.Read(m);
                    pictureBox1.Image = m.Bitmap;

                    fps = capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps);

                    frames = capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameCount);

                    framesCounter = 1;
                }
                else
                {
                    MessageBox.Show("видео не выбрано", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }

        }

        private void распознатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (capture == null)
                    throw new Exception("Видео не выбрано");
                play = true;
                ReadFrames();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                play = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (capture == null)
                    throw new Exception("Видео не выбрано");
                play = true;
                ReadFrames();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            try
            {
                framesCounter -= Convert.ToDouble(numericUpDown1.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                framesCounter += Convert.ToDouble(numericUpDown1.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void файлToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
