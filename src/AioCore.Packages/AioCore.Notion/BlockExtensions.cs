using System.Runtime.CompilerServices;
using AioCore.Notion.Lib.ApiV3.Model;
using Newtonsoft.Json.Linq;

[assembly: InternalsVisibleTo("NotionSharpTest")]

namespace AioCore.Notion
{
    public class TransformOptions
    {
        /// <summary>
        /// block types that are selected
        /// </summary>
        /// <remarks>
        /// null = all types accepted
        /// </remarks>
        public IList<string> AcceptedBlockTypes { get; set; }

        /// <summary>
        /// Maximum blocks to transform (from AcceptedBlockTypes)
        /// </summary>
        /// <remarks>
        /// 0 = all blocks
        /// </remarks>
        public int MaxBlocks { get; set; }

        /// <summary>
        /// Optional. DIV.
        /// </summary>
        public Func<BlockTextData, Block, bool> TransformText { get; set; }

        /// <summary>
        /// Optional. H1.
        /// </summary>
        public Func<BlockTextData, Block, bool> TransformHeader { get; set; }

        /// <summary>
        /// Optional. H2.
        /// </summary>
        public Func<BlockTextData, Block, bool> TransformSubHeader { get; set; }

        /// <summary>
        /// Optional. H3.
        /// </summary>
        public Func<BlockTextData, Block, bool> TransformSubSubHeader { get; set; }

        /// <summary>
        /// Optional. UL+LI.
        /// </summary>
        public Func<BlockTextData, Block, (bool Ok, Action ContentTransformed)> TransformBulletedList { get; set; }

        /// <summary>
        /// Optional. DIV.
        /// </summary>
        public Func<BlockTextData, Block, bool> TransformQuote { get; set; }

        /// <summary>
        /// Optional
        /// </summary>
        /// <remarks>
        /// BlockImageData can be null
        /// </remarks>
        public Func<BlockImageData, Block, bool> TransformImage { get; set; }

        /// <summary>
        /// Optional. Columns.
        /// </summary>
        public Func<BlockColumnListData, Block, (bool Ok, Func<int, BlockColumnData, Action> StartColumn, Action<int>
            TransformColumnSeparator, Action EndColumnList)> TransformColumnList { get; internal set; }

        /// <summary>
        /// Optional. Callouts.
        /// </summary>
        public Func<BlockCalloutData, Block, bool> TransformCallout { get; set; }

        /// <summary>
        /// Optional
        /// </summary>
        public Func<Block, bool> TransformOther { get; set; }

        /// <summary>
        /// If a block in the selected page is missing (ie: page content is not fully loaded), throw an exception
        /// </summary>
        public bool ThrowIfBlockMissing { get; set; } = true;

        public bool ThrowIfCantDecodeTextData { get; set; } = true;
    }

    public class BlockColumnListData
    {
        public IList<BlockColumnData> Columns { get; set; }
    }

    public class BlockColumnData
    {
        public Block ColumnBlock { get; set; }
        public float Ratio { get; set; }
    }

    public class BlockTextData
    {
        public IList<BlockTextPart> Lines { get; set; }
    }

    public class BlockCalloutData
    {
        public BlockTextData Text { get; set; }
        public PageFormat Format { get; set; }
    }

    public class BlockTextPart
    {
        public string Text { get; set; }
        public bool HasProperty => HyperlinkUrl != null || HasStyle;
        public bool HasStyle => IsItalic || IsBold || HtmlColor != null;

        public string HyperlinkUrl { get; set; }
        public bool IsItalic { get; set; }
        public bool IsBold { get; set; }
        public string HtmlColor { get; set; }
    }

    public class BlockImageData
    {
        /// <summary>
        /// Can not be null
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Can be null
        /// </summary>
        public string ImagePrivateUrl { get; set; }

        /// <summary>
        /// Can be null
        /// </summary>
        public BlockImageFormat Format { get; set; }

        /// <summary>
        /// Can be null
        /// </summary>
        public string Caption { get; set; }
    }

    public static class BlockExtensions
    {
        public static void Transform(this RecordMap recordMap, TransformOptions transformOptions, Guid pageId = default)
        {
            var pageBlock = pageId != default
                ? recordMap.Block[pageId]
                : recordMap.Block.First(b => b.Value.Type == "page").Value;

            Transform(pageBlock.Content, recordMap.Block, transformOptions);
        }

        static bool Transform(IEnumerable<Guid> contentIds, Dictionary<Guid, Block> allBlocks,
            TransformOptions transformOptions)
        {
            var blocks = from itemId in contentIds
                let block = !transformOptions.ThrowIfBlockMissing && !allBlocks.ContainsKey(itemId)
                    ? null
                    : allBlocks[itemId]
                where block != null && (transformOptions?.AcceptedBlockTypes == null ||
                                        transformOptions.AcceptedBlockTypes.Contains(block.Type))
                select block;

            if (transformOptions.MaxBlocks > 0)
                blocks = blocks.Take(transformOptions.MaxBlocks);

            return blocks.All(block => Transform(block, allBlocks, transformOptions));
        }

        static bool Transform(Block block, Dictionary<Guid, Block> allBlocks, TransformOptions transformOptions)
        {
            var okToContinue = block.Type switch
            {
                "text" => transformOptions.TransformText?.Invoke(
                    block.ToTextData(transformOptions.ThrowIfCantDecodeTextData), block),
                "quote" => transformOptions.TransformQuote?.Invoke(
                    block.ToTextData(transformOptions.ThrowIfCantDecodeTextData), block),
                "header" => transformOptions.TransformHeader?.Invoke(
                    block.ToTextData(transformOptions.ThrowIfCantDecodeTextData), block),
                "sub_header" => transformOptions.TransformSubHeader?.Invoke(
                    block.ToTextData(transformOptions.ThrowIfCantDecodeTextData), block),
                "sub_sub_header" => transformOptions.TransformSubSubHeader?.Invoke(
                    block.ToTextData(transformOptions.ThrowIfCantDecodeTextData), block),
                "bulleted_list" => TransformBulletedList(transformOptions, block, allBlocks),
                "image" => transformOptions.TransformImage?.Invoke(block.ToImageData(), block),
                "column_list" => TransformColumnList(transformOptions, block, allBlocks),
                "callout" => transformOptions.TransformCallout?.Invoke(
                    block.ToCalloutData(transformOptions.ThrowIfCantDecodeTextData), block),
                _ => transformOptions.TransformOther?.Invoke(block)
            };

            return okToContinue ?? true;
        }

        static bool TransformBulletedList(TransformOptions transformOptions, Block block,
            Dictionary<Guid, Block> allBlocks)
        {
            if (transformOptions.TransformBulletedList == null)
                return true;

            var result =
                transformOptions.TransformBulletedList(block.ToTextData(transformOptions.ThrowIfCantDecodeTextData),
                    block);

            if (block.Content?.Count > 0)
                Transform(block.Content, allBlocks, transformOptions);

            result.ContentTransformed();
            return result.Ok;
        }

        static bool TransformColumnList(TransformOptions transformOptions, Block block,
            Dictionary<Guid, Block> allBlocks)
        {
            if (transformOptions.TransformColumnList == null || (block.Content?.Count ?? 0) == 0)
                return true;

            var data = new BlockColumnListData
            {
                Columns = block.Content
                    .Select(blockId => allBlocks.TryGetValue(blockId, out var b) ? b : null)
                    .Where(b => b != null)
                    .Select(b => new BlockColumnData { ColumnBlock = b, Ratio = b.ColumnFormat?.Ratio ?? 0 })
                    .ToList()
            };
            var result = transformOptions.TransformColumnList(data, block);

            var columnIndex = 0;
            var totalColumns = data.Columns.Count;
            foreach (var column in data.Columns)
            {
                var endColumnAction = result.StartColumn(columnIndex, column);
                var ok = Transform(column.ColumnBlock.Content, allBlocks, transformOptions);
                endColumnAction();

                if (!ok)
                {
                    result.Ok = false;
                    break;
                }

                if (columnIndex < totalColumns - 1)
                    result.TransformColumnSeparator(columnIndex);

                columnIndex++;
            }

            result.EndColumnList();
            return result.Ok;
        }

        private static BlockImageData ToImageData(this Block imageBlock)
        {
            if (imageBlock.Properties == null) return null;
            var data = new BlockImageData();

            if (imageBlock.Value.ContainsKey("format"))
            {
                data.Format = imageBlock.Value["format"]?.ToObject<BlockImageFormat>();
                if (data.Format != null) data.ImageUrl = data.Format.DisplaySource;
            }

            if (imageBlock.Properties.ContainsKey("source"))
            {
                data.ImagePrivateUrl = (string)imageBlock.Properties["source"]?[0]?[0];
                //This url has priority over the one in "format"
                if (data.ImagePrivateUrl != null)
                    data.ImageUrl = $"https://www.notion.so/image/{Uri.EscapeDataString(data.ImagePrivateUrl)}";
            }

            if (imageBlock.Properties.ContainsKey("caption"))
                data.Caption = (string)imageBlock.Properties["caption"]?[0]?[0];

            return data.ImageUrl == null ? null : data;
        }

        internal static BlockCalloutData ToCalloutData(this Block calloutBlock, bool throwIfCantDecodeTextData)
        {
            var blockTextData = calloutBlock.ToTextData(throwIfCantDecodeTextData);

            return new BlockCalloutData
            {
                Text = blockTextData,
                Format = calloutBlock.Format
            };
        }

        private static BlockTextData ToTextData(this Block textBlock, bool throwIfCantDecodeTextData)
        {
            var data = new BlockTextData { Lines = new List<BlockTextPart>() };

            if (textBlock.Properties == null || !textBlock.Properties.ContainsKey("title")) return data;
            var textParts = (((JArray)textBlock.Properties["title"]) ?? throw new InvalidOperationException())
                .Cast<JArray>();

            foreach (var textPart in textParts)
                data.Lines.Add(ToTextData(textPart, throwIfCantDecodeTextData));

            return data;
        }

        private static BlockTextPart ToTextData(JArray textPart, bool throwIfCantDecodeTextData)
        {
            var blockTextPart = new BlockTextPart { Text = (string)textPart[0] };

            switch (textPart.Count)
            {
                case 1:
                    return blockTextPart;
                case 2 when textPart[1] is JArray subParts:
                {
                    foreach (var subPart in subParts)
                    {
                        if (subPart is JArray outerTag && outerTag.All(t => t is not JArray))
                        {
                            var outerTagValue = (string)outerTag[0];
                            switch (outerTagValue)
                            {
                                case "a" when outerTag.Count == 2:
                                    blockTextPart.HyperlinkUrl = (string)outerTag[1];
                                    break;
                                case "h" when outerTag.Count == 2:
                                    blockTextPart.HtmlColor = (string)outerTag[1];
                                    break;
                                default:
                                {
                                    if (outerTag.Count == 1 && outerTagValue is "i" or "b")
                                    {
                                        switch (outerTagValue)
                                        {
                                            //bold, italic
                                            case "b":
                                                blockTextPart.IsBold = true;
                                                break;
                                            case "i":
                                                blockTextPart.IsItalic = true;
                                                break;
                                        }
                                    }
                                    else if (throwIfCantDecodeTextData)
                                        throw new NotSupportedException(
                                            $"unknown outer tag {outerTag[0]} with counts:{outerTag.Count}");

                                    break;
                                }
                            }
                        }
                        else if (throwIfCantDecodeTextData)
                            throw new NotSupportedException($"unknown subParts {subParts}");
                    }

                    break;
                }
                case 2 when throwIfCantDecodeTextData:
                    throw new NotSupportedException($"unknown subParts structure {textPart[1]}");
                case 2:
                    break;
                default:
                {
                    if (throwIfCantDecodeTextData)
                        throw new NotSupportedException(
                            $"this decoder supports only 1 or 2 textParts. This block has {textPart.Count} parts. textPart: {textPart}");
                    break;
                }
            }

            return blockTextPart;
        }
    }
}