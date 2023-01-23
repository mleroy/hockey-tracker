
namespace HockeyTracker
{
    public class LocalizedStrings
    {
        public LocalizedStrings()
        { 
        }

        private static AppResources localizedResources = new AppResources();

        public AppResources Str { get { return localizedResources; } }
    }
}
