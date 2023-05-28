using System.ComponentModel;
using System.Windows.Documents;
using System.Windows.Shapes;

namespace CounterPOS.Common;

public class TextShape : Shape
{
    private double _height;

    private Geometry _textGeometry;

    private double _width;

  

    protected override sealed Geometry DefiningGeometry
    {
        get
        {
            return _textGeometry ?? Geometry.Empty;
        }
    }

    #region Dependency Properties

    /// <summary>
    /// DependencyProperty for <see cref="FontFamily" /> property.
    /// </summary>
    public static readonly DependencyProperty FontFamilyProperty =
            TextElement.FontFamilyProperty.AddOwner(typeof(TextShape));

    /// <summary>
    /// DependencyProperty for <see cref="FontSize" /> property.
    /// </summary>
    public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(typeof(TextShape));

    /// <summary>
    /// DependencyProperty for <see cref="FontStretch" /> property.
    /// </summary>
    public static readonly DependencyProperty FontStretchProperty = TextElement.FontStretchProperty.AddOwner(typeof(TextShape));

    /// <summary>
    /// DependencyProperty for <see cref="FontStyle" /> property.
    /// </summary>
    public static readonly DependencyProperty FontStyleProperty = TextElement.FontStyleProperty.AddOwner(typeof(TextShape));

    /// <summary>
    /// DependencyProperty for <see cref="FontWeight" /> property.
    /// </summary>
    public static readonly DependencyProperty FontWeightProperty = TextElement.FontWeightProperty.AddOwner(typeof(TextShape));

    /// <summary>
    /// DependencyProperty for <see cref="Text" /> property.
    /// </summary>
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
                                                              "Text",
                                                              typeof(string),
                                                              typeof(TextShape),
                                                              new FrameworkPropertyMetadata(
                                                                      string.Empty,
                                                                      FrameworkPropertyMetadataOptions.AffectsMeasure |
                                                                      FrameworkPropertyMetadataOptions.AffectsRender));

    /// <summary>
    /// The FontFamily property specifies the name of font family.
    /// </summary>
    [Localizability(LocalizationCategory.Font)]
    public System.Windows.Media.FontFamily FontFamily
    {
        get { return (System.Windows.Media.FontFamily)GetValue(FontFamilyProperty); }
        set { SetValue(FontFamilyProperty, value); }
    }

    /// <summary>
    /// The FontSize property specifies the size of the font.
    /// </summary>
    [TypeConverter(typeof(FontSizeConverter))]
    [Localizability(LocalizationCategory.None)]
    public double FontSize
    {
        get { return (double)GetValue(FontSizeProperty); }
        set { SetValue(FontSizeProperty, value); }
    }

    /// <summary>
    /// The FontStretch property selects a normal, condensed, or extended face from a font family.
    /// </summary>
    public FontStretch FontStretch
    {
        get { return (FontStretch)GetValue(FontStretchProperty); }
        set { SetValue(FontStretchProperty, value); }
    }

    /// <summary>
    /// The FontStyle property requests normal, italic, and oblique faces within a font family.
    /// </summary>
    public System.Windows.FontStyle FontStyle
    {
        get { return (System.Windows.FontStyle)GetValue(FontStyleProperty); }
        set { SetValue(FontStyleProperty, value); }
    }

    /// <summary>
    /// The FontWeight property specifies the weight of the font.
    /// </summary>
    public FontWeight FontWeight
    {
        get { return (FontWeight)GetValue(FontWeightProperty); }
        set { SetValue(FontWeightProperty, value); }
    }

    /// <summary>
    /// The Text property defines the content (text) to be displayed.
    /// </summary>
    [Localizability(LocalizationCategory.Text)]
    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    #endregion Dependency Properties

    protected override System.Windows.Size MeasureOverride(System.Windows.Size availableSize)
    {
        RealizeGeometry();
        return new System.Windows.Size(Math.Min(availableSize.Width, _width), Math.Min(availableSize.Height, _height));
    }

    private void RealizeGeometry()
    {
        var formattedText = new FormattedText(
                                   Text,
                                   CultureInfo.CurrentCulture,
                                   FlowDirection.LeftToRight,
                                   new Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize, Brushes.Black, 100);

        _height = formattedText.Height;
        _width = formattedText.Width;
        _textGeometry = formattedText.BuildGeometry(new System.Windows.Point());

        if (Text == " ")
        {
            formattedText = new FormattedText(
                                   "_",
                                   CultureInfo.CurrentCulture,
                                   FlowDirection.LeftToRight,
                                   new Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize, Brushes.Black, 100);
            _width = formattedText.Width;
        }
    }
}