using System.Globalization;

namespace MyTfsMobile.App.enums
{
    public enum BuildStatus
    {
        Failed,
        Partial,
        Running,
        Ok,
        Cancelled
    }

    public static class BuildStatusConverter
    {
        public static BuildStatus GetFromString(string buildStatus)
        {
            switch (buildStatus.ToLower(CultureInfo.CurrentCulture))
            {
                case "succeeded":
                    return BuildStatus.Ok;
                case "partiallysucceeded":
                    return BuildStatus.Partial;
                case "failed":
                    return BuildStatus.Failed;
                case "running":
                    return BuildStatus.Running;
                default:
                    return BuildStatus.Cancelled;
            }
        }
    }
}
