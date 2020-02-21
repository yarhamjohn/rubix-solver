using Newtonsoft.Json;

namespace rubix_solver
{
    public class Cube
    {
        public class Face
        {
            public class Row
            {
                public class Column
                {
                    [JsonProperty("left")] public string? Left { get; set; }
                    [JsonProperty("right")] public string? Right { get; set; }
                    [JsonProperty("top")] public string? Top { get; set; }
                    [JsonProperty("bottom")] public string? Bottom { get; set; }
                    [JsonProperty("front")] public string? Front { get; set; }
                    [JsonProperty("back")] public string? Back { get; set; }
                }

                [JsonProperty("leftColumn")] public Column LeftColumn { get; set; }
                [JsonProperty("middleColumn")] public Column MiddleColumn { get; set; }
                [JsonProperty("rightColumn")] public Column RightColumn { get; set; }
            }

            [JsonProperty("topRow")] public Row TopRow { get; set; }
            [JsonProperty("middleRow")] public Row MiddleRow { get; set; }
            [JsonProperty("bottomRow")] public Row BottomRow { get; set; }
        }

        [JsonProperty("frontFace")] public Face FrontFace { get; set; }

        [JsonProperty("middleFace")] public Face MiddleFace { get; set; }

        [JsonProperty("backFace")] public Face BackFace { get; set; }
    }
}