using SharpAstrology.DataModels;
using SharpAstrology.Utility;
using SharpAstrology.Enums;

namespace SharpAstrology.BlazorComponents;

public partial class WesternAstrologyChart
{
    private (Dictionary<Planets, double> outerDegrees, Dictionary<Planets, double> innerDegrees) _calculateObjectsDisplayPosition(IDictionary<Planets, PlanetPosition> planetPositions)
    {
        var outerOrbit = new OrbitOfPlanets(planetPositions);
        var innerOrbit = new OrbitOfPlanets();
        const double orbit = 4.0;
        var first = outerOrbit.GetFirst();
        var current = first;

        if (first is null) return (outerOrbit.ToDictionary(), innerOrbit.ToDictionary());
        do
        {
            var next = current.Next;
                
            if (Math.Abs(AstrologyUtility.AngleDifference(current.Degree, next.Degree)) < orbit)
            {
                //prevent infinite loop
                if (next.Equals(first))
                {
                    innerOrbit.Add(current.Object, current.Degree);
                    outerOrbit.Remove(current);
                    break;
                }
                innerOrbit.Add(next.Object, next.Degree);
                outerOrbit.Remove(next);
            }
            current = current.Next;
        } while (!current.Equals(first));

        return (outerOrbit.ToDictionary(), innerOrbit.ToDictionary());
    }
    private (Dictionary<Planets, double> outerDegrees, Dictionary<Planets, double> innerDegrees) _calculateObjectsDisplayPosition(AstrologyChart chart)
    {
        var outerOrbit = new OrbitOfPlanets(chart);
        var innerOrbit = new OrbitOfPlanets();
        const double orbit = 4.0;
        var first = outerOrbit.GetFirst();
        var current = first;

        if (first is null) return (outerOrbit.ToDictionary(), innerOrbit.ToDictionary());
        do
        {
            var next = current.Next;
                
            if (Math.Abs(AstrologyUtility.AngleDifference(current.Degree, next.Degree)) < orbit)
            {
                //prevent infinite loop
                if (next.Equals(first))
                {
                    innerOrbit.Add(current.Object, current.Degree);
                    outerOrbit.Remove(current);
                    break;
                }
                innerOrbit.Add(next.Object, next.Degree);
                outerOrbit.Remove(next);
            }
            current = current.Next;
        } while (!current.Equals(first));

        return (outerOrbit.ToDictionary(), innerOrbit.ToDictionary());
    }
    
}