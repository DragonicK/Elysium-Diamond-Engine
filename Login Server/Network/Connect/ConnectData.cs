﻿using Lidgren.Network;
using LoginServer.Common;
using LoginServer.Server;
using LoginServer.Database;

namespace LoginServer.Network {
    public static class ConnectData {
        /// <summary>
        /// Analisa o cabeçalho e processa a mensagem.
        /// </summary>
        /// <param name="msg"></param>
        public static void HandleData(NetIncomingMessage msg) {
            //se algum pacote estiver com menos que 2 bytes, sai do método
            if (msg.LengthBytes < 2) return;
            // cabeçalho da msg
            var header = msg.ReadInt16();

            // chama o método
            switch (header) {
                case (short)PacketList.LS_WS_IsPlayerConnected: Authentication.Login(msg.ReadBoolean(), msg.ReadString()); break;
                case (short)PacketList.WS_LS_UpdatePin: UpdateUserPin(msg); break;
                case (short)PacketList.WS_LS_UpdatePinAttempt: UpdateUserPinAttempt(msg); break;
                case (short)PacketList.WS_LS_UpdateBan: UpdateUserBan(msg); break;
                case (short)PacketList.WS_LS_UpdateCash: UpdateUserCash(msg); break;
                case (short)PacketList.WS_LS_UpdateDisconnect: UpdateUserDisconnect(msg); break;
                case (short)PacketList.WS_LS_UpdateUserStatus: UpdateUserStatus(msg.ReadInt32()); break;
                case (short)PacketList.WS_LS_InsertService: InsertUserService(msg); break;
            }
        }

        /// <summary>
        /// Atualiza o PIN do usuário.
        /// </summary>
        /// <param name="msg"></param>
        public static void UpdateUserPin(NetIncomingMessage msg) {
            var id = msg.ReadInt32();
            var pin = msg.ReadString();
            AccountDB.UpdatePin(id, pin);
        }

        /// <summary>
        /// Atualiza o número de tentativas PIN.
        /// </summary>
        /// <param name="msg"></param>
        public static void UpdateUserPinAttempt(NetIncomingMessage msg) {
            var id = msg.ReadInt32();
            var value = msg.ReadByte();
            AccountDB.UpdatePinAttempt(id, value);
        }

        /// <summary>
        /// Bloqueia o usuário por determinado tempo.
        /// </summary>
        /// <param name="msg"></param>
        public static void UpdateUserBan(NetIncomingMessage msg) {
            var id = msg.ReadInt32();
            var minutes = msg.ReadInt16();
            var reason = msg.ReadString();

            AccountDB.BanAccount(id, minutes, reason);
        }

        /// <summary>
        /// Altera o cash do usuário.
        /// </summary>
        /// <param name="msg"></param>
        public static void UpdateUserCash(NetIncomingMessage msg) {
            var id = msg.ReadInt32();
            var value = msg.ReadInt32();

            AccountDB.UpdateCash(id, value);
        }

        /// <summary>
        /// Indica se o jogador está online ou não.
        /// </summary>
        /// <param name="msg"></param>
        public static void UpdateUserDisconnect(NetIncomingMessage msg) {
            var id = msg.ReadInt32();
            var ip = msg.ReadString();

            AccountDB.UpdateLoggedIn(id, 0); //0 disconnected
            AccountDB.UpdateLastIP(id, ip);
            AccountDB.UpdateCurrentIP(id, string.Empty); //limpa o ip atual
        }

        /// <summary>
        /// Indica que o usuário conectou ao world server.
        /// </summary>
        /// <param name="msg"></param>
        public static void UpdateUserStatus(int accountID) {
            Authentication.FindByAccountID(accountID).IsWorldConnected = true;
        }

        /// <summary>
        /// Adiciona um novo serviço à conta de usuário.
        /// </summary>
        /// <param name="msg"></param>
        public static void InsertUserService(NetIncomingMessage msg) {
            var accountID = msg.ReadInt32();
            var serviceID = msg.ReadInt32();
            var days = msg.ReadInt32();

            AccountDB.InsertService(accountID, serviceID, days);
        }
     }
}