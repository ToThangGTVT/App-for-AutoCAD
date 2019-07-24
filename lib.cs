using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;

namespace TQCAD
{
    class lib
    {
        public static Arc AddArc(Point3d _center, double _radius, double _start, double _end, string _lyr)
        {
            Document mdiActiveDocument = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;
            Database database = mdiActiveDocument.Database;
            Arc arc = new Arc();
            using (mdiActiveDocument.LockDocument())
            {
                using (Transaction transaction = database.TransactionManager.StartTransaction())
                {
                    BlockTable blockTable = transaction.GetObject(database.BlockTableId, OpenMode.ForRead) as BlockTable;
                    BlockTableRecord blockTableRecord = transaction.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                    arc = new Arc(_center, _radius, _end, _start);
                    arc.Layer = _lyr;
                    arc.SetDatabaseDefaults();
                    blockTableRecord.AppendEntity(arc);
                    transaction.AddNewlyCreatedDBObject(arc, true);
                    transaction.Commit();
                }
            }
            return arc;
        }

        public static Circle AddCircle(Point3d _center, double _radius, string _lyr)
        {
            Document mdiActiveDocument = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;
            Database database = mdiActiveDocument.Database;
            Editor editor = mdiActiveDocument.Editor;
            Circle circle = new Circle();
            using (mdiActiveDocument.LockDocument())
            {
                using (Transaction transaction = database.TransactionManager.StartTransaction())
                {
                    try
                    {
                        BlockTable blockTable = transaction.GetObject(database.BlockTableId, OpenMode.ForRead) as BlockTable;
                        BlockTableRecord blockTableRecord = transaction.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                        circle.Center = _center;
                        circle.Radius = _radius;
                        circle.Layer = _lyr;
                        blockTableRecord.AppendEntity(circle);
                        transaction.AddNewlyCreatedDBObject(circle, true);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        editor.WriteMessage(ex.Message + ex.StackTrace);
                    }
                }
            }
            return circle;
        }

        public static Line AddLine(Point3d p1, Point3d p2, string lyr, short color, double scale)
        {
            Document mdiActiveDocument = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;
            Line line = new Line();
            Database database = mdiActiveDocument.Database;
            Editor editor = mdiActiveDocument.Editor;
            Transaction transaction = database.TransactionManager.StartTransaction();
            using (mdiActiveDocument.LockDocument())
            {
                using (transaction)
                {
                    try
                    {
                        BlockTable blockTable = transaction.GetObject(database.BlockTableId, OpenMode.ForRead) as BlockTable;
                        BlockTableRecord blockTableRecord = transaction.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                        line.StartPoint = p1;
                        line.EndPoint = p2;
                        line.Color = Color.FromColorIndex(ColorMethod.ByAci, color);
                        if (scale != 0.0)
                        {
                            line.LinetypeScale = scale;
                        }
                        LayerTable layerTable = (LayerTable)transaction.GetObject(database.LayerTableId, OpenMode.ForRead, true, true);
                        if (layerTable.Has(lyr))
                        {
                            line.Layer = lyr;
                        }
                        blockTableRecord.AppendEntity(line);
                        transaction.AddNewlyCreatedDBObject(line, true);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        editor.WriteMessage(ex.Message + ex.StackTrace);
                    }
                }
            }
            return line;
        }

        public static Spline AddSPLine(Point3dCollection pts, string _lyr)
        {
            Document mdiActiveDocument = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;
            Database database = mdiActiveDocument.Database;
            Editor editor = mdiActiveDocument.Editor;
            Spline spline = new Spline();
            using (mdiActiveDocument.LockDocument())
            {
                using (Transaction transaction = database.TransactionManager.StartTransaction())
                {
                    try
                    {
                        BlockTable blockTable = transaction.GetObject(database.BlockTableId, OpenMode.ForRead) as BlockTable;
                        BlockTableRecord blockTableRecord = transaction.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                        Vector3d asVector = new Point3d(0.0, 0.0, 0.0).GetAsVector();
                        spline = new Spline(pts, asVector, asVector, 4, 0.0);
                        spline.Layer = _lyr;
                        spline.SetDatabaseDefaults();
                        blockTableRecord.AppendEntity(spline);
                        transaction.AddNewlyCreatedDBObject(spline, true);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        editor.WriteMessage(ex.Message + ex.StackTrace);
                    }
                }
            }
            return spline;
        }

        public static DBText AddText(Point3d insPoint, string content, string vJustify, string hJustify, double height, double rotate, string layer, string textStyle, short color)
        {
            Document mdiActiveDocument = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;
            Database database = mdiActiveDocument.Database;
            DBText dbtext = new DBText();
            using (mdiActiveDocument.LockDocument())
            {
                using (Transaction transaction = database.TransactionManager.StartTransaction())
                {
                    BlockTable blockTable = transaction.GetObject(database.BlockTableId, OpenMode.ForRead) as BlockTable;
                    BlockTableRecord blockTableRecord = transaction.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                    dbtext.TextString = content;
                    dbtext.Height = height;
                    dbtext.Rotation = rotate;
                    if (vJustify != null)
                    {
                        if (vJustify == "Top")
                        {
                            dbtext.VerticalMode = TextVerticalMode.TextTop;
                            dbtext.AlignmentPoint = insPoint;
                            goto IL_128;
                        }
                        if (vJustify == "Mid")
                        {
                            dbtext.VerticalMode = TextVerticalMode.TextVerticalMid;
                            dbtext.AlignmentPoint = insPoint;
                            goto IL_128;
                        }
                        if (vJustify == "Bottom")
                        {
                            dbtext.VerticalMode = TextVerticalMode.TextBottom;
                            dbtext.AlignmentPoint = insPoint;
                            goto IL_128;
                        }
                    }
                    dbtext.VerticalMode = TextVerticalMode.TextBase;
                IL_128:
                    if (hJustify != null)
                    {
                        if (hJustify == "Right")
                        {
                            dbtext.HorizontalMode = TextHorizontalMode.TextRight;
                            dbtext.AlignmentPoint = insPoint;
                            goto IL_19D;
                        }
                        if (hJustify == "Center")
                        {
                            dbtext.HorizontalMode = TextHorizontalMode.TextCenter;
                            dbtext.AlignmentPoint = insPoint;
                            goto IL_19D;
                        }
                    }
                    dbtext.HorizontalMode = TextHorizontalMode.TextLeft;
                IL_19D:
                    LayerTable layerTable = transaction.GetObject(database.LayerTableId, OpenMode.ForRead) as LayerTable;
                    if (layerTable.Has(layer))
                    {
                        dbtext.Layer = layer;
                    }
                    TextStyleTable textStyleTable = transaction.GetObject(database.TextStyleTableId, OpenMode.ForRead) as TextStyleTable;
                    if (textStyleTable.Has(textStyle))
                    {
                        dbtext.TextStyleId = textStyleTable[textStyle];
                    }
                    dbtext.Position = insPoint;
                    dbtext.Color = Color.FromColorIndex(ColorMethod.ByAci, color);
                    dbtext.WidthFactor = 1.0;
                    dbtext.SetDatabaseDefaults();
                    blockTableRecord.AppendEntity(dbtext);
                    transaction.AddNewlyCreatedDBObject(dbtext, true);
                    transaction.Commit();
                }
            }
            return dbtext;
        }

        public static ObjectId AlignDim(Point3d line_pt1, Point3d line_pt2, Point3d dim_pt, string text, ObjectId dimstyleid)
        {
            Document mdiActiveDocument = Application.DocumentManager.MdiActiveDocument;
            Database database = mdiActiveDocument.Database;
            ObjectId result;
            using (mdiActiveDocument.LockDocument())
            {
                using (Transaction transaction = database.TransactionManager.StartTransaction())
                {
                    BlockTable blockTable = transaction.GetObject(database.BlockTableId, OpenMode.ForRead) as BlockTable;
                    BlockTableRecord blockTableRecord = transaction.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                    AlignedDimension alignedDimension = new AlignedDimension(line_pt1, line_pt2, dim_pt, text, dimstyleid);
                    result = blockTableRecord.AppendEntity(alignedDimension);
                    transaction.AddNewlyCreatedDBObject(alignedDimension, true);
                    transaction.Commit();
                }
            }
            return result;
        }

        public static Entity Copy(Entity _ent, Vector3d _v)
        {
            Document mdiActiveDocument = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;
            Database database = mdiActiveDocument.Database;
            Entity entity = null;
            using (mdiActiveDocument.LockDocument())
            {
                using (Transaction transaction = database.TransactionManager.StartTransaction())
                {
                    BlockTable blockTable = transaction.GetObject(database.BlockTableId, OpenMode.ForRead) as BlockTable;
                    BlockTableRecord blockTableRecord = transaction.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                    entity = (_ent.Clone() as Entity);
                    entity.TransformBy(Matrix3d.Displacement(_v));
                    blockTableRecord.AppendEntity(entity);
                    transaction.AddNewlyCreatedDBObject(entity, true);
                    transaction.Commit();
                }
            }
            return entity;
        }

        public static void CreateBlock(string _name, DBObjectCollection ents)
        {
            Document mdiActiveDocument = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;
            Database database = mdiActiveDocument.Database;
            using (mdiActiveDocument.LockDocument())
            {
                using (Transaction transaction = database.TransactionManager.StartTransaction())
                {
                    BlockTable blockTable = (BlockTable)transaction.GetObject(database.BlockTableId, OpenMode.ForRead);
                    BlockTableRecord blockTableRecord = new BlockTableRecord();
                    if (blockTable.Has(_name))
                    {
                        blockTableRecord = (BlockTableRecord)transaction.GetObject(blockTable[_name], OpenMode.ForWrite, false, true);
                        foreach (ObjectId id in blockTableRecord)
                        {
                            Entity entity = (Entity)transaction.GetObject(id, OpenMode.ForWrite);
                            entity.Erase();
                        }
                        ObjectId objectId = blockTableRecord.ObjectId;
                    }
                    else
                    {
                        blockTableRecord.Name = _name;
                        blockTable.UpgradeOpen();
                        ObjectId objectId = blockTable.Add(blockTableRecord);
                        transaction.AddNewlyCreatedDBObject(blockTableRecord, true);
                    }
                    foreach (object obj2 in ents)
                    {
                        Entity entity = (Entity)obj2;
                        blockTableRecord.AppendEntity(entity);
                        transaction.AddNewlyCreatedDBObject(entity, true);
                    }
                    blockTableRecord.Units = UnitsValue.Millimeters;
                    transaction.Commit();
                }
            }
        }

        public static ObjectId CreateDimStyle(string _name, ObjectId _text, ObjectId _arrow)
        {

            Document mdiActiveDocument = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;
            Database database = mdiActiveDocument.Database;
            Editor editor = mdiActiveDocument.Editor;
            ObjectId result;
            using (mdiActiveDocument.LockDocument())
            {
                using (Transaction transaction = database.TransactionManager.StartTransaction())
                {
                    DimStyleTable dimStyleTable = transaction.GetObject(database.DimStyleTableId, OpenMode.ForWrite) as DimStyleTable;
                    if (!dimStyleTable.Has(_name))
                    {
                        DimStyleTableRecord dimStyleTableRecord = new DimStyleTableRecord();
                        dimStyleTableRecord.Name = _name;
                        dimStyleTableRecord.Dimadec = 2;
                        dimStyleTableRecord.Dimalt = false;
                        dimStyleTableRecord.Dimaltd = 0;
                        dimStyleTableRecord.Dimaltf = 25.4;
                        dimStyleTableRecord.Dimaltrnd = 0.0;
                        dimStyleTableRecord.Dimalttd = 0;
                        dimStyleTableRecord.Dimalttz = 0;
                        dimStyleTableRecord.Dimaltu = 2;
                        dimStyleTableRecord.Dimaltz = 0;
                        dimStyleTableRecord.Dimapost = "";
                        dimStyleTableRecord.Dimarcsym = 0;
                        dimStyleTableRecord.Dimatfit = 3;
                        dimStyleTableRecord.Dimaunit = 1;
                        dimStyleTableRecord.Dimazin = 0;
                        dimStyleTableRecord.Dimblk = _arrow;
                        dimStyleTableRecord.Dimblk1 = _arrow;
                        dimStyleTableRecord.Dimblk2 = _arrow;
                        dimStyleTableRecord.Dimcen = 2.0;
                        dimStyleTableRecord.Dimclrd = Color.FromColorIndex(ColorMethod.ByBlock, 8);
                        dimStyleTableRecord.Dimclre = Color.FromColorIndex(ColorMethod.ByBlock, 8);
                        dimStyleTableRecord.Dimclrt = Color.FromColorIndex(ColorMethod.ByBlock, 4);
                        dimStyleTableRecord.Dimdec = 0;
                        dimStyleTableRecord.Dimdle = 0.0;
                        dimStyleTableRecord.Dimdli = 7.0;
                        dimStyleTableRecord.Dimdsep = char.Parse("Null");
                        dimStyleTableRecord.Dimexe = 0.1;
                        dimStyleTableRecord.Dimexo = 0.1;
                        dimStyleTableRecord.Dimfrac = 1;
                        dimStyleTableRecord.Dimgap = 0.5;
                        dimStyleTableRecord.Dimldrblk = _arrow;
                        dimStyleTableRecord.Dimlfac = 1.0;
                        dimStyleTableRecord.Dimlim = false;
                        dimStyleTableRecord.Dimltex1 = database.ByBlockLinetype;
                        dimStyleTableRecord.Dimltex2 = database.ByBlockLinetype;
                        dimStyleTableRecord.Dimltype = database.ByBlockLinetype;
                        dimStyleTableRecord.Dimlunit = 2;
                        dimStyleTableRecord.Dimlwd = LineWeight.ByBlock;
                        dimStyleTableRecord.Dimlwe = LineWeight.ByBlock;
                        dimStyleTableRecord.Dimpost = "";
                        dimStyleTableRecord.Dimrnd = 0.5;
                        dimStyleTableRecord.Dimsah = true;
                        dimStyleTableRecord.Dimscale = 0.2;
                        dimStyleTableRecord.Dimsd1 = false;
                        dimStyleTableRecord.Dimsd2 = false;
                        dimStyleTableRecord.Dimse1 = false;
                        dimStyleTableRecord.Dimse2 = false;
                        dimStyleTableRecord.Dimsoxd = false;
                        dimStyleTableRecord.Dimtad = 1;
                        dimStyleTableRecord.Dimtdec = 1;
                        dimStyleTableRecord.Dimtfac = 1.0;
                        dimStyleTableRecord.Dimtih = false;
                        dimStyleTableRecord.Dimtix = true;
                        dimStyleTableRecord.Dimtm = 0.0;
                        dimStyleTableRecord.Dimtmove = 2;
                        dimStyleTableRecord.Dimtofl = true;
                        dimStyleTableRecord.Dimtoh = false;
                        dimStyleTableRecord.Dimtol = false;
                        dimStyleTableRecord.Dimtolj = 0;
                        dimStyleTableRecord.Dimtp = 0.0;
                        dimStyleTableRecord.Dimtsz = 0.0;
                        dimStyleTableRecord.Dimtxsty = _text;
                        dimStyleTableRecord.Dimtvp = 0.0;
                        dimStyleTableRecord.Dimtxt = 1.7;
                        dimStyleTableRecord.Dimtzin = 8;
                        dimStyleTableRecord.Dimupt = false;
                        dimStyleTableRecord.Dimzin = 8;
                        dimStyleTableRecord.DimfxlenOn = true;
                        dimStyleTableRecord.Dimfxlen = 40.0;
                        dimStyleTableRecord.Dimtxt = 0.5;
                        dimStyleTableRecord.Dimasz = 0.2;
                        result = dimStyleTable.Add(dimStyleTableRecord);
                        transaction.AddNewlyCreatedDBObject(dimStyleTableRecord, true);
                    }
                    else
                    {
                        result = dimStyleTable[_name];
                    }
                    transaction.Commit();
                }
            }
            return result;
        }

        public static void CreateRadialDimension(Point3d _center, ObjectId _dimstyle, string _text)
        {
            Document mdiActiveDocument = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;
            Database database = mdiActiveDocument.Database;
            using (mdiActiveDocument.LockDocument())
            {
                using (Transaction transaction = database.TransactionManager.StartTransaction())
                {
                    BlockTable blockTable = transaction.GetObject(database.BlockTableId, OpenMode.ForRead) as BlockTable;
                    BlockTableRecord blockTableRecord = transaction.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                    RadialDimension radialDimension = new RadialDimension();
                    radialDimension.SetDatabaseDefaults();
                    radialDimension.Center = _center;
                    radialDimension.ChordPoint = new Point3d(_center.X + 0.1, _center.Y + 0.1, 0.0);
                    radialDimension.LeaderLength = 0.5;
                    radialDimension.DimensionStyle = _dimstyle;
                    radialDimension.DimensionText = _text;
                    blockTableRecord.AppendEntity(radialDimension);
                    transaction.AddNewlyCreatedDBObject(radialDimension, true);
                    transaction.Commit();
                }
            }
        }

    }
}
