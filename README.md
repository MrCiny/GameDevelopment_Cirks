# GameDevelopment_Cirks
Unity 2.5D Circus game project, demonstrates how to use UI elements, animate then. Basics of C# scripting in Unity 2022.3.11f1

## Game explanation and rules
- There are 3 players, it's you against other two players to reach tile 100!
- Each round, a player can throw their dice and the landed number is the spaces the player moves
- If you land on a ladder, you move up to the tile that is the ladder end, but if you land on a snake, you move down to the end of the snake
- If there are two players in the same tile, they fight each other, the winner is decided by a 50/50 chance, and the loser moves 3 spaces back
- The winning tile is 100, so if you land on the tile and you win, and your total points are among the best total points, you are shown on the leaderboard
- Points are calculated based on efficieny, which means, faster finish gets you more points

## Setup 
1. Clone the repository
2. Open it in Unity 2022.3.11f1 or later
3. Run the game (Either through editor by playing or downloading the game from Github releases)
 
## To-do list:
- [x] Create script to change cursor
- [x] Add and animate UI elements
- [x] Add background music and sounds
- [x] Add animated characters and prefabs
- [x] Create character selection screen
- [x] Learn about player prefs and saving in json
- [x] Write script for dice rolling
- [x] Implement snakes and ladders board logic (spaces)
- [x] Add combat logic
- [x] Add victory space
- [x] Add victory screen
- [x] Add point system A.K.A. Leaderboard
- [x] Add settings system
- [x] Add pause menu
- [x] Add camera zoom
- [ ] Automated moves
