namespace Assets.Scripts.Utility
{
    public static class Helper
    {
        public static float CalculateAdditionalSpeedByRemainingTime(int maxTime, int remainingTime, int divider)
        {
            return (float)(maxTime - remainingTime) / (float)(divider);
        }
    }
}
