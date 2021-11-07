using System;
using System.Collections.Generic;
using System.Linq;

namespace TennisTournament {

    enum SurfaceType { CLAY, GRASS, HARD}
    enum TournamentType { LEAGUE, ELIMINATION}

    abstract class Tournament {

        public int id { get; }
        public SurfaceType surface { get; set; }
        public TournamentType type { get; set; }


        public Tournament() { }

        public abstract void MakeTournament(List<Player> _players);



        #region OneMatch
        float currentMatchPlayer1Points, currentMatchPlayer2Points;
        public int OneMatch(Player _player1, Player _player2) {
            currentMatchPlayer1Points = currentMatchPlayer2Points = 1;

            AddLeftHandPoints(_player1, _player2);
            AddExperiencePoints(_player1, _player2);
            AddSurfacePoints(_player1, _player2);

            return GetCurrentMatchWinner(_player1, _player2);
        }

        void AddLeftHandPoints(Player _player1, Player _player2) {
            if (_player1.hand == HandType.LEFT) currentMatchPlayer1Points += 2;
            if (_player2.hand == HandType.LEFT) currentMatchPlayer2Points += 2;
        }

        void AddExperiencePoints(Player _player1, Player _player2) {
            if (_player1.experience > _player2.experience) currentMatchPlayer1Points += 3;
            else if (_player1.experience < _player2.experience) currentMatchPlayer2Points += 3;
        }

        void AddSurfacePoints(Player _player1, Player _player2) {
            if (_player1.GetSurfaceSkill(surface) > _player2.GetSurfaceSkill(surface)) currentMatchPlayer1Points += 4;
            else if (_player1.GetSurfaceSkill(surface) < _player2.GetSurfaceSkill(surface)) currentMatchPlayer2Points += 4;
        }

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
