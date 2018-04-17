using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Effects;
using BITS.UI.WPF.Core;

namespace Core.UI
{
    public class DialogBehaviour
    {
        private static ConditionalWeakTable<Window, IList<IDisposable>> _Subscriptions = new ConditionalWeakTable<Window, IList<IDisposable>>();

        private static List<WeakReference<FrameworkElement>> _Elements =
            new List<WeakReference<FrameworkElement>>();

        public static readonly DependencyProperty LayoutTypeProperty
            = DependencyProperty.RegisterAttached("LayoutType",
                typeof(LayoutType), typeof(DialogBehaviour),
                new FrameworkPropertyMetadata(LayoutType.None, DialogBehaviour.OnLayoutTypeChanged));

        public static LayoutType GetLayoutType(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            return (LayoutType)element.GetValue(LayoutTypeProperty);
        }

        public static void SetLayoutType(DependencyObject element, LayoutType value)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            element.SetValue(LayoutTypeProperty, value);
        }


        private static void OnLayoutTypeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var element = (FrameworkElement)obj;

            _Elements.Add(new WeakReference<FrameworkElement>(element));

            InitDialog(element);
        }

        private static void InitDialog(DependencyObject dpo)
        {
            var root = dpo.GetRootVisual();
            var window = root as Window;
            IList<IDisposable> _;

            if (window != null && !_Subscriptions.TryGetValue(window, out _))
            {
                _Subscriptions.Add(window, new List<IDisposable>());

                window.ContentRendered += (s, e) =>
                {
                    FrameworkElement layoutRoot = null;
                    FrameworkElement dialogRoot = null;
                    FrameworkElement dialogPanel = null;
                    ContentPresenter dialogContent = null;

                    foreach (var element in _Elements)
                    {
                        FrameworkElement fe;

                        if (!element.TryGetTarget(out fe))
                            continue;

                        switch (GetLayoutType(fe))
                        {
                            case LayoutType.LayoutRoot:
                                layoutRoot = fe;
                                break;

                            case LayoutType.DialogRoot:
                                dialogRoot = fe;
                                break;

                            case LayoutType.DialogPanel:
                                dialogPanel = fe;
                                break;

                            case LayoutType.DialogContent:
                                dialogContent = fe as ContentPresenter;
                                break;
                        }
                    }

                    Init(window, layoutRoot, dialogRoot, dialogPanel, dialogContent);
                };

                window.Closed += (s, e) => _Subscriptions.Remove(window);
            }
        }

        public static void Init(Window window, UIElement layoutRoot, FrameworkElement dialogRoot,
            FrameworkElement dialogPanel, ContentPresenter dialogContent)
        {
            InitLayoutRoot(layoutRoot, dialogRoot, dialogContent);
            InitDialogPanel(window, dialogPanel);
        }

        private static void InitLayoutRoot(UIElement layoutRoot, FrameworkElement dialogRoot, ContentPresenter dialogContent)
        {
            var blurEffect = new BlurEffect();

            layoutRoot.Effect = blurEffect;

            BindingOperations.SetBinding(blurEffect, BlurEffect.RadiusProperty,
                new Binding()
                {
                    Source = dialogContent,
                    Path = new PropertyPath(nameof(ContentPresenter.Content)),
                    Mode = BindingMode.OneWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,

                    Converter = new Converters.Converter<object, Visibility>()
                        .Init((value, parameter) => value == null ? 0 : Double.Parse(parameter?.ToString() ?? "4")),

                    ConverterParameter = 3.0
                });

            BindingOperations.SetBinding(dialogContent, UIElement.VisibilityProperty,
                new Binding()
                {
                    Source = dialogContent,
                    Path = new PropertyPath(nameof(dialogContent.Content)),
                    Mode = BindingMode.OneWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,

                    Converter = new Converters.Converter<object, Visibility>()
                        .Init(value => value == null ? Visibility.Hidden : Visibility.Visible)
                });

            // DialogRoot.Visibility <==> DialogContent.Visibility
            BindingOperations.SetBinding(dialogRoot, UIElement.VisibilityProperty, new Binding()
            {
                Source = dialogContent,
                Path = new PropertyPath(nameof(dialogContent.Visibility)),
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
        }

        private static void InitDialogPanel(Window window, FrameworkElement dialogPanel)
        {
            var top = Math.Max(window.Height / 2 - 100, 0);

            dialogPanel.Margin = new Thickness(0, top, 0, top);
            IList<IDisposable> subscriptions;

            _Subscriptions.TryGetValue(window, out subscriptions);

            subscriptions.Add(window.PropertyChanged(Window.HeightProperty, args =>
            {
                var value = args.NewValue;

                if (window.WindowState == WindowState.Maximized)
                    value = args.OldValue;

                var height = 100;

                if (value is double)
                {
                    var y = (double)value;
                    top = Math.Max(y / 2 - height, 0);

                    dialogPanel.Margin = new Thickness(0, top, 0, top);

                    return;
                }

                dialogPanel.Margin = new Thickness(0, 10, 0, 10);
            }));

            subscriptions.Add(window.PropertyChanged(Window.WindowStateProperty, args =>
            {
                if (args.NewValue is WindowState && (WindowState)args.NewValue == WindowState.Maximized)
                    dialogPanel.Margin = new Thickness(0, 412, 0, 412);
                else
                {
                    top = Math.Max(window.Height / 2 - 100, 0);

                    dialogPanel.Margin = new Thickness(0, top, 0, top);
                }
            }));

        }
    }
}
