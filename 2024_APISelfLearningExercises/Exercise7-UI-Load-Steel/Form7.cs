using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using Tekla.Structures;
using Tekla.Structures.Catalogs;
using Tekla.Structures.Dialog;
using Tekla.Structures.Dialog.UIControls;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using TSD = Tekla.Structures.Datatype;

namespace Exercise
{
    public partial class Form7 : ApplicationFormBase
    {
        public Form7()
        {
            InitializeComponent();
            base.InitializeForm();
            SetAttributeValue(FootingSize, "1500");
            SetAttributeValue(ColumnsProfileTextBox, "HEA300");
            SetAttributeValue(SizeTextBox, "12");
            SetAttributeValue(GradeTextBox, "A500HW");
            SetAttributeValue(BendingRadiusTextBox, new TSD.Distance(144.0));
            SetAttributeValue(ColumnsMaterialTextBox, "S235JR");
            MyModel = new Model();

            SteelMaterials = new List<MaterialItem>();
        }

        private readonly Model MyModel;
        private readonly List<MaterialItem> SteelMaterials;

        /// <summary>
        /// Callback function to create objects on corrent places.
        /// Two loops are used to go through all positions.
        /// </summary>
        private void CreatePadFootings(object sender, EventArgs e)
        {
            // Always remember to check that you really have working connection
            if (MyModel.GetConnectionStatus())
            {
                // Loop through X-axis  (these loops should be changed to match current grid)
                for (double PositionX = 0.0; PositionX <= 12000.0; PositionX += 3000.0)
                {
                    // In first and in last line
                    if (PositionX.Equals(0.0) || PositionX.Equals(12000.0))
                    {
                        // Loop through Y-axis to get pad footings on the longer sides of the grid
                        for (double PositionY = 0.0; PositionY <= 30000.0; PositionY += 6000.0)
                        {
                            CreateFootingAndColumn(PositionX, PositionY);
                        }
                    }
                    else
                    {
                        CreateFootingAndColumn(PositionX, 0.0);
                        CreateFootingAndColumn(PositionX, 30000.0);
                    }
                }
                // Always remember to commit changes to Tekla Structures, otherwise some things might be left in uncertain state
                MyModel.CommitChanges();
            }
        }

        /// <summary>
        /// Loop through all footings (Name == "FOOTING") and create rebar on those.
        /// </summary>
        private void CreateRebars()
        {
            if (MyModel.GetConnectionStatus())
            {
                ModelObjectEnumerator ObjectsInModel = MyModel.GetModelObjectSelector().GetAllObjects();

                while (ObjectsInModel.MoveNext())
                {
                    Beam MyBeam = ObjectsInModel.Current as Beam;

                    if (MyBeam != null && MyBeam.Name == "FOOTING") //use same name as given in exercise 1
                    {
                        //Here is included also method for checking if pad footing already has rebar
                        //This still adds the new rebar to it, but could also skip the creation of new rebar
                        ModelObjectEnumerator BeamChildren = MyBeam.GetChildren();
                        bool HasRebars = false;

                        while (BeamChildren.MoveNext())
                        {
                            if (BeamChildren.Current is Reinforcement)
                            {
                                HasRebars = true;
                            }
                        }

                        if (HasRebars)
                        {
                            CreateRebar(MyBeam, MyBeam.StartPoint.X, MyBeam.StartPoint.Y);
                        }
                        else
                        {
                            CreateRebar(MyBeam, MyBeam.StartPoint.X, MyBeam.StartPoint.Y);
                        }
                    }
                }
                MyModel.CommitChanges();
            }
        }

        /// <summary>
        /// Create rebar on the given object to given position using FootingSize from dialog.
        /// Polygon for the rebar is calculated from position and FootingSize from dialog, Polygon could also
        /// be calculated using PadFooting's extrema.
        /// </summary>
        /// <param name="PadFooting">Father of the rebar</param>
        /// <param name="PositionX">X-coordination</param>
        /// <param name="PositionY"></param>
        /// <returns></returns>
        private void CreateRebar(ModelObject PadFooting, double PositionX, double PositionY)
        {
            RebarGroup Rebar = new RebarGroup();
            Polygon RebarPolygon1 = new Polygon();

            double MyFootingSize = double.Parse(FootingSize.Text);

            //use given position and footing size
            RebarPolygon1.Points.Add(new Point(PositionX - MyFootingSize / 2.0, PositionY - MyFootingSize / 2.0, 0));
            RebarPolygon1.Points.Add(new Point(PositionX + MyFootingSize / 2.0, PositionY - MyFootingSize / 2.0, 0));
            RebarPolygon1.Points.Add(new Point(PositionX + MyFootingSize / 2.0, PositionY + MyFootingSize / 2.0, 0));
            RebarPolygon1.Points.Add(new Point(PositionX - MyFootingSize / 2.0, PositionY + MyFootingSize / 2.0, 0));
            RebarPolygon1.Points.Add(new Point(PositionX - MyFootingSize / 2.0, PositionY - MyFootingSize / 2.0, 0));
            Rebar.Polygons.Add(RebarPolygon1);

            //or calculate by rebar's solid's Axis Aligned Bounding Box
            //Rebar.Polygons.Add(GetPolygonBySolidsAABB(PadFooting as Beam));

            Rebar.StartPoint.X = PositionX;
            Rebar.StartPoint.Y = PositionY;
            Rebar.StartPoint.Z = 0.0;
            Rebar.EndPoint.X = PositionX;
            Rebar.EndPoint.Y = PositionY;
            Rebar.EndPoint.Z = -500.0;
            Rebar.Father = PadFooting;
            Rebar.EndPointOffsetType = Reinforcement.RebarOffsetTypeEnum.OFFSET_TYPE_COVER_THICKNESS;
            Rebar.EndPointOffsetValue = 20.0;
            Rebar.StartPointOffsetType = Reinforcement.RebarOffsetTypeEnum.OFFSET_TYPE_COVER_THICKNESS;
            Rebar.StartPointOffsetValue = 20.0;
            Rebar.Class = 3;
            Rebar.Name = "FootingRebar";
            Rebar.Grade = GradeTextBox.Text;
            Rebar.Size = SizeTextBox.Text;

            char[] Separator = { ' ' };
            string[] Radiuses = BendingRadiusTextBox.Text.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
            foreach (string Item in Radiuses)
                Rebar.RadiusValues.Add(Convert.ToDouble(Item));

            Rebar.SpacingType = BaseRebarGroup.RebarGroupSpacingTypeEnum.SPACING_TYPE_TARGET_SPACE;
            Rebar.Spacings.Add(100.0);
            Rebar.ExcludeType = BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_BOTH;
            Rebar.NumberingSeries.StartNumber = 0;
            Rebar.NumberingSeries.Prefix = "Group";
            Rebar.OnPlaneOffsets.Add(25.0);
            Rebar.FromPlaneOffset = 40;

            //Adding hooks to make rebar stronger
            Rebar.StartHook.Shape = RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;
            Rebar.EndHook.Shape = RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;
            Rebar.OnPlaneOffsets.Add(10.0);
            Rebar.OnPlaneOffsets.Add(25.0);

            if (!Rebar.Insert())
            {
                Console.WriteLine("Inserting of rebar failed.");
            }
        }


        /// <summary>
        /// Calculate polygon for rebar group by solid of InputObject.
        /// Object's Axis Aligned Bounding Box (AABB) is used for calculation.
        /// </summary>
        /// <param name="InputObject"></param>
        /// <returns></returns>
        private Polygon GetPolygonBySolidsAABB(Part InputObject)
        {
            Polygon Result = new Polygon();

            Solid BeamSolid = InputObject.GetSolid();
            Result.Points.Add(new Point(BeamSolid.MaximumPoint.X, BeamSolid.MaximumPoint.Y, 0));
            Result.Points.Add(new Point(BeamSolid.MinimumPoint.X, BeamSolid.MaximumPoint.Y, 0));
            Result.Points.Add(new Point(BeamSolid.MinimumPoint.X, BeamSolid.MinimumPoint.Y, 0));
            Result.Points.Add(new Point(BeamSolid.MaximumPoint.X, BeamSolid.MinimumPoint.Y, 0));
            Result.Points.Add(new Point(BeamSolid.MaximumPoint.X, BeamSolid.MaximumPoint.Y, 0));

            return Result;
        }

        private void FootingSize_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            CreateRebars();
        }

        /// <summary>
        /// Create pad footing, column on a given position and create base plate connection between those
        /// </summary>
        /// <param name="PositionX">X-coordination of the position</param>
        /// <param name="PositionY">Y-coordination of the position</param>
        private void CreateFootingAndColumn(double PositionX, double PositionY)
        {
            ModelObject PadFooting = CreatePadFooting(PositionX, PositionY, double.Parse(FootingSize.Text));
            Beam Column = CreateColumn(PositionX, PositionY);
            CreateBasePlate(Column);
        }

        /// <summary>
        /// Method that creates a pad footing on given position and returns the created pad footing.
        /// The created pad footing is recognized as beam in Tekla Structures.
        /// </summary>
        /// <param name="PositionX">X-coordination of the position</param>
        /// <param name="PositionY">Y-coordination of the position</param>
        /// <param name="FootingSize">Size of the footing: FootingSize*FootingSize for profile</param>
        /// <returns></returns>
        private static ModelObject CreatePadFooting(double PositionX, double PositionY, double FootingSize)
        {
            Beam PadFooting = new Beam();

            PadFooting.Name = "FOOTING";
            PadFooting.Profile.ProfileString = FootingSize + "*" + FootingSize; //"1500*1500";
            PadFooting.Material.MaterialString = "C50/60";
            PadFooting.Class = "8";
            PadFooting.StartPoint.X = PositionX;
            PadFooting.StartPoint.Y = PositionY;
            PadFooting.EndPoint.X = PositionX;
            PadFooting.EndPoint.Y = PositionY;
            PadFooting.EndPoint.Z = -500.0;
            PadFooting.Position.Rotation = Position.RotationEnum.FRONT;
            PadFooting.Position.Plane = Position.PlaneEnum.MIDDLE;
            PadFooting.Position.Depth = Position.DepthEnum.MIDDLE;

            if (!PadFooting.Insert())
            {
                Console.WriteLine("Insertion of pad footing failed.");
            }

            return PadFooting;
        }

        /// <summary>
        /// Method that creates a column to given position and returns the created column.
        /// The created column is recognized as beam in Tekla Structures.
        /// </summary>
        /// <param name="PositionX">X-coordination of the position</param>
        /// <param name="PositionY">Y-coordination of the position</param>
        /// <returns></returns>
        private Beam CreateColumn(double PositionX, double PositionY)
        {
            Beam Column = new Beam();

            Column.Name = "COLUMN";
            Column.Profile.ProfileString = ColumnsProfileTextBox.Text;
            Column.Material.MaterialString = ColumnsMaterialTextBox.Text;
            Column.Class = "2";
            Column.StartPoint.X = PositionX;
            Column.StartPoint.Y = PositionY;
            Column.EndPoint.X = PositionX;
            Column.EndPoint.Y = PositionY;
            Column.EndPoint.Z = 5000.0;
            Column.Position.Rotation = Position.RotationEnum.FRONT;
            Column.Position.Plane = Position.PlaneEnum.MIDDLE;
            Column.Position.Depth = Position.DepthEnum.MIDDLE;

            if (!Column.Insert())
            {
                Console.WriteLine("Insertion of column failed.");
            }

            return Column;
        }

        /// <summary>
        /// Method that creates detail (1014)
        /// </summary>
        /// <param name="PrimaryObject"></param>
        private static void CreateBasePlate(Beam PrimaryObject)
        {
            Detail BasePlate = new Detail();

            BasePlate.Name = "Stiffened Base Plate";
            BasePlate.Number = 1014;
            BasePlate.LoadAttributesFromFile("standard");
            BasePlate.AutoDirectionType = AutoDirectionTypeEnum.AUTODIR_FROM_ATTRIBUTE_FILE;
            BasePlate.DetailType = DetailTypeEnum.END;

            BasePlate.SetPrimaryObject(PrimaryObject);
            BasePlate.SetReferencePoint(PrimaryObject.StartPoint);
            BasePlate.SetAttribute("cut", 1);  //Enable anchor rods

            if (!BasePlate.Insert())
            {
                Console.WriteLine("Insertion of stiffened base plate failed.");
            }
        }

        /*-----------------------------------------*
         * Exercise 5-7 under this                 *
         * ----------------------------------------*/
        /// <summary>
        /// Callback function to show the profile selection dialog. The value set in 
        /// ColumnsProfileTextBox is set as SelectedProfile to be selected in the 
        /// dialog (if it exists).
        /// </summary>
        private void profileCatalog1_SelectClicked(object sender, EventArgs e)
        {
            profileCatalog1.SelectedProfile = ColumnsProfileTextBox.Text;
        }


        /// <summary>
        /// Callback function to show the value selected in the profile selection form.
        /// </summary>
        private void profileCatalog1_SelectionDone(object sender, EventArgs e)
        {
            SetAttributeValue(ColumnsProfileTextBox, profileCatalog1.SelectedProfile);
        }

        /// <summary>
        /// Callback function to show the reinforcement selection dialog. The value set in 
        /// text box will be selected in the dialog (if it exists).
        /// </summary>
        private void reinforcementCatalog1_SelectClicked(object sender, EventArgs e)
        {
            reinforcementCatalog1.SelectedRebarSize = SizeTextBox.Text;
            reinforcementCatalog1.SelectedRebarGrade = GradeTextBox.Text;
            reinforcementCatalog1.SelectedRebarBendingRadius = TSD.Distance.Parse(BendingRadiusTextBox.Text, CultureInfo.InvariantCulture).Millimeters;
        }

        /// <summary>
        /// Callback function to show the values selected in the reinforcement
        /// selection form.
        /// </summary>
        private void reinforcementCatalog1_SelectionDone(object sender, EventArgs e)
        {
            SetAttributeValue(SizeTextBox, reinforcementCatalog1.SelectedRebarSize);
            SetAttributeValue(GradeTextBox, reinforcementCatalog1.SelectedRebarGrade);
            SetAttributeValue(BendingRadiusTextBox, new TSD.Distance(reinforcementCatalog1.SelectedRebarBendingRadius));
        }

        /// <summary>
        /// List of filtered materials (just steel ones) is shown in the material selection form.
        /// </summary>
        private void SelectMaterialButton_Click(object sender, EventArgs e)
        {
            MaterialSelectionForm SelectionForm = new MaterialSelectionForm(SteelMaterials, ColumnsMaterialTextBox.Text);

            SelectionForm.ShowDialog();

            if (SelectionForm.DialogResult == DialogResult.OK)
                SetAttributeValue(ColumnsMaterialTextBox, SelectionForm.SelectedMaterial);

        }

        /// <summary>
        /// When the Form is loaded the the steel materials are loaded from the catalog
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //Set the value for the footings
            FootingSize.Text = "1500";

            //Create a filtered list of steel materials available in the material catalog
            CatalogHandler CatalogHandler = new CatalogHandler();

            MaterialItemEnumerator Materials = CatalogHandler.GetMaterialItems();

            while (Materials.MoveNext())
            {
                MaterialItem Item = Materials.Current;

                if (Item.Type == MaterialItem.MaterialItemTypeEnum.MATERIAL_STEEL)
                {
                    SteelMaterials.Add(Item);
                }
            }
        }
    }
}