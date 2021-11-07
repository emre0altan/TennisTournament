using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TennisTournament {
    struct FinalResults {
        public List<PlayerResult> results;
    }

    class TournamentSimulation {

        /// <summary>
        /// Reads a json file and returns it as a JOBject
        /// </summary>
        static JObject ReadJsonFile(string _path) {
            using (StreamReader file = File.OpenText(_path))
            using (JsonTextReader reader = new JsonTextReader(file)) {
                return (JObject)JToken.ReadFrom(reader);
            }
        }

        /// <summary>
        /// Deserializing the players from json
        /// </summary>
        static void GetJsonPlayers(JToken _jTokens, List<Player> _players) {
            foreach (JToken jToken in _jTokens) {
                _players.Add(JsonConvert.DeserializeObject<Player>(jToken.ToString(Formatting.None)));
            }
        }

        /// <summary>
        /// Deserializing the tournaments from json
        /// </summary>
        static void GetJsonTournaments(JToken _jTokens, List<Tournament> _tournaments) {
            foreach (JToken jToken in _jTokens) {
                if(jToken["type"].Value<string>() == "league") {
                    _tournaments.Add(JsonConvert.DeserializeObject<LeagueTournament>(jToken.ToString(Formatting.None)));
                }
                else {
                    _tournaments.Add(JsonConvert.DeserializeObject<EliminationTournament>(jToken.ToString(Formatting.None)));
                }
            }
        }

        /// <summary>
        /// Sorts players related to gained experience, if it is equal then total experience
        /// </summary>
        static void SortPlayers(List<Player> _players) {
            _players.Sort((p1, p2) => {
                int result = p2.GetGainedExperience().CompareTo(p1.GetGainedExperience());
                if (result == 0)
                    result = p2.experience.CompareTo(p1.experience);
                return result;
            });
        }

        /// <summary>
        /// Sets player results with sort order
        /// </summary>
        static void SetPlayerResults(List<Player> _players) {
            for(int i = 0; i < _players.Count; i++) {
                _players[i].SetResults(i+1);
            }
        }

        /// <summary>
        /// Serializes the FinalResults to the json file 
        /// </summary>
        static void WriteJsonResults(FinalResults _finalResults, string _path) {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;

            using (StreamWriter sw = new StreamWriter(_path))
            using (JsonWriter writer = new JsonTextWriter(sw)) {

                serializer.Serialize(writer, _finalResults);
            }
        }


        static void Main(string[] args) {
            string TournamentInputPath = "C:\\Users\\emre_\\Desktop\\Github\\TennisTournament\\input.json";
            string TournamentOutputPath = "C:\\Users\\emre_\\Desktop\\Github\\TennisTournament\\output.json";

            JObject inputData = ReadJsonFile(TournamentInputPath);

            List<Player> players = new List<Player>();
            GetJsonPlayers(inputData["players"], players);

            List<Tournament> tournaments = new List<Tournament>();
            GetJsonTournaments(inputData["tournaments"], tournaments);

            foreach (Tournament tournament in tournaments)
                tournament.MakeTournament(players);

            SortPlayers(players);
            SetPlayerResults(players);

            FinalResults finalResults = new FinalResults();
            finalResults.results = players.Select(o => o.GetPlayerResult()).ToList();
            WriteJsonResults(finalResults, TournamentOutputPath);
            
        }
    }
}
