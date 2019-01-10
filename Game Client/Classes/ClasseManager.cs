using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

namespace Elysium_Diamond.Classes {
    public static class ClasseManager {
        /// <summary>
        /// Lista de classes.
        /// </summary>
        public static List<ClasseDescription> Classes { get; set; }

        /// <summary>
        /// Inicializa e carrega todas as informações de classes.
        /// </summary>
        public static void Initialize() {
            Classes = new List<ClasseDescription>();
            var value = Directory.GetFiles($"{Common.Configuration.GamePath}/Data/Classes/");
            foreach (var file in value) { Classes.Add(GetClasse(file)); }
        }

        /// <summary>
        /// Obtem o índice da classe a partir do ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int GetIndexByID(int id) {
            for(var index = 0; index < Classes.Count; index++) {
                if (Classes[index].ID == id) { return index; }
            }

            return 0;
        }

        /// <summary>
        /// Carrega o arquivo de classe.
        /// </summary>
        /// <param name="classe_file"></param>
        /// <returns></returns>
        private static ClasseDescription GetClasse(string classe_file) {
            var file = new FileStream(classe_file, FileMode.Open);
            var reader = new BinaryReader(file, Encoding.Unicode);
            var classe = new ClasseDescription();

            classe.Name = reader.ReadString();
            classe.Description = reader.ReadString();
            classe.ID = reader.ReadInt32();
            classe.HP = reader.ReadInt32();
            classe.MP = reader.ReadInt32();
            classe.Attack = reader.ReadInt32();
            classe.Defense = reader.ReadInt32();
            classe.MagicAttack = reader.ReadInt32();
            classe.MagicDefense = reader.ReadInt32();
            classe.Strenght = reader.ReadInt32();
            classe.Dexterity = reader.ReadInt32();
            classe.Agility = reader.ReadInt32();
            classe.Constitution = reader.ReadInt32();
            classe.Intelligence = reader.ReadInt32();
            classe.Wisdom = reader.ReadInt32();
            classe.Will = reader.ReadInt32();
            classe.Mind = reader.ReadInt32();
            classe.Charisma = reader.ReadInt32();
            classe.FemaleSprite = reader.ReadInt32();
            classe.MaleSprite = reader.ReadInt32();
            classe.Sprite = new int[7];
            for (var n = 0; n < 7; n++) { classe.Sprite[n] = reader.ReadInt32(); }

            reader.Close();

            return classe;
        }
    }
}
