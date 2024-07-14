using SharpAstrology.DataModels;
using SharpAstrology.Enums;

namespace SharpAstrology.BlazorComponents;

internal class RingOfPlanets
{ 
    private HashSet<OrbitOfPlanetsElement> Planets { get; } = new();

    #region Constructor

    public RingOfPlanets() {}
    public RingOfPlanets(IDictionary<Planets, PlanetPosition> objectPositions)
    {
        OrbitOfPlanetsElement before = null;
        foreach (var p in objectPositions.OrderBy(x => x.Value.Longitude))
        {
            if (before is null)
            {
                before = new OrbitOfPlanetsElement()
                {
                    Object = p.Key,
                    Degree = p.Value.Longitude
                };
                Planets.Add(before);
                continue;
            }

            var obj = new OrbitOfPlanetsElement()
            {
                Object = p.Key,
                Degree = p.Value.Longitude,
                Before = before
            };
            before.Next = obj;
            Planets.Add(obj);
            before = obj;
        }

        var first = Planets.MinBy(x => x.Degree);
        before.Next = first;
        first.Before = before;
    }
    public RingOfPlanets(AstrologyChart chart)
    {
        OrbitOfPlanetsElement before = null;
        foreach (var p in chart.SupportedObjects.OrderBy(p=>chart.PositionOf(p).Longitude))
        {
            if (before is null)
            {
                before = new OrbitOfPlanetsElement()
                {
                    Object = p,
                    Degree = chart.PositionOf(p).Longitude
                };
                Planets.Add(before);
                continue;
            }

            var obj = new OrbitOfPlanetsElement()
            {
                Object = p,
                Degree = chart.PositionOf(p).Longitude,
                Before = before
            };
            before.Next = obj;
            Planets.Add(obj);
            before = obj;
        }

        var first = Planets.MinBy(x => x.Degree);
        before.Next = first;
        first.Before = before;
    }

    #endregion

    public void Add(Planets planet, double longitude)
    {
        var element = new OrbitOfPlanetsElement()
        {
            Object = planet,
            Degree = longitude
        };
        if (Planets.Count == 0)
        {
            element.Before = element;
            element.Next = element;
            Planets.Add(element);
            return;
        }
        var before = Planets.Where(x => x.Degree <= element.Degree).MaxBy(x=>x.Degree);
        if (before is null) // Added element ist the smallest
        {
            var smallest = Planets.MinBy(x => x.Degree);
            var last = smallest.Before;
            element.Next = smallest;
            element.Before = last;
            smallest.Before = element;
            last.Next = element;
            Planets.Add(element);
            return;
        }
        before.Next.Before = element;
        element.Next = before.Next;
        element.Before = before;
        before.Next = element;
        Planets.Add(element);
    }

    public void Remove(OrbitOfPlanetsElement element)
    {
        if (!Planets.Contains(element)) throw new ArgumentException("Element is not Part of the Orbit.");
        switch (Planets.Count)
        {
            case 1:
                Planets.Remove(element);
                return;
            case 2:
            {
                Planets.Remove(element);
                var lastElement = Planets.First();
                lastElement.Next = lastElement;
                lastElement.Before = lastElement;
                return;
            }
        }

        element.Next.Before = element.Before;
        element.Before.Next = element.Next;
        Planets.Remove(element);
    }
    
    public OrbitOfPlanetsElement? GetFirst()
    {
        return Planets.Count == 0 ? null : Planets.MinBy(p => p.Degree);
    }

    public Dictionary<Planets, double> ToDictionary()
    {
        return Planets.ToDictionary(p => p.Object, p => p.Degree);
    }
}

internal class OrbitOfPlanetsElement
{
    public Planets Object { get; set; }
    public double Degree { get; set; }
    public OrbitOfPlanetsElement Next { get; set; }
    public OrbitOfPlanetsElement Before { get; set; }
}