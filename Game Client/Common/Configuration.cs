using System;
using System.Collections;
using System.IO;

namespace Elysium_Diamond.Common {
    public static class Configuration {

        #region Constant
        public const string FILE_CONFIG = "config.bin";
        public const string GAME_VERSION = "1.2.10";
        public const int MAX_CLASSE = 4;
        public const int MAX_CHARACTER = 4;
        public const int MAX_GAME_SERVER = 3;
        public const int MAX_CHANNEL = 5;
        #endregion

        #region Variable
        public static IPAddress[] IPAddress { get; set; }

        /// <summary>
        /// True quando ocorrer alguma desconexão.
        /// </summary>
        public static bool Disconnected { get; set; } = false;

        public static byte DisconnectState { get; set; }

        /// <summary>
        /// CheckSum do arquivo.
        /// </summary>
        public static string ClientSerial { get; set; }

        /// <summary>
        /// HexID recebido do login server.
        /// </summary>
        public static string HexID { get; set; }

        /// <summary>
        /// Idioma da UI.
        /// </summary>
        public static Language Language { get; set; } = Language.Portuguese;

        /// <summary>
        /// Caminho do cliente.
        /// </summary>
        public static string GamePath { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +"/Elysium/";

        #endregion

        #region Variable for latency
        public static bool PingSend { get; set; } = true;
        public static int PingStart { get;set; }
        public static int PingEnd { get; set; }
        public static int Latency { get; set; }
        #endregion


        /// <summary>
        /// lista de config.
        /// </summary>
        private static Hashtable cache = new Hashtable();

        /// <summary>
        /// Abre o arquivo de configuração.
        /// </summary>
        /// <returns></returns>
        public static bool Open() {
            if (!File.Exists(GamePath + FILE_CONFIG)) { return false; }

            using (FileStream file = new FileStream(GamePath + FILE_CONFIG, FileMode.Open, FileAccess.Read)) {
                BinaryReader reader = new BinaryReader(file);

                cache.Add("IP", "127.0.0.1");
                cache.Add("Port", 44405);
               // cache.Add("IP", reader.ReadString());
              //  cache.Add("Port", reader.ReadInt32());

                reader.Close();
            }

            IPAddress = new IPAddress[MAX_GAME_SERVER];
         
            //prepare some basic data
            IPAddress[(int)SocketEnum.LoginServer] = new IPAddress((string)cache["IP"], (int)cache["Port"]);
            IPAddress[(int)SocketEnum.WorldServer] = new IPAddress(string.Empty);
            IPAddress[(int)SocketEnum.GameServer] = new IPAddress(string.Empty);

            return true;
        }

        /// <summary>
        /// Salva o arquivo de configuração.
        /// </summary>
        public static void Save() {
            using (FileStream file = new FileStream(GamePath + FILE_CONFIG, FileMode.Create, FileAccess.Write)) {
                BinaryWriter writer = new BinaryWriter(file);

                //save
                //cache.Add("IP", "juliosperandio.ddns.net");
                //cache.Add("Port", 44405);

                writer.Write((string)cache["IP"]);
                writer.Write((int)cache["Port"]);

                writer.Close();
            }
        }

        /// <summary>
        /// Converte e retorna um bool.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetBoolean(string key) {
            return Convert.ToBoolean(Convert.ToInt32(cache[key]));
        }

        /// <summary>
        /// Converte e retorna em byte.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte GetByte(string key) {
            return Convert.ToByte(cache[key]);
        }
        /// <summary>
        /// Retorna um inteiro de 32 bits.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetInt32(string key) {
            return Convert.ToInt32(cache[key]);
        }

        /// <summary>
        /// Retorna um inteiro de 64 bits.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long GetInt64(string key) {
            return Convert.ToInt64(cache[key]);
        }

        /// <summary>
        /// Converte para string.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetString(string key) {
            if (cache[key] == null) return "command not found";

            return cache[key].ToString();
        }
    }
}
