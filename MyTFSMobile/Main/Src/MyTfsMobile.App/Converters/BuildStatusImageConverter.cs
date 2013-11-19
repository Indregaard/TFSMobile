using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using MyTfsMobile.App.enums;

namespace MyTfsMobile.App.Converters
{
    public sealed class BuildStatusImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BuildStatus)
            {
                var convertFrom = (BuildStatus)(((int)value));
                if (convertFrom == BuildStatus.Failed)
                    return "../Images/failed.png";
                if (convertFrom == BuildStatus.Partial)
                    return "../Images/partial.png";
                if (convertFrom == BuildStatus.Ok)
                    return "../Images/ok.png";
                if (convertFrom == BuildStatus.Running)
                    return "../Images/running.png";
                if (convertFrom == BuildStatus.Cancelled)
                    return "../Images/cancel.png";
                if (convertFrom == BuildStatus.BuildDefinition)
                    return "../Images/build_def.png";
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
