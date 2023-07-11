using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Loaders.XmlElementConverters;
using SceneContents;

namespace Animations
{
    public class AnimationChain : IAnimation
    {
        private readonly AnimeElementConverter converter = new AnimeElementConverter();
        private List<IAnimation> animations = new List<IAnimation>();
        private bool canChangeTarget = true;
        private bool initialGenerate;
        private List<IAnimation> playingAnimations = new List<IAnimation>();
        private IDisplayObject target;

        public AnimationChain()
        {
        }

        public AnimationChain(AnimeElementConverter aec)
        {
            converter = aec;
        }

        public List<XElement> AnimeTags { get; private set; } = new List<XElement>();

        public string AnimationName => "AnimationChain";

        public bool IsWorking { get; private set; } = true;

        public IDisplayObject Target
        {
            private get => target;
            set
            {
                if (!canChangeTarget)
                {
                    // セット不可のタイミングでターゲットの変更が行われた場合、
                    // 外部から画像描画の命令が出ているということであるため、このアニメーションを停止する。
                    Stop();
                    return;
                }

                foreach (var a in animations)
                {
                    a.Target = value;
                }

                target = value;
            }
        }

        public ImageContainer TargetContainer { get; set; }

        public int TargetLayerIndex { get; set; }

        public int RepeatCount { get; set; }

        public int Delay { get; set; }

        public int Interval { get; set; }

        public string GroupName { get; set; } = string.Empty;

        public void Execute()
        {
            if (!IsWorking)
            {
                return;
            }

            if (Delay-- > 0)
            {
                return;
            }

            if (animations.Count(a => a.IsWorking) == 0 && RepeatCount >= 0)
            {
                foreach (var tag in AnimeTags)
                {
                    AddAnimation(converter.GenerateAnimation(tag));
                }

                if (!initialGenerate)
                {
                    Delay = Interval;
                }

                RepeatCount--;
                initialGenerate = false;
            }

            // .All は空のリストの場合は true を返す。よってリストが空ならブロックに突入する。
            if (playingAnimations.All(p => !p.IsWorking))
            {
                var first = animations.FirstOrDefault(a => a.IsWorking);
                if (first != null)
                {
                    playingAnimations = !string.IsNullOrWhiteSpace(first.GroupName)
                        ? animations.Where(a => a.GroupName == first.GroupName && a.IsWorking).ToList()
                        : new List<IAnimation>() { first };

                    animations = animations.Where(a => a.IsWorking).ToList();
                }
                else
                {
                    playingAnimations = new List<IAnimation>();
                }
            }

            if (playingAnimations.Count == 0)
            {
                Stop();
                return;
            }

            if (playingAnimations.Any(a => a is Draw or Image))
            {
                // 実行アニメーションが Draw の場合は、ターゲット画像の変更が発生する可能性があるため、セッターを許可する。
                canChangeTarget = true;
            }

            playingAnimations.ForEach(p => p.Execute());

            // セッターを許可するのは、Execute() 実行中のみ
            canChangeTarget = false;
        }

        public void Start()
        {
        }

        public void Stop()
        {
            IsWorking = false;
            foreach (var a in animations.Where(a => a.IsWorking))
            {
                a.Stop();
            }
        }

        public void AddAnimation(IAnimation anime)
        {
            if (anime is AnimationChain)
            {
                throw new ArgumentException("AnimationChain に AnimationChain は含められません");
            }

            animations.Add(anime);

            if (Target != null)
            {
                anime.Target = Target;
            }
        }

        public void AddAnimationTag(XElement tag)
        {
            AnimeTags.Add(tag);
        }
    }
}