//OverlapDetector
using System.Collections.Generic;
using Clipper2Lib;

public class OverlapDetector //: MonoBehaviour
{
    //void Start()
    //{
    //    // Define the polygons for the two objects
    //    List<IntPoint> poly1 = new List<IntPoint>
    //    {
    //        new IntPoint(0, 0),
    //        new IntPoint(100, 0),
    //        new IntPoint(100, 100),
    //        new IntPoint(0, 100)
    //    };

    //    List<IntPoint> poly2 = new List<IntPoint>
    //    {
    //        new IntPoint(50, 50),
    //        new IntPoint(150, 50),
    //        new IntPoint(150, 150),
    //        new IntPoint(50, 150)
    //    };

    //    // Create a new Clipper object
    //    Clipper c = new();

    //    // Add the polygons to the Clipper object
    //    c.AddPath(poly1, PolyType.ptSubject, true);
    //    c.AddPath(poly2, PolyType.ptClip, true);

    //    // Perform the intersection operation
    //    List<List<IntPoint>> solution = new List<List<IntPoint>>();
    //    c.Execute(ClipType.ctIntersection, solution);

    //    // The solution now contains the overlapping area of the two polygons
    //    foreach (var path in solution)
    //    {
    //        foreach (var point in path)
    //        {
    //            Debug.Log("Point: " + point.X + ", " + point.Y);
    //        }
    //    }
    //}
}
