using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Bitcoin.Curses.Effects
{
    public class UnderlineEffect : RoutingEffect
    {
        public const string EffectNamespace = "Effects";

        public UnderlineEffect()
            : base($"{EffectNamespace}.{nameof(UnderlineEffect)}")
        {
        }
    }
}