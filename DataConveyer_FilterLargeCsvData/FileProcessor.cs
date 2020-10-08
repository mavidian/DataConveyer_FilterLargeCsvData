// Copyright © 2019-2020 Mavidian Technologies Limited Liability Company. All Rights Reserved.

using Mavidian.DataConveyer.Common;
using Mavidian.DataConveyer.Orchestrators;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DataConveyer_FilterLargeCsvData
{
   /// <summary>
   /// Represents Data Conveyer functionality specific to translating CSV airplane records
   /// into a flat (fixed-width filed) format.
   /// </summary>
   internal class FileProcessor
   {
      private readonly IOrchestrator Orchestrator;

      internal FileProcessor(string inFile, string outLocation)
      {
         var config = new OrchestratorConfig()
         {
            ReportProgress = true,
            ProgressInterval = 1000,
            ProgressChangedHandler = (s, e) => { if (e.Phase == Phase.Intake) Console.Write($"\rProcessed {e.RecCnt:N0} records so far..."); },
            PhaseFinishedHandler = (s, e) => { if (e.Phase == Phase.Intake) Console.WriteLine($"\rProcessed {e.RecCnt:N0} records. Done!   "); },
            InputDataKind = KindOfTextData.Delimited,
            HeadersInFirstInputRow = true,
            InputFileName = inFile,
            TransformerType = TransformerType.RecordFilter,
            RecordFilterPredicate = r => (string)r["NPPES Provider State"] == "NJ" && ((string)r["Specialty Description"]).ToLower() == "dentist",
            OutputDataKind = KindOfTextData.Delimited,
            HeadersInFirstOutputRow = true,
            OutputFileName = outLocation + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(inFile) + "_NJ_dentists.csv"
         };

         Orchestrator = OrchestratorCreator.GetEtlOrchestrator(config);
      }

      /// <summary>
      /// Execute Data Conveyer process.
      /// </summary>
      /// <returns>Task containing the process results.</returns>
      internal async Task<ProcessResult> ProcessFileAsync()
      {
         var result = await Orchestrator.ExecuteAsync();
         Orchestrator.Dispose();

         return result;
      }

   }
}
