// Copyright © 2019 Mavidian Technologies Limited Liability Company. All Rights Reserved.

using Mavidian.DataConveyer.Common;
using Mavidian.DataConveyer.Orchestrators;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace DataConveyer_FilterLargeCsvData
{
   internal class Program
   {
      // Locations of Data Conveyer input and output:
      private const string InputFolder = @"..\..\..\Data\In";
      private const string OutputFolder = @"..\..\..\Data\Out";

      private static string FullOutputLocation;  // full means absolute path

      static void Main()
      {
         var asmName = System.Reflection.Assembly.GetExecutingAssembly().GetName();
         var fullInputLocation = Path.GetFullPath(InputFolder);
         FullOutputLocation = Path.GetFullPath(OutputFolder);
         Console.WriteLine($"{asmName.Name} v{asmName.Version} started execution on {DateTime.Now:MM-dd-yyyy a\\t hh:mm:ss tt}");
         Console.WriteLine($"DataConveyer library used: {ProductInfo.CurrentInfo.ToString()}");
         Console.WriteLine();
         Console.WriteLine("This application filters a large set of provider utiliization data to only retain data for NJ dentists.");
         Console.WriteLine();
         Console.WriteLine($"Input location : {fullInputLocation}");
         Console.WriteLine($"Output location: {FullOutputLocation}");
         Console.WriteLine();

         var dropOffWatcher = new FileSystemWatcher
         {
            Path = InputFolder,
            EnableRaisingEvents = true
         };
         dropOffWatcher.Created += FileDroppedHandler;

         Console.WriteLine($"Waiting for file(s) to be placed at input location...");
         Console.WriteLine("To exit, hit Enter key...");
         Console.WriteLine();

         var _ = StartReadingConsoleKeysAsync();

         ReadConsoleKeyAsync(ConsoleKey.Enter).Wait();
      }


      private static async void FileDroppedHandler(object sender, FileSystemEventArgs e)
      {
         // "Fire & forget" async void method is OK here - it is of FileSystemEventHandler delegate type.
         var fname = e.Name;
         Console.WriteLine($"Detected {fname} file... hit Spacebar when ready to start...'.");

         await ReadConsoleKeyAsync(ConsoleKey.Spacebar);

         Console.WriteLine($"Processing {fname} started...'.");

         var processor = new FileProcessor(e.FullPath, FullOutputLocation);

         var stopWatch = new Stopwatch();
         stopWatch.Start();
         var result = await processor.ProcessFileAsync();
         stopWatch.Stop();

         if (result.CompletionStatus == CompletionStatus.IntakeDepleted)
         {
            Console.WriteLine($"Processing {fname} file completed in {stopWatch.Elapsed.TotalSeconds.ToString("##0.000")}s; {result.ClustersWritten:N0} out of {result.ClustersRead:N0} records posted onto output.");
         }
         else Console.WriteLine($"Oops! Processing resulted in unexpected status of " + result.CompletionStatus.ToString());
      }


      /// <summary>
      /// A list of keys that are awaited for (expected to be hit).
      /// </summary>
      private static ConcurrentDictionary<ConsoleKey,byte> AwaitedConsoleKeys;  //value (byte) is irrelevant
      /// <summary>
      /// Neverending method that removes awaited keys once they are hit.
      /// </summary>
      /// <returns></returns>
      private static async Task StartReadingConsoleKeysAsync()
      {
         AwaitedConsoleKeys = new ConcurrentDictionary<ConsoleKey, byte>();
         while (true)
         {
            if (Console.KeyAvailable) AwaitedConsoleKeys.TryRemove(Console.ReadKey(true).Key, out _);
            await Task.Delay(50);
         }
      }
      /// <summary>
      /// Do not return until a given key is hit.
      /// </summary>
      /// <param name="keyToRead"></param>
      /// <returns></returns>
      private static async Task ReadConsoleKeyAsync(ConsoleKey keyToRead)
      {
         AwaitedConsoleKeys.TryAdd(keyToRead, 0);
         while (AwaitedConsoleKeys.TryGetValue(keyToRead, out _)) await Task.Delay(60);
      }
   }
}
