
namespace FileWatcherService
{
    public class Options
    {
        internal string Target { get; set; }

        internal string Source { get; set; }

        internal string Archive { get; set; }

        public Options()
        {
            Target = @"G:\SourceDirectory";

            Source = @"G:\TargetDirectory";

            Archive = @"G:\TargetDirectory\Archive";
        }

    }
}
