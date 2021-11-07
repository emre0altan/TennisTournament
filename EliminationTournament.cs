using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisTournament {
    class EliminationTournament : Tournament {

        public override void MakeTournament(List<Player> _players) {
            int[] eliminations = new int[_players.Count];
            for (int i = 0; i < eliminations.Length; i++) eliminations[i] = _players[i].id - 1;

            int totalLayerCount = (int)MathF.Log2(_players.Count);
            int currentLayer = 0;
            MakeEliminations(_players, eliminations, totalLayerCount, currentLayer);
        }

        void MakeEliminations(List<Player> _players, int[] _eliminations, int _totalLayerCount, int _currentLayer) {
            for (int i = 0, j = 0; i < _eliminations.Length / MathF.Pow(2, (_currentLayer + 1)); i++, j += 2) {
                _eliminations[i] = OneMatch(_players[_eliminations[j]], _players[_eliminations[j + 1]]);
            }
            _currentLayer++;
            if (_currentLayer == _totalLayerCount) return;
            else MakeEliminations(_players, _eliminations, _totalLayerCount, _currentLayer);
        }
    }
}
