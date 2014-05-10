
namespace TeamDashboard
{
    public static class ColorHelper
    {
        public static string StatusToStringColor(string status)
        {
            switch (status)
            {
                case "To do":
                    return "#FF515151";
                case "Running":
                    return "#FF4EA2CF";
                case "Done":
                    return "#FF8FCD3E";
            }

            return "#FFFFFFFF";
        }
    }
}
