using System.Globalization;

namespace MyTfsMobile.App.enums
{
    public enum BuildStatus
    {
        Failed,
        Partial,
        Running,
        Ok,
        Cancelled,
        BuildDefinition
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
                case "builddefinition":
                    return BuildStatus.BuildDefinition;
                default:
                    return BuildStatus.Cancelled;
            }
        }
    }
}
