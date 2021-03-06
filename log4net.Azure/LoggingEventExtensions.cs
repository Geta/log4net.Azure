using System;
using log4net.Core;

namespace log4net.Appender
{
    internal static class LoggingEventExtensions
    {
        internal static string MakeRowKey(this LoggingEvent loggingEvent)
        {
            return
                $"{DateTime.MaxValue.Ticks - loggingEvent.TimeStamp.Ticks:D19}.{Guid.NewGuid().ToString().ToLower()}";
        }

        internal static string MakePartitionKey(this LoggingEvent loggingEvent, PartitionKeyTypeEnum partitionKeyType)
        {
            switch (partitionKeyType)
            {
                case PartitionKeyTypeEnum.LoggerName:
                    return loggingEvent.LoggerName;
                case PartitionKeyTypeEnum.DateReverse:
                    // substract from DateMaxValue the Tick Count of the current hour
                    // so a Table Storage Partition spans an hour
                    return
                        $"{(DateTime.MaxValue.Ticks - loggingEvent.TimeStamp.Date.AddHours(loggingEvent.TimeStamp.Hour).Ticks + 1):D19}";
                default:
		            // ReSharper disable once NotResolvedInText
                    throw new ArgumentOutOfRangeException("PartitionKeyType", partitionKeyType, null);
            }
        }
    }
}