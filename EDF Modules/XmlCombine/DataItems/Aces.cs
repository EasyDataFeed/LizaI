namespace XmlCombine.DataItems
{
    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]

    public class ACES
    {
        private ACESHeader headerField;

        private ACESApp[] appField;

        private ACESFooter footerField;

        private decimal versionField;

        /// <remarks/>
        public ACESHeader Header
        {
            get { return this.headerField; }
            set { this.headerField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("App")]
        public ACESApp[] App
        {
            get { return this.appField; }
            set { this.appField = value; }
        }

        /// <remarks/>
        public ACESFooter Footer
        {
            get { return this.footerField; }
            set { this.footerField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal version
        {
            get { return this.versionField; }
            set { this.versionField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESHeader
    {
        private string companyField;

        private string senderNameField;

        private string senderPhoneField;

        private string senderPhoneExtField;

        private System.DateTime transferDateField;

        private string brandAAIAIDField;

        private string documentTitleField;

        private System.DateTime effectiveDateField;

        private string submissionTypeField;

        private System.DateTime vcdbVersionDateField;

        private System.DateTime qdbVersionDateField;

        private System.DateTime pcdbVersionDateField;

        /// <remarks/>
        public string Company
        {
            get { return this.companyField; }
            set { this.companyField = value; }
        }

        /// <remarks/>
        public string SenderName
        {
            get { return this.senderNameField; }
            set { this.senderNameField = value; }
        }

        /// <remarks/>
        public string SenderPhone
        {
            get { return this.senderPhoneField; }
            set { this.senderPhoneField = value; }
        }

        /// <remarks/>
        public string SenderPhoneExt
        {
            get { return this.senderPhoneExtField; }
            set { this.senderPhoneExtField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime TransferDate
        {
            get { return this.transferDateField; }
            set { this.transferDateField = value; }
        }

        /// <remarks/>
        public string BrandAAIAID
        {
            get { return this.brandAAIAIDField; }
            set { this.brandAAIAIDField = value; }
        }

        /// <remarks/>
        public string DocumentTitle
        {
            get { return this.documentTitleField; }
            set { this.documentTitleField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime EffectiveDate
        {
            get { return this.effectiveDateField; }
            set { this.effectiveDateField = value; }
        }

        /// <remarks/>
        public string SubmissionType
        {
            get { return this.submissionTypeField; }
            set { this.submissionTypeField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime VcdbVersionDate
        {
            get { return this.vcdbVersionDateField; }
            set { this.vcdbVersionDateField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime QdbVersionDate
        {
            get { return this.qdbVersionDateField; }
            set { this.qdbVersionDateField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime PcdbVersionDate
        {
            get { return this.pcdbVersionDateField; }
            set { this.pcdbVersionDateField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESApp
    {
        // private ACESAppTransmissionControlType transmissionControlTypeField;

        private ACESAppBaseVehicle baseVehicleField;

        private ACESAppSubModel subModelField;

        private ACESAppMfrBodyCode mfrBodyCodeField;

        private ACESAppBodyNumDoors bodyNumDoorsField;

        private ACESAppBodyType bodyTypeField;

        private ACESAppDriveType driveTypeField;

        private ACESAppEngineBase engineBaseField;

        private ACESAppEngineDesignation engineDesignationField;

        private ACESAppEngineVIN engineVINField;

        private ACESAppEngineVersion engineVersionField;

        private ACESAppEngineMfr engineMfrField;

        private ACESAppPowerOutput powerOutputField;

        private ACESAppValvesPerEngine valvesPerEngineField;

        private ACESAppFuelDeliveryType fuelDeliveryTypeField;

        private ACESAppFuelDeliverySubType fuelDeliverySubTypeField;

        private ACESAppFuelSystemControlType fuelSystemControlTypeField;

        private ACESAppFuelSystemDesign fuelSystemDesignField;

        private ACESAppAspiration aspirationField;

        private ACESAppCylinderHeadType cylinderHeadTypeField;

        private ACESAppFuelType fuelTypeField;

        private ACESAppIgnitionSystemType ignitionSystemTypeField;

        private ACESAppTransmissionMfrCode transmissionMfrCodeField;

        private ACESAppTransmissionBase transmissionBaseField;

        private ACESAppTransElecControlled transElecControlledField;

        private ACESAppTransmissionMfr transmissionMfrField;

        private ACESAppBedLength bedLengthField;

        private ACESAppBedType bedTypeField;

        private ACESAppWheelBase wheelBaseField;

        private ACESAppBrakeSystem brakeSystemField;

        private ACESAppFrontBrakeType frontBrakeTypeField;

        private ACESAppRearBrakeType rearBrakeTypeField;

        private ACESAppBrakeABS brakeABSField;

        private ACESAppFrontSpringType frontSpringTypeField;

        private ACESAppRearSpringType rearSpringTypeField;

        private ACESAppSteeringSystem steeringSystemField;

        private ACESAppSteeringType steeringTypeField;

        private ACESAppRegion regionField;

        private ACESAppQual[] qualField;

        private string[] noteField;

        private uint qtyField;

        private ACESAppPartType partTypeField;

        private string mfrLabelField;

        private ACESAppPosition positionField;

        private string partField;

        private uint idField;

        private string actionField;

        public ACESAppBaseVehicle BaseVehicle
        {
            get { return this.baseVehicleField; }

            set { this.baseVehicleField = value; }
        }

        /// <remarks/>
        public ACESAppSubModel SubModel
        {
            get { return this.subModelField; }

            set { this.subModelField = value; }
        }

        /// <remarks/>
        public ACESAppMfrBodyCode MfrBodyCode
        {
            get { return this.mfrBodyCodeField; }

            set { this.mfrBodyCodeField = value; }
        }

        /// <remarks/>
        public ACESAppBodyNumDoors BodyNumDoors
        {
            get { return this.bodyNumDoorsField; }

            set { this.bodyNumDoorsField = value; }
        }

        /// <remarks/>
        public ACESAppBodyType BodyType
        {
            get { return this.bodyTypeField; }

            set { this.bodyTypeField = value; }
        }

        /// <remarks/>
        public ACESAppDriveType DriveType
        {
            get { return this.driveTypeField; }

            set { this.driveTypeField = value; }
        }

        /// <remarks/>
        public ACESAppEngineBase EngineBase
        {
            get { return this.engineBaseField; }

            set { this.engineBaseField = value; }
        }

        /// <remarks/>
        public ACESAppEngineDesignation EngineDesignation
        {
            get { return this.engineDesignationField; }

            set { this.engineDesignationField = value; }
        }

        /// <remarks/>
        public ACESAppEngineVIN EngineVIN
        {
            get { return this.engineVINField; }

            set { this.engineVINField = value; }
        }

        /// <remarks/>
        public ACESAppEngineVersion EngineVersion
        {
            get { return this.engineVersionField; }

            set { this.engineVersionField = value; }
        }

        /// <remarks/>
        public ACESAppEngineMfr EngineMfr
        {
            get { return this.engineMfrField; }

            set { this.engineMfrField = value; }
        }

        /// <remarks/>
        public ACESAppPowerOutput PowerOutput
        {
            get { return this.powerOutputField; }

            set { this.powerOutputField = value; }
        }

        /// <remarks/>
        public ACESAppValvesPerEngine ValvesPerEngine
        {
            get { return this.valvesPerEngineField; }

            set { this.valvesPerEngineField = value; }
        }

        /// <remarks/>
        public ACESAppFuelDeliveryType FuelDeliveryType
        {
            get { return this.fuelDeliveryTypeField; }

            set { this.fuelDeliveryTypeField = value; }
        }

        /// <remarks/>
        public ACESAppFuelDeliverySubType FuelDeliverySubType
        {
            get { return this.fuelDeliverySubTypeField; }

            set { this.fuelDeliverySubTypeField = value; }
        }

        /// <remarks/>
        public ACESAppFuelSystemControlType FuelSystemControlType
        {
            get { return this.fuelSystemControlTypeField; }

            set { this.fuelSystemControlTypeField = value; }
        }

        /// <remarks/>
        public ACESAppFuelSystemDesign FuelSystemDesign
        {
            get { return this.fuelSystemDesignField; }

            set { this.fuelSystemDesignField = value; }
        }

        /// <remarks/>
        public ACESAppAspiration Aspiration
        {
            get { return this.aspirationField; }

            set { this.aspirationField = value; }
        }

        /// <remarks/>
        public ACESAppCylinderHeadType CylinderHeadType
        {
            get { return this.cylinderHeadTypeField; }

            set { this.cylinderHeadTypeField = value; }
        }

        /// <remarks/>
        public ACESAppFuelType FuelType
        {
            get { return this.fuelTypeField; }

            set { this.fuelTypeField = value; }
        }

        /// <remarks/>
        public ACESAppIgnitionSystemType IgnitionSystemType
        {
            get { return this.ignitionSystemTypeField; }

            set { this.ignitionSystemTypeField = value; }
        }

        /// <remarks/>
        public ACESAppTransmissionMfrCode TransmissionMfrCode
        {
            get { return this.transmissionMfrCodeField; }

            set { this.transmissionMfrCodeField = value; }
        }

        /// <remarks/>
        public ACESAppTransmissionBase TransmissionBase
        {
            get { return this.transmissionBaseField; }

            set { this.transmissionBaseField = value; }
        }

        /// <remarks/>
        public ACESAppTransElecControlled TransElecControlled
        {
            get { return this.transElecControlledField; }

            set { this.transElecControlledField = value; }
        }

        /// <remarks/>
        public ACESAppTransmissionMfr TransmissionMfr
        {
            get { return this.transmissionMfrField; }

            set { this.transmissionMfrField = value; }
        }

        /// <remarks/>
        public ACESAppBedLength BedLength
        {
            get { return this.bedLengthField; }

            set { this.bedLengthField = value; }
        }

        /// <remarks/>
        public ACESAppBedType BedType
        {
            get { return this.bedTypeField; }

            set { this.bedTypeField = value; }
        }

        /// <remarks/>
        public ACESAppWheelBase WheelBase
        {
            get { return this.wheelBaseField; }

            set { this.wheelBaseField = value; }
        }

        /// <remarks/>
        public ACESAppBrakeSystem BrakeSystem
        {
            get { return this.brakeSystemField; }

            set { this.brakeSystemField = value; }
        }

        /// <remarks/>
        public ACESAppFrontBrakeType FrontBrakeType
        {
            get { return this.frontBrakeTypeField; }

            set { this.frontBrakeTypeField = value; }
        }

        /// <remarks/>
        public ACESAppRearBrakeType RearBrakeType
        {
            get { return this.rearBrakeTypeField; }

            set { this.rearBrakeTypeField = value; }
        }

        /// <remarks/>
        public ACESAppBrakeABS BrakeABS
        {
            get { return this.brakeABSField; }

            set { this.brakeABSField = value; }
        }

        /// <remarks/>
        public ACESAppFrontSpringType FrontSpringType
        {
            get { return this.frontSpringTypeField; }

            set { this.frontSpringTypeField = value; }
        }

        /// <remarks/>
        public ACESAppRearSpringType RearSpringType
        {
            get { return this.rearSpringTypeField; }

            set { this.rearSpringTypeField = value; }
        }

        /// <remarks/>
        public ACESAppSteeringSystem SteeringSystem
        {
            get { return this.steeringSystemField; }

            set { this.steeringSystemField = value; }
        }

        /// <remarks/>
        public ACESAppSteeringType SteeringType
        {
            get { return this.steeringTypeField; }

            set { this.steeringTypeField = value; }
        }

        /// <remarks/>
        public ACESAppRegion Region
        {
            get { return this.regionField; }

            set { this.regionField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Qual")]
        public ACESAppQual[] Qual
        {
            get { return this.qualField; }

            set { this.qualField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Note")]
        public string[] Note
        {
            get { return this.noteField; }

            set { this.noteField = value; }
        }

        /// <remarks/>
        public uint Qty
        {
            get { return this.qtyField; }

            set { this.qtyField = value; }
        }

        /// <remarks/>
        public ACESAppPartType PartType
        {
            get { return this.partTypeField; }

            set { this.partTypeField = value; }
        }

        /// <remarks/>
        public string MfrLabel
        {
            get { return this.mfrLabelField; }

            set { this.mfrLabelField = value; }
        }

        /// <remarks/>
        public ACESAppPosition Position
        {
            get { return this.positionField; }

            set { this.positionField = value; }
        }

        /// <remarks/>
        public string Part
        {
            get { return this.partField; }

            set { this.partField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint id
        {
            get { return this.idField; }

            set { this.idField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string action
        {
            get { return this.actionField; }

            set { this.actionField = value; }
        }

    }

    /// <remarks/>
        [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppBaseVehicle
    {

        private uint idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppSubModel
    {

        private ushort idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppMfrBodyCode
    {

        private ushort idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppBodyNumDoors
    {

        private uint idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppBodyType
    {

        private uint idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppDriveType
    {

        private uint idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppEngineBase
    {

        private ushort idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppEngineDesignation
    {

        private uint idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppEngineVIN
    {

        private uint idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppEngineVersion
    {

        private uint idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppEngineMfr
    {

        private uint idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppPowerOutput
    {

        private ushort idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppValvesPerEngine
    {

        private uint idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppFuelDeliveryType
    {

        private uint idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppFuelDeliverySubType
    {

        private uint idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppFuelSystemControlType
    {

        private uint idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppFuelSystemDesign
    {

        private uint idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppAspiration
    {

        private uint idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppCylinderHeadType
    {

        private uint idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppFuelType
    {

        private uint idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppIgnitionSystemType
    {

        private uint idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppTransmissionMfrCode
    {

        private ushort idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppTransmissionBase
    {

        private uint idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppTransElecControlled
    {

        private uint idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppTransmissionMfr
    {

        private uint idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppBedLength
    {

        private uint idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppBedType
    {

        private uint idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppWheelBase
    {

        private uint idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppBrakeSystem
    {

        private uint idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppFrontBrakeType
    {

        private uint idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppRearBrakeType
    {

        private uint idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppBrakeABS
    {

        private uint idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppFrontSpringType
    {

        private uint idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppRearSpringType
    {

        private uint idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppSteeringSystem
    {

        private uint idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppSteeringType
    {

        private uint idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppRegion
    {

        private uint idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppQual
    {

        private string textField;

        private ushort idField;

        /// <remarks/>
        public string text
        {
            get { return this.textField; }
            set { this.textField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppPartType
    {

        private int idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESAppPosition
    {

        private uint idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ACESFooter
    {

        private int recordCountField;

        /// <remarks/>
        public int RecordCount
        {
            get { return this.recordCountField; }
            set { this.recordCountField = value; }
        }
    }
}