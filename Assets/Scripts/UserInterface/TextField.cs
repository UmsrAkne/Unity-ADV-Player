using UnityEngine.UI;

namespace UserInterface
{
    public class TextField : IWritable
    {
        public string Text
        {
            get => Field.text;
            set => Field.text = value;
        }

        public Text Field { get; set; }
    }
}