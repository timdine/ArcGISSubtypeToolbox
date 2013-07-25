using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.GeoprocessingUI;
using SubtypeTools;
using ESRI.ArcGIS.esriSystem;


namespace CreateToolbox
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IAoInitialize aoInit = new AoInitialize();
            aoInit.Initialize(esriLicenseProductCode.esriLicenseProductCodeAdvanced);
           

            IWorkspaceFactory theWorkspaceFactory = new ESRI.ArcGIS.Geoprocessing.ToolboxWorkspaceFactory();
            IToolboxWorkspace theToolboxWorkspace = (IToolboxWorkspace)theWorkspaceFactory.OpenFromFile("C:\\temp", 0);

            IGPToolbox theToolbox = theToolboxWorkspace.CreateToolbox("SubtypeToolbox", "subtypetools");

            SubtypeTools.SubtypeFunctionFactory theSubtypeFactory = new SubtypeFunctionFactory();

            IGPFunction theFunction = theSubtypeFactory.GetFunction("getSubtypeField");
            IGPTool theTool = theToolbox.CreateTool(esriGPToolType.esriGPFunctionTool, "Name", "DisplayName", "Description", "", null);
            IGPFunctionTool theFunctionTool = (IGPFunctionTool)theTool;
            theFunctionTool.Function = theFunction;
            theTool.Store();

            IGPFunction theFunction2 = theSubtypeFactory.GetFunction("getDomainList");
            IGPTool theTool2 = theToolbox.CreateTool(esriGPToolType.esriGPFunctionTool, "", "", "", "", null);
            IGPFunctionTool theFunctionTool2 = (IGPFunctionTool)theTool2;
            theFunctionTool2.Function = theFunction2;
            theTool2.Store();

            IGPFunction theFunction3 = theSubtypeFactory.GetFunction("getRelationshipClassList");
            IGPTool theTool3 = theToolbox.CreateTool(esriGPToolType.esriGPFunctionTool, "", "", "", "", null);
            IGPFunctionTool theFunctionTool3 = (IGPFunctionTool)theTool3;
            theFunctionTool3.Function = theFunction3;
            theTool3.Store();

            IGPFunction theFunction4 = theSubtypeFactory.GetFunction("getSubtypeList");
            IGPTool theTool4 = theToolbox.CreateTool(esriGPToolType.esriGPFunctionTool, "", "", "", "", null);
            IGPFunctionTool theFunctionTool4 = (IGPFunctionTool)theTool4;
            theFunctionTool4.Function = theFunction4;
            theTool4.Store();

            IGPFunction theFunction5 = theSubtypeFactory.GetFunction("changeAnnotationLabelField");
            IGPTool theTool5 = theToolbox.CreateTool(esriGPToolType.esriGPFunctionTool, "", "", "", "", null);
            IGPFunctionTool theFunctionTool5 = (IGPFunctionTool)theTool5;
            theFunctionTool5.Function = theFunction5;
            theTool5.Store();

            IGPFunction theFunction6 = theSubtypeFactory.GetFunction("getSubtypeCodeFromDescription");
            IGPTool theTool6 = theToolbox.CreateTool(esriGPToolType.esriGPFunctionTool, "", "", "", "", null);
            IGPFunctionTool theFunctionTool6 = (IGPFunctionTool)theTool6;
            theFunctionTool6.Function = theFunction6;
            theTool6.Store();
            
            IGPFunction theFunction7 = theSubtypeFactory.GetFunction("getDefaultForFieldForSubtype");
            IGPTool theTool7 = theToolbox.CreateTool(esriGPToolType.esriGPFunctionTool, "", "", "", "", null);
            IGPFunctionTool theFunctionTool7 = (IGPFunctionTool)theTool7;
            theFunctionTool7.Function = theFunction7;
            theTool7.Store();

            IGPFunction theFunction8 = theSubtypeFactory.GetFunction("getDomainForFieldForSubtype");
            IGPTool theTool8 = theToolbox.CreateTool(esriGPToolType.esriGPFunctionTool, "", "", "", "", null);
            IGPFunctionTool theFunctionTool8 = (IGPFunctionTool)theTool8;
            theFunctionTool8.Function = theFunction8;
            theTool8.Store();
            
            
        }
    }
}
