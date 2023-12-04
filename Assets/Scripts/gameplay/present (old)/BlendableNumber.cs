using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

namespace gameplay.present
{
    public class BlendableNumber
    {
        public float value;
    }
    
    public static class BlendNumber_DOTween
    {
        public static TweenerCore<float, float, FloatOptions> DOBlend(this BlendableNumber target, int endValue, float duration)
        {
            TweenerCore<float, float, FloatOptions> t = DOTween.To(() => target.value, x => target.value = x, endValue, duration);
            t.SetTarget(target);
            return t;
        }
    }
}