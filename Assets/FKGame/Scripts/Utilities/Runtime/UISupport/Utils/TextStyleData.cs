using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class TextStyleData
    {
        public string fontName { get; set; }
        //
        // ժҪ:
        //     Font size.
        public int fontSize { get; set; }
        //
        // ժҪ:
        //     Font Style.
        public FontStyle fontStyle { get; set; }
        //
        // ժҪ:
        //     Is best fit used.
        public bool bestFit { get; set; }
        //
        // ժҪ:
        //     Minimum text size.
        public int minSize { get; set; }
        //
        // ժҪ:
        //     Maximum text size.
        public int maxSize { get; set; }
        //
        // ժҪ:
        //     How is the text aligned.
        public TextAnchor alignment { get; set; }
        //
        // ժҪ:
        //     Use the extents of glyph geometry to perform horizontal alignment rather than
        //     glyph metrics.
        public bool alignByGeometry { get; set; }
        //
        // ժҪ:
        //     Should RichText be used?
        public bool richText { get; set; }
        //
        // ժҪ:
        //     Horizontal overflow mode.
        public HorizontalWrapMode horizontalOverflow { get; set; }
        //
        // ժҪ:
        //     Vertical overflow mode.
        public VerticalWrapMode verticalOverflow { get; set; }
        //
        // ժҪ:
        //     Line spacing.
        public float lineSpacing { get; set; }
    }
}