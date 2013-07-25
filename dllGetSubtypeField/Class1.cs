using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;

namespace SubtypeTools
{
    public class getDomainForFieldForSubtype : IGPFunction2
    {
        #region IGPFunction2 Members
        private string m_ToolName = "getDomainForFieldForSubtype"; //Function Name
        private string m_metadatafile = "";
        private IArray m_Parameters;             // Array of Parameters
        private IGPUtilities m_GPUtilities;      // GPUtilities object


        public getDomainForFieldForSubtype()
        {
            m_GPUtilities = new GPUtilitiesClass();
        }

        public UID DialogCLSID
        {
            get { return null; }
        }

        public string DisplayName
        {
            get { return "Get Domain for Field for Subtype"; }
        }


        public void Execute(IArray paramvalues, ITrackCancel trackcancel, IGPEnvironmentManager envMgr, IGPMessages message)
        {
            message.AddMessage("New");

            IGPParameter parameter0 = (IGPParameter)paramvalues.get_Element(0);
            IGPParameter parameter1 = (IGPParameter)paramvalues.get_Element(1);
            IGPParameter parameter2 = (IGPParameter)paramvalues.get_Element(2);

            IGPValue parameter0Value = m_GPUtilities.UnpackGPValue(parameter0);
            IGPValue parameter1Value = m_GPUtilities.UnpackGPValue(parameter1);
            IGPValue parameter2Value = m_GPUtilities.UnpackGPValue(parameter2);

            message.AddMessage(parameter0Value.GetAsText());
            message.AddMessage(parameter1Value.GetAsText());
            message.AddMessage(parameter2Value.GetAsText());


            IDataset theDataset = m_GPUtilities.OpenDataset(parameter0Value);
            String thePath = theDataset.Workspace.PathName.ToString() + "\\" + theDataset.Name.ToString();
            message.AddMessage(thePath);

            ITable theITable = m_GPUtilities.OpenTableFromString(thePath);

            String theOutputString = "";

            ISubtypes theSubtypes = (ISubtypes)theITable;

            int theSubtypeCode = Convert.ToInt16(parameter1Value.GetAsText());
            //object theSubtypeDefault = theSubtypes.get_DefaultValue(theSubtypeCode, (string)parameter2Value.GetAsText());
            IDomain theDomain = theSubtypes.get_Domain(theSubtypeCode, (string)parameter2Value.GetAsText());

            message.AddMessage(theDomain.Name);

            IGPParameter theOutput = (IGPParameter)paramvalues.get_Element(3); //This is the last parameter in this case
            IGPValue theParameterValue = m_GPUtilities.UnpackGPValue(theOutput);
            theParameterValue.SetAsText(theDomain.Name);
        }

        public IName FullName
        {
            get
            {
                IGPFunctionFactory functionFactory = new SubtypeFunctionFactory();
                return (IName)functionFactory.GetFunctionName("getDomainForFieldForSubtype");
            }
        }

        public object GetRenderer(IGPParameter pParam)
        {
            return null;
        }

        public int HelpContext
        {
            get { return 0; }
        }

        public string HelpFile
        {
            get { return ""; }
        }

        public bool IsLicensed()
        {
            return true;
        }

        public string MetadataFile
        {
            get { return m_metadatafile; }
        }

        public string Name
        {
            get { return m_ToolName; }
        }

        public IArray ParameterInfo
        {
            get
            {

                IArray parameters = new ArrayClass();

                //Input DataType is GPFeatureLayerType
                IGPParameterEdit3 inputParameter = new GPParameterClass();
                inputParameter.DataType = new GPFeatureLayerTypeClass();

                IGPParameterEdit3 theInput = new GPParameterClass();
                theInput.DataType = new DETableTypeClass();
                theInput.Value = new DETableClass();
                theInput.Direction = esriGPParameterDirection.esriGPParameterDirectionInput;
                theInput.DisplayName = "Input Featureclass";
                theInput.Name = "theInputFC";
                theInput.ParameterType = esriGPParameterType.esriGPParameterTypeRequired;
                parameters.Add(theInput);

                IGPParameterEdit3 theInputExpression = new GPParameterClass();
                theInputExpression.DataType = new GPStringTypeClass();
                theInputExpression.Value = new GPStringClass();
                theInputExpression.Direction = esriGPParameterDirection.esriGPParameterDirectionInput;
                theInputExpression.DisplayName = "The Subtype Code";
                theInputExpression.Name = "theSubtypeCode";
                theInputExpression.ParameterType = esriGPParameterType.esriGPParameterTypeRequired;
                parameters.Add(theInputExpression);

                IGPParameterEdit3 theOtherInputExpression = new GPParameterClass();
                theOtherInputExpression.DataType = new GPStringTypeClass();
                theOtherInputExpression.Value = new GPStringClass();
                theOtherInputExpression.Direction = esriGPParameterDirection.esriGPParameterDirectionInput;
                theOtherInputExpression.DisplayName = "The Field";
                theOtherInputExpression.Name = "theField";
                theOtherInputExpression.ParameterType = esriGPParameterType.esriGPParameterTypeRequired;
                parameters.Add(theOtherInputExpression);

                IGPParameterEdit3 newOutputParameter = new GPParameterClass();
                newOutputParameter.DataType = new GPStringTypeClass();
                newOutputParameter.Value = new GPStringClass();
                newOutputParameter.Direction = esriGPParameterDirection.esriGPParameterDirectionOutput;
                newOutputParameter.Name = "outputString";
                newOutputParameter.ParameterType = esriGPParameterType.esriGPParameterTypeDerived;
                parameters.Add(newOutputParameter);

                return parameters;



            }
        }

        public void UpdateMessages(IArray paramvalues, IGPEnvironmentManager pEnvMgr, IGPMessages Messages)
        {
            IGPMessage msg = (IGPMessage)Messages;
            if (msg.IsError())
                return;
            IGPParameter parameter = (IGPParameter)paramvalues.get_Element(0);
            IGPValue parameterValue = m_GPUtilities.UnpackGPValue(parameter);
            return;
        }

        public void UpdateParameters(IArray paramvalues, IGPEnvironmentManager pEnvMgr)
        {
            m_Parameters = paramvalues;
            IGPValue parameterValue = m_GPUtilities.UnpackGPValue(m_Parameters.get_Element(0));
            if (parameterValue.IsEmpty() == false)
            {
                String one = "1";
            }
        }

        public IGPMessages Validate(IArray paramvalues, bool updateValues, IGPEnvironmentManager envMgr)
        {
            if (m_Parameters == null)
                m_Parameters = ParameterInfo;
            if (updateValues == true)
            {
                UpdateParameters(paramvalues, envMgr);
            }
            IGPMessages validateMsgs = m_GPUtilities.InternalValidate(m_Parameters, paramvalues, updateValues, true, envMgr);
            UpdateMessages(paramvalues, envMgr, validateMsgs);
            return validateMsgs;
        }


        #endregion
    }


    public class getDefaultForFieldForSubtype : IGPFunction2
    {
        #region IGPFunction2 Members
        private string m_ToolName = "getDefaultForFieldForSubtype"; //Function Name
        private string m_metadatafile = "";
        private IArray m_Parameters;             // Array of Parameters
        private IGPUtilities m_GPUtilities;      // GPUtilities object


        public getDefaultForFieldForSubtype()
        {
            m_GPUtilities = new GPUtilitiesClass();
        }

        public UID DialogCLSID
        {
            get { return null; }
        }

        public string DisplayName
        {
            get { return "Get Default for Field for Subtype"; }
        }


        public void Execute(IArray paramvalues, ITrackCancel trackcancel, IGPEnvironmentManager envMgr, IGPMessages message)
        {
            message.AddMessage("New");

            IGPParameter parameter0 = (IGPParameter)paramvalues.get_Element(0);
            IGPParameter parameter1 = (IGPParameter)paramvalues.get_Element(1);
            IGPParameter parameter2 = (IGPParameter)paramvalues.get_Element(2);

            IGPValue parameter0Value = m_GPUtilities.UnpackGPValue(parameter0);
            IGPValue parameter1Value = m_GPUtilities.UnpackGPValue(parameter1);
            IGPValue parameter2Value = m_GPUtilities.UnpackGPValue(parameter2);

            message.AddMessage(parameter0Value.GetAsText());
            message.AddMessage(parameter1Value.GetAsText());
            message.AddMessage(parameter2Value.GetAsText());

            
            IDataset theDataset = m_GPUtilities.OpenDataset(parameter0Value);
            String thePath = theDataset.Workspace.PathName.ToString() + "\\" + theDataset.Name.ToString();
            message.AddMessage(thePath);

            ITable theITable = m_GPUtilities.OpenTableFromString(thePath);

            String theOutputString = "";

            ISubtypes theSubtypes = (ISubtypes)theITable;

            int theSubtypeCode = Convert.ToInt16(parameter1Value.GetAsText());
            object theSubtypeDefault = theSubtypes.get_DefaultValue(theSubtypeCode, (string)parameter2Value.GetAsText());

            message.AddMessage(theSubtypeDefault.ToString());

            IGPParameter theOutput = (IGPParameter)paramvalues.get_Element(3); //This is the last parameter in this case
            IGPValue theParameterValue = m_GPUtilities.UnpackGPValue(theOutput);
            theParameterValue.SetAsText(theSubtypeDefault.ToString());
        }

        public IName FullName
        {
            get
            {
                IGPFunctionFactory functionFactory = new SubtypeFunctionFactory();
                return (IName)functionFactory.GetFunctionName("getDefaultForFieldForSubtype");
            }
        }

        public object GetRenderer(IGPParameter pParam)
        {
            return null;
        }

        public int HelpContext
        {
            get { return 0; }
        }

        public string HelpFile
        {
            get { return ""; }
        }

        public bool IsLicensed()
        {
            return true;
        }

        public string MetadataFile
        {
            get { return m_metadatafile; }
        }

        public string Name
        {
            get { return m_ToolName; }
        }

        public IArray ParameterInfo
        {
            get
            {

                IArray parameters = new ArrayClass();

                //Input DataType is GPFeatureLayerType
                IGPParameterEdit3 inputParameter = new GPParameterClass();
                inputParameter.DataType = new GPFeatureLayerTypeClass();

                IGPParameterEdit3 theInput = new GPParameterClass();
                theInput.DataType = new DETableTypeClass();
                theInput.Value = new DETableClass();
                theInput.Direction = esriGPParameterDirection.esriGPParameterDirectionInput;
                theInput.DisplayName = "Input Featureclass";
                theInput.Name = "theInputFC";
                theInput.ParameterType = esriGPParameterType.esriGPParameterTypeRequired;
                parameters.Add(theInput);

                IGPParameterEdit3 theInputExpression = new GPParameterClass();
                theInputExpression.DataType = new GPStringTypeClass();
                theInputExpression.Value = new GPStringClass();
                theInputExpression.Direction = esriGPParameterDirection.esriGPParameterDirectionInput;
                theInputExpression.DisplayName = "The Subtype Code";
                theInputExpression.Name = "theSubtypeCode";
                theInputExpression.ParameterType = esriGPParameterType.esriGPParameterTypeRequired;
                parameters.Add(theInputExpression);

                IGPParameterEdit3 theOtherInputExpression = new GPParameterClass();
                theOtherInputExpression.DataType = new GPStringTypeClass();
                theOtherInputExpression.Value = new GPStringClass();
                theOtherInputExpression.Direction = esriGPParameterDirection.esriGPParameterDirectionInput;
                theOtherInputExpression.DisplayName = "The Field";
                theOtherInputExpression.Name = "theField";
                theOtherInputExpression.ParameterType = esriGPParameterType.esriGPParameterTypeRequired;
                parameters.Add(theOtherInputExpression);

                IGPParameterEdit3 newOutputParameter = new GPParameterClass();
                newOutputParameter.DataType = new GPStringTypeClass();
                newOutputParameter.Value = new GPStringClass();
                newOutputParameter.Direction = esriGPParameterDirection.esriGPParameterDirectionOutput;
                newOutputParameter.Name = "outputString";
                newOutputParameter.ParameterType = esriGPParameterType.esriGPParameterTypeDerived;
                parameters.Add(newOutputParameter);

                return parameters;



            }
        }

        public void UpdateMessages(IArray paramvalues, IGPEnvironmentManager pEnvMgr, IGPMessages Messages)
        {
            IGPMessage msg = (IGPMessage)Messages;
            if (msg.IsError())
                return;
            IGPParameter parameter = (IGPParameter)paramvalues.get_Element(0);
            IGPValue parameterValue = m_GPUtilities.UnpackGPValue(parameter);
            return;
        }

        public void UpdateParameters(IArray paramvalues, IGPEnvironmentManager pEnvMgr)
        {
            m_Parameters = paramvalues;
            IGPValue parameterValue = m_GPUtilities.UnpackGPValue(m_Parameters.get_Element(0));
            if (parameterValue.IsEmpty() == false)
            {
                String one = "1";
            }
        }

        public IGPMessages Validate(IArray paramvalues, bool updateValues, IGPEnvironmentManager envMgr)
        {
            if (m_Parameters == null)
                m_Parameters = ParameterInfo;
            if (updateValues == true)
            {
                UpdateParameters(paramvalues, envMgr);
            }
            IGPMessages validateMsgs = m_GPUtilities.InternalValidate(m_Parameters, paramvalues, updateValues, true, envMgr);
            UpdateMessages(paramvalues, envMgr, validateMsgs);
            return validateMsgs;
        }


        #endregion
    }

    public class getSubtypeCodeFromDescription : IGPFunction2
        {

        #region IGPFunction2 Members
        private string m_ToolName = "getSubtypeCodeFromDescription"; //Function Name
        private string m_metadatafile = "";
        private IArray m_Parameters;             // Array of Parameters
        private IGPUtilities m_GPUtilities;      // GPUtilities object

        public getSubtypeCodeFromDescription()
        {
            m_GPUtilities = new GPUtilitiesClass();
        }

        public UID DialogCLSID
        {
            get { return null; }
        }

        public string DisplayName
        {
            get { return "Get Subtype Code From Description"; }
        }

        public void Execute(IArray paramvalues, ITrackCancel trackcancel, IGPEnvironmentManager envMgr, IGPMessages message)
        {
            message.AddMessage("New");

            IGPParameter parameter = (IGPParameter)paramvalues.get_Element(0);
            IGPParameter parameter2 = (IGPParameter)paramvalues.get_Element(1);

            IGPValue parameterValue = m_GPUtilities.UnpackGPValue(parameter);
            IGPValue parameter2Value = m_GPUtilities.UnpackGPValue(parameter2);
            message.AddMessage(parameter2Value.GetAsText());

            IDataset theDataset = m_GPUtilities.OpenDataset(parameterValue);
            String thePath = theDataset.Workspace.PathName.ToString() + "\\" + theDataset.Name.ToString();
            message.AddMessage(thePath);

            ITable theITable = m_GPUtilities.OpenTableFromString(thePath);

            String theOutputString = "";

            ISubtypes theSubtypes = (ISubtypes)theITable;
            IEnumSubtype enumSubtype;
            int subtypeCode;
            string subtypeName;
            if (theSubtypes.HasSubtype)
            {
                enumSubtype = theSubtypes.Subtypes;
                subtypeName = enumSubtype.Next(out subtypeCode);
                while (subtypeName != null)
                {
                    //'1: H-Frame';'2: Non-Wood Power Pole';'4: Non-Wood Street Light Pole';'6: Tower'
                    if (subtypeName == parameter2Value.ToString())
                    {
                        theOutputString = subtypeCode.ToString();
                    }
                    subtypeName = enumSubtype.Next(out subtypeCode);
                }
            }
            else
            {
                theOutputString = null;
            }

            //String theOutputString = "Something";
            IGPParameter theOutput = (IGPParameter)paramvalues.get_Element(1); //This is the last parameter in this case
            IGPValue theParameterValue = m_GPUtilities.UnpackGPValue(theOutput);
            theParameterValue.SetAsText(theOutputString);
        }

        public IName FullName
        {
            get
            {
                IGPFunctionFactory functionFactory = new SubtypeFunctionFactory();
                return (IName)functionFactory.GetFunctionName("getSubtypeCodeFromDescription");
            }
        }

        public object GetRenderer(IGPParameter pParam)
        {
            return null;
        }

        public int HelpContext
        {
            get { return 0; }
        }

        public string HelpFile
        {
            get { return ""; }
        }

        public bool IsLicensed()
        {
            return true;
        }

        public string MetadataFile
        {
            get { return m_metadatafile; }
        }

        public string Name
        {
            get { return m_ToolName; }
        }

        public IArray ParameterInfo
        {
            get
            {
                //Array to the hold the parameters  
                IArray parameters = new ArrayClass();

                //Input DataType is GPFeatureLayerType
                IGPParameterEdit3 inputParameter = new GPParameterClass();
                inputParameter.DataType = new GPFeatureLayerTypeClass();

                IGPParameterEdit3 theInput = new GPParameterClass();
                theInput.DataType = new DETableTypeClass();
                theInput.Value = new DETableClass();
                theInput.Direction = esriGPParameterDirection.esriGPParameterDirectionInput;
                theInput.DisplayName = "Input Featureclass";
                theInput.Name = "theInputFC";
                theInput.ParameterType = esriGPParameterType.esriGPParameterTypeRequired;
                parameters.Add(theInput);

                IGPParameterEdit3 theInputExpression = new GPParameterClass();
                theInputExpression.DataType = new GPStringTypeClass();
                theInputExpression.Value = new GPStringClass();
                theInputExpression.Direction = esriGPParameterDirection.esriGPParameterDirectionInput;
                theInputExpression.DisplayName = "The Subtype Description";
                theInputExpression.Name = "theSubtypeDescription";
                theInputExpression.ParameterType = esriGPParameterType.esriGPParameterTypeRequired;
                parameters.Add(theInputExpression);

                IGPParameterEdit3 newOutputParameter = new GPParameterClass();
                newOutputParameter.DataType = new GPStringTypeClass();
                newOutputParameter.Value = new GPStringClass();
                newOutputParameter.Direction = esriGPParameterDirection.esriGPParameterDirectionOutput;
                newOutputParameter.Name = "outputString";
                newOutputParameter.ParameterType = esriGPParameterType.esriGPParameterTypeDerived;
                parameters.Add(newOutputParameter);

                return parameters;

            }
        }
        
        public void UpdateMessages(IArray paramvalues, IGPEnvironmentManager pEnvMgr, IGPMessages Messages)
        {
            IGPMessage msg = (IGPMessage)Messages;
            if (msg.IsError())
                return;
            IGPParameter parameter = (IGPParameter)paramvalues.get_Element(0);
            IGPValue parameterValue = m_GPUtilities.UnpackGPValue(parameter);
            return;
        }

        public void UpdateParameters(IArray paramvalues, IGPEnvironmentManager pEnvMgr)
        {
            m_Parameters = paramvalues;
            IGPValue parameterValue = m_GPUtilities.UnpackGPValue(m_Parameters.get_Element(0));
            if (parameterValue.IsEmpty() == false)
            {
                String one = "1";
            }
        }

        public IGPMessages Validate(IArray paramvalues, bool updateValues, IGPEnvironmentManager envMgr)
        {
            if (m_Parameters == null)
                m_Parameters = ParameterInfo;
            if (updateValues == true)
            {
                UpdateParameters(paramvalues, envMgr);
            }
            IGPMessages validateMsgs = m_GPUtilities.InternalValidate(m_Parameters, paramvalues, updateValues, true, envMgr);
            UpdateMessages(paramvalues, envMgr, validateMsgs);
            return validateMsgs;
        }

        #endregion
    }


    public class changeAnnotationLabelField : IGPFunction2
    {

        #region IGPFunction2 Members
        private string m_ToolName = "changeAnnoLabelField"; //Function Name
        private string m_metadatafile = "";
        private IArray m_Parameters;             // Array of Parameters
        private IGPUtilities m_GPUtilities;      // GPUtilities object

        public changeAnnotationLabelField()
        {
            m_GPUtilities = new GPUtilitiesClass();
        }

        public UID DialogCLSID
        {
            get { return null; }
        }

        public string DisplayName
        {
            get { return "Change Annotation Label Field"; }
        }

        public void Execute(IArray paramvalues, ITrackCancel trackcancel, IGPEnvironmentManager envMgr, IGPMessages message)
        {
            message.AddMessage("New");

            IGPParameter parameter = (IGPParameter)paramvalues.get_Element(0);
            IGPParameter parameter2 = (IGPParameter)paramvalues.get_Element(1);

            IGPValue parameterValue = m_GPUtilities.UnpackGPValue(parameter);
            IGPValue parameter2Value = m_GPUtilities.UnpackGPValue(parameter2);
            message.AddMessage(parameter2Value.GetAsText());

            IDataset theDataset = m_GPUtilities.OpenDataset(parameterValue);
            String thePath = theDataset.Workspace.PathName.ToString() + "\\" + theDataset.Name.ToString();
            message.AddMessage(thePath);

            IFeatureClass theFC = (IFeatureClass)theDataset;

            message.AddMessage("One");

            IAnnoClass theAnnoClass = (IAnnoClass)theFC.Extension;
            //IAnnotationClassExtension theAnnoExtension = (IAnnotationClassExtension)theAnnoClass;

            message.AddMessage("Mark-1");
            IAnnotateLayerPropertiesCollection theAnnotationLayerPropertiesCollection = theAnnoClass.AnnoProperties;
            IAnnotateLayerProperties annotateLayerProps;// = (ILabelEngineLayerProperties)annoProps;
            IElementCollection theEColl;
            IElementCollection theUnplaced;
            message.AddMessage("Mark-2");
            theAnnotationLayerPropertiesCollection.QueryItem(0, out annotateLayerProps, out theEColl, out theUnplaced);
            message.AddMessage("Mark-3");
            ILabelEngineLayerProperties theLabelEngineLayerProperties = (ILabelEngineLayerProperties)annotateLayerProps;
            message.AddMessage(theLabelEngineLayerProperties.Expression);

            theLabelEngineLayerProperties.Expression = parameter2Value.GetAsText();
            theAnnotationLayerPropertiesCollection.Clear();
            message.AddMessage("Mark-4");
            theAnnotationLayerPropertiesCollection.Add(annotateLayerProps);



            //IAnnotateLayerProperties annotateLayerProps = theAnnoExtension.AnnoProperties;

            /*



            
            //IAnnotateLayerProperties annoProps = (IAnnotateLayerProperties)theFeatureLayer;

            

            
            */
            IAnnoClassAdmin3 theAnnoClassAdmin = (IAnnoClassAdmin3)theAnnoClass;
            theAnnoClassAdmin.AnnoProperties = theAnnotationLayerPropertiesCollection;
            theAnnoClassAdmin.UpdateProperties();
            message.AddMessage(theLabelEngineLayerProperties.Expression);

            String theOutputString = "True";

            //String theOutputString = "Something";
            IGPParameter theOutput = (IGPParameter)paramvalues.get_Element(2); //This is the last parameter in this case
            IGPValue theParameterValue = m_GPUtilities.UnpackGPValue(theOutput);
            theParameterValue.SetAsText(theOutputString);
            m_GPUtilities.PackGPValue(theParameterValue, theOutput);
        }

        public IName FullName
        {
            get
            {
                IGPFunctionFactory functionFactory = new SubtypeFunctionFactory();
                return (IName)functionFactory.GetFunctionName("changeAnnotationLabelField");
            }
        }

        public object GetRenderer(IGPParameter pParam)
        {
            return null;
        }

        public int HelpContext
        {
            get { return 0; }
        }

        public string HelpFile
        {
            get { return ""; }
        }

        public bool IsLicensed()
        {
            return true;
        }

        public string MetadataFile
        {
            get { return m_metadatafile; }
        }

        public string Name
        {
            get { return m_ToolName; }
        }

        public IArray ParameterInfo
        {
            get
            {
                //Array to the hold the parameters  
                IArray parameters = new ArrayClass();

                //Input DataType is GPFeatureLayerType
                IGPParameterEdit3 inputParameter = new GPParameterClass();
                inputParameter.DataType = new GPFeatureLayerTypeClass();

                IGPParameterEdit3 theInput = new GPParameterClass();
                theInput.DataType = new DETableTypeClass();
                theInput.Value = new DETableClass();
                theInput.Direction = esriGPParameterDirection.esriGPParameterDirectionInput;
                theInput.DisplayName = "Input Annotation Featureclass";
                theInput.Name = "theAnnotationFC";
                theInput.ParameterType = esriGPParameterType.esriGPParameterTypeRequired;
                parameters.Add(theInput);

                IGPParameterEdit3 theInputExpression = new GPParameterClass();
                theInputExpression.DataType = new GPStringTypeClass();
                theInputExpression.Value = new GPStringClass();
                theInputExpression.Direction = esriGPParameterDirection.esriGPParameterDirectionInput;
                theInputExpression.DisplayName = "The Expression";
                theInputExpression.Name = "theExpression";
                theInputExpression.ParameterType = esriGPParameterType.esriGPParameterTypeRequired;
                parameters.Add(theInputExpression);

                IGPParameterEdit3 newOutputParameter = new GPParameterClass();
                newOutputParameter.DataType = new GPStringTypeClass();
                newOutputParameter.Value = new GPStringClass();
                newOutputParameter.Direction = esriGPParameterDirection.esriGPParameterDirectionOutput;
                newOutputParameter.Name = "outputString";
                newOutputParameter.ParameterType = esriGPParameterType.esriGPParameterTypeDerived;
                parameters.Add(newOutputParameter);



                return parameters;

            }
        }
        
        public void UpdateMessages(IArray paramvalues, IGPEnvironmentManager pEnvMgr, IGPMessages Messages)
        {
            IGPMessage msg = (IGPMessage)Messages;
            if (msg.IsError())
                return;
            IGPParameter parameter = (IGPParameter)paramvalues.get_Element(0);
            IGPValue parameterValue = m_GPUtilities.UnpackGPValue(parameter);
            return;
        }

        public void UpdateParameters(IArray paramvalues, IGPEnvironmentManager pEnvMgr)
        {
            m_Parameters = paramvalues;
            IGPValue parameterValue = m_GPUtilities.UnpackGPValue(m_Parameters.get_Element(0));
            if (parameterValue.IsEmpty() == false)
            {
                String one = "1";
            }
        }

        public IGPMessages Validate(IArray paramvalues, bool updateValues, IGPEnvironmentManager envMgr)
        {
            if (m_Parameters == null)
                m_Parameters = ParameterInfo;
            if (updateValues == true)
            {
                UpdateParameters(paramvalues, envMgr);
            }
            IGPMessages validateMsgs = m_GPUtilities.InternalValidate(m_Parameters, paramvalues, updateValues, true, envMgr);
            UpdateMessages(paramvalues, envMgr, validateMsgs);
            return validateMsgs;
        }

        #endregion
    }

    public class getSubtypeList : IGPFunction2
    {


        #region IGPFunction2 Members
        private string m_ToolName = "getSubtypeList"; //Function Name
        private string m_metadatafile = "";
        private IArray m_Parameters;             // Array of Parameters
        private IGPUtilities m_GPUtilities;      // GPUtilities object

        public getSubtypeList()
        {
            m_GPUtilities = new GPUtilitiesClass();
        }
        public UID DialogCLSID
        {
            get { return null; }
        }
        public string DisplayName
        {
            get { return "Get Subtype List"; }
        }
        public void Execute(IArray paramvalues, ITrackCancel trackcancel, IGPEnvironmentManager envMgr, IGPMessages message)
        {
            IGPParameter parameter = (IGPParameter)paramvalues.get_Element(0);

            IGPValue parameterValue = m_GPUtilities.UnpackGPValue(parameter);
            
            IDataset theDataset = m_GPUtilities.OpenDataset(parameterValue);
            String thePath = theDataset.Workspace.PathName.ToString() + "\\" + theDataset.Name.ToString();
            message.AddMessage(thePath);
            ITable theITable = m_GPUtilities.OpenTableFromString(thePath);

            String theOutputString = "";
            
            ISubtypes theSubtypes = (ISubtypes)theITable;
            IEnumSubtype enumSubtype;
            int subtypeCode;
            string subtypeName;
            if (theSubtypes.HasSubtype)
            {
                enumSubtype = theSubtypes.Subtypes;
                subtypeName = enumSubtype.Next(out subtypeCode);
                while (subtypeName != null)
                {
                    //'1: H-Frame';'2: Non-Wood Power Pole';'4: Non-Wood Street Light Pole';'6: Tower'
                    theOutputString += "'" + subtypeCode + ": " + subtypeName + "';";
                    subtypeName = enumSubtype.Next(out subtypeCode);
                }
                theOutputString = theOutputString.Remove(theOutputString.Length - 1);
            }
            else
            {
                theOutputString = null;
            }
            
            //String theOutputString = "Something";
            IGPParameter theOutput = (IGPParameter)paramvalues.get_Element(1); //This is the last parameter in this case
            IGPValue theParameterValue = m_GPUtilities.UnpackGPValue(theOutput);
            theParameterValue.SetAsText(theOutputString);
            m_GPUtilities.PackGPValue(theParameterValue, theOutput);
        }
        public IName FullName
        {
            get
            {
                IGPFunctionFactory functionFactory = new SubtypeFunctionFactory();
                return (IName)functionFactory.GetFunctionName("getSubtypeList");
            }
        }
        public object GetRenderer(IGPParameter pParam)
        {
            return null;
        }
        public int HelpContext
        {
            get { return 0; }
        }
        public string HelpFile
        {
            get { return ""; }
        }
        public bool IsLicensed()
        {
            return true;
        }
        public string MetadataFile
        {
            get { return m_metadatafile; }
        }
        public string Name
        {
            get { return m_ToolName; }
        }
        public IArray ParameterInfo
        {
            get
            {
                IArray parameters = new ArrayClass();

                IGPParameterEdit3 theInput = new GPParameterClass();
                theInput.DataType = new DETableTypeClass();
                theInput.Value = new DETableClass();
                theInput.Direction = esriGPParameterDirection.esriGPParameterDirectionInput;
                theInput.DisplayName = "Input object to read subtypes from:";
                theInput.Name = "theObject";
                theInput.ParameterType = esriGPParameterType.esriGPParameterTypeRequired;
                parameters.Add(theInput);

                IGPParameterEdit3 newOutputParameter = new GPParameterClass();
                newOutputParameter.DataType = new GPStringTypeClass();
                newOutputParameter.Value = new GPStringClass();
                newOutputParameter.Direction = esriGPParameterDirection.esriGPParameterDirectionOutput;
                newOutputParameter.Name = "outputString";
                newOutputParameter.ParameterType = esriGPParameterType.esriGPParameterTypeDerived;
                parameters.Add(newOutputParameter);

                return parameters;
            }
        }
        public void UpdateMessages(IArray paramvalues, IGPEnvironmentManager pEnvMgr, IGPMessages Messages)
        {
            IGPMessage msg = (IGPMessage)Messages;
            if (msg.IsError())
                return;
            IGPParameter parameter = (IGPParameter)paramvalues.get_Element(0);
            IGPValue parameterValue = m_GPUtilities.UnpackGPValue(parameter);
            return;
        }
        public void UpdateParameters(IArray paramvalues, IGPEnvironmentManager pEnvMgr)
        {
            m_Parameters = paramvalues;
            IGPValue parameterValue = m_GPUtilities.UnpackGPValue(m_Parameters.get_Element(0));
            if (parameterValue.IsEmpty() == false)
            {
                String one = "1";
            }
        }
        public IGPMessages Validate(IArray paramvalues, bool updateValues, IGPEnvironmentManager envMgr)
        {
            if (m_Parameters == null)
                m_Parameters = ParameterInfo;
            if (updateValues == true)
            {
                UpdateParameters(paramvalues, envMgr);
            }
            IGPMessages validateMsgs = m_GPUtilities.InternalValidate(m_Parameters, paramvalues, updateValues, true, envMgr);
            UpdateMessages(paramvalues, envMgr, validateMsgs);
            return validateMsgs;
        }
        #endregion
 
    }

    public class getRelationshipClassList : IGPFunction2
    {
        #region IGPFunction2 Members

        private string m_ToolName = "getRelationshipClassList"; //Function Name
        private string m_metadatafile = "";
        private IArray m_Parameters;             // Array of Parameters
        private IGPUtilities m_GPUtilities;      // GPUtilities object

        public getRelationshipClassList()
        {
            m_GPUtilities = new GPUtilitiesClass();
        }
        public UID DialogCLSID
        {
            get { return null; }
        }
        public string DisplayName
        {
            get { return "Get Relationship Class List"; }
        }
        public void Execute(IArray paramvalues, ITrackCancel trackcancel, IGPEnvironmentManager envMgr, IGPMessages message)
        {
            IGPParameter parameter = (IGPParameter)paramvalues.get_Element(0);
            IGPValue parameterValue = m_GPUtilities.UnpackGPValue(parameter);
            String theSDEPath = parameterValue.GetAsText();
            message.AddMessage("Working through database:" + theSDEPath);
            ESRI.ArcGIS.esriSystem.IPropertySet propertySet = new ESRI.ArcGIS.esriSystem.PropertySetClass();
            IWorkspaceFactory workspaceFactory = new ESRI.ArcGIS.DataSourcesGDB.SdeWorkspaceFactoryClass();
            propertySet = workspaceFactory.ReadConnectionPropertiesFromFile(theSDEPath);
            IWorkspace theWorkspace = workspaceFactory.Open(propertySet, 0);

            IEnumDataset theRelationships = theWorkspace.get_Datasets(esriDatasetType.esriDTRelationshipClass);
            theRelationships.Reset();

            String theOutputString = "";

            IDataset theRelationship = theRelationships.Next();
            while (theRelationship != null)
            {
                theOutputString += theRelationship.Name + ",";
                theRelationship = theRelationships.Next();
            }

            IEnumDataset theFeatureDatasets = theWorkspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
            theFeatureDatasets.Reset();
            IDataset theFD = theFeatureDatasets.Next();
            while (theFD != null)
            {
                IFeatureDataset theFeatureDataset = (IFeatureDataset)theFD;
                IEnumDataset theRCinFD = theFeatureDataset.Subsets;
                theRCinFD.Reset();
                IDataset theRC = theRCinFD.Next();
                while (theRC != null)
                {
                    if (theRC.Type == esriDatasetType.esriDTRelationshipClass)
                    {
                        theOutputString += theFD.Name + "\\" + theRC.Name + ",";
                    }
                    
                    theRC = theRCinFD.Next();
                }

                theFD = theFeatureDatasets.Next();
            }

            theOutputString = theOutputString.Remove(theOutputString.Length - 1);
            
            IGPParameter theOutput = (IGPParameter)paramvalues.get_Element(1); //This is the last parameter in this case
            IGPValue theParameterValue = m_GPUtilities.UnpackGPValue(theOutput);
            //theParameterValue.SetAsText(theRelationships);
            theParameterValue.SetAsText(theOutputString);
            m_GPUtilities.PackGPValue(theParameterValue, theOutput);
        }
        public IName FullName
        {
            get
            {
                IGPFunctionFactory functionFactory = new SubtypeFunctionFactory();
                return (IName)functionFactory.GetFunctionName("getRelationshipClassList");
            }
        }
        public object GetRenderer(IGPParameter pParam)
        {
            return null;
        }
        public int HelpContext
        {
            get { return 0; }
        }
        public string HelpFile
        {
            get { return ""; }
        }
        public bool IsLicensed()
        {
            return true;
        }
        public string MetadataFile
        {
            get { return m_metadatafile; }
        }
        public string Name
        {
            get { return m_ToolName; }
        }
        public IArray ParameterInfo
        {
            get
            {
                IArray parameters = new ArrayClass();

                IGPParameterEdit3 theInput = new GPParameterClass();
                theInput.DataType = new DEWorkspaceTypeClass();
                theInput.Value = new DEWorkspaceClass();
                theInput.Direction = esriGPParameterDirection.esriGPParameterDirectionInput;
                theInput.DisplayName = "Input workspace to read relationships from:";
                theInput.Name = "theWorkspace";
                theInput.ParameterType = esriGPParameterType.esriGPParameterTypeRequired;
                parameters.Add(theInput);

                IGPParameterEdit3 newOutputParameter = new GPParameterClass();
                newOutputParameter.DataType = new GPStringTypeClass();
                newOutputParameter.Value = new GPStringClass();
                newOutputParameter.Direction = esriGPParameterDirection.esriGPParameterDirectionOutput;
                newOutputParameter.Name = "outputString";
                newOutputParameter.ParameterType = esriGPParameterType.esriGPParameterTypeDerived;
                parameters.Add(newOutputParameter);

                return parameters;
            }
        }
        public void UpdateMessages(IArray paramvalues, IGPEnvironmentManager pEnvMgr, IGPMessages Messages)
        {
            IGPMessage msg = (IGPMessage)Messages;
            if (msg.IsError())
                return;
            IGPParameter parameter = (IGPParameter)paramvalues.get_Element(0);
            IGPValue parameterValue = m_GPUtilities.UnpackGPValue(parameter);
            return;
        }
        public void UpdateParameters(IArray paramvalues, IGPEnvironmentManager pEnvMgr)
        {
            m_Parameters = paramvalues;
            IGPValue parameterValue = m_GPUtilities.UnpackGPValue(m_Parameters.get_Element(0));
            if (parameterValue.IsEmpty() == false)
            {
                String one = "1";
            }
        }
        public IGPMessages Validate(IArray paramvalues, bool updateValues, IGPEnvironmentManager envMgr)
        {
            if (m_Parameters == null)
                m_Parameters = ParameterInfo;
            if (updateValues == true)
            {
                UpdateParameters(paramvalues, envMgr);
            }
            IGPMessages validateMsgs = m_GPUtilities.InternalValidate(m_Parameters, paramvalues, updateValues, true, envMgr);
            UpdateMessages(paramvalues, envMgr, validateMsgs);
            return validateMsgs;
        }
        #endregion
    }

    public class getSubtypeField:IGPFunction2
    {

        //private string functionName = "getSubtypeField";
        private string m_ToolName = "getSubtypeField"; //Function Name
        private string m_metadatafile = "";
        private IArray m_Parameters;             // Array of Parameters
        private IGPUtilities m_GPUtilities;      // GPUtilities object


        public getSubtypeField() {
            m_GPUtilities = new GPUtilitiesClass();
        }

        public string Name
        {
            get { return m_ToolName; }
        }
        public string DisplayName
        {
            get { return "Get Subtype Field"; }
        }

        public IArray ParameterInfo
        {
            get
            {
                //Array to the hold the parameters  
                IArray parameters = new ArrayClass();

                //Input DataType is GPFeatureLayerType
                IGPParameterEdit3 inputParameter = new GPParameterClass();
                inputParameter.DataType = new GPFeatureLayerTypeClass();

                /*
                // Default Value object is GPFeatureLayer
                inputParameter.Value = new GPFeatureLayerClass();

                // Set Input Parameter properties
                inputParameter.Direction = esriGPParameterDirection.esriGPParameterDirectionInput;
                inputParameter.DisplayName = "Input Features";
                inputParameter.Name = "input_features";
                inputParameter.ParameterType = esriGPParameterType.esriGPParameterTypeRequired;
                parameters.Add(inputParameter);
                */

                IGPParameterEdit3 theInput = new GPParameterClass();
                theInput.DataType = new DETableTypeClass();
                theInput.Value = new DETableClass();
                theInput.Direction = esriGPParameterDirection.esriGPParameterDirectionInput;
                theInput.DisplayName = "Input Table View";
                theInput.Name = "theTableView";
                theInput.ParameterType = esriGPParameterType.esriGPParameterTypeRequired;
                parameters.Add(theInput);

                /*
                // Area field parameter
                inputParameter = new GPParameterClass();
                inputParameter.DataType = new GPStringTypeClass();

                // Value object is GPString
                IGPString gpStringValue = new GPStringClass();
                gpStringValue.Value = "Area";
                inputParameter.Value = (IGPValue)gpStringValue;

                // Set field name parameter properties
                inputParameter.Direction = esriGPParameterDirection.esriGPParameterDirectionInput;
                inputParameter.DisplayName = "Area Field Name";
                inputParameter.Name = "field_name";
                inputParameter.ParameterType = esriGPParameterType.esriGPParameterTypeRequired;
                parameters.Add(inputParameter);
                */


                /*
                // Output parameter (Derived) and data type is DEFeatureClass
                IGPParameterEdit3 outputParameter = new GPParameterClass();
                outputParameter.DataType = new GPFeatureLayerTypeClass();

                // Value object is DEFeatureClass
                outputParameter.Value = new DEFeatureClassClass();

                // Set output parameter properties
                outputParameter.Direction = esriGPParameterDirection.esriGPParameterDirectionOutput;
                outputParameter.DisplayName = "Output FeatureClass";
                outputParameter.Name = "out_featureclass";
                outputParameter.ParameterType = esriGPParameterType.esriGPParameterTypeDerived;

                // Create a new schema object - schema means the structure or design of the feature class (field information, geometry information, extent)
                IGPFeatureSchema outputSchema = new GPFeatureSchemaClass();
                IGPSchema schema = (IGPSchema)outputSchema;

                // Clone the schema from the dependency. 
                //This means update the output with the same schema as the input feature class (the dependency).                                
                schema.CloneDependency = true;

                // Set the schema on the output because this tool will add an additional field.
                outputParameter.Schema = outputSchema as IGPSchema;
                outputParameter.AddDependency("input_features");
                parameters.Add(outputParameter);
                */

                //New output Parameter
                
                IGPParameterEdit3 newOutputParameter = new GPParameterClass();
                newOutputParameter.DataType = new GPStringTypeClass();
                newOutputParameter.Value = new GPStringClass();
                newOutputParameter.Direction = esriGPParameterDirection.esriGPParameterDirectionOutput;
                newOutputParameter.Name = "outputString";
                newOutputParameter.ParameterType = esriGPParameterType.esriGPParameterTypeDerived;
                parameters.Add(newOutputParameter);
                


                return parameters;



                /*
                //Array to the hold the parameters	
                IArray parameters = new ArrayClass();

                IGPParameterEdit3 theInput = new GPParameterClass();
                theInput.DataType = new DETableTypeClass();
                theInput.Value = new DETableClass();
                theInput.Direction = esriGPParameterDirection.esriGPParameterDirectionInput;
                theInput.DisplayName = "Input Table View";
                theInput.Name = "theTableView";
                theInput.ParameterType = esriGPParameterType.esriGPParameterTypeRequired;
                parameters.Add(theInput);

                // Output parameter (Derived) and data type is DEFeatureClass
                IGPParameterEdit3 outputParameter = new GPParameterClass();
                outputParameter.DataType = new GPStringTypeClass();
                outputParameter.Value = new GPStringClass();
                outputParameter.Direction = esriGPParameterDirection.esriGPParameterDirectionOutput;
                outputParameter.DisplayName = "OutputFieldName";
                outputParameter.Name = "outFieldName";
                outputParameter.ParameterType = esriGPParameterType.esriGPParameterTypeDerived;
                parameters.Add(outputParameter);


                return parameters;
                 * */
            }
        }

        public IGPMessages Validate(IArray paramvalues, bool updateValues, IGPEnvironmentManager envMgr)
        {
            if (m_Parameters == null)
                m_Parameters = ParameterInfo;

            // Call UpdateParameters(). 
            // Only Call if updatevalues is true.
            if (updateValues == true)
            {
                UpdateParameters(paramvalues, envMgr);
            }

            // Call InternalValidate (Basic Validation). Are all the required parameters supplied?
            // Are the Values to the parameters the correct data type?
            IGPMessages validateMsgs = m_GPUtilities.InternalValidate(m_Parameters, paramvalues, updateValues, true, envMgr);

            // Call UpdateMessages();
            UpdateMessages(paramvalues, envMgr, validateMsgs);

            // Return the messages
            return validateMsgs;

        }

        // This method will update the output parameter value with the additional area field.
        public void UpdateParameters(IArray paramvalues, IGPEnvironmentManager pEnvMgr)
        {
            m_Parameters = paramvalues;

            // Retrieve the input parameter value
            IGPValue parameterValue = m_GPUtilities.UnpackGPValue(m_Parameters.get_Element(0));

            // Get the derived output feature class schema and empty the additional fields. This will ensure you don't get dublicate entries.
            //IGPParameter3 derivedFeatures = (IGPParameter3)paramvalues.get_Element(2);
            //IGPFeatureSchema schema = (IGPFeatureSchema)derivedFeatures.Schema;
            //schema.AdditionalFields = null;

            // If we have an input value, create a new field based on the field name the user entered.            
            if (parameterValue.IsEmpty() == false)
            {
                //IGPParameter3 fieldNameParameter = (IGPParameter3)paramvalues.get_Element(1);
                //string fieldName = fieldNameParameter.Value.GetAsText();

                // Check if the user's input field already exists
                /*
                IField areaField = m_GPUtilities.FindField(parameterValue, fieldName);
                if (areaField == null)
                {
                    IFieldsEdit fieldsEdit = new FieldsClass();
                    IFieldEdit fieldEdit = new FieldClass();
                    fieldEdit.Name_2 = fieldName;
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
                    fieldsEdit.AddField(fieldEdit);

                    // Add an additional field for the area values to the derived output.
                    IFields fields = fieldsEdit as IFields;
                    schema.AdditionalFields = fields;
                }
                 * */

            }
        }

        // Called after returning from the update parameters routine. 
        // You can examine the messages created from internal validation and change them if desired. 
        public void UpdateMessages(IArray paramvalues, IGPEnvironmentManager pEnvMgr, IGPMessages Messages)
        {
            IGPMessage msg = (IGPMessage)Messages;
            if (msg.IsError())
                return;
            IGPParameter parameter = (IGPParameter)paramvalues.get_Element(0);
            IGPValue parameterValue = m_GPUtilities.UnpackGPValue(parameter);
            return;
        }
   
        public void Execute(IArray paramvalues, ITrackCancel trackcancel, IGPEnvironmentManager envMgr, IGPMessages message)
        {

            // Get the first Input Parameter
            IGPParameter parameter = (IGPParameter)paramvalues.get_Element(0);

            // UnPackGPValue. This ensures you get the value either form the dataelement or GpVariable (modelbuilder)
            IGPValue parameterValue = m_GPUtilities.UnpackGPValue(parameter);


            IDataset theDataset = m_GPUtilities.OpenDataset(parameterValue);
            String thePath = theDataset.Workspace.PathName.ToString() + "\\" + theDataset.Name.ToString();
            message.AddMessage(thePath);
            ITable theITable = m_GPUtilities.OpenTableFromString(thePath);
            ISubtypes theSubtypes = (ISubtypes)theITable;
            String theSubtypeFieldString = theSubtypes.SubtypeFieldName.ToString();
            message.AddMessage(theSubtypeFieldString);
            if (theSubtypeFieldString == "" || theSubtypeFieldString == null)
            {
                theSubtypeFieldString = "NoSubtypeField";
            }


            IGPParameter theOutput = (IGPParameter)paramvalues.get_Element(1); //This is the last parameter in this case
            IGPValue theParameterValue = m_GPUtilities.UnpackGPValue(theOutput);
            theParameterValue.SetAsText(theSubtypeFieldString);
            m_GPUtilities.PackGPValue(theParameterValue, theOutput);

            /*
            // Open Input Dataset
            IFeatureClass inputFeatureClass;
            IQueryFilter qf;
            m_GPUtilities.DecodeFeatureLayer(parameterValue, out inputFeatureClass, out qf);

            if (inputFeatureClass == null)
            {
                message.AddError(2, "Could not open input dataset.");
                return;
            }

            // Add the field if it does not exist.
            int indexA;

            parameter = (IGPParameter)paramvalues.get_Element(1);
            string field = parameter.Value.GetAsText();


            indexA = inputFeatureClass.FindField(field);
            if (indexA < 0)
            {
                IFieldEdit fieldEdit = new FieldClass();
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
                fieldEdit.Name_2 = field;
                inputFeatureClass.AddField(fieldEdit);
            }

            int featcount = inputFeatureClass.FeatureCount(null);

            //Set the properties of the Step Progressor
            IStepProgressor pStepPro = (IStepProgressor)trackcancel;
            pStepPro.MinRange = 0;
            pStepPro.MaxRange = featcount;
            pStepPro.StepValue = (1);
            pStepPro.Message = "Calculating Area";
            pStepPro.Position = 0;
            pStepPro.Show();

            // Create an Update Cursor
            indexA = inputFeatureClass.FindField(field);
            IFeatureCursor updateCursor = inputFeatureClass.Update(qf, false);
            IFeature updateFeature = updateCursor.NextFeature();
            IGeometry geometry;
            IArea area;
            double dArea;

            while (updateFeature != null)
            {
                geometry = updateFeature.Shape;
                area = (IArea)geometry;
                dArea = area.Area;
                updateFeature.set_Value(indexA, dArea);
                updateCursor.UpdateFeature(updateFeature);
                updateFeature.Store();
                updateFeature = updateCursor.NextFeature();
                pStepPro.Step();
            }

            pStepPro.Hide();

            // Release the update cursor to remove the lock on the input data.
            System.Runtime.InteropServices.Marshal.ReleaseComObject(updateCursor);
            */


        }

        public IName FullName
        {
            get
            {
                IGPFunctionFactory functionFactory = new SubtypeFunctionFactory();
                return (IName)functionFactory.GetFunctionName("getSubtypeField");
            }
        }
        public object GetRenderer(IGPParameter pParam)
        {
            return null;
        }
        public int HelpContext
        {
            get { return 0; }
        }
        public string HelpFile
        {
            get { return ""; }
        }
        public bool IsLicensed()
        {
            return true;
        }

        public string MetadataFile
        {
            get { return m_metadatafile; }
        }
        public UID DialogCLSID
        {
            get { return null; }
        }
 
    }

    public class getDomainList : IGPFunction2
    {
        private string m_ToolName = "getDomainList"; //Function Name
        private string m_metadatafile = "";
        private IArray m_Parameters;             // Array of Parameters
        private IGPUtilities m_GPUtilities;      // GPUtilities object
        
        public getDomainList() {
            m_GPUtilities = new GPUtilitiesClass();
        }
        public string Name
        {
            get { return m_ToolName; }
        }
        public string DisplayName
        {
            get { return "Get Domain List"; }
        }
        public IArray ParameterInfo
        {
            get
            {
                //Array to the hold the parameters  
                IArray parameters = new ArrayClass();

                IGPParameterEdit3 theInput = new GPParameterClass();
                theInput.DataType = new DEWorkspaceTypeClass();
                theInput.Value = new DEWorkspaceClass();
                theInput.Direction = esriGPParameterDirection.esriGPParameterDirectionInput;
                theInput.DisplayName = "Input workspace to read domains from:";
                theInput.Name = "theWorkspace";
                theInput.ParameterType = esriGPParameterType.esriGPParameterTypeRequired;
                parameters.Add(theInput);

                IGPParameterEdit3 newOutputParameter = new GPParameterClass();
                newOutputParameter.DataType = new GPMultiValueTypeClass(); //new GPStringTypeClass();
                newOutputParameter.Value = new GPMultiValueClass(); //new GPStringClass();
                newOutputParameter.Direction = esriGPParameterDirection.esriGPParameterDirectionOutput;
                newOutputParameter.Name = "outputStringList";
                newOutputParameter.ParameterType = esriGPParameterType.esriGPParameterTypeDerived;
                parameters.Add(newOutputParameter);

                return parameters;

            }
        }
        public IGPMessages Validate(IArray paramvalues, bool updateValues, IGPEnvironmentManager envMgr)
        {
            if (m_Parameters == null)
                m_Parameters = ParameterInfo;

            // Call UpdateParameters(). 
            // Only Call if updatevalues is true.
            if (updateValues == true)
            {
                UpdateParameters(paramvalues, envMgr);
            }

            // Call InternalValidate (Basic Validation). Are all the required parameters supplied?
            // Are the Values to the parameters the correct data type?
            IGPMessages validateMsgs = m_GPUtilities.InternalValidate(m_Parameters, paramvalues, updateValues, true, envMgr);

            // Call UpdateMessages();
            UpdateMessages(paramvalues, envMgr, validateMsgs);

            // Return the messages
            return validateMsgs;

        }
        public void UpdateParameters(IArray paramvalues, IGPEnvironmentManager pEnvMgr)
        {
            m_Parameters = paramvalues;

            // Retrieve the input parameter value
            IGPValue parameterValue = m_GPUtilities.UnpackGPValue(m_Parameters.get_Element(0));

            // If we have an input value, create a new field based on the field name the user entered.            
            if (parameterValue.IsEmpty() == false)
            {
            }
        }
        public void UpdateMessages(IArray paramvalues, IGPEnvironmentManager pEnvMgr, IGPMessages Messages)
        {
            // Check for error messages
            IGPMessage msg = (IGPMessage)Messages;
            if (msg.IsError())
                return;

            // Get the first Input Parameter
            IGPParameter parameter = (IGPParameter)paramvalues.get_Element(0);

            // UnPackGPValue. This ensures you get the value either form the dataelement or GpVariable (modelbuilder)
            IGPValue parameterValue = m_GPUtilities.UnpackGPValue(parameter);

            return;
        }
        public void Execute(IArray paramvalues, ITrackCancel trackcancel, IGPEnvironmentManager envMgr, IGPMessages message)
        {

            // Get the first Input Parameter
            IGPParameter parameter = (IGPParameter)paramvalues.get_Element(0);

            // UnPackGPValue. This ensures you get the value either form the dataelement or GpVariable (modelbuilder)
            IGPValue parameterValue = m_GPUtilities.UnpackGPValue(parameter);

            String theSDEPath = parameterValue.GetAsText();

            message.AddMessage("Working through database:" + theSDEPath);

            ESRI.ArcGIS.esriSystem.IPropertySet propertySet = new ESRI.ArcGIS.esriSystem.PropertySetClass();
            IWorkspaceFactory workspaceFactory = new ESRI.ArcGIS.DataSourcesGDB.SdeWorkspaceFactoryClass();
            propertySet = workspaceFactory.ReadConnectionPropertiesFromFile(theSDEPath);

            IWorkspace theWorkspace = workspaceFactory.Open(propertySet, 0);

            IWorkspaceDomains theWorkspaceDomains = (IWorkspaceDomains)theWorkspace;

            String theDomainListString = "";

            IEnumDomain enumDomain = theWorkspaceDomains.Domains;
            IDomain domain = enumDomain.Next();
            while (domain != null)
            {
                message.AddMessage(domain.Name);
                if (theDomainListString != ""){
                    theDomainListString += ",";
                }
                theDomainListString += domain.Name;
                domain = enumDomain.Next();
            }

            message.AddMessage(theDomainListString);

            //String theDomainListString = "Something";

            IGPParameter theOutput = (IGPParameter)paramvalues.get_Element(1); //This is the last parameter in this case
            IGPValue theParameterValue = m_GPUtilities.UnpackGPValue(theOutput);
            theParameterValue.SetAsText(theDomainListString);
            m_GPUtilities.PackGPValue(theParameterValue, theOutput);
        }
        public IName FullName
        {
            get
            {
                IGPFunctionFactory functionFactory = new SubtypeFunctionFactory();
                return (IName)functionFactory.GetFunctionName("getDomainList");
            }
        }
        public object GetRenderer(IGPParameter pParam)
        {
            return null;
        }
        public int HelpContext
        {
            get { return 0; }
        }
        public string HelpFile
        {
            get { return ""; }
        }
        public bool IsLicensed()
        {
            return true;
        }

        public string MetadataFile
        {
            get { return m_metadatafile; }
        }
        public UID DialogCLSID
        {
            get { return null; }
        }
    }
    /*
    public class createGeometricNetwork :IGPFunction2
    {
        private string m_ToolName = "createGeometricNetwork"; //Function Name
        private string m_metadatafile = "";
        private IArray m_Parameters;             // Array of Parameters
        private IGPUtilities m_GPUtilities;      // GPUtilities object

        #region IGPFunction2 Members

        public UID DialogCLSID
        {
            get { return null; }
        }

        public string DisplayName
        {
            get { return m_ToolName; }
        }

        public void Execute(IArray paramvalues, ITrackCancel trackcancel, IGPEnvironmentManager envMgr, IGPMessages message)
        {
            INetworkLoader2 networkLoader2 = new NetworkLoaderClass();

        }

        public IName FullName
        {
            get {
                IGPFunctionFactory functionFactory = new SubtypeFunctionFactory();
                return (IName)functionFactory.GetFunctionName("createGeometricNetwork");
            }
        }

        public object GetRenderer(IGPParameter pParam)
        {
            return null;
        }

        public int HelpContext
        {
            get { return 0; }
        }

        public string HelpFile
        {
            get { return ""; }
        }

        public bool IsLicensed()
        {
            return true;
        }

        public string MetadataFile
        {
            get { return m_metadatafile; }
        }

        public string Name
        {
            get { return m_ToolName; }
        }

        public IArray ParameterInfo
        {
            get { throw new NotImplementedException(); }
        }

        public void UpdateMessages(IArray paramvalues, IGPEnvironmentManager pEnvMgr, IGPMessages Messages)
        {
            throw new NotImplementedException();
        }

        public void UpdateParameters(IArray paramvalues, IGPEnvironmentManager pEnvMgr)
        {
            throw new NotImplementedException();
        }

        public IGPMessages Validate(IArray paramvalues, bool updateValues, IGPEnvironmentManager envMgr)
        {
            if (m_Parameters == null)
                m_Parameters = ParameterInfo;

            // Call UpdateParameters(). 
            // Only Call if updatevalues is true.
            if (updateValues == true)
            {
                UpdateParameters(paramvalues, envMgr);
            }

            // Call InternalValidate (Basic Validation). Are all the required parameters supplied?
            // Are the Values to the parameters the correct data type?
            IGPMessages validateMsgs = m_GPUtilities.InternalValidate(m_Parameters, paramvalues, updateValues, true, envMgr);

            // Call UpdateMessages();
            UpdateMessages(paramvalues, envMgr, validateMsgs);

            // Return the messages
            return validateMsgs;
        }

        #endregion
    }
    */
        [
    Guid("68BD901D-052A-441C-AEDE-BC001C0E2A0A"),
    ComVisible(true)
    ]

    public class SubtypeFunctionFactory : IGPFunctionFactory
    {
        public IGPFunctionName CreateGPFunctionNames(long index)
        {
            IGPFunctionName functionName = new GPFunctionNameClass();
            IGPName name;

            switch (index)
            {
                case (0):
                    name = (IGPName)functionName;
                    name.Category = "";
                    name.Description = "Find the subtype field for a featureclass";
                    name.DisplayName = "Get Subtype Field";
                    name.Name = "getSubtypeField";
                    name.Factory = (IGPFunctionFactory)this;
                    break;
                case (1):
                    name = (IGPName)functionName;
                    name.Category = "";
                    name.Description = "Return a list of the Domains in a workspace";
                    name.DisplayName = "Get Domain List";
                    name.Name = "getDomainList";
                    name.Factory = (IGPFunctionFactory)this;
                    break;
                case (2):
                    name = (IGPName)functionName;
                    name.Category = "";
                    name.Description = "Return a list of the Relationship Classes in a workspace";
                    name.DisplayName = "Get Relationship Class List";
                    name.Name = "getRelationshipClassList";
                    name.Factory = (IGPFunctionFactory)this;
                    break;
                case (3):
                    name = (IGPName)functionName;
                    name.Category = "";
                    name.Description = "Return a list of the subtypes in a feature class";
                    name.DisplayName = "Get Subtype List";
                    name.Name = "getSubtypeList";
                    name.Factory = (IGPFunctionFactory)this;
                    break;
                case (4):
                    name = (IGPName)functionName;
                    name.Category = "";
                    name.Description = "Change the Expression of an Annotation Layer";
                    name.DisplayName = "Change Annotation Expression";
                    name.Name = "changeAnnotationLabelField";
                    name.Factory = (IGPFunctionFactory)this;
                    break;
                case (5):
                    name = (IGPName)functionName;
                    name.Category = "";
                    name.Description = "Get Subtype Code From Description";
                    name.DisplayName = "Get Subtype Code From Description";
                    name.Name = "getSubtypeCodeFromDescription";
                    name.Factory = (IGPFunctionFactory)this;
                    break;
                case (6):
                    name = (IGPName)functionName;
                    name.Category = "";
                    name.Description = "Get Default for Field for Subtype";
                    name.DisplayName = "Get Default for Field for Subtype";
                    name.Name = "getDefaultForFieldForSubtype";
                    name.Factory = (IGPFunctionFactory)this;
                    break;
                case (7):
                    name = (IGPName)functionName;
                    name.Category = "";
                    name.Description = "Get Domain for Field for Subtype";
                    name.DisplayName = "Get Domain for Field for Subtype";
                    name.Name = "getDomainForFieldForSubtype";
                    name.Factory = (IGPFunctionFactory)this;
                    break;
                    
                     
            }

            return functionName;
        }

        // This is the name of the function factory. 
        // This is used when generating the Toolbox containing the function tools of the factory.
        public string Name
        {
            get { return "SubtypeTools"; }
        }

        // This is the alias name of the factory.
        public string Alias
        {
            get { return "subtypes"; }
        }

        // This method will create and return a function object based upon the input name.
        public IGPFunction GetFunction(string Name)
        {
            IGPFunction gpFunction;
            switch (Name)
            {
                case ("getSubtypeField"):
                    gpFunction = new getSubtypeField();
                    return gpFunction;
                case ("getDomainList"):
                    gpFunction = new getDomainList();
                    return gpFunction;
                case ("getRelationshipClassList"):
                    gpFunction = new getRelationshipClassList();
                    return gpFunction;
                case ("getSubtypeList"):
                    gpFunction = new getSubtypeList();
                    return gpFunction;
                case ("changeAnnotationLabelField"):
                    gpFunction = new changeAnnotationLabelField();
                    return gpFunction;
                case ("getSubtypeCodeFromDescription"):
                    gpFunction = new getSubtypeCodeFromDescription();
                    return gpFunction;
                case ("getDefaultForFieldForSubtype"):
                    gpFunction = new getDefaultForFieldForSubtype();
                    return gpFunction;
                case ("getDomainForFieldForSubtype"):
                    gpFunction = new getDomainForFieldForSubtype();
                    return gpFunction;

                    
            }

            return null;
        }

        // This method will create and return a function name object based upon the input name.
        public IGPName GetFunctionName(string Name)
        {
            IGPName gpName = new GPFunctionNameClass();

            switch (Name)
            {
                case ("getSubtypeField"):
                    return (IGPName)CreateGPFunctionNames(0);
                case ("getDomainList"):
                    return (IGPName)CreateGPFunctionNames(1);
                case ("getRelationshipClassList"):
                    return (IGPName)CreateGPFunctionNames(2);
                case ("getSubtypeList"):
                    return (IGPName)CreateGPFunctionNames(3);
                case ("changeAnnotationLabelField"):
                    return (IGPName)CreateGPFunctionNames(4);
                case ("getSubtypeCodeFromDescription"):
                    return (IGPName)CreateGPFunctionNames(5);
                case ("getDefaultForFieldForSubtype"):
                    return (IGPName)CreateGPFunctionNames(6);
                case ("getDomainForFieldForSubtype"):
                    return (IGPName)CreateGPFunctionNames(7);
            }
            return null;
        }

        // This method will create and return an enumeration of function names that the factory supports.
        public IEnumGPName GetFunctionNames()
        {
            IArray nameArray = new EnumGPNameClass();
            nameArray.Add(CreateGPFunctionNames(0));
            nameArray.Add(CreateGPFunctionNames(1));
            nameArray.Add(CreateGPFunctionNames(2));
            nameArray.Add(CreateGPFunctionNames(3));
            nameArray.Add(CreateGPFunctionNames(4));
            nameArray.Add(CreateGPFunctionNames(5));
            nameArray.Add(CreateGPFunctionNames(6));
            nameArray.Add(CreateGPFunctionNames(7));
            return (IEnumGPName)nameArray;
        }

        public UID CLSID
        {
            get
            {
                UID id = new UIDClass();
                id.Value = this.GetType().GUID.ToString("B");
                return id;
            }
        }
        public IEnumGPEnvironment GetFunctionEnvironments()
        {
            return null;
        }
    }
}
