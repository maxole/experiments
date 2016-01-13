using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace OCR.Learn
{
    public interface ILearner
    {        
        /// <summary>
        /// выполнить обучение
        /// </summary>
        /// <returns></returns>
        ILearnedImage Learn(IExportedImage content, IAlphabet alphabet, LearnerOptions options);
    }

    public class Learner : ILearner
    {
        private readonly List<string> _scripts = new List<string>
        {
            "displayData.m",
            "fmincg.m",
            "lrCostFunction.m",
            "oneVsAll.m",
            "predictOneVsAll.m",
            "sigmoid.m"
        };

        public ILearnedImage Learn(IExportedImage content, IAlphabet alphabet, LearnerOptions options)
        {
            //save current dir
            var dir = Environment.CurrentDirectory;
            var tempPath = Path.Combine(Path.GetTempPath(), "octave");
            if(Directory.Exists(tempPath))
                Directory.Delete(tempPath, true);
            Directory.CreateDirectory(tempPath);

            Environment.CurrentDirectory = tempPath;
            try
            {
                // файл с данными
                var data = Path.GetRandomFileName();
                File.WriteAllText(data, content.ExportData);
                // выходной файл
                var outFile = Path.GetRandomFileName();

                // главный скрипт
                var mainContent = GetScriptContent("main.m")
                    .Replace("%0%", alphabet.Labels.ToString(CultureInfo.InvariantCulture))
                    .Replace("%1%", data)
                    .Replace("%2%", outFile);

                var mainScript = Path.GetRandomFileName();
                File.WriteAllText(mainScript, mainContent);

                CopyScriptsToLocalFolder();

                var process = new Process
                {
                    StartInfo =
                    {
                        FileName = options.ExecutePath,
                        Arguments = mainScript,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = false
                    }
                };

                process.Start();

                Debug.WriteLine(process.StandardOutput.ReadToEnd());

                process.WaitForExit();

                var d = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, outFile));                
                return new LearnedImage(content, d);
            }
            finally
            {
                // restore current dir
                Environment.CurrentDirectory = dir;
                Directory.Delete(tempPath, true);                
            }
        }

        private void CopyScriptsToLocalFolder()
        {
            foreach (var script in _scripts)
            {
                var content = GetScriptContent(script);
                File.WriteAllText(Environment.CurrentDirectory + "\\" + script, content);
            }
        }

        private string GetScriptContent(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "OCR.scripts." + name;
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
                return reader.ReadToEnd();
        }
    }
}