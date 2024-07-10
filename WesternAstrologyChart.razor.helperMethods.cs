using System.Globalization;
using SharpAstrology.Enums;

namespace SharpAstrology.West.BlazorComponents;

public partial class WesternAstrologyChart
{
    private string S(double value) => value.ToString(CultureInfo.InvariantCulture);

    private (double x, double y) _getPoint(double angle, double d)
    {
        return (d * Math.Cos(Math.PI * (angle - _zero) / 180), -d * Math.Sin(Math.PI * (angle - _zero) / 180));
    }
    
    private string _getPointAsString(double angle, double d)
    {
        var x = d * Math.Cos(Math.PI*(angle)/180);
        var y = - d * Math.Sin(Math.PI*(angle)/180);

        return $"{S(x)} {S(y)}";
    }

    private string _getOffset(int index)
    {
        var offset = 0.0;
        for (var i = 0; i < index; i++)
        {
            offset += Chart.HousePositions.HouseCusps[(Houses) (i + 1)] - Chart.HousePositions.HouseCusps[(Houses)i];
        }
        return (180 - offset).ToString(CultureInfo.InvariantCulture);
    }
}