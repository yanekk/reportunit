namespace ReportUnit.Utils
{
    public static class DoubleExtensions
    {
        public static decimal AsPercentageOf(this double partNumber, double fullNumber)
        {
            return decimal.Round((decimal)(partNumber/fullNumber * 100), 1);
        }
    }
}
