using System;
using System.Collections.Generic;
using System.Linq;

namespace IntervalProblem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            do
            {
                var intervalDataDetails = GetIntervalDataFromUser();
                GetOutputDataForGivenInterval(intervalDataDetails);

                Console.WriteLine("\nPlease enter any key to continue or press enter to close ");
            }
            while (!Console.ReadKey().Key.ToString().Equals("Enter"));
        }

        // Method to get input data from user
        private static List<IntervalDataDetails> GetIntervalDataFromUser()
        {
            Console.WriteLine("\nPlease enter number of intervals: ");

            var executionCount = Convert.ToInt32(Console.ReadLine().TrimEnd());
            var intervalsDetailsList = new List<IntervalDataDetails>();

            for (int i = 0; i < executionCount; i++)
            {
                Console.WriteLine("Please enter the input intervals (From,To) and data as , comma-seperated: ");

                var receivedInputData = new List<string>(Console.ReadLine().Split(","));
                try
                {
                    var IntervalDataDetails = new IntervalDataDetails
                    {
                        FromInterval = Convert.ToInt32(receivedInputData[0]),
                        ToInterval = Convert.ToInt32(receivedInputData[1]),
                        Data = receivedInputData.Skip(2).ToList()
                    };
                    intervalsDetailsList.Add(IntervalDataDetails);
                }
                catch
                {
                    Console.WriteLine("Please re-enter the input in correct format");
                    i--;
                }
            }
            return intervalsDetailsList.ToList();
        }

        //Gets the required output intervals from user for which data need to be dsiplayed
        private static void GetOutputDataForGivenInterval(List<IntervalDataDetails> intervalDataDetails)
        {
            Console.WriteLine("\nPlease enter the intervals (From,To) for which data need to be displayed :");

            var requiredInterval = new List<string>(Console.ReadLine().Split(','));
            var fromInterval = Convert.ToInt32(requiredInterval[0]);
            var toInterval = Convert.ToInt32(requiredInterval[1]);

            var filteredResult = GetFilteredOutputData(intervalDataDetails, fromInterval, toInterval);

            Console.WriteLine("\nOutput data for the given intervals are: " + string.Join(", ", filteredResult).ToUpper());
        }

        //Applying from, to interval as filter and selecting applicable output data
        private static List<string> GetFilteredOutputData(List<IntervalDataDetails> intervalDataDetails, int fromInterval, int toInterval)
        {
            var intervalData = MapIntervalWithData(intervalDataDetails);

            List<IntervalData> filteredData = intervalData.Where(x => x.Interval >= fromInterval && x.Interval <= toInterval)
                                    .Select(x => new IntervalData()
                                    {
                                        Interval = x.Interval,
                                        Data = x.Data,
                                    }).OrderBy(x => x.Data).Distinct().ToList();

            List<string> outputData = new List<string>();

            foreach (var item in filteredData)
            {
                if (!outputData.Contains(item.Data))
                    outputData.Add(item.Data);
            }
            return outputData;
        }

        // Mapping data with applicable interval
        private static List<IntervalData> MapIntervalWithData(List<IntervalDataDetails> intervalsDetailsList)
        {
            var intervalDataList = new List<IntervalData>();

            foreach (var intervalsDetails in intervalsDetailsList)
            {
                for (int i = intervalsDetails.FromInterval; i <= intervalsDetails.ToInterval; i++)
                {
                    intervalDataList.AddRange(from item in intervalsDetails.Data
                                              let intervalData = new IntervalData { Interval = i, Data = item }
                                              select intervalData);
                }
            }
            return intervalDataList.ToList();
        }
    }
}
