using System.Collections.Generic;

namespace SceneContents
{
    public class ImageOrder
    {
        public ImageOrder()
        {
            
        }
        
        /// <summary>
        /// BlinkOrder のデータを使って ImageOrder を生成します。
        /// </summary>
        /// <param name="blinkOrder">データの元となる BlinkOrder を指定</param>
        /// <param name="imageIndex">このオブジェクトの Names[1] に入力される blinkOrder.Names のインデックス</param>
        public ImageOrder(BlinkOrder blinkOrder, int imageIndex)
        {
            Names = new List<string>
            {
                string.Empty, blinkOrder.Names[imageIndex], string.Empty, string.Empty,
            };

            Depth = 0.4;
            IsExpressionOrder = true;
            IsDrawOrder = true;
        }

        public List<string> Names { get; private set; } = new();

        public int TargetLayerIndex { get; set; }

        public double Scale { get; set; } = 1.0;

        public int X { get; set; }

        public int Y { get; set; }

        public int Angle { get; set; }

        public bool IsDrawOrder { get; set; }

        public double Depth { get; set; } = 0.1;
        
        public int Delay { get; set; }

        public string MaskImageName { get; set; } = string.Empty;

        public bool InheritStatus { get; set; }

        public bool IsExpressionOrder { get; set; }
    }
}