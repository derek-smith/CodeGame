﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using CodeGame.Classes.Screens;

namespace CodeGame.Classes.Network.Client {

    class Client2 {
        MenuScreen menuScreen = null;
        LobbyScreen lobbyScreen = null;
        GameScreen gameScreen = null;

        BinaryReader reader = null;
        BinaryWriter writer = null;

        //
        // Constructor
        //
        
        public Client2(ScreenManager mgr) {
            menuScreen = mgr.MenuScreen;
            lobbyScreen = mgr.LobbyScreen;
            gameScreen = mgr.GameScreen;

            Thread thread = new Thread(ClientLoop);
            thread.Start();
        }

        //
        // Infinite loop (on it's own thread)
        //

        private void ClientLoop() {
            
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(new IPEndPoint(menuScreen.IPAddress, 12345));

            NetworkStream stream = new NetworkStream(socket);
            reader = new BinaryReader(stream);
            writer = new BinaryWriter(stream);

            // First command sent
            writer.Write((int)Command.JoinGame);
            writer.Write(menuScreen.Nick);

            int cmd = -1;

            while (true) {
                // Block here, we ALWAYS expect an int FIRST
                cmd = reader.ReadInt32();

                // Cast the int to a Command and switch
                switch ((Command)cmd) {

                    // This a confirmation that we joined the lobby
                    case Command.JoinGameSuccess:

                        // We expect the number of players next
                        int numPlayers = reader.ReadInt32();

                        // And loop, grabbing all players and
                        // giving them to lobbyScreen to draw
                        for (int i = 0; i < numPlayers; i++) {
                            string nick = reader.ReadString();
                            lobbyScreen.PlayerJoin(nick);
                        }
                        break;

                    // A new player has joined
                    case Command.PlayerJoin:

                        // Grab new player's nick and give it
                        // to lobbyScreen
                        string newNick = reader.ReadString();
                        lobbyScreen.PlayerJoin(newNick);
                        break;

                    // A player is ready to begin
                    case Command.ReadyYes:

                        int yesID = reader.ReadInt32();
                        lobbyScreen.ReadyYes(yesID);
                        break;

                    // A player is NOT ready to begin
                    case Command.ReadyNo:

                        int noID = reader.ReadInt32();
                        lobbyScreen.ReadyNo(noID);
                        break;
                }
            }
        }
    }

    class Client {
        Commands _commands = new Commands();
        BinaryWriter _writer = null;
        List<string> _playerNames = new List<string>(4);
        List<int> _playerIDs = new List<int>(4);

        public Client(string nickname) {
            Thread clientThread = new Thread(Run);
            clientThread.Start(nickname);
        }

        // Todo: Add error handling
        private void Run(object o) {
            string nickname = (string)o;
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345));
            NetworkStream stream = new NetworkStream(socket);
            BinaryReader reader = new BinaryReader(stream);
            _writer = new BinaryWriter(stream);

            _writer.Write((int)Command.JoinGame);
            _writer.Write(nickname);

            int cmd = -1;

            while (true) {
                cmd = reader.ReadInt32();

                switch ((Command)cmd) {
                    case Command.JoinGameSuccess:
                        int numPlayers = reader.ReadInt32();
                        for (int i = 0; i < numPlayers; i++) {
                            string name = reader.ReadString();
                            _playerNames.Add(name);
                        }
                        break;
                }
            }
        }
    }
}
