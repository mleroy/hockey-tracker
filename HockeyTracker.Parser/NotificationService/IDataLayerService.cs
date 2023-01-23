using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using HockeyTracker.Data;

namespace NotificationService
{
    [ServiceContract]
    public interface IDataLayerService
    {
        [OperationContract]
        void UpdateGame(int gameNumber, DateTime gameTime, string homeTeam, string visitorTeam, int homeGoals, int visitorGoals, DateTime? started, DateTime? ended);

        [OperationContract]
        List<Game> GetGamesToParse(DateTime gameTimeAfter, DateTime gameTimeBefore);

        [OperationContract]
        Game GetNextGameForTeam(string teamShortName);

        [OperationContract]
        int GetOldestIncompleteGame();
    }
}
