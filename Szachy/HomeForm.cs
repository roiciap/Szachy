using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Szachy.Game;

namespace Szachy
{
    public partial class HomeForm : Form
    {
        public HomeForm()
        {
            InitializeComponent();
        }

        private void GraczVsGracz_Click(object sender, EventArgs e)
        {
            this.Hide();
            var game = new Form1(new Gamer(), new Gamer());
            game.FormClosed += (s, args) => this.Close();
            game.Show();
        }

        private void GraczVsKomputer_Click(object sender, EventArgs e)
        {
            this.Hide();
            var game = new Form1(new Gamer(), new PCPlayer());
            game.FormClosed += (s, args) => this.Close();
            game.Show();
        }

        private void KomputerVsGracz_Click(object sender, EventArgs e)
        {
            this.Hide();
            var game = new Form1(new PCPlayer(), new Gamer());
            game.FormClosed += (s, args) => this.Close();
            game.Show();
        }
    }
}
