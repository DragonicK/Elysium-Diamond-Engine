using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

namespace Elysium_Diamond.Npcs {
    public class NpcManager {
        public static HashSet<NpcData> Npc { get; set; } = new HashSet<NpcData>();

        public static NpcData FindNpcByID(int id) {
            var find = from npc in Npc
                       where npc.ID.CompareTo(id) == 0
                       select npc;

            return find.FirstOrDefault();
        }

        /// <summary>
        /// Abre o arquivo para leitura.
        /// </summary>
        /// <returns></returns>
        public static bool OpenData() {
            if (!File.Exists($"{Common.Configuration.GamePath}/Data/npc.bin")) return false;

            using (FileStream file = new FileStream($"{Common.Configuration.GamePath}/Data/npc.bin", FileMode.Open, FileAccess.Read)) {
                BinaryReader reader = new BinaryReader(file);

                var lenght = reader.ReadInt32();

                for (int n = 0; n < lenght; n++) {
                    var npc = new NpcData();
                    npc.ID = reader.ReadInt32();
                    npc.Name = reader.ReadString();
                    npc.Sprite = reader.ReadInt16();
                    npc.Type = (NpcType)reader.ReadByte();
                    npc.Elite = (NpcEliteType)reader.ReadByte();
                    npc.Level = reader.ReadInt32();

                    Npc.Add(npc);
                }

                reader.Close();
            }

            return true;
        }
    }
}
