using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;

namespace JARVIS
{
    public partial class SelectVoz : Form
    {
        private SpeechSynthesizer sp = new SpeechSynthesizer();
        public SelectVoz()
        {
            InitializeComponent();

            comboBox1.Items.Clear();

            foreach(InstalledVoice voice in sp.GetInstalledVoices())
            {
                comboBox1.Items.Add(voice.VoiceInfo.Name);
            }
            comboBox1.SelectedIndex = 0;
        }

        //form carregado
        private void SelectVoz_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Speaker.SetVoice(comboBox1.SelectedItem.ToString());
            Speaker.Speak("A voz  foi alterada com sucesso", "Voz modificada com sucesso");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
