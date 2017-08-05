using Bitcoin.Curses.Windows.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinRT;

[assembly: ResolutionGroupName(Bitcoin.Curses.Effects.UnderlineEffect.EffectNamespace)]
[assembly: ExportEffect(typeof(UnderlineEffect), nameof(UnderlineEffect))]

namespace Bitcoin.Curses.Windows.Effects
{
    public class UnderlineEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            SetUnderline(true);
        }

        protected override void OnDetached()
        {
            SetUnderline(false);
        }

        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);

            if (args.PropertyName == Label.TextProperty.PropertyName || args.PropertyName == Label.FormattedTextProperty.PropertyName)
            {
                SetUnderline(true);
            }
        }

        private void SetUnderline(bool underlined)
        {
            try
            {
                var textView = (TextBlock)Control;
                if (underlined)
                {
                    var text = textView.Text;

                    //clear original text
                    textView.Text = "";

                    var run = new Run
                    {
                        Text = text
                    };
                    var underline = new Underline();
                    underline.Inlines.Add(run);

                    textView.Inlines.Add(underline);
                }
                else
                {
                    var underlines = textView.Inlines.Where(x => x is Underline);
                    foreach (var underline in underlines)
                    {
                        //restore original text
                        var run = ((Underline)underline).Inlines.FirstOrDefault(x => x is Run);
                        if (run != null)
                        {
                            textView.Text = ((Run)run).Text;
                        }

                        textView.Inlines.Remove(underline);
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine("Cannot underline Label. Error: ", ex.Message);
#endif
            }
        }
    }
}