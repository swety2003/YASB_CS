using MaterialColorUtilities.Schemes;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;
using Color = System.Windows.Media.Color;

namespace MaterialDesign3.Styles.Colors
{
    [Localizability(LocalizationCategory.Ignore)]
    [Ambient]
    [UsableDuringInitialization(true)]
    public class ColorSystem : ResourceDictionary
    {


        public static ColorSystem? Instance;
        public ColorSystem()
        {
            Instance = this;


        }

        public ColorSystem(ThemeType type) : base()
        {
            Type = type;
        }

        private ThemeType _type;

        public ThemeType Type
        {
            get => _type;
            set
            {
                switch (value)
                {
                    //浅色主题
                    case ThemeType.Light:
                        this.Source = new Uri($"pack://application:,,,/MaterialDesign3;component/Styles/Colors/LightColor.xaml", UriKind.Absolute);
                        break;
                    case ThemeType.Dark:
                        //深色主题
                        this.Source = new Uri($"pack://application:,,,/MaterialDesign3;component/Styles/Colors/DarkColor.xaml", UriKind.Absolute);
                        break;
                    case ThemeType.DynamicDark:

                        GenerateDynamicColor(true);
                        break;
                    case ThemeType.DynamicLight:

                        GenerateDynamicColor();
                        break;
                    default:
                        this.Source = new Uri($"pack://application:,,,/MaterialDesign3;component/Styles/Colors/DarkColor.xaml", UriKind.Absolute);

                        break;
                }
                _type = value;
            }
        }

        static ResourceDictionary BrushRes;
        public static void SetTheme(ThemeType themeType)
        {

            Application.Current.Resources.MergedDictionaries.Remove(BrushRes);


            BrushRes = new ResourceDictionary { Source = new Uri("pack://application:,,,/MaterialDesign3;component/Styles/Brushes/MD3Brush.xaml", UriKind.Absolute) };

            BrushRes.MergedDictionaries.Clear();

            BrushRes.MergedDictionaries.Add(new ColorSystem(themeType));

            Application.Current.Resources.MergedDictionaries.Add(BrushRes);

        }

        public void GenerateDynamicColor(bool dark = false)
        {
            var colors = ThemeBuilder.Create(dark);

            foreach (PropertyInfo item in typeof(Scheme<Color>).GetProperties())
            {
                //Console.WriteLine(item.Name);

                Add($"{item.Name}", item.GetValue(colors));
            }
        }
    }


    public enum ThemeType
    {
        Light, Dark,
        DynamicDark,
        DynamicLight
    }
}
