using System;
using System.Collections.Generic;
using System.Linq;
using ScenarioSceneParts;
using SceneContents;

namespace Animations
{
    public class AnimationsManager : IScenarioSceneParts
    {
        private Scenario scn;

        public AnimationsManager(ImageContainer imageContainer)
        {
            TargetImageContainer = imageContainer;
            TargetImageContainer.Added += ImageAddedEventHandler;
        }

        public ImageContainer TargetImageContainer { get; }

        private List<IAnimation> Animations { get; set; } = new List<IAnimation>();

        public bool NeedExecuteEveryFrame => true;

        public ExecutionPriority Priority => ExecutionPriority.Low;

        /// <summary>
        /// Animations に追加されるまで待機している Animation です。
        /// Animations を for each で回している間は、Animations に要素を追加することができないため、一時的に退避させるためのプロパティです。
        /// </summary>
        private List<IAnimation> WaitingAnimations { get; } = new List<IAnimation>();

        public void Execute()
        {
        }

        public void ExecuteEveryFrame()
        {
            bool deleteFlag = false;

            foreach (var a in Animations)
            {
                a.Execute();

                if (!a.IsWorking)
                {
                    deleteFlag = true;
                }
            }

            if (deleteFlag)
            {
                Animations = new List<IAnimation>(Animations.Where(anime => anime.IsWorking));
            }

            while (WaitingAnimations.Count > 0)
            {
                Animations.Add(WaitingAnimations.FirstOrDefault());
                WaitingAnimations.RemoveAt(0);
            }
        }

        public void SetResource(Resource resource)
        {
        }

        public void Reload(Resource resource)
        {
            foreach (var a in Animations)
            {
                a.Stop();
            }

            scn = null;
        }

        public void SetScenario(Scenario scenario)
        {
            scn = scenario;

            if (scenario.Animations.Count == 0 && scenario.StopOrders.All(s => !s.IsAnimationStopOrder()))
            {
                return;
            }

            foreach (var s in scenario.StopOrders.Where(so => so.IsAnimationStopOrder()))
            {
                foreach (var a in Animations)
                {
                    if (a.AnimationName.Equals(s.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        a.Stop();
                    }
                }
            }

            foreach (var anime in scenario.Animations)
            {
                if (anime.TargetLayerIndex == TargetImageContainer.Index)
                {
                    AddAnimation(anime);
                }
            }
        }

        private void ImageAddedEventHandler(object sender, ImageAddedEventArgs e)
        {
            if (!scn.Animations.Any(a =>
                    a.AnimationName == nameof(AlphaChanger) && a.TargetLayerIndex == TargetImageContainer.Index))
            {
                var alphaChanger = new AlphaChanger()
                {
                    Amount = e.CurrentOrder.Depth,
                    Delay = e.CurrentOrder.Delay,
                    Duration = e.CurrentOrder.Duration,
                    Target = TargetImageContainer.FrontChild,
                };

                WaitingAnimations.Add(alphaChanger);
            }

            foreach (var a in Animations)
            {
                a.Target = TargetImageContainer.FrontChild;
            }
        }

        /// <summary>
        /// 指定したアニメーションを Animations に追加します。
        /// また、内部に追加するアニメーションと同じものがあった場合、既にある側の Stop() を呼び出します。
        /// </summary>
        private void AddAnimation(IAnimation anime)
        {
            foreach (var a in Animations)
            {
                if (a.AnimationName == anime.AnimationName)
                {
                    a.Stop();
                }
            }

            anime.Target = TargetImageContainer.FrontChild;
            anime.Start();
            Animations.Add(anime);
        }
    }
}