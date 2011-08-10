// This is a helper class for event sharing in a
// non-event-based environment.

using System;

namespace CodeGame.Network {
    
    public enum Command {

        Null,
        JoinGame,
        JoinGameSuccess,
        Ready, 
        ReadyYes,
        ReadyNo,
        PlayerJoin, 
        PlayerExit,
        GameBeginRequest,
        GameBegin,
        DrawCard,
        YourTurn, 
        PlayersTurn, 
        CardPlayed

    }
}
