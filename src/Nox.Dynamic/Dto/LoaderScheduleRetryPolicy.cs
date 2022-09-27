﻿namespace Nox.Dynamic.Dto
{
    internal class LoaderScheduleRetryPolicy
    {
        public int Limit { get; set; } = 5;
        public int DelaySeconds { get; set; } = 60;
        public int DoubleDelayLimit { get; set; } = 10;
    }
}