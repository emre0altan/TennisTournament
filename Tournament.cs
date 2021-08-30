using System;
using System.Collections.Generic;
using System.Linq;

namespace TennisTournament {

    enum SurfaceType { CLAY, GRASS, HARD}
    enum TournamentType { LEAGUE, ELIMINATION}

    class Tournament {

        public int id { get; }
        public SurfaceType surface { get; set; }
        public TournamentType type { get; set; }

        float currentMatchPlayer1Points, currentMatchPlayer2Points;

        Tournament() { }

        /// <summary>
        /// Starts the tournament
        /// </summary>
        public void MakeTournament(List<Player> _players) {
            if (type == TournamentType.LEAGUE) MakeLeagueTournament(_players);
            else MakeEliminationTournament(_players);
        }

        #region League
        private static Random rand = new Random();

        /// <summary>
        /// Starts the league tournament with randomized list of players
        /// </summary>
        void MakeLeagueTournament(List<Player> _players) {
            List<Player> circledPlayers = _players.OrderBy(a => rand.Next()).ToList();
            MakeLeagueMatches(circledPlayers);
        }

        /// <summary>
        /// Starts the league matches
        /// </summary>
        void MakeLeagueMatches(List<Player> _circledPlayers) {
            int playerCount = _circledPlayers.Count;
            for (int i = 0; i < playerCount - 1; i++) {
                for (int j = 0; j < playerCount / 2; j++) {
                    OneMatch(_circledPlayers[j], _circledPlayers[playerCount / 2 - j]);
                }
                CircleOneRight(_circledPlayers);
            }
        }

        /// <summary>
        /// Shifts players in the list to right by holding the first player
        /// </summary>
        void CircleOneRight(List<Player> _circledPlayers) {
            Player tempPlayer = null;
            for(int i = 1; i < _circledPlayers.Count - 1; i++) {
                tempPlayer = _circledPlayers[i + 1];
                _circledPlayers[i + 1] = _circledPlayers[i];
            }
            _circledPlayers[1] = tempPlayer;
        }

        #endregion

        #region Elimination

        /// <summary>
        /// Starts the elimination tournament
        /// </summary>
        void MakeEliminationTournament(List<Player> _players) {
            int[] eliminations = new int[_players.Count];
            for (int i = 0; i < eliminations.Length; i++) eliminations[i] = _players[i].id - 1;

            int totalLayerCount = (int)MathF.Log2(_players.Count);
            int currentLayer = 0;
            MakeEliminations(_players, eliminations, totalLayerCount, currentLayer);
        }

        /// <summary>
        /// Makes the eliminations recursively
        /// </summary>
        void MakeEliminations(List<Player> _players, int[] _eliminations, int _totalLayerCount, int _currentLayer) {
            for(int i = 0, j = 0; i < _eliminations.Length / MathF.Pow(2, (_currentLayer + 1)); i++, j += 2) {
                _eliminations[i] = OneMatch(_players[_eliminations[j]], _players[_eliminations[j + 1]]);
            }
            _currentLayer++;
            if (_currentLayer == _totalLayerCount) return;
            else MakeEliminations(_players, _eliminations, _totalLayerCount, _currentLayer);
        }
        #endregion

        #region OneMatch

        /// <summary>
        /// Makes a match of 2 players
        /// </summary>
        int OneMatch(Player _player1, Player _player2) {
            currentMatchPlayer1Points = currentMatchPlayer2Points = 1;

            AddLeftHandPoints(_player1, _player2);
            AddExperiencePoints(_player1, _player2);
            AddSurfacePoints(_player1, _player2);

            return GetCurrentMatchWinner(_player1, _player2);
        }

        /// <summary>
        /// Handles left hand points
        /// </summary>
        void AddLeftHandPoints(Player _player1, Player _player2) {
            if (_player1.hand == HandType.LEFT) currentMatchPlayer1Points += 2;
            if (_player2.hand == HandType.LEFT) currentMatchPlayer2Points += 2;
        }

        /// <summary>
        /// Handles experience points
        /// </summary>
        void AddExperiencePoints(Player _player1, Player _player2) {
            if (_player1.experience > _player2.experience) currentMatchPlayer1Points += 3;
            else if (_player1.experience < _player2.experience) currentMatchPlayer2Points += 3;
        }

        /// <summary>
        /// Handles surface points
        /// </summary>
        void AddSurfacePoints(Player _player1, Player _player2) {
            if (_player1.GetSurfaceSkill(surface) > _player2.GetSurfaceSkill(surface)) currentMatchPlayer1Points += 4;
            else if (_player1.GetSurfaceSkill(surface) < _player2.GetSurfaceSkill(surface)) currentMatchPlayer2Points += 4;
        }

        /// <summary>
        /// Decides to the winner by the possibility of an random double related to the points
        /// </summary>
        int GetCurrentMatchWinner(Player _player1, Player _player2) {
            Random rand = new Random();

            if (rand.NextDouble() < currentMatchPlayer1Points / (currentMatchPlayer1Points + currentMatchPlayer2Points)) {
                AddWinLoseExperience(_player1, _player2);
                return _player1.id - 1;
            }
            else {
                AddWinLoseExperience(_player2, _player1);
                return _player2.id - 1;
            }
        }

        /// <summary>
        /// Adds match experiences related to tournament type
        /// </summary>
        void AddWinLoseExperience(Player _winnerPlayer, Player _loserPlayer) {
            if(type == TournamentType.ELIMINATION) {
                _winnerPlayer.AddGainedExperience(20);
                _loserPlayer.AddGainedExperience(10);
            }
            else {
                _winnerPlayer.AddGainedExperience(10);
                _loserPlayer.AddGainedExperience(1);
            }
        }

        #endregion

    }
}
