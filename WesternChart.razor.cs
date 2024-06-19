using Microsoft.AspNetCore.Components;
using SharpAstrology.DataModels;
using SharpAstrology.Enums;
using SharpAstrology.ExtensionMethods;

namespace SharpAstrology.West.BlazorComponents;

public partial class WesternChart
{
    private double _zero;
    private string _viewBox;
    bool _outer;
    private Dictionary<Planets, double> _outerPlanetDisplay;
    private Dictionary<Planets, double> _innerPlanetDisplay;
    private Dictionary<Planets, double> _outerPlanetDisplayOther;
    private Dictionary<Planets, double> _innerPlanetDisplayOther;
    private Dictionary<Planets, Dictionary<Planets, Aspects>> _aspects;
    

    [Parameter] public string Width { get; set; } = "100%";
    [Parameter] public string Height { get; set; } = "100%";
    [Parameter] public string HoverColor { get; set; } = "yellow";
    [Parameter] public string StrokeColor { get; set; } = "black";
    [Parameter] public Dictionary<Aspects,string> AspectsColorMap { get; set; } = new()
    {
        [Aspects.Conjunction] = "red",
        [Aspects.Opposition] = "red",
        [Aspects.Sextile] = "blue",
        [Aspects.Square] = "red",
        [Aspects.Trine] = "blue",
        [Aspects.Quincunx] = "green",
        [Aspects.None] = "transparent"
    };
    [Parameter] public EventCallback<Enum> OnClickCallback { get; set; }
    
    
    private AstrologyChart _chart;
    [EditorRequired][Parameter] public AstrologyChart Chart
    {
        get => _chart;
        set
        {
            _chart = value;
            _aspects = value.CalculateAspects();
            _zero = 180;
            if (Chart.HousePositions is not null)
            {
                _zero += Chart.HousePositions.Cross[Cross.Asc];
                Chart.HousePositions.HouseCusps[0] = Chart.HousePositions.Cross[Cross.Asc];
            }
            (_outerPlanetDisplay, _innerPlanetDisplay) = _calculateObjectsDisplayPosition(_chart);
        }
    }
    
    private Dictionary<Planets, PlanetPosition> _planetsPositionsOther;
    [Parameter] public Dictionary<Planets, PlanetPosition> PlanetsPositionsOther 
    {
        get => _planetsPositionsOther;
        set
        {
            _planetsPositionsOther = value;
            (_outerPlanetDisplayOther, _innerPlanetDisplayOther) = _calculateObjectsDisplayPosition(_planetsPositionsOther);
        }
    }
    [Parameter] public WheelOptions ShowOnInnerWheel { get; set; } = WheelOptions.Signs;
    
    private WheelOptions _showOnOuterWheel = WheelOptions.None;
    [Parameter] public WheelOptions ShowOnOuterWheel
    {
        get => _showOnOuterWheel;
        set
        {
            _showOnOuterWheel = value;
            _outer = value != WheelOptions.None;
            _viewBox = _outer ? "-1.4 -1.4 2.8 2.8" : "-1.2 -1.2 2.4 2.4";
        } 
    }
    [Parameter] public bool ShowAspectsToOther { get; set; }
    // [Parameter] public IWesternChartSettings Settings { get; set; } = new DefaultWesternChartSettings();
    
    protected override void OnParametersSet()
    {
        _viewBox = _outer ? "-1.4 -1.4 2.8 2.8" : "-1.2 -1.2 2.4 2.4";
    }
    
}