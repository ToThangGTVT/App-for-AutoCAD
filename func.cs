using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TQCAD
{
    public class func
    {
        [CommandMethod("AddHatch")]
        public void AHA()
        {
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                BlockTable acBlkTbl;
                acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead) as BlockTable;

                BlockTableRecord acBlkTblRec;
                acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                using (Polyline polyline = new Polyline())
                {
                    polyline.AddVertexAt(0, new Point2d(0, 0), 0, 0, 0);
                    polyline.AddVertexAt(0, new Point2d(2, 0), 0, 0, 0);
                    polyline.AddVertexAt(0, new Point2d(2, 1), 0, 0, 0);
                    polyline.AddVertexAt(0, new Point2d(0, 1), 0, 0, 0);
                    polyline.AddVertexAt(0, new Point2d(0, 0), 0, 0, 0);

                    // Add the new circle object to the block table record and the transaction
                    acBlkTblRec.AppendEntity(polyline);
                    acTrans.AddNewlyCreatedDBObject(polyline, true);

                    // Adds the circle to an object id array
                    ObjectIdCollection acObjIdColl = new ObjectIdCollection();
                    acObjIdColl.Add(polyline.ObjectId);

                    // Create the hatch object and append it to the block table record
                    using (Hatch acHatch = new Hatch())
                    {
                        acBlkTblRec.AppendEntity(acHatch);
                        acTrans.AddNewlyCreatedDBObject(acHatch, true);

                        // Set the properties of the hatch object
                        // Associative must be set after the hatch object is appended to the 
                        // block table record and before AppendLoop
                        acHatch.SetHatchPattern(HatchPatternType.PreDefined, "ANSI31");
                        acHatch.Associative = true;
                        acHatch.AppendLoop(HatchLoopTypes.Outermost, acObjIdColl);
                        acHatch.EvaluateHatch(true);
                    }
                }

                // Save the new object to the database
                acTrans.Commit();
            }
        }

        [CommandMethod("HCN")]
        internal void HCN()
        {
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {

                LayerTable layerTable;
                layerTable = acTrans.GetObject(acCurDb.LayerTableId,
                                                OpenMode.ForRead) as LayerTable;

                string sLayerName = "Center";

                if (layerTable.Has(sLayerName) == false)
                {
                    using (LayerTableRecord acLyrTblRec = new LayerTableRecord())
                    {
                        // Assign the layer the ACI color 3 and a name
                        acLyrTblRec.Color = Color.FromColorIndex(ColorMethod.ByAci, 3);
                        acLyrTblRec.Name = sLayerName;
                        acDoc.LockDocument();
                        // Upgrade the Layer table for write
                        layerTable.UpgradeOpen();

                        // Append the new layer to the Layer table and the transaction
                        layerTable.Add(acLyrTblRec);
                        acTrans.AddNewlyCreatedDBObject(acLyrTblRec, true);
                    }
                }

                BlockTable acBlkTbl;
                acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead) as BlockTable;

                BlockTableRecord acBlkTblRec;
                acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                using (Polyline polyline = new Polyline())
                {
                    polyline.AddVertexAt(0, new Point2d(0, 0), 0, 0, 0);
                    polyline.AddVertexAt(1, new Point2d(5, 3), 0, 0, 0);
                    polyline.AddVertexAt(2, new Point2d(9, 1), 0, 0, 0);


                    polyline.Layer = sLayerName;

                    acBlkTblRec.AppendEntity(polyline);
                    acTrans.AddNewlyCreatedDBObject(polyline, true);


                }
                acTrans.Commit();
            }
        }


        [CommandMethod("EDITHCN")]
        public void EditHCN()
        {
            const double pi = 3.14159265359;

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor acDocEd = Application.DocumentManager.MdiActiveDocument.Editor;
            TypedValue[] acTypValAr = new TypedValue[1];
            acTypValAr.SetValue(new TypedValue((int)DxfCode.LayerName, "Center"), 0); //lọc ra để lấy layer có tên là "Center"
            SelectionFilter acSelFtr = new SelectionFilter(acTypValAr);
            PromptSelectionResult acSSPrompt;
            acSSPrompt = acDocEd.GetSelection(acSelFtr);

            if (acSSPrompt.Status == PromptStatus.OK)
            {
                SelectionSet acSSet = acSSPrompt.Value;
                int npts = acSSet.Count;

                ObjectId[] idarray = acSSet.GetObjectIds();

                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    Polyline ent;
                    int k = 0;
                    int R = 3;
                    double a, b, c, goc, goc1, goc2;
                    double m = 1;
                    for (int i = 0; i < npts; i++)
                    {
                        ent = tr.GetObject(idarray[k], OpenMode.ForWrite, true) as Polyline;
                        Point2d p0 = ent.GetPoint2dAt(0);
                        Point2d p1 = ent.GetPoint2dAt(1);
                        Point2d p2 = ent.GetPoint2dAt(2);

                        double x0 = p0.X;
                        double y0 = p0.Y;
                        double x1 = p1.X;
                        double y1 = p1.Y;
                        double x2 = p2.X;
                        double y2 = p2.Y;

                        b = Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
                        a = Math.Sqrt((x1 - x0) * (x1 - x0) + (y1 - y0) * (y1 - y0));
                        c = Math.Sqrt((x2 - x0) * (x2 - x0) + (y2 - y0) * (y2 - y0));
                        goc = Math.Acos((a * a + b * b - c * c) / (2 * a * b));

                        m = Math.Abs(R / Math.Tan(goc / 2));
                        Point3d point3D = new Point3d(p1.X, p1.Y, 0);

                        BlockTable blockTable;
                        blockTable = tr.GetObject(db.BlockTableId, OpenMode.ForWrite) as BlockTable;
                        BlockTableRecord blockTableRecord;
                        blockTableRecord = tr.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                        Circle circle = new Circle();
                        circle.Center = point3D;
                        circle.Radius = m;
                        blockTableRecord.AppendEntity(circle);
                        tr.AddNewlyCreatedDBObject(circle, true);

                        var points = new Point3dCollection();
                        ent.IntersectWith(circle, Intersect.OnBothOperands, points, new IntPtr(), new IntPtr());

                        Circle c1 = new Circle();
                        c1.Radius = R;
                        c1.Center = points[1];
                        blockTableRecord.AppendEntity(c1);
                        tr.AddNewlyCreatedDBObject(c1, true);

                        Circle c2 = new Circle();
                        c2.Radius = R;
                        c2.Center = points[0];
                        blockTableRecord.AppendEntity(c2);
                        tr.AddNewlyCreatedDBObject(c2, true);

                        var points2 = new Point3dCollection();
                        c1.IntersectWith(c2, Intersect.OnBothOperands, points2, new IntPtr(), new IntPtr());

                        for (i = 0; i < 2; i++)
                        {
                            if (goc_diem_p1(c1.Center, p1, points2[i]) == true)
                            {
                                x0 = points2[i].X + 1;
                                y0 = points2[i].Y;
                                x1 = points2[i].X;
                                y1 = points2[i].Y;
                                x2 = c1.Center.X;
                                y2 = c1.Center.Y;

                                if (y2 < y1)
                                {
                                    goc1 = Math.Acos(((x0 - x1) * (x2 - x1) + (y0 - y1) * (y2 - y1)) / (Math.Sqrt((x0 - x1) * (x0 - x1) + (y0 - y1) * (y0 - y1)) * Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1))));
                                    goc1 = 2 * pi - goc1;
                                }
                                else
                                {
                                    goc1 = Math.Acos(((x0 - x1) * (x2 - x1) + (y0 - y1) * (y2 - y1)) / (Math.Sqrt((x0 - x1) * (x0 - x1) + (y0 - y1) * (y0 - y1)) * Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1))));

                                }

                                x1 = points2[i].X;
                                y1 = points2[i].Y;
                                x2 = c2.Center.X;
                                y2 = c2.Center.Y;

                                if ((y2 < y1))
                                {
                                    goc2 = Math.Acos(((x0 - x1) * (x2 - x1) + (y0 - y1) * (y2 - y1)) / (Math.Sqrt((x0 - x1) * (x0 - x1) + (y0 - y1) * (y0 - y1)) * Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1))));
                                    goc2 = pi + pi - goc2;
                                }
                                else
                                {
                                    goc2 = Math.Acos(((x0 - x1) * (x2 - x1) + (y0 - y1) * (y2 - y1)) / (Math.Sqrt((x0 - x1) * (x0 - x1) + (y0 - y1) * (y0 - y1)) * Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1))));
                                }

                                Application.ShowAlertDialog((goc1 * 180 / pi).ToString());
                                Application.ShowAlertDialog((goc2 * 180 / pi).ToString());
                                //Application.ShowAlertDialog((goc2*180/3.14156).ToString());
                                if (Math.Abs(goc1 - goc2) > pi)
                                {
                                    Arc arc = new Arc(points2[i], R, goc2, goc1);
                                    blockTableRecord.AppendEntity(arc);
                                    tr.AddNewlyCreatedDBObject(arc, true);
                                }
                                else
                                {
                                    if (goc1 < goc2)
                                    {
                                        Arc arc = new Arc(points2[i], R, goc1, goc2);
                                        blockTableRecord.AppendEntity(arc);
                                        tr.AddNewlyCreatedDBObject(arc, true);
                                    }
                                    else
                                    {
                                        Arc arc = new Arc(points2[i], R, goc2, goc1);
                                        blockTableRecord.AppendEntity(arc);
                                        tr.AddNewlyCreatedDBObject(arc, true);
                                    }
                                }


                                doc.Editor.Regen();
                            }
                        }
                        c1.Erase(true);
                        c2.Erase(true);
                        circle.Erase(true);

                    }
                    tr.Commit();
                }
            }
            else
            {
                Application.ShowAlertDialog("Number of objects selected: 0");
            }
        }

        static bool goc_diem_p1(Point3d p0, Point2d p1, Point3d p2)
        {
            double a, b, c, goc;
            double x0 = p0.X;
            double y0 = p0.Y;
            double x1 = p1.X;
            double y1 = p1.Y;
            double x2 = p2.X;
            double y2 = p2.Y;

            c = Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
            a = Math.Sqrt((x1 - x0) * (x1 - x0) + (y1 - y0) * (y1 - y0));
            b = Math.Sqrt((x2 - x0) * (x2 - x0) + (y2 - y0) * (y2 - y0));
            goc = Math.Acos((a * a + b * b - c * c) / (2 * a * b));
            const double pi = 3.14159265359;
            if (Math.Abs(goc * 180 / pi - 90) < 0.001)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [CommandMethod("TOTLEN", CommandFlags.UsePickSet)]
        public void TotalLength()
        {
            
        }


    }
}
