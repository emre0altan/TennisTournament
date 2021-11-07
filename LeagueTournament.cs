using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisTournament {

    struct Pair {
        public Player player1, player2;

        public Pair(Player player1, Player player2) {
            this.player1 = player1;
            this.player2 = player2;
        }
    }

    class LeagueTournament : Tournament {

        private static Random rand = new Random();
        List<Pair> pairs;

        public override void MakeTournament(List<Player> _players) {
            pairs = new List<Pair>();
            for (int i = 0; i < _players.Count; i++) {
                for (int j = i; j < _players.Count; j++) {
                    pairs.Add(new Pair(_players[i], _players[j]));
                }
            }

            pairs = pairs.OrderBy(a => rand.Next()).ToList();

            for (int i = 0; i < pairs.Count; i++) {
                OneMatch(pairs[i].player1, pairs[i].player2);
            }
        }
    }
}
