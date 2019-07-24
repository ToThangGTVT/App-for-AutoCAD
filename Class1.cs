using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Windows;
using System;

namespace TQCAD
{
    public class Class1
    {
        [CommandMethod("LD1")]
        public static void Main()
        {
            PaletteSet palette = new PaletteSet("TQCad");
            uct_main uct_Main = new uct_main();
            palette.Add("TQCad", uct_Main);
            palette.Visible = true;
            palette.Dock = DockSides.Left;
            
        }

        internal void HCN()
        {
            throw new NotImplementedException();
        }

        [CommandMethod("SSS")]
        public static void SelectObjectsOnscreen()
        {
            // Get the current document and database
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;
            // Start a transaction
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // Request for objects to be selected in the drawing area
                PromptSelectionResult acSSPrompt = acDoc.Editor.GetSelection();

                // If the prompt status is OK, objects were selected
                if (acSSPrompt.Status == PromptStatus.OK)
                {
                    SelectionSet acSSet = acSSPrompt.Value;

                    // Step through the objects in the selection set
                    foreach (SelectedObject acSSObj in acSSet)
                    {
                        // Check to make sure a valid SelectedObject object was returned
                        if (acSSObj != null)
                        {
                            // Open the selected object for write
                            Entity acEnt = acTrans.GetObject(acSSObj.ObjectId,
                                                                OpenMode.ForWrite) as Entity;

                            if (acEnt != null)
                            {
                                // Change the object's color to Green
                                acEnt.ColorIndex = 3;
                            }
                        }
                    }

                    // Save the new object to the database
                    acTrans.Commit();
                }

                // Dispose of the transaction
            }
        }

        public bool circum(double x1, double y1, double x2, double y2, double x3, double y3, ref double xc, ref double yc, ref double r)
        {

            // Calculation of circumscribed circle coordinates and

            // squared radius
            const double eps = 1e-6;
            const double big = 1e12;
            bool result = true;
            double m1, m2, mx1, mx2, my1, my2, dx, dy;
            if ((Math.Abs(y1 - y2) < eps) && (Math.Abs(y2 - y3) < eps))
            {
                result = false;
                xc = x1; yc = y1; r = big;
            }
            else
            {
                if (Math.Abs(y2 - y1) < eps)
                {
                    m2 = -(x3 - x2) / (y3 - y2);
                    mx2 = (x2 + x3) / 2;
                    my2 = (y2 + y3) / 2;
                    xc = (x2 + x1) / 2;
                    yc = m2 * (xc - mx2) + my2;
                }
                else if (Math.Abs(y3 - y2) < eps)
                {
                    m1 = -(x2 - x1) / (y2 - y1);
                    mx1 = (x1 + x2) / 2;
                    my1 = (y1 + y2) / 2;
                    xc = (x3 + x2) / 2;
                    yc = m1 * (xc - mx1) + my1;
                }
                else
                {
                    m1 = -(x2 - x1) / (y2 - y1);
                    m2 = -(x3 - x2) / (y3 - y2);
                    if (Math.Abs(m1 - m2) < eps)
                    {
                        result = false;
                        xc = x1;
                        yc = y1;
                        r = big;
                    }
                    else
                    {
                        mx1 = (x1 + x2) / 2;
                        mx2 = (x2 + x3) / 2;
                        my1 = (y1 + y2) / 2;
                        my2 = (y2 + y3) / 2;
                        xc = (m1 * mx1 - m2 * mx2 + my2 - my1) / (m1 - m2);
                        yc = m1 * (xc - mx1) + my1;
                    }
                }
            }
            dx = x2 - xc;
            dy = y2 - yc;
            r = dx * dx + dy * dy;
            return result;
        }



        [CommandMethod("TRIANG")]
        public void TriangulateCommand()
        {
            const double maxpoints = 1000000;
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            TypedValue[] tvs = { new TypedValue(0, "POINT") };
            SelectionFilter sf = new SelectionFilter(tvs);
            PromptSelectionOptions pso = new PromptSelectionOptions();
            pso.MessageForAdding = "Select Points:";
            pso.AllowDuplicates = false;
            PromptSelectionResult psr = ed.GetSelection(pso, sf);
            if (psr.Status == PromptStatus.Error) return;
            if (psr.Status == PromptStatus.Cancel) return;
            SelectionSet ss = psr.Value;
            int npts = ss.Count;
            if (npts < 3)
            {
                ed.WriteMessage("Minimum 3 points must be selected!");
                return;
            }
            if (npts > maxpoints)
            {
                ed.WriteMessage("Maximum nuber of points exceeded!");
                return;
            }
            int i, j, k, ntri, ned, status1 = 0, status2 = 0;
            bool status;
            // Point coordinates

            double[] ptx = new double[Convert.ToInt64(maxpoints + 3)];
            double[] pty = new double[Convert.ToInt64(maxpoints + 3)];
            double[] ptz = new double[Convert.ToInt64(maxpoints + 3)];

            // Triangle definitions
            int[] pt1 = new int[Convert.ToInt64(maxpoints * 2 + 1)];
            int[] pt2 = new int[Convert.ToInt64(maxpoints * 2 + 1)];
            int[] pt3 = new int[Convert.ToInt64(maxpoints * 2 + 1)];
            // Circumscribed circle
            double[] cex = new double[Convert.ToInt64(maxpoints * 2 + 1)];
            double[] cey = new double[Convert.ToInt64(maxpoints * 2 + 1)];
            double[] rad = new double[Convert.ToInt64(maxpoints * 2 + 1)];
            double xmin, ymin, xmax, ymax, dx, dy, xmid, ymid;
            int[] ed1 = new int[Convert.ToInt64(maxpoints * 2 + 1)];
            int[] ed2 = new int[Convert.ToInt64(maxpoints * 2 + 1)];
            ObjectId[] idarray = ss.GetObjectIds();
            Transaction tr = db.TransactionManager.StartTransaction();
            using (tr)
            {
                DBPoint ent;
                k = 0;
                for (i = 0; i < npts; i++)
                {
                    ent = (DBPoint)tr.GetObject(idarray[k], OpenMode.ForRead, false);
                    ptx[i] = ent.Position[0];
                    pty[i] = ent.Position[1];
                    ptz[i] = ent.Position[2];
                    for (j = 0; j < i; j++)
                        if ((ptx[i] == ptx[j]) && (pty[i] == pty[j]))
                        {
                            i--;
                            npts--;
                            status2++;
                        }
                    k++;
                }
                tr.Commit();
            }
            if (status2 > 0)
                ed.WriteMessage("\nIgnored {0} point(s) with same coordinates.", status2);

            // Supertriangle
            xmin = ptx[0]; xmax = xmin;
            ymin = pty[0]; ymax = ymin;
            for (i = 0; i < npts; i++)
            {
                if (ptx[i] < xmin) xmin = ptx[i];
                if (ptx[i] > xmax) xmax = ptx[i];
                if (pty[i] < xmin) ymin = pty[i];
                if (pty[i] > xmin) ymax = pty[i];
            }
            dx = xmax - xmin; dy = ymax - ymin;
            xmid = (xmin + xmax) / 2; ymid = (ymin + ymax) / 2;
            i = npts;
            ptx[i] = xmid - (90 * (dx + dy)) - 100;
            pty[i] = ymid - (50 * (dx + dy)) - 100;
            ptz[i] = 0;
            pt1[0] = i;
            i++;
            ptx[i] = xmid + (90 * (dx + dy)) + 100;
            pty[i] = ymid - (50 * (dx + dy)) - 100;
            ptz[i] = 0;
            pt2[0] = i;
            i++;
            ptx[i] = xmid;
            pty[i] = ymid + 100 * (dx + dy + 1);
            ptz[i] = 0;
            pt3[0] = i;
            ntri = 1;
            circum(ptx[pt1[0]], pty[pt1[0]], ptx[pt2[0]], pty[pt2[0]], ptx[pt3[0]], pty[pt3[0]], ref cex[0], ref cey[0], ref rad[0]);

            // main loop
            for (i = 0; i < npts; i++)
            {
                ned = 0;
                xmin = ptx[i]; ymin = pty[i];
                j = 0;
                while (j < ntri)
                {
                    dx = cex[j] - xmin; dy = cey[j] - ymin;
                    if (((dx * dx) + (dy * dy)) < rad[j])
                    {
                        ed1[ned] = pt1[j]; ed2[ned] = pt2[j];
                        ned++;
                        ed1[ned] = pt2[j]; ed2[ned] = pt3[j];
                        ned++;
                        ed1[ned] = pt3[j]; ed2[ned] = pt1[j];
                        ned++;
                        ntri--;
                        pt1[j] = pt1[ntri];
                        pt2[j] = pt2[ntri];
                        pt3[j] = pt3[ntri];
                        cex[j] = cex[ntri];
                        cey[j] = cey[ntri];
                        rad[j] = rad[ntri];
                        j--;
                    }
                    j++;
                }
                for (j = 0; j < ned - 1; j++)
                    for (k = j + 1; k < ned; k++)
                    {
                        if ((ed1[j] == ed2[k]) && (ed2[j] == ed1[k]))
                        {
                            ed1[j] = -1; ed2[j] = -1; ed1[k] = -1; ed2[k] = -1;
                        }
                    }
                for (j = 0; j < ned; j++)
                {
                    if ((ed1[j] >= 0) && (ed2[j] >= 0))
                    {
                        pt1[ntri] = ed1[j]; pt2[ntri] = ed2[j]; pt3[ntri] = i;
                        status = circum(ptx[pt1[ntri]], pty[pt1[ntri]], ptx[pt2[ntri]], pty[pt2[ntri]], ptx[pt3[ntri]], pty[pt3[ntri]], ref cex[ntri], ref cey[ntri], ref rad[ntri]);
                        if (!status)
                        {
                            status1++;
                        }
                        ntri++;
                    }
                }
            }
            // removal of outer triangles
            i = 0;
            while (i < ntri)
            {
                if ((pt1[i] >= npts) || (pt2[i] >= npts) || (pt3[i] >= npts))
                {
                    ntri--;
                    pt1[i] = pt1[ntri];
                    pt2[i] = pt2[ntri];
                    pt3[i] = pt3[ntri];
                    cex[i] = cex[ntri];
                    cey[i] = cey[ntri];
                    rad[i] = rad[ntri];
                    i--;
                }
                i++;
            }
            tr = db.TransactionManager.StartTransaction();
            using (tr)
            {
                BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead, false);
                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false);
                PolyFaceMesh pfm = new PolyFaceMesh();
                btr.AppendEntity(pfm);
                tr.AddNewlyCreatedDBObject(pfm, true);
                for (i = 0; i < npts; i++)
                {
                    PolyFaceMeshVertex vert = new PolyFaceMeshVertex(new Point3d(ptx[i], pty[i], ptz[i]));
                    pfm.AppendVertex(vert);
                    tr.AddNewlyCreatedDBObject(vert, true);
                }
                for (i = 0; i < ntri; i++)
                {
                    FaceRecord face = new FaceRecord((short)(pt1[i] + 1), (short)(pt2[i] + 1), (short)(pt3[i] + 1), 0);
                    pfm.AppendFaceRecord(face);
                    tr.AddNewlyCreatedDBObject(face, true);
                    // add group
                    Group group = new Group();
                    DBDictionary dictionary = tr.GetObject(db.GroupDictionaryId, OpenMode.ForWrite) as DBDictionary;
                    dictionary.SetAt("123", group);
                    group.Append(face.ObjectId);
                    tr.AddNewlyCreatedDBObject(group, true);
                }
                tr.Commit();
            }
            if (status1 > 0)
            {
                ed.WriteMessage("\nWarning! {0} thin triangle(s) found!" + " Wrong result possible!", status1);

            }
            Application.UpdateScreen();

        }
        /// vẽ HATCH

        public class JigUtils
        {
            public static double Atan(double y, double x)
            {
                if (x > 0)
                    return Math.Atan(y / x);
                else if (x < 0)
                    return Math.Atan(y / x) - Math.PI;
                else  // x == 0
                {
                    if (y > 0)
                        return Math.PI;
                    else if (y < 0)
                        return -Math.PI;
                    else // if (y == 0) theta is undefined
                        return 0.0;
                }
            }
            // Computes Angle between current direction
            // (vector from last vertex to current vertex)
            // and the last pline segment
            public static double ComputeAngle(Point3d startPoint, Point3d endPoint, Vector3d xdir, Matrix3d ucs)
            {
                Vector3d v =
                new Vector3d((endPoint.X - startPoint.X) / 2, (endPoint.Y - startPoint.Y) / 2, (endPoint.Z - startPoint.Z) / 2);
                double cos = v.DotProduct(xdir);
                double sin = v.DotProduct(Vector3d.ZAxis.TransformBy(ucs).CrossProduct(xdir));
                return Atan(sin, cos);
            }

        }
    }


    public class HatchJig : DrawJig
    {
        Point3d _tempPoint;
        bool _isArcSeg = false;
        bool _isUndoing = false;
        Matrix3d _ucs;
        Plane _plane;
        Polyline _pline = null;
        Hatch _hat = null;
        public HatchJig(Matrix3d ucs, Plane plane, Polyline pl, Hatch hat)
        {
            _ucs = ucs;
            _plane = plane;
            _pline = pl;
            _hat = hat;
            AddDummyVertex();
        }
        protected override bool WorldDraw(Autodesk.AutoCAD.GraphicsInterface.WorldDraw wd)
        {
            // Update the dummy vertex to be our 3D point
            // projected onto our plane
            if (_isArcSeg)
            {
                Point3d lastVertex = _pline.GetPoint3dAt(_pline.NumberOfVertices - 2);
                Vector3d refDir;
                if (_pline.NumberOfVertices < 3)
                    refDir = new Vector3d(1.0, 1.0, 0.0);
                else
                {
                    // Check bulge to see if last segment was an arc or a line
                    if (_pline.GetBulgeAt(_pline.NumberOfVertices - 3) != 0)
                    {
                        CircularArc3d arcSegment = _pline.GetArcSegmentAt(_pline.NumberOfVertices - 3);
                        Line3d tangent = arcSegment.GetTangent(lastVertex);
                        // Reference direction is the invert of the arc tangent
                        // at last vertex
                        refDir = tangent.Direction.MultiplyBy(-1.0);
                    }
                    else
                    {
                        Point3d pt = _pline.GetPoint3dAt(_pline.NumberOfVertices - 3);
                        refDir = new Vector3d(lastVertex.X - pt.X, lastVertex.Y - pt.Y, lastVertex.Z - pt.Z);
                    }
                }
                double angle = Class1.JigUtils.ComputeAngle(lastVertex, _tempPoint, refDir, _ucs);
                // Bulge is defined as tan of one fourth of included angle
                // Need to double the angle since it represents the included
                // angle of the arc
                // So formula is: bulge = Tan(angle * 2 * 0.25)
                double bulge = Math.Tan(angle * 0.5);
                _pline.SetBulgeAt(_pline.NumberOfVertices - 2, bulge);
            }
            else
            {
                // Line mode. Need to remove last bulge if there was one
                if (_pline.NumberOfVertices > 1)
                    _pline.SetBulgeAt(_pline.NumberOfVertices - 2, 0);
            }
            _pline.SetPointAt(_pline.NumberOfVertices - 1, _tempPoint.Convert2d(_plane));

            if (_pline.NumberOfVertices == 3)
            {
                _pline.Closed = true;
                ObjectIdCollection ids = new ObjectIdCollection();
                ids.Add(_pline.ObjectId);
                // Add the hatch loops and complete the hatch
                _hat.Associative = true;
                _hat.AppendLoop(HatchLoopTypes.Default, ids);
            }
            if (!wd.RegenAbort)
            {
                wd.Geometry.Draw(_pline);
                if (_pline.NumberOfVertices > 2)
                {
                    _hat.EvaluateHatch(true);
                    if (!wd.RegenAbort)
                        wd.Geometry.Draw(_hat);
                }
            }
            return true;
        }
        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            JigPromptPointOptions jigOpts = new JigPromptPointOptions();
            jigOpts.UserInputControls = (UserInputControls.Accept3dCoordinates | UserInputControls.NullResponseAccepted | UserInputControls.NoNegativeResponseAccepted | UserInputControls.GovernedByOrthoMode);
            _isUndoing = false;
            if (_pline.NumberOfVertices == 1)
            {
                // For the first vertex, just ask for the point
                jigOpts.Message = "\nSpecify start point: ";
            }
            else if (_pline.NumberOfVertices > 1)
            {
                string msgAndKwds = (_isArcSeg ? "\nSpecify endpoint of arc or [Line/Undo]: " : "\nSpecify next point or [Arc/Undo]: ");
                string kwds = (_isArcSeg ? "Line Undo" : "Arc Undo");
                jigOpts.SetMessageAndKeywords(msgAndKwds, kwds);
            }
            else
                return SamplerStatus.Cancel; // Should never happen
            // Get the point itself
            PromptPointResult res = prompts.AcquirePoint(jigOpts);
            if (res.Status == PromptStatus.Keyword)
            {
                if (res.StringResult.ToUpper() == "ARC")
                    _isArcSeg = true;
                else if (res.StringResult.ToUpper() == "LINE")
                    _isArcSeg = false;
                else if (res.StringResult.ToUpper() == "UNDO")
                    _isUndoing = true;
                return SamplerStatus.OK;
            }
            else if (res.Status == PromptStatus.OK)
            {
                // Check if it has changed or not (reduces flicker)
                if (_tempPoint == res.Value)
                    return SamplerStatus.NoChange;
                else
                {
                    _tempPoint = res.Value;
                    return SamplerStatus.OK;
                }
            }
            return SamplerStatus.Cancel;
        }



        public bool IsUndoing
        {
            get
            {
                return _isUndoing;
            }

        }



        public void AddDummyVertex()
        {
            // Create a new dummy vertex... can have any initial value
            _pline.AddVertexAt(_pline.NumberOfVertices, new Point2d(0, 0), 0, 0, 0);
        }

        public void RemoveLastVertex()
        {
            // Let's first remove our dummy vertex   
            if (_pline.NumberOfVertices > 0)
                _pline.RemoveVertexAt(_pline.NumberOfVertices - 1);
            // And then check the type of the last segment
            if (_pline.NumberOfVertices >= 2)
            {
                double blg = _pline.GetBulgeAt(_pline.NumberOfVertices - 2);
                _isArcSeg = (blg != 0);
            }
        }

        [CommandMethod("HATJIG")]
        public static void RunHatchJig()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            // Create a transaction, as we're jigging
            // db-resident objects
            Transaction tr = db.TransactionManager.StartTransaction();
            using (tr)
            {
                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
                Vector3d normal = Vector3d.ZAxis.TransformBy(ed.CurrentUserCoordinateSystem);
                // We will pass a plane to our jig, to help
                // with UCS transformations
                Plane plane = new Plane(Point3d.Origin, normal);
                // We also pass a db-resident polyline
                Polyline pl = new Polyline();
                pl.Normal = normal;
                btr.AppendEntity(pl);
                tr.AddNewlyCreatedDBObject(pl, true);
                // And a db-resident hatch
                Hatch hat = new Hatch();
                // Use a non-solid hatch pattern, to aid jigging
                hat.SetHatchPattern(HatchPatternType.PreDefined, "ANGLE");
                // But let's make it transparent, for fun
                // Alpha value is Truncate(255 * (100-n)/100)
                hat.ColorIndex = 1;
                hat.Transparency = new Transparency(127);
                // Add the hatch to the modelspace & transaction
                ObjectId hatId = btr.AppendEntity(hat);
                tr.AddNewlyCreatedDBObject(hat, true);
                // And finally pass everything to the jig
                HatchJig jig = new HatchJig(ed.CurrentUserCoordinateSystem, plane, pl, hat);
                while (true)
                {
                    PromptResult res = ed.Drag(jig);
                    switch (res.Status)
                    {
                        // New point was added, keep going
                        case PromptStatus.OK:
                            jig.AddDummyVertex();
                            break;
                        // Keyword was entered
                        case PromptStatus.Keyword:
                            if (jig.IsUndoing)
                                jig.RemoveLastVertex();
                            break;
                        // The jig completed successfully
                        case PromptStatus.None:
                            // You can remove this next line if you want
                            // the vertex being jigged to be included
                            jig.RemoveLastVertex();
                            tr.Commit();
                            return;
                        // User cancelled the command
                        default:
                            // No need to erase the polyline & hatch, as
                            // the transaction will simply be aborted
                            return;
                    }
                }
            }
        }

    }

    
}


