﻿using Newtonsoft.Json;

namespace AioCore.Notion.Lib.ApiV3.Model
{
    public class PageFormat
    {
        /// <summary>
        /// Url or string of the icon
        /// </summary>
        [JsonProperty("page_icon")]
        public string PageIcon { get; set; }

        [JsonProperty("page_cover_position")]
        public double PageCoverPositon { get; set; }

        /// <summary>
        /// type=callout
        /// </summary>
        [JsonProperty("block_color")]
        public string BlockColor { get; set; }

        [JsonProperty("block_locked")]
        public bool BlockLocked { get; set; }

        [JsonProperty("block_locked_by")]
        public Guid BlockLockedBy { get; set; }
    }
}
