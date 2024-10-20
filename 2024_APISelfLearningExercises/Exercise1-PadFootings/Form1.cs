using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Tekla.Structures.Model;
using System.IO;
using Tekla.Structures.Dialog;
using Tekla.Structures.Geometry3d;
using Tekla.Structures;


namespace PadFootingCreator
{
    public partial class Form1 : Form
    {

        private readonly Dictionary<string, Action> optionActions;
        private readonly Model MyModel;
        
        public Form1()
        {
            InitializeComponent();
            //base.InitializeForm();
            MyModel = new Model();
            if (MyModel.GetConnectionStatus())
            {

                MessageBox.Show("Successfully connected to Tekla Structures model.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                
            }
            else
            {
                MessageBox.Show("Could not connect to Tekla Structures model.\nPlease Check Tekla is insstalled in the correct path: \nC>Program files>Tekla Structures>Version> \nAnd have access to >Bin> folder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }

                SpacingXInput.KeyPress += SpacingXInput_KeyPress;
                SpacingYInput.KeyPress += SpacingYInput_KeyPress;
                NumPadsXInput.KeyPress += NumPadsXInput_KeyPress;
                NumPadsYInput.KeyPress += NumPadsYInput_KeyPress;
                HeightInput.KeyPress += HeightInput_KeyPress;
                comboBox1.Items.Add("Option 1");
                comboBox1.Items.Add("Option 2");
                //default option selected 1 with index 0 
                comboBox1.SelectedIndex = 0;
                // Initialize the dictionary
                optionActions = new Dictionary<string, Action>
                    {
                        { "Option 1", ExecuteOption1 },
                        { "Option 2", ExecuteOption2 }
                    };


            //string clmDirectory = @"C:\TeklaStructuresModels\New model 2\attributes";
            string xsFirmPath = "";
            TeklaStructuresSettings.GetAdvancedOption("XS_FIRM", ref xsFirmPath);
            //MessageBox.Show($"XS_FIRM Path: {xsFirmPath}");
            string modelFolder = MyModel.GetInfo().ModelPath;
            //MessageBox.Show($"XS_FIRM Path: {modelFolder}");
            string clmDirectory = Path.Combine(modelFolder, "attributes");
            MessageBox.Show($"CLM Directory: {clmDirectory}");
            // Add .clm files to the ComboBox

            string[] allFiles = Directory.GetFiles(clmDirectory);
            //MessageBox.Show($"Number of files found: {allFiles.Length}");
            foreach (string file in allFiles)
            {
                string fileName = Path.GetFileName(file);
                //MessageBox.Show($"File  found: {fileName}");
                if (file.EndsWith(".clm"))
                {
                  
                    LoadColumnSettings.Items.Add(fileName);
                }



            }

            // Set the background color of the form 

            this.BackColor = System.Drawing.Color.FromArgb(29, 36, 57);
            
        }


        private void SpacingXInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void SpacingYInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void NumPadsXInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void NumPadsYInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void HeightInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

       


        private bool ValidateInputs()
        {
            var inputControls = new (System.Windows.Forms.TextBox, string)[]
            {
        (SpacingXInput, "Spacing X"),
        (SpacingYInput, "Spacing Y"),
        (NumPadsXInput, "Number of Pads X"),
        (NumPadsYInput, "Number of Pads Y"),
        (HeightInput, "Height"),
             };

            foreach (var (input, name) in inputControls)
            {
                if (string.IsNullOrWhiteSpace(input.Text))
                {
                    MessageBox.Show($"Please insert a value for {name}.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            return true;
        }



        //private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    // Optional: Handle any immediate actions when selection changes
        //}
        private void Execute_OnButtonclick(object sender, EventArgs e)
        {
            // Get the selected item ?? means option 1 is selected by default
            string selectedOption = comboBox1.SelectedItem?.ToString() ?? "Option 1";

            // Execute the corresponding action
            if (optionActions.ContainsKey(selectedOption))
            {
                optionActions[selectedOption].Invoke();
            }
        }

        private void ExecuteOption1()
        {
            // Do something for Option 1
            if (MyModel.GetConnectionStatus())
            {
                if (!ValidateInputs())
                {
                    return;
                }
                double spacingX = double.Parse(SpacingXInput.Text);
                double spacingY = double.Parse(SpacingYInput.Text);
                int numPadsX = int.Parse(NumPadsXInput.Text);
                int numPadsY = int.Parse(NumPadsYInput.Text);
                double height = double.Parse(HeightInput.Text);
                for (int i = 0; i < numPadsX; i++)
                {
                    double PositionX = i * spacingX;
                    if (i == 0 || i == numPadsX - 1)
                    {
                        for (int j = 0; j < numPadsY; j++)
                        {
                            double PositionY = j * spacingY;
                            CreatePadFooting(PositionX, PositionY);
                            CreateColumn(PositionX, PositionY, height);
                            // Create beams along Y direction
                            if (i == 0 && j < numPadsY - 1)
                            {
                                CreateBeam(PositionX, PositionY, PositionX, PositionY + spacingY, height);
                            }
                            else if (i == numPadsX - 1 && j > 0)
                            {
                                CreateBeam(PositionX, PositionY, PositionX, PositionY - spacingY, height);
                            }
                        }
                    }
                    else
                    {
                        CreatePadFooting(PositionX, 0.0);
                        CreatePadFooting(PositionX, (numPadsY - 1) * spacingY);
                        CreateColumn(PositionX, 0.0, height);
                        CreateColumn(PositionX, (numPadsY - 1) * spacingY, height);
                        // Create beams along X direction
                        if (i < numPadsX - 1)
                        {
                            CreateBeam(PositionX, 0.0, PositionX + spacingX, 0.0, height);
                            CreateBeam(PositionX, (numPadsY - 1) * spacingY, PositionX + spacingX, (numPadsY - 1) * spacingY, height);
                        }
                    }
                }
                // Special case for the first beam
                CreateBeam(0.0, 0.0, spacingX, 0.0, height);
                // Special case for the last beam
                CreateBeam(0.0, (numPadsY - 1) * spacingY, spacingX, (numPadsY - 1) * spacingY, height);
                MyModel.CommitChanges();
            }
        }

        private void ExecuteOption2()
        {
            // Do something for Option 2
            if (MyModel.GetConnectionStatus())
            {
                if (!ValidateInputs())
                {
                    return;
                }
                double spacingX = double.Parse(SpacingXInput.Text);
                double spacingY = double.Parse(SpacingYInput.Text);
                int numPadsX = int.Parse(NumPadsXInput.Text);
                int numPadsY = int.Parse(NumPadsYInput.Text);
                double height = double.Parse(HeightInput.Text);
                for (int i = 0; i < numPadsX; i++)
                {
                    double PositionX = i * spacingX;
                    for (int j = 0; j < numPadsY; j++)
                    {
                        double PositionY = j * spacingY;
                        CreatePadFooting(PositionX, PositionY);
                        CreateColumn(PositionX, PositionY, height);
                        // Create beams along Y direction between columns
                        if (j < numPadsY - 1)
                        {
                            CreateBeam(PositionX, PositionY, PositionX, PositionY + spacingY, height);
                        }
                        // Create beams along X direction between columns
                        if (i < numPadsX - 1)
                        {
                            CreateBeam(PositionX, PositionY, PositionX + spacingX, PositionY, height);
                        }
                    }
                }
                MyModel.CommitChanges();
            }
        }

        private static void CreatePadFooting(double PositionX, double PositionY)
        {
            
            Beam PadFooting1 = new Beam
            {
                Name = "PAD-FOOTING",
                Profile = { ProfileString = "1000*1000" },
                Material = { MaterialString = "C25" },
                Class = "8",
                StartPoint = { X = PositionX, Y = PositionY, Z = -200.0 },
                EndPoint = { X = PositionX, Y = PositionY, Z = -1000.0 },
                Position = { Rotation = Position.RotationEnum.FRONT, Plane = Position.PlaneEnum.MIDDLE, Depth = Position.DepthEnum.MIDDLE }
            };
            PadFooting1.Insert();
        }

        private static void CreateColumn(double PositionX, double PositionY, double PositionZ)
        {
            //string colclass = $"{i}";
            Beam Column = new Beam
            {
                Name = "COLUMN",
                Profile = { ProfileString = "CHS139.7*5.0" },
                Material = { MaterialString = "S235JR" },
                Class = "1",
                StartPoint = { X = PositionX, Y = PositionY, Z = -200.0 },
                EndPoint = { X = PositionX, Y = PositionY, Z = PositionZ },
                Position = { Rotation = Position.RotationEnum.FRONT, Plane = Position.PlaneEnum.MIDDLE, Depth = Position.DepthEnum.MIDDLE }
            };
            Column.Insert();
        }
        private static void CreateBeam(double startX, double startY, double endX, double endY, double height)
        {
            Beam Beam = new Beam
            {
                Name = "BEAM",
                Profile = { ProfileString = "SHS100*100*5.0" },
                Material = { MaterialString = "S235JR" },
                Class = "3",
                StartPoint = { X = startX, Y = startY, Z = height },
                EndPoint = { X = endX, Y = endY, Z = height },
                Position = { Rotation = Position.RotationEnum.TOP, Plane = Position.PlaneEnum.MIDDLE, Depth = Position.DepthEnum.MIDDLE }
            };
            Beam.Insert();
        }


        private void InsertJobDescription(object sender, EventArgs e)
        {
            if (MyModel.GetConnectionStatus())
            {
                ProjectInfo myPI = MyModel.GetProjectInfo();

                myPI.ProjectNumber = JobNumberText.Text;
                myPI.Name = JobNameText.Text;
                myPI.Builder = JobBuilderText.Text;
                myPI.Object = JobDescription.Text;
                myPI.Designer = JobDesignerText.Text;
                myPI.Location = JobPostCodeText.Text;
                myPI.Info1 = JobInfo1.Text;
                myPI.Modify();

            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }




    }
}