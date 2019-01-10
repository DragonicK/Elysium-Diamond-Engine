using System.Collections.Generic;
using System.Linq;
using System.IO;
using Elysium_Diamond.DirectX;
using SharpDX.Direct3D9;
using System;

namespace Elysium_Diamond.Resource {
    public class DataManager {
        const int MAX_TALENT = 24;
        private static HashSet<Item> items;
        private static HashSet<TalentResource> talents;

        /// <summary>
        /// Realiza a busca pelo ID do item.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Item FindItemByID(int id) {
            var find_item = from item in items
                               where item.ID.CompareTo(id) == 0
                               select item;

            return find_item.FirstOrDefault();
        }

        /// <summary>
        /// Realiza a busca pelo ID do talento.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static TalentResource FindTalentByID(int id) {
            var find_talent = from talent in talents
                            where talent.ID.CompareTo(id) == 0
                            select talent;

            return find_talent.FirstOrDefault();
        }

        /// <summary>
        /// Carrega os arquivos de recursos.
        /// </summary>
        public static void Initialize() {
            items = new HashSet<Item>();
            talents = new HashSet<TalentResource>();
            var max_sprites = Directory.GetFiles($"{Common.Configuration.GamePath}/Data/Sprite/").Length;
            for (int n = 1; n <= max_sprites; n++) {
                EngineTexture.AddTexture(n, EngineTexture.TextureFromFile($"{Common.Configuration.GamePath}/Data/Sprite/{n}.png"), EngineTextureType.Sprites);
            }
    
            var max_icons = Directory.GetFiles($"{Common.Configuration.GamePath}/Data/Icon/").Length;
            for (int n = 1; n <= max_icons; n++) {
                EngineTexture.AddTexture(n, EngineTexture.TextureFromFile($"{Common.Configuration.GamePath}/Data/Icon/{n}.png"), EngineTextureType.Icons);
            }

            var max_items = Directory.GetFiles($"{Common.Configuration.GamePath}/Data/Items/").Length;
            for (int n = 1; n <= max_items; n++) {
                items.Add(Item.Read($"{Common.Configuration.GamePath}/Data/Items/{n}.item"));
            }

            var max_talent = Directory.GetFiles($"{Common.Configuration.GamePath}/Data/Talent/").Length;
            for (int n = 1; n <= max_talent; n++) {
                talents.Add(ReadTalent($"{Common.Configuration.GamePath}/Data/Talent/{n}.talent"));
            }
        }

        //private static void OpenTalent() {
        //    if (!File.Exists("Talent.bin")) {
        //        MessageBox.Show("Arquivo não encontrado", "Erro");
        //        return;
        //    }

        //    using (FileStream file = new FileStream("Talent.bin", FileMode.Open, FileAccess.Read)) {
        //        BinaryReader reader = new BinaryReader(file);

        //        //obtem a quantidade de classes
        //        var count = reader.ReadInt32();

        //        for (var n = 0; n < count; n++) {
        //            TalentStatic.Classes.Add(new ClasseTalent(reader.ReadInt32(), reader.ReadString()));

        //            for (var i = 0; i < TalentStatic.MAX_TALENT_NAME; i++) {
        //                TalentStatic.Classes[n].TalentName[i] = reader.ReadString();
        //            }

        //            for (var j = 0; j < TalentStatic.MAX_TALENT; j++) {
        //                TalentStatic.Classes[n].Balance[j] = reader.ReadInt32();
        //                TalentStatic.Classes[n].Physic[j] = reader.ReadInt32();
        //                TalentStatic.Classes[n].Magic[j] = reader.ReadInt32();
        //                TalentStatic.Classes[n].Restoration[j] = reader.ReadInt32();
        //            }
        //        }

        //        reader.Close();
        //    }
        //}

        private static TalentResource ReadTalent(string path) {
            var talent = new TalentResource();

            if (!File.Exists(path)) { return null; }

            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read)) {
                BinaryReader reader = new BinaryReader(file);

                talent.ID = reader.ReadInt32();
                talent.IconID = reader.ReadInt32();
                talent.Title = reader.ReadString();
                talent.Description = reader.ReadString();
                talent.MaxLevel = reader.ReadInt32();
                talent.ReqTalentID = reader.ReadInt32();
                talent.ReqTalentLevel = reader.ReadInt32();
                var count = reader.ReadInt32();
                for (var n = 0; n < count; n++) talent.Effect.Add(reader.ReadInt32());

                reader.Close();
            }

            return talent;
        }

        //private static void ReadTalent() {
        //    if (!File.Exists("./Data/talentdata.bin")) {
        //        throw new System.Exception("O arquivo talentdata.bin não foi encontrado");
        //    }

        //    using (FileStream file = new FileStream("./Data/Talent/Talent.bin", FileMode.Open, FileAccess.Read)) {
        //        BinaryReader reader = new BinaryReader(file);

        //        ReadTalent(ref reader, Balance);
        //        ReadTalent(ref reader, Physic);
        //        ReadTalent(ref reader, Magic);
        //        ReadTalent(ref reader, Restoration);

        //        reader.Close();
        //    }
        //}

        //private static void ReadTalent(ref BinaryReader reader, TalentResource[] talent) {
        //    for (var n = 0; n < MAX_TALENT; n++) {
        //        talent[n] = new TalentResource();
        //        talent[n].ID = reader.ReadInt32();
        //        talent[n].IconID = reader.ReadInt32();
        //        talent[n].Title = reader.ReadString();
        //        talent[n].Description = reader.ReadString();
        //        talent[n].MaxLevel = reader.ReadInt32();
        //        talent[n].Effect = reader.ReadInt32();
        //        talent[n].EffectPerLevel = reader.ReadInt32();
        //        talent[n].ReqTalentID = reader.ReadInt32();
        //        talent[n].ReqTalentLevel = reader.ReadInt32();
        //    }
        //}
    }
}
