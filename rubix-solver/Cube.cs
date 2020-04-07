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
                    public Colour? LeftColour
                    {
                        get { return Left switch {"red" => Colour.Red, "blue" => Colour.Blue, "green" => Colour.Green, "white" => Colour.White, "yellow" => Colour.Yellow, "orange" => Colour.Orange, _ => null}; }
                    }

                    [JsonProperty("left")] public string? Left { get; set; }
                    [JsonProperty("right")] public string? Right { get; set; }
                    [JsonProperty("top")] public string? Top { get; set; }
                    [JsonProperty("bottom")] public string? Bottom { get; set; }
                    [JsonProperty("front")] public string? Front { get; set; }
                    [JsonProperty("back")] public string? Back { get; set; }
                }

                [JsonProperty("leftColumn")] public Row.Column LeftColumn { get; set; }
                [JsonProperty("middleColumn")] public Row.Column MiddleColumn { get; set; }
                [JsonProperty("rightColumn")] public Row.Column RightColumn { get; set; }
            }

            [JsonProperty("topRow")] public Face.Row TopRow { get; set; }
            [JsonProperty("middleRow")] public Face.Row MiddleRow { get; set; }
            [JsonProperty("bottomRow")] public Face.Row BottomRow { get; set; }
        }

        [JsonProperty("frontFace")] public Face FrontFace { get; set; }

        [JsonProperty("middleFace")] public Face MiddleFace { get; set; }

        [JsonProperty("backFace")] public Face BackFace { get; set; }
    }
}
