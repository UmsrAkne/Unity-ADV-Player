using System.Collections.Generic;
using System.Linq;
using SceneContents;
using UserInterface;

namespace ScenarioSceneParts
{
    public class TextWriter : IScenarioSceneParts
    {
        private int counter;
        private bool initialExecute = true;

        public bool Writing { get; set; }

        public int ScenarioIndex { get; private set; }

        private IWritable TextField { get; set; }

        public string CurrentText { get; private set; } = string.Empty;

        private Scenario Scenario { get; set; }

        private List<Scenario> Scenarios { get; set; }

        public bool NeedExecuteEveryFrame => true;

        public ExecutionPriority Priority => ExecutionPriority.High;

        public void Execute()
        {
            counter = 0;
            if (initialExecute)
            {
                initialExecute = false;
                Scenario = Scenarios.First();
                Writing = true;
                WriteText(string.Empty);
                return;
            }

            if (ScenarioIndex == Scenarios.Count)
            {
                return;
            }

            if (Writing)
            {
                ScenarioIndex++;
                WriteText(Scenario.Text);
                Writing = false;
            }
            else
            {
                Scenario = Scenarios[ScenarioIndex];
                Writing = true;
                WriteText(string.Empty);
            }
        }

        public void ExecuteEveryFrame()
        {
            if (!Writing)
            {
                return;
            }

            AppendText(Scenario.Text[counter]);
            counter++;

            if (Scenario.Text.Length > counter)
            {
                return;
            }

            Writing = false;
            counter = 0;
            ScenarioIndex++;
        }

        public void SetScenario(Scenario scenario)
        {
            Scenario = scenario;
        }

        public void SetResource(Resource resource)
        {
            Scenarios = resource.Scenarios;
        }

        public void SetText(IWritable writable)
        {
            TextField ??= writable;
        }

        public void SetUI(IWritable ui)
        {
            TextField = ui;
        }

        /// <summary>
        /// シナリオのインデックスを指定の番号にセットし、文字列の描画処理を停止します。
        /// このメソッド単体では、次のテキストが表示されないので、直後に Execute() を呼び出してください。
        /// </summary>
        /// <param name="index"></param>
        public void SetScenarioIndex(int index)
        {
            if (Scenario != null)
            {
                counter = Scenario.Text.Length;
                WriteText(string.Empty);
            }

            if (initialExecute)
            {
                initialExecute = false;
                Scenario = Scenarios[index];
            }

            ScenarioIndex = index;
            Writing = false;
        }

        private void AppendText(char character)
        {
            if (TextField != null)
            {
                TextField.Text += character;
            }

            CurrentText += character;
        }

        private void WriteText(string str)
        {
            if (TextField != null)
            {
                TextField.Text = str;
            }

            CurrentText = str;
        }
    }
}