using SharpAstrology.Enums;

namespace SharpAstrology.BlazorComponents;

public sealed class WesternAstrologyChartStyleOptions
{
    public string HoverColorSign { get; set; } = "rgba(0, 255, 255, 0.4)";
    public string HoverColorHouse { get; set; } = "rgba(0, 255, 255, 0.4)";
    public string HoverColorPlanets { get; set; } = "rgba(0, 255, 255, 0.4)";
    public string HoverColorAspects { get; set; } = "rgba(0, 255, 255, 0.4)";
    public string ColorFireSign { get; set; } = "red";
    public string ColorWaterSign { get; set; } = "blue";
    public string ColorAirSign { get; set; } = "darkgoldenrod";
    public string ColorEarthSign { get; set; } = "green";
    public Dictionary<Aspects,string> AspectsColorMap { get; set; } = new()
    {
        [Aspects.Conjunction] = "red",
        [Aspects.Opposition] = "red",
        [Aspects.Sextile] = "blue",
        [Aspects.Square] = "red",
        [Aspects.Trine] = "blue",
        [Aspects.Quincunx] = "green",
        [Aspects.Quintile] = "green",
        [Aspects.SemiSextile] = "green",
        [Aspects.None] = "transparent"
    };
}