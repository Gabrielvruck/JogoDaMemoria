using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JogoDaMemoria
{
    public partial class Form1 : Form
    {
        int movimentos, cliques, cartasEncontradas, tagIndex;
        Image[] img = new Image[20];
        List<string> lista = new List<string>();

        int[] tags = new int[2];

        public Form1()
        {
            InitializeComponent();
            inicio();
        }

        private void inicio()
        {
            foreach (PictureBox item in Controls.OfType<PictureBox>())
            {
                int tagIndex = int.Parse(string.Format("{0}", item.Tag));
                img[tagIndex] = item.Image;

                item.Image = Properties.Resources.verso;
                item.Enabled = true;
            }

            //posicoes();
        }

        private void posicoes()
        {
            foreach (PictureBox item in Controls.OfType<PictureBox>())
            {
                Random rdn = new Random();
                //Para cada imagem pegar o X/Y
                //X = HORIZONTE X = LINHA
                //Y = VERTICAL Y= COLUNA
                int[] xP = { 66, 270, 475, 681, 871, 1073, 1276, 1484, 1492, 1284, 1070, 872, 682, 475, 270, 66, 66, 272, 475, 678, 877, 1075, 1272, 1480, 61, 268, 479, 682, 876, 1075, 1280, 1488, 66, 273, 482, 685, 874, 1078, 1283, 1495 };
                int[] yP = { 10, 199, 384, 585, 788, 10, 199, 394, 587, 788, 10, 199, 394, 588, 788, 8, 201, 394, 589, 788, 788, 588, 393, 199, 10, 10, 197, 390, 587, 789, 789, 585, 392, 195, 2, 10, 201, 391, 587, 789 };

                repete:
                var x = xP[rdn.Next(0, xP.Length)];
                var y = yP[rdn.Next(0, yP.Length)];

                string verificacao = x.ToString() + y.ToString();

                if (lista.Contains(verificacao))
                {
                    goto repete;
                }
                else
                {
                    item.Location = new Point(x, y);
                    lista.Add(verificacao);
                }

            }
        }

        private void ImagensClick_Click(object sender, EventArgs e)
        {
            bool parEncontrado = false;

            PictureBox pic = (PictureBox)sender;
            cliques++;
            tagIndex = int.Parse(string.Format("{0}", pic.Tag));
            pic.Image = img[tagIndex];
            pic.Refresh();

            if (cliques == 1)
            {
                tags[0] = int.Parse(string.Format("{0}", pic.Tag));
            }
            else if (cliques == 2)
            {
                movimentos++;
                lblMovimentos.Text = "Movimentos: " + movimentos.ToString();
                tags[1] = int.Parse(string.Format("{0}", pic.Tag));
                parEncontrado = ChecagemPares();
                Desvirar(parEncontrado);
            }

        }

        private bool ChecagemPares()
        {
            cliques = 0;

            if (tags[0] == tags[1])
                return true;
            else
                return false;

        }

        private void Desvirar(bool check)
        {
            Thread.Sleep(500);

            foreach (PictureBox item in Controls.OfType<PictureBox>())
            {
                if (int.Parse(string.Format("{0}", item.Tag)) == tags[0] ||
                    int.Parse(string.Format("{0}", item.Tag)) == tags[1])
                {
                    if (check)
                    {
                        item.Enabled = false;
                        cartasEncontradas++;
                    }
                    else
                    {
                        item.Image = Properties.Resources.verso;
                        item.Refresh();
                    }
                }
            }
            FinalJogo();
        }

        private void FinalJogo()
        {
            if(cartasEncontradas == (img.Length * 2))
            {
                MessageBox.Show("Parabens, você terminou o jogo com " + movimentos.ToString() + "movimentos");
                DialogResult msg = MessageBox.Show("Deseja continuar o jogo ?", "Caixa de Pergunta", MessageBoxButtons.YesNo);

                if(msg != DialogResult.Yes)
                {
                    cliques = 0;movimentos = 0;cartasEncontradas = 0;
                    lista.Clear();
                    inicio();
                }
                else if(msg == DialogResult.No)
                {
                    MessageBox.Show("Obrigado por jogar!");
                    Application.Exit();
                }
            }
        }
    }
}
