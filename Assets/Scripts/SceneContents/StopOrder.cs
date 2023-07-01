using ScenarioSceneParts;

namespace SceneContents
{
    public class StopOrder
    {
        public StoppableSceneParts Target { get; set; }

        public string Name { get; set; }

        public int Channel { get; set; }

        public int LayerIndex { get; set; }

        public bool IsAnimationStopOrder()
        {
            if (string.IsNullOrWhiteSpace(Name) || Target != StoppableSceneParts.Anime)
            {
                return false;
            }

            return true;

            // return Name.Equals(nameof(AlphaChanger), StringComparison.OrdinalIgnoreCase) ||
            //        Name.Equals(nameof(Shake), StringComparison.OrdinalIgnoreCase) ||
            //        Name.Equals(nameof(Slide), StringComparison.OrdinalIgnoreCase) ||
            //        Name.Equals(nameof(Flash), StringComparison.OrdinalIgnoreCase) ||
            //        Name.Equals(nameof(Bound), StringComparison.OrdinalIgnoreCase) ||
            //        Name.Equals(nameof(AnimationChain), StringComparison.OrdinalIgnoreCase) ||
            //        Name.Equals("chain", StringComparison.OrdinalIgnoreCase) ||
            //        Name.Equals(nameof(ScaleChange), StringComparison.OrdinalIgnoreCase) ||
            //        Name.Equals(nameof(Draw), StringComparison.OrdinalIgnoreCase);
        }
    }
}