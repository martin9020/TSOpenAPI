https://chatgpt.com/share/671a919d-8bac-8004-a90c-2e822e3c4793


namespace PadFootingCreator
{
    public class GridPoint
    {   
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public string ElementType { get; set; }  // "Pad", "Column", "Beam"

    public GridPoint(double x, double y, double z, string elementType)
    {
        X = x;
        Y = y;
        Z = z;
        ElementType = elementType;
        }
    }

    public class StructureGrid
    {
        public Dictionary<(double, double, double), GridPoint> Points { get; set; } = new Dictionary<(double, double, double), GridPoint>();

        public void AddPoint(double x, double y, double z, string elementType)
        {
            var point = new GridPoint(x, y, z, elementType);
            Points[(x, y, z)] = point;
        }

    // Remove points in a range, for example between (X2; Xn-1) and (Y2; Ym-1)
        public void RemovePointsInRange(double startX, double endX, double startY, double endY)
        {
            var keysToRemove = Points
                .Where(p => p.Key.Item1 >= startX && p.Key.Item1 <= endX && 
                            p.Key.Item2 >= startY && p.Key.Item2 <= endY)
                .Select(p => p.Key)
                .ToList();

            foreach (var key in keysToRemove)
            {
                Points.Remove(key);
            }   
        }   
    }
    public void CreatePadFooting(double x, double y)
    {
    double padZ = -200.0;  // Foundation depth
    AddPoint(x, y, padZ, "Pad");
    
    // Logic to create the pad in the model, e.g., Tekla Structures or similar
    Beam pad = new Beam 
    {
        StartPoint = { X = x, Y = y, Z = padZ },
        EndPoint = { X = x, Y = y, Z = padZ - 800.0 }  // Pad length
    };
    pad.Insert();
    }

    public void CreateColumn(double x, double y, double height)
    {
        double columnStartZ = -200.0;  // Same as pad top
        double columnEndZ = height;    // Column height (3000 by default)

        AddPoint(x, y, columnStartZ, "Column");
        AddPoint(x, y, columnEndZ, "Column");

        // Logic to create the column
        Beam column = new Beam
        {
            StartPoint = { X = x, Y = y, Z = columnStartZ },
            EndPoint = { X = x, Y = y, Z = columnEndZ }
        };
        column.Insert();
    }
    public void CreateBeam(double startX, double startY, double endX, double endY, double height)
    {
        AddPoint(startX,
            startY,
            height,
            "Beam");
        AddPoint(endX, endY, height, "Beam");

        // Logic to create the beam
        Beam beam = new Beam
        {
            StartPoint = { X = startX, Y = startY, Z = height },
            EndPoint = { X = endX, Y = endY, Z = height }
        };
        beam.Insert();
    }


    public void PlaceElements(StructureGrid grid)
    {
        foreach (var point in grid.Points.Values)
        {
            if (point.ElementType == "Pad")
            {
                CreatePadFooting(point.X, point.Y);
            }
            else if (point.ElementType == "Column")
            {
                CreateColumn(point.X, point.Y, 3000);  // Default column height is 3000
            }
            else if (point.ElementType == "Beam")
            {
                // You'll need to determine the start and end points of the beams
                CreateBeam(point.X, point.Y, point.X + 800, point.Y, 3000);  // Example
            }
        }
    }


    public void GenerateGrid(int n, int m, double spacingX, double spacingY)
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                double x = i * spacingX;
                double y = j * spacingY;
                AddPoint(x, y, -200.0, "Pad");
                AddPoint(x, y, 3000, "Column");  // Column at top of the pad
            }
        }
    }
    public void ExtendBoundaryBeams(StructureGrid grid, double extension)
    {
        // Variables to hold the extreme beams
        GridPoint firstXBeamStart = null;
        GridPoint lastXBeamEnd = null;
        GridPoint firstYBeamStart = null;
        GridPoint lastYBeamEnd = null;

        // Loop through all grid points and identify beams
        foreach (var point in grid.Points.Values)
        {
            // We focus only on beams
            if (point.ElementType == "Beam")
            {
                // Identify the first and last beams along the X direction
                if (firstXBeamStart == null || point.X < firstXBeamStart.X)
                    firstXBeamStart = point;  // Smallest X coordinate (first beam)
                if (lastXBeamEnd == null || point.X > lastXBeamEnd.X)
                    lastXBeamEnd = point;    // Largest X coordinate (last beam)

                // Identify the first and last beams along the Y direction
                if (firstYBeamStart == null || point.Y < firstYBeamStart.Y)
                    firstYBeamStart = point; // Smallest Y coordinate (first beam)
                if (lastYBeamEnd == null || point.Y > lastYBeamEnd.Y)
                    lastYBeamEnd = point;    // Largest Y coordinate (last beam)
            }
        }

        // Now that we have the extreme beams, we can extend them
        if (firstXBeamStart != null)
        {
            // Extend the first X beam start point in the negative X direction
            firstXBeamStart.X -= extension;
            UpdateBeam(firstXBeamStart.X, firstXBeamStart.Y, "start");
        }

        if (lastXBeamEnd != null)
        {
            // Extend the last X beam end point in the positive X direction
            lastXBeamEnd.X += extension;
            UpdateBeam(lastXBeamEnd.X, lastXBeamEnd.Y, "end");
        }

        if (firstYBeamStart != null)
        {
            // Extend the first Y beam start point in the negative Y direction
            firstYBeamStart.Y -= extension;
            UpdateBeam(firstYBeamStart.X, firstYBeamStart.Y, "start");
        }

        if (lastYBeamEnd != null)
        {
            // Extend the last Y beam end point in the positive Y direction
            lastYBeamEnd.Y += extension;
            UpdateBeam(lastYBeamEnd.X, lastYBeamEnd.Y, "end");
        }
    }

    public void UpdateBeam(double x, double y, string pointType)
    {
        // Logic to update the beam's start or end point
        if (pointType == "start")
        {
            // Update the beam's start point with the new coordinates
            Beam beam = new Beam
            {
                StartPoint = { X = x, Y = y, Z = 3000 }  // Assuming Z stays the same
            };
            beam.Insert(); // Replace with logic that updates the beam in your model
        }
        else if (pointType == "end")
        {
            // Update the beam's end point with the new coordinates
            Beam beam = new Beam
            {
                EndPoint = { X = x, Y = y, Z = 3000 }
            };
            beam.Insert(); // Replace with logic that updates the beam in your model
        }
    }



}


