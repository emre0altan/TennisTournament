namespace TennisTournament {

    enum HandType { LEFT, RIGHT}

    struct Skills {
        public int clay;
        public int grass;
        public int hard;
    }

    struct PlayerResult {
        public int order;
        public int player_id;
        public int gained_experience;
        public int total_experience;
    }

    class Player {

        public int id  { get; set; }
        public int experience  { get; set; }
        public HandType hand { get; set; }
        public Skills skills;

        int gainedExperience;

        PlayerResult playerResult;

        Player() { }

        /// <summary>
        /// Returns skill amount of the surface
        /// </summary>
        public int GetSurfaceSkill(SurfaceType _surface) {
            if (_surface == SurfaceType.CLAY) return skills.clay;
            else if (_surface == SurfaceType.GRASS) return skills.grass;
            else return skills.hard;
        }

        /// <summary>
        /// Returns gained experience during tournaments
        /// </summary>
        public int GetGainedExperience() {
            return gainedExperience;
        }

        /// <summary>
        /// Returns result of the player after the all tournaments
        /// </summary>
        public PlayerResult GetPlayerResult() {
            return playerResult;
        }

        /// <summary>
        /// Adds new experience
        /// </summary>
        public void AddGainedExperience(int _amount) {
            experience += _amount;
            gainedExperience += _amount;
        }

        /// <summary>
        /// Sets values of the result
        /// </summary>
        public void SetResults(int _order) {
            playerResult.order = _order;
            playerResult.player_id = id;
            playerResult.gained_experience = gainedExperience;
            playerResult.total_experience = experience;
        }
    }
}
