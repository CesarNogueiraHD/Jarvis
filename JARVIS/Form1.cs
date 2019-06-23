using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.Speech.Recognition;


namespace JARVIS
{
    public partial class Form1 : Form
    {
        private SelectVoz selectVoice = null;

        private SpeechRecognitionEngine engine;

        private Browser browser;
        private MediaPlayer mediaPlayer;

        private bool isJarvisListening = true;
        public Form1()
        {
            InitializeComponent();
        }

        private void LoadSpeech()
        {
            try
            {
                engine = new SpeechRecognitionEngine();
                engine.SetInputToDefaultAudioDevice(); //microfone

                Choices cNumbers = new Choices();

                for (int i = 0; i <= 100; i++)
                    cNumbers.Add(i.ToString());

                Choices c_commandsOfSystem = new Choices();
                c_commandsOfSystem.Add(GrammarRules.WhatTimeIs.ToArray());
                c_commandsOfSystem.Add(GrammarRules.WhatDateIs.ToArray());
                c_commandsOfSystem.Add(GrammarRules.JarvisStartListening.ToArray());
                c_commandsOfSystem.Add(GrammarRules.JarvisStopListening.ToArray());
                c_commandsOfSystem.Add(GrammarRules.MinimizeWindow.ToArray());
                c_commandsOfSystem.Add(GrammarRules.NormalWindow.ToArray());
                c_commandsOfSystem.Add(GrammarRules.ChangeVoice.ToArray());
                c_commandsOfSystem.Add(GrammarRules.OpenProgram.ToArray());
                c_commandsOfSystem.Add(GrammarRules.MediaPlayerCommands.ToArray());

                GrammarBuilder gb_commandsOfSystem = new GrammarBuilder();
                gb_commandsOfSystem.Append(c_commandsOfSystem);

                Grammar g_commandsOfSystem = new Grammar(gb_commandsOfSystem);
                g_commandsOfSystem.Name = "sys";

                GrammarBuilder gbNumber = new GrammarBuilder();
                gbNumber.Append(cNumbers);
                gbNumber.Append(new Choices("vezes", "mais", "menos", "por"));
                gbNumber.Append(cNumbers);

                Grammar gNumbers = new Grammar(gbNumber);
                gNumbers.Name = "calc";

                engine.LoadGrammar(g_commandsOfSystem);
                engine.LoadGrammar(gNumbers);

                //string[] words = { "olá", "bom dia" 
                //engine.LoadGrammar(new Grammar(new GrammarBuilder(new Choices(words))));

                engine.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(rec);

                engine.AudioLevelUpdated += new EventHandler<AudioLevelUpdatedEventArgs>(audioLevel);

                engine.SpeechRecognitionRejected += new EventHandler<SpeechRecognitionRejectedEventArgs>(rej);

                //inicia o reconhecimento
                engine.RecognizeAsync(RecognizeMode.Multiple);

                Speaker.Speak("estou carregando os arquivos.");
            }
            catch (Exception e)
            {
                MessageBox.Show("Ocorreu erro no LoadSpeech(): " + e.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadSpeech();
            Speaker.Speak("Ja carreguei os arquivos, Estou pronto!");
        }

        //método que é chamado quando algo é reconhecido
        private void rec(object s, SpeechRecognizedEventArgs e)
        {
            //MessageBox.Show(e.Result.Text);
            //Speaker.Speak(e.Result.Text);

            string speech = e.Result.Text;
            float conf = e.Result.Confidence;

            //string date = DateTime.Now.ToShortDateString().Replace("\\", "-");
            string date = DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString();
            string log_filename = "log\\" + date + ".txt";

            StreamWriter sw = File.AppendText(log_filename);

            if (File.Exists(log_filename))
            {
                //File.WriteAllText(log_filename, "\n" + speech, Encoding.UTF8);
                sw.WriteLine(speech);
            } else
            {
                /*File.Create(log_filename);
                File.WriteAllText(log_filename, "\n" + speech, Encoding.UTF8);*/
                sw.WriteLine(speech);
            }

            sw.Close();

            if(conf > 0.35f)
            {
                this.label1.BackColor = Color.DarkCyan;
                this.label1.ForeColor = Color.LawnGreen;

                this.label1.Text = "Reconhecido: " + speech;

                if (GrammarRules.JarvisStopListening.Any(x => x == speech))
                {
                    isJarvisListening = false;
                    Speaker.Speak("Como quiser!");
                } else if(GrammarRules.JarvisStartListening.Any(x => x == speech))
                {
                    isJarvisListening = true;
                    Speaker.Speak("Estou ouvindo, pode falar!");
                }

                if(isJarvisListening == true)
                {
                    switch (e.Result.Grammar.Name)
                    {
                        case "sys":
                            if (GrammarRules.WhatTimeIs.Any(x => x == speech))
                            {
                                Runner.WhatTimeIs();
                            }
                            else if (GrammarRules.WhatDateIs.Any(x => x == speech))
                            {
                                Runner.WhatDateIs();
                            }
                            else if (GrammarRules.MinimizeWindow.Any(x => x == speech))
                            {
                                MinimizeWindow();
                            }
                            else if (GrammarRules.NormalWindow.Any(x => x == speech))
                            {
                                NormalWindow();
                            }
                            else if (GrammarRules.ChangeVoice.Any(x => x == speech))
                            {
                                if (selectVoice == null || selectVoice.IsDisposed == true)
                                    selectVoice = new SelectVoz();
                                selectVoice.Show();
                            }
                            else if (GrammarRules.OpenProgram.Any(x => x == speech))
                            {
                                switch (speech)
                                {
                                    case "Navegador":
                                        browser = new Browser();
                                        browser.Show();
                                        break;
                                    case "Media Player":
                                        mediaPlayer = new MediaPlayer();
                                        mediaPlayer.Show();
                                        break;
                                }
                            } else if (GrammarRules.MediaPlayerCommands.Any(x => x == speech))
                            {
                                switch (speech)
                                {
                                    case "Abrir arquivo":
                                        if (mediaPlayer != null)
                                        {
                                            mediaPlayer.OpenFile();
                                            Speaker.Speak("Selecione um arquivo");
                                        } else
                                        {
                                            Speaker.Speak("Media Player não está aberto");
                                        }
                                        break;
                                }
                            }
                            break;
                        case "calc":
                            Speaker.Speak(CalcSolver.Solve(speech));
                            break;
                    }
                }
                
            } else
            {
                this.label1.ForeColor = Color.Orange;
            }
        }

        private void audioLevel(object s, AudioLevelUpdatedEventArgs e)
        {
            this.progressBar1.Maximum = 100;
            this.progressBar1.Value = e.AudioLevel;
        }

        private void rej(object s, SpeechRecognitionRejectedEventArgs e)
        {
            this.label1.ForeColor = Color.Red;
        }

        private void MinimizeWindow()
        {
            if(this.WindowState == FormWindowState.Normal || this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Minimized;
                Speaker.Speak("Minimizando a Janela!", "Como quiser!", "Tudo bem", "OK");
            } else
            {
                Speaker.Speak("Já está minimizada", "A janela já está minimizada", "Já fiz isso");
            }
        }

        private void NormalWindow()
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
                Speaker.Speak("Normalizando a janela", "Como quiser!", "Tudo bem", "OK");
            } else
            {
                Speaker.Speak("Já está em tamanho normal", "A janela já está em tamanho normal", "Já fiz isso");
            }
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
    }
}
