using System;
using System.Linq;
using System.ServiceModel.Activation;
using HockeyTracker.Data;
using System.Collections.Generic;

namespace NotificationService
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class DataLayerService : IDataLayerService
    {
        public void UpdateGame(int gameNumber, DateTime gameTime, string homeTeam, string visitorTeam, int homeGoals, int visitorGoals, DateTime? started, DateTime? ended)
        {
            using (DataLayerDataContext context = new DataLayerDataContext())
            {
                Game game = context.Games.SingleOrDefault(g => g.GameNumber == gameNumber);

                if (game == null)
                {
                    game = new Game
                    {
                        GameNumber = gameNumber
                    };

                    context.Games.InsertOnSubmit(game);
                }

                game.GameTime = gameTime;
                game.HomeTeam = homeTeam;
                game.VisitorTeam = visitorTeam;
                game.HomeGoals = homeGoals;
                game.VisitorGoals = visitorGoals;
                game.Started = started;
                game.Ended = ended;

                context.SubmitChanges();
            }
        }

        public List<Game> GetGamesToParse(DateTime gameTimeAfter, DateTime gameTimeBefore)
        {
            using (DataLayerDataContext context = new DataLayerDataContext())
            {
                var games =
                    from game in context.Games
                    where game.GameTime >= gameTimeAfter
                        && game.GameTime <= gameTimeBefore
                    orderby game.GameNumber ascending
                    select game;

                return games.ToList();
            }
        }

        public Game GetNextGameForTeam(string teamShortName)
        {
            using (DataLayerDataContext context = new DataLayerDataContext())
            {
                var games =
                    from game in context.Games
                    where (game.HomeTeam == teamShortName || game.VisitorTeam == teamShortName)
                        && game.GameTime >= DateTime.UtcNow
                    orderby game.GameNumber ascending
                    select game;

                Game nextGame = games.FirstOrDefault();

                return nextGame;
            }
        }

        public int GetOldestIncompleteGame()
        {
            using (DataLayerDataContext context = new DataLayerDataContext())
            {
                Game game = context.Games.Where(g => g.Ended == null).OrderBy(g => g.GameNumber).FirstOrDefault();

                if (game == null)
                {
                    game = context.Games.Last();
                    return game.GameNumber + 1;
                }
                {
                    return game.GameNumber;
                }
            }
        }
    }
}
