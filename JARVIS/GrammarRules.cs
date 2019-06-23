using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JARVIS
{
    public class GrammarRules
    {
        public static IList<string> WhatTimeIs = new List<string>()
        {
            "Que horas são",
            "Me diga as horas",
            "Poderia me dizer que horas são"
        };

        public static IList<string> WhatDateIs = new List<string>()
        {
            "Data de hoje",
            "Que dia é hoje",
            "Qual é a data de hoje",
            "Você sabe me dizer a data de hoje"
        };

        public static IList<string> JarvisStartListening = new List<string>()
        {
            "Jarvis",
            "Jarvis, você está ai",
            "Olá Jarvis",
            "Oi Jarvis",
            "Você esta ai Jarvis",
            "Pode voltar a ouvir",
            "Volte a ouvir",
            "Pode voltar a escutar",
            "Volte a escutar"
        };

        public static IList<string> JarvisStopListening = new List<string>(){
            "Pare de Ouvir",
            "Pare de me ouvir",
            "Pare de escutar",
            "Pare de me escutar"
        };

        public static IList<string> MinimizeWindow = new List<string>()
        {
            "Minimizar janela",
            "Minimize a janela",
            "Pome minimizar a janela"
        };

        public static IList<string> NormalWindow = new List<string>()
        {
            "Normalizar janela",
            "Normalize a janela",
            "Pode normalizar a janela",
            "Janela em tamanho normal",
            "Deixe a janela em tamanho normal",
            "Tamanho normal"
        };

        public static IList<string> ChangeVoice = new List<string>()
        {
            "Altere a voz",
            "Alterar voz"
        };

        public static IList<string> OpenProgram = new List<string>()
        {
            "Navegador",
            "Media Player"
        };

        public static IList<string> MediaPlayerCommands = new List<string>()
        {
            "Abrir arquivo"

        };

        /*public static IList<string> OpenBrowser = new List<string>()
        {
            ""
        };*/
    }
}
