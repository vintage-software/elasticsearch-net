﻿using System;
using System.Diagnostics;
using System.IO;

namespace DocGenerator
{
	public static class Program
	{
		static Program()
		{
			var currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
            if (currentDirectory.Name == "DocGenerator" && currentDirectory.Parent.Name == "CodeGeneration")
			{
                InputDirPath = @"..\..\";
				OutputDirPath = @"..\..\..\docs";
                BuildOutputPath = @"..\..\..\build\output";
			}
			else
			{
				InputDirPath = @"..\..\..\..\..\src";
				OutputDirPath = @"..\..\..\..\..\docs";
                BuildOutputPath = @"..\..\..\..\..\build\output";
			}

			var process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					UseShellExecute = false,
					RedirectStandardOutput = true,
					FileName = "git.exe",
					CreateNoWindow = true,
					WorkingDirectory = Environment.CurrentDirectory,
					Arguments = "rev-parse --abbrev-ref HEAD"
				}
			};

			try
			{
				process.Start();
				BranchName = process.StandardOutput.ReadToEnd().Trim();
				Console.WriteLine($"Using branch name {BranchName} in documentation");
				process.WaitForExit();
			}
			catch (Exception)
			{
				BranchName = "master";
				Console.WriteLine($"Could not get the git branch name. Assuming {BranchName}");
			}
			finally
			{
				process.Dispose();
			}
        }

        public static string BuildOutputPath { get; }

		public static string InputDirPath { get; }

		public static string OutputDirPath { get; }

		public static string BranchName { get; }

		public static string DocVersion => "6.1";

		static int Main(string[] args)
		{
		    try
		    {
                LitUp.GoAsync(args).Wait();
			    return 0;
		    }
		    catch (AggregateException ae)
		    {
			    var ex = ae.InnerException ?? ae;
                Console.WriteLine(ex.Message);
			    return 1;
		    }
		}
	}
}


