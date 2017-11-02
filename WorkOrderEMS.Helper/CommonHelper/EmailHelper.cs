using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;


namespace WorkOrderEMS.Helper
{
    /// <summary>
    /// This Class is used all the Function related to send email
    /// </summary>
    /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
    /// <CreatedDate>Aug-22-2014</CreatedDate>
    public class EmailHelper
    {
        //Added By Bhushan Dod on Feb-25-2015 for attachment
        public HttpPostedFileBase[] objHttpPostedFileBase { get; set; }
        public FileInfo Info { get; set; }
        public string LogoPath;
        public List<Attachment> test { get; set; }
        public string FirstName = string.Empty;
        public string LastName = string.Empty;
        public string emailid = string.Empty;
        public string vendorEmail = string.Empty;
        public string Password = string.Empty;
        public long UserType = 0;
        public string SigninLink = string.Empty;
        public string RegistrationLink = string.Empty;
        public string RegistrationCode = string.Empty;
        public string MailType = string.Empty;
        public string verificationCode = string.Empty;
        public string VerificationLink = string.Empty;

        public string TemplatePath = string.Empty;
        public string Subject = string.Empty;
        private string strMailBody = string.Empty;
        private string strMailHeading = string.Empty;

        public string Name = string.Empty;
        public string Question = string.Empty;
        public string Answer = string.Empty;
        public string LocationName = string.Empty;
        public string LocationCode = string.Empty;
        public string LocAddress = string.Empty;
        public bool? SafetyHazard = false;

        public string VehicleIdentificationNumber = string.Empty;
        public string VehiclePermitType = string.Empty;
        public string VehicleType = string.Empty;
        public string Year = DateTime.UtcNow.Year.ToString();

        //BOOKAPPOINTMENT email for Doctor and Patients
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string AppointmentDate { get; set; }
        public string PatientFirstName { get; set; }
        public string DoctorName { get; set; }
        public string PatientEmailId { get; set; }
        public string DoctorEmail { get; set; }

        //Added By Bhushan Dod on 16-02-2015
        //Email to Manager WorkOrderRequest of brake not functional
        public long ManagerUserId { get; set; }
        public string ManagerName { get; set; }
        public string ManagerEmail { get; set; }
        public string ProblemDesc { get; set; }
        public long LocationID { get; set; }
        //public string LocationName { get; set; }
        public long RequestBy { get; set; }
        public string UserName { get; set; }

        //For MaintainLog of Email
        public long SentBy { get; set; }

        //For change in stock
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public long ItemQuantity { get; set; }
        public string AssignedTo { get; set; }

        //Added By Bhushan Dod on 11-04-2015
        //Email to Manager and Client for Infraction

        public long ClientUserId { get; set; }
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public string VehicleMake { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleIdentificationNo { get; set; }
        public string DriverName { get; set; }
        public string LicenseNo { get; set; }
        public string PermitType { get; set; }
        public string DeclineReason { get; set; }
        public string DeclinedBy { get; set; }
        //For Facilty Request Sent
        public long WorkRequestAssignmentID { get; set; }
        public string CustomerName { get; set; }
        public Nullable<long> CurrentLocation { get; set; }
        public string DriverLicenseNo { get; set; }
        public string CustomerContact { get; set; }
        public string VehicleYear { get; set; }
        public string VehicleColor { get; set; }
        public string VehicleMake1 { get; set; }
        public string VehicleModel1 { get; set; }
        public long FacilityRequest { get; set; }
        public string AddressFacilityReq { get; set; }
        public string LicensePlateNo { get; set; }

        //Added by Roshan Rahood    on Apr 20 2015
        public string InfractionType { get; set; }
        public string InfractionLevel { get; set; }
        public string InfractionComment { get; set; }
        public string InfractionSumittedOn { get; set; }
        public string PermitRevokeTimeORSusspension { get; set; }
        public string InfractionStatus { get; set; }
        public string InfractionLevelByManager { get; set; }
        public string Comment { get; set; }
        public string TimeSpan { get; set; }

        public string CompanyName { get; set; }
        public string BusinessNo { get; set; }

        //Added By Bhushan Dod on 27-04-2015
        //Email to Manager for Vehicle Mileage Change
        public string PreviousMileage { get; set; }
        public string CurrentMileage { get; set; }
        public string Action { get; set; }

        public string WorkOrderCodeId { get; set; }
        public string FacilityRequestType { get; set; }
        public string FrCustomerName { get; set; }
        public string FrCurrentLocation { get; set; }
        public string FrDriverLicenseNo { get; set; }
        public string FrCustomerContact { get; set; }
        public string FrVehicleYear { get; set; }
        public string FrVehicleColor { get; set; }
        public string FrVehicleMake { get; set; }
        public string FrVehicleModel { get; set; }
        public string FrAddress { get; set; }
        public string FrVehicleTagNo { get; set; }
        public string FrPermitDetailsType { get; set; }

        //Added by Bhushan Dod on 08/06/2015 for Continuous Request
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string WeekDays { get; set; }
        //public Nullable<System.DateTime> StartTime { get; set; }
        public string AssignedTime { get; set; }
        public string ProjectDesc { get; set; }

        //Checkout 
        public string CheckoutUserName { get; set; }
        public string NewCheckoutUserName { get; set; }
        public string QrCodeId { get; set; }
        public string QrcType { get; set; }
        public string TimeAttempted { get; set; }

        public string TaxIDNumber { get; set; }
        public string VehicleIdentification { get; set; }
        public string RenewalCost { get; set; }
        public string PermitDuration { get; set; }
        public string QrcName { get; set; }

        //Permit Type Mail
        public string c350 { get; set; }
        public string c351 { get; set; }
        public string c352 { get; set; }
        public string c353 { get; set; }
        public string c354 { get; set; }
        public string c355 { get; set; }
        public Nullable<decimal> PermitTypePrice { get; set; }

        //Added By Ashwajit Bansod email for eFleet Preventative Maintenance
        public string Category { get; set; }
        public string Meter { get; set; }
        public string VehicleNumber { get; set; }
        public string RemiderMetric { get; set; }
        public string PMMetric { get; set; }
        public string ServiceDueDate { get; set; }

        // Added by Ashwajit Bansod email for eFleet Vehicle Incident
        public string Prevenatability { get; set; }
        public string NumberOfInjuries { get; set; }
        public string VehicleNumberForVehicleIncident { get; set; }
        public string DriverNameForVehicleIncident { get; set; }
        public string City { get; set; }
        public string IncidentDescription { get; set; }
        public string AccidentDate { get; set; }

        // Addded By Ashwajit Bansod for email - eFleet Fueling
        public string DriverNameforFueling { get; set; }
        public string FuelType { get; set; }
        public string GasStatioName { get; set; }
        public string Mileage { get; set; }
        public string CurrentFuel { get; set; }
        public string Total { get; set; }
        public string FuelingDate { get; set; }

        public bool SendEmailWithTemplate(string[] attachedUrl = null)
        {
            bool IsSent = false;
            try
            {

                switch (MailType)
                {

                    case "FORGOTPASSWORD":
                        TemplatePath = ConfigurationManager.AppSettings["ForgotPasswordTemplate"];
                        LogoPath = "<img src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">";
                        Subject = "eTrac:Password Recovery.";
                        strMailBody = GetMailBody(TemplatePath);
                        strMailBody = strMailBody.Replace("##Year", Year);
                        strMailBody = strMailBody.Replace("##Name", FirstName);
                        strMailBody = strMailBody.Replace("##USERNAME", UserName);
                        strMailBody = strMailBody.Replace("##PASSWORD", Password);
                        strMailBody = strMailBody.Replace("##SIGNINLINK", RegistrationLink);
                        strMailBody = strMailBody.Replace("##REGISTRATIONLINK", RegistrationLink);
                        strMailBody = strMailBody.Replace("##Logo", LogoPath);
                        strMailBody = strMailBody.Replace("##Sign", "<img height='50px' src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">");

                        break;

                    case "USERREGISTRATION":

                        string _mailHeading = "";

                        switch ((WorkOrderEMS.Helper.UserType)UserType)
                        {
                            case WorkOrderEMS.Helper.UserType.ITAdministrator:
                                _mailHeading = "You are registered as an IT Administrator.";
                                break;
                            case WorkOrderEMS.Helper.UserType.Administrator:
                                _mailHeading = "You are registered as an Administrator.";
                                break;
                            case WorkOrderEMS.Helper.UserType.Manager:
                                _mailHeading = "You are registered as a Manager.";
                                break;
                            case WorkOrderEMS.Helper.UserType.Employee:
                                _mailHeading = "You are registered as an Employee.";
                                break;
                            case WorkOrderEMS.Helper.UserType.Client:
                                _mailHeading = "You are registered as a Client.";
                                break;
                            default: _mailHeading = "";
                                break;

                        }
                        TemplatePath = ConfigurationManager.AppSettings["RegistrationMailTemplate"];
                        LogoPath = "<img src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">";
                        Subject = "eTrac: Thanks for registration.";
                        strMailHeading = _mailHeading;
                        strMailBody = GetMailBody(TemplatePath);
                        strMailBody = strMailBody.Replace("##Logo", LogoPath);
                        strMailBody = strMailBody.Replace("##Year", Year);
                        strMailBody = strMailBody.Replace("##MailHeading", strMailHeading);
                        strMailBody = strMailBody.Replace("##Email", emailid);
                        strMailBody = strMailBody.Replace("##UserName", UserName);
                        strMailBody = strMailBody.Replace("##Password", Password);


                        strMailBody = strMailBody.Replace("##Name", FirstName + ((!string.IsNullOrEmpty(LastName)) ? (" " + LastName) : ""));

                        strMailBody = strMailBody.Replace("##LocationName", LocationName);
                        strMailBody = strMailBody.Replace("##LocationCode", LocationCode);
                        strMailBody = strMailBody.Replace("##Address", LocAddress);
                        strMailBody = strMailBody.Replace("##REGISTRATIONLINK", RegistrationLink);
                        strMailBody = strMailBody.Replace("##REGISTRATIONCODE", RegistrationCode);
                        strMailBody = strMailBody.Replace("##Sign", "<img height='50px' src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">");
                        break;


                    case "UserAddedToLocation":

                        _mailHeading = "";

                        switch ((WorkOrderEMS.Helper.UserType)UserType)
                        {
                            case WorkOrderEMS.Helper.UserType.ITAdministrator:
                                _mailHeading = "You are added as an IT Administrator.";
                                break;
                            case WorkOrderEMS.Helper.UserType.Administrator:
                                _mailHeading = "You are added as an Administrator.";
                                break;
                            case WorkOrderEMS.Helper.UserType.Manager:
                                _mailHeading = "You are added as a Manager.";
                                break;
                            case WorkOrderEMS.Helper.UserType.Employee:
                                _mailHeading = "You are added as an Employee.";
                                break;
                            case WorkOrderEMS.Helper.UserType.Client:
                                _mailHeading = "You are added as a Client.";
                                break;
                            default: _mailHeading = "";
                                break;
                        }
                        TemplatePath = ConfigurationManager.AppSettings["UserAddedToLocation"];
                        LogoPath = "<img src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">";
                        Subject = "eTrac: Info! You have been added to a new location.";
                        strMailHeading = _mailHeading;
                        strMailBody = GetMailBody(TemplatePath);

                        strMailBody = strMailBody.Replace("##Year", Year);
                        strMailBody = strMailBody.Replace("##MailHeading", strMailHeading);
                        strMailBody = strMailBody.Replace("##Email", emailid);
                        strMailBody = strMailBody.Replace("##UserName", UserName);
                        strMailBody = strMailBody.Replace("##Password", Password);
                        strMailBody = strMailBody.Replace("##LocationCode", LocationCode);

                        strMailBody = strMailBody.Replace("##Name", FirstName + ((!string.IsNullOrEmpty(LastName)) ? (" " + LastName) : ""));
                        strMailBody = strMailBody.Replace("##Logo", LogoPath);
                        strMailBody = strMailBody.Replace("##LocationName", LocationName);
                        strMailBody = strMailBody.Replace("##LocationCode", LocationCode);
                        strMailBody = strMailBody.Replace("##Address", LocAddress);
                        strMailBody = strMailBody.Replace("##REGISTRATIONLINK", RegistrationLink);
                        strMailBody = strMailBody.Replace("##REGISTRATIONCODE", RegistrationCode);
                        strMailBody = strMailBody.Replace("##Sign", "<img height='50px' src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">");
                        break;

                    case "REGISTRATIONMAIL":
                        TemplatePath = ConfigurationManager.AppSettings["RegistrationMailTemplate"];
                        Subject = "eTrac:Thanks for registration.";
                        strMailBody = GetMailBody(TemplatePath);
                        strMailBody = strMailBody.Replace("##Year", Year);
                        strMailBody = strMailBody.Replace("##Name", FirstName + ((!string.IsNullOrEmpty(LastName)) ? (" " + LastName) : ""));
                        strMailBody = strMailBody.Replace("##REGISTRATIONLINK", RegistrationLink);
                        strMailBody = strMailBody.Replace("##REGISTRATIONCODE", RegistrationCode);
                        strMailBody = strMailBody.Replace("##Sign", "<img height='50px' src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">");
                        break;
                  
                    case "LOCATIONVERIFICATION":

                        TemplatePath = ConfigurationManager.AppSettings["LocationVerficationTemplate"];
                        Subject = "eTrac: Location Verification.";
                        strMailHeading = "Welcome to eTrac";
                        strMailBody = GetMailBody(TemplatePath);

                        strMailBody = strMailBody.Replace("##Year", Year);
                        strMailBody = strMailBody.Replace("##MailHeading", strMailHeading);
                        strMailBody = strMailBody.Replace("##Name", FirstName + ((!string.IsNullOrEmpty(LastName)) ? (" " + LastName) : ""));
                        strMailBody = strMailBody.Replace("##LocationName", LocationName);
                        strMailBody = strMailBody.Replace("##VERIFICATIONLINK", VerificationLink);
                        strMailBody = strMailBody.Replace("##Sign", "<img height='50px' src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">");

                        break;                  

                    case "CreateNewUser":

                        TemplatePath = ConfigurationManager.AppSettings["CreateNewUser"];
                        LogoPath = "<img src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">";
                        _mailHeading = "Welcome to eTrac.";

                        switch ((WorkOrderEMS.Helper.UserType)UserType)
                        {
                            case WorkOrderEMS.Helper.UserType.ITAdministrator:
                                _mailHeading = "You are Registered as an IT Administrator.";
                                break;
                            case WorkOrderEMS.Helper.UserType.Administrator:
                                _mailHeading = "You are Registered as an Administrator.";
                                break;
                            case WorkOrderEMS.Helper.UserType.Manager:
                                _mailHeading = "You are Registered as a Manager.";
                                break;
                            case WorkOrderEMS.Helper.UserType.Employee:
                                _mailHeading = "You are Registered as an Employee.";
                                break;
                            case WorkOrderEMS.Helper.UserType.Client:
                                _mailHeading = "You are Registered as a Client.";
                                break;
                            default: _mailHeading = "Welcome to eTrac.";
                                break;

                        }
                        TemplatePath = ConfigurationManager.AppSettings["RegistrationMailTemplate"];
                        Subject = "eTrac: ssYou have been registered in eTrac.";
                        strMailHeading = _mailHeading;
                        strMailBody = GetMailBody(TemplatePath);
                        strMailBody = strMailBody.Replace("##Logo", LogoPath);
                        strMailBody = strMailBody.Replace("##Year", Year);
                        strMailBody = strMailBody.Replace("##LocationName", LocationName);
                        strMailBody = strMailBody.Replace("##MailHeading", strMailHeading);
                        strMailBody = strMailBody.Replace("##Email", emailid);
                        strMailBody = strMailBody.Replace("##UserName", UserName);
                        strMailBody = strMailBody.Replace("##Password", Password);


                        strMailBody = strMailBody.Replace("##Name", FirstName + ((!string.IsNullOrEmpty(LastName)) ? (" " + LastName) : ""));

                        strMailBody = strMailBody.Replace("##Address", LocAddress);
                        strMailBody = strMailBody.Replace("##REGISTRATIONLINK", RegistrationLink);
                        strMailBody = strMailBody.Replace("##REGISTRATIONCODE", RegistrationCode);
                        strMailBody = strMailBody.Replace("##Sign", "<img height='50px' src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">");
                        break;

                    //Added By Bhushan Dod on Feb-25-2015 for WorkOrder Generated Vehicle Brake Not Functional
                    case "WORKORDERREQUEST":

                        TemplatePath = ConfigurationManager.AppSettings["WorkOrderRequestTemplate"];
                        LogoPath = "<img src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">";
                        Subject = "eTrac: WorkOrder Generated for Vehicle Brake Not Functional.";
                        strMailHeading = "Welcome to eTrac";
                        strMailBody = GetMailBody(TemplatePath);
                        strMailBody = strMailBody.Replace("##Year", Year);
                        strMailBody = strMailBody.Replace("##Logo", LogoPath);
                        strMailBody = strMailBody.Replace("##MailHeading", strMailHeading);
                        strMailBody = strMailBody.Replace("##ManagerName", ManagerName);
                        strMailBody = strMailBody.Replace("##ProblemDesc", ProblemDesc);
                        strMailBody = strMailBody.Replace("##UserName", UserName);
                        strMailBody = strMailBody.Replace("##LocationName", LocationName);
                        strMailBody = strMailBody.Replace("##QrCodeId", QrCodeId);
                        strMailBody = strMailBody.Replace("##Sign", "<img height='50px' src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">");

                        break;
                    //Added By Bhushan Dod on Feb-25-2015 for WorkOrder Generated Vehicle Brake Not Functional
                    case "QRCVEHICLEITEMMISSING":

                        TemplatePath = ConfigurationManager.AppSettings["ItemMissingTemplate"];
                        LogoPath = "<img src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">";

                        Subject = "eTrac: Item Missing at " + LocationName;
                        strMailHeading = "Welcome to eTrac";
                        strMailBody = GetMailBody(TemplatePath);
                        strMailBody = strMailBody.Replace("##Year", Year);
                        strMailBody = strMailBody.Replace("##Logo", LogoPath);
                        strMailBody = strMailBody.Replace("##MailHeading", strMailHeading);
                        strMailBody = strMailBody.Replace("##ManagerName", ManagerName);
                        strMailBody = strMailBody.Replace("##ProblemDesc", ProblemDesc);
                        strMailBody = strMailBody.Replace("##QrCodeId", QrCodeId);
                        strMailBody = strMailBody.Replace("##UserName", UserName);
                        strMailBody = strMailBody.Replace("##LocationName", LocationName);
                        strMailBody = strMailBody.Replace("##Sign", "<img height='50px' src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">");

                        break;
                    //Added By Bhushan Dod on April-27-2015 for vehicle mileage
                    case "QRCVEHICLEMILEAGECO":

                        TemplatePath = ConfigurationManager.AppSettings["VehicleMileageTemplate"];
                        LogoPath = "<img src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">";

                        Subject = "eTrac: Vehicle Mileage Change at  " + LocationName;
                        strMailHeading = "Welcome to eTrac";
                        strMailBody = GetMailBody(TemplatePath);
                        strMailBody = strMailBody.Replace("##Year", Year);
                        strMailBody = strMailBody.Replace("##Logo", LogoPath);
                        strMailBody = strMailBody.Replace("##MailHeading", strMailHeading);
                        strMailBody = strMailBody.Replace("##ManagerName", ManagerName);
                        strMailBody = strMailBody.Replace("##Action", Action);
                        strMailBody = strMailBody.Replace("##UserName", UserName);
                        strMailBody = strMailBody.Replace("##LocationName", LocationName);
                        strMailBody = strMailBody.Replace("##PreviousMileage", PreviousMileage);
                        strMailBody = strMailBody.Replace("##CurrentMileage", CurrentMileage);
                        strMailBody = strMailBody.Replace("##QrCodeId", QrCodeId);
                        strMailBody = strMailBody.Replace("##Sign", "<img height='50px' src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">");
                        break;
                    //Added By Bhushan Dod on Feb-26-2015 for Vehicle part damaged
                    case "QRCVEHICLEPARTDAMAGE":
                        LogoPath = "<img src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">";
                        TemplatePath = ConfigurationManager.AppSettings["VehiclePartDamageTemplate"];
                        Subject = "eTrac: Vehicle Part is Damaged at " + LocationName;
                        strMailHeading = "Welcome to eTrac";
                        strMailBody = GetMailBody(TemplatePath);
                        strMailBody = strMailBody.Replace("##Year", Year);
                        strMailBody = strMailBody.Replace("##Logo", LogoPath);
                        strMailBody = strMailBody.Replace("##MailHeading", strMailHeading);
                        strMailBody = strMailBody.Replace("##ManagerName", ManagerName);
                        strMailBody = strMailBody.Replace("##ProblemDesc", ProblemDesc);
                        strMailBody = strMailBody.Replace("##UserName", UserName);
                        strMailBody = strMailBody.Replace("##LocationName", LocationName);
                        strMailBody = strMailBody.Replace("##TimeAttempted", TimeAttempted);
                        strMailBody = strMailBody.Replace("##QrCodeId", QrCodeId);
                        strMailBody = strMailBody.Replace("##Sign", "<img height='50px' src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">");

                        break;
                    //Added By Bhushan Dod on Feb-26-2015 for Vehicle part damaged
                    case "QRCVEHICLEPARTDAMAGEWEEKLY":
                        LogoPath = "<img src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">";
                        TemplatePath = ConfigurationManager.AppSettings["VehiclePartDamageTemplate"];
                        Subject = "eTrac: Weekly Checked Vehicle Part is Damaged at " + LocationName;
                        strMailHeading = "Welcome to eTrac";
                        strMailBody = GetMailBody(TemplatePath);
                        strMailBody = strMailBody.Replace("##Year", Year);
                        strMailBody = strMailBody.Replace("##Logo", LogoPath);
                        strMailBody = strMailBody.Replace("##MailHeading", strMailHeading);
                        strMailBody = strMailBody.Replace("##ManagerName", ManagerName);
                        strMailBody = strMailBody.Replace("##ProblemDesc", ProblemDesc);
                        strMailBody = strMailBody.Replace("##UserName", UserName);
                        strMailBody = strMailBody.Replace("##LocationName", LocationName);
                        strMailBody = strMailBody.Replace("##TimeAttempted", TimeAttempted);
                        strMailBody = strMailBody.Replace("##QrCodeId", QrCodeId);
                        strMailBody = strMailBody.Replace("##Sign", "<img height='50px' src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">");
                        break;
                    //Added By Bhushan Dod on March-13-2015 for Cellphone
                    case "QRCCELLPHONE":
                        LogoPath = "<img src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">";
                        TemplatePath = ConfigurationManager.AppSettings["CellphoneTemplate"];
                        Subject = "eTrac: Cellphone Screen is Cracked at " + LocationName;
                        strMailHeading = "eTrac Parking Services";
                        strMailBody = GetMailBody(TemplatePath);
                        strMailBody = strMailBody.Replace("##Year", Year);
                        strMailBody = strMailBody.Replace("##Logo", LogoPath);
                        strMailBody = strMailBody.Replace("##MailHeading", strMailHeading);
                        strMailBody = strMailBody.Replace("##ManagerName", ManagerName);
                        strMailBody = strMailBody.Replace("##ProblemDesc", ProblemDesc);
                        strMailBody = strMailBody.Replace("##UserName", UserName);
                        strMailBody = strMailBody.Replace("##QrCodeId", QrCodeId);
                        strMailBody = strMailBody.Replace("##QrcName", QrcName);
                        strMailBody = strMailBody.Replace("##LocationName", LocationName);
                        strMailBody = strMailBody.Replace("##Sign", "<img height='50px' src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">");

                        break;
                    //Added By Bhushan Dod on March-13-2015 for Cellphone
                    case "QRCCELLPHONEBUTTONS":
                        LogoPath = "<img src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">";
                        TemplatePath = ConfigurationManager.AppSettings["CellphoneButtonsTemplate"];
                        Subject = "eTrac: Cellphone Buttons Not Present at " + LocationName;
                        strMailHeading = "Welcome to eTrac";
                        strMailBody = GetMailBody(TemplatePath);
                        strMailBody = strMailBody.Replace("##Year", Year);
                        strMailBody = strMailBody.Replace("##Logo", LogoPath);
                        strMailBody = strMailBody.Replace("##MailHeading", strMailHeading);
                        strMailBody = strMailBody.Replace("##ManagerName", ManagerName);
                        strMailBody = strMailBody.Replace("##ProblemDesc", ProblemDesc);
                        strMailBody = strMailBody.Replace("##UserName", UserName);
                        strMailBody = strMailBody.Replace("##QrCodeId", QrCodeId);
                        strMailBody = strMailBody.Replace("##QrcName", QrcName);
                        strMailBody = strMailBody.Replace("##LocationName", LocationName);
                        strMailBody = strMailBody.Replace("##Sign", "<img height='50px' src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">");

                        break;
                    //Added By Bhushan Dod on March-13-2015 for Cellphone
                    case "QRCCELLPHONEFUNCTIONAL":
                        LogoPath = "<img src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">";
                        TemplatePath = ConfigurationManager.AppSettings["CellphoneFunctionalTemplate"];
                        Subject = "eTrac: Cellphone Not Functional at " + LocationName;
                        strMailHeading = "Welcome to eTrac";
                        strMailBody = GetMailBody(TemplatePath);
                        strMailBody = strMailBody.Replace("##Year", Year);
                        strMailBody = strMailBody.Replace("##Logo", LogoPath);
                        strMailBody = strMailBody.Replace("##MailHeading", strMailHeading);
                        strMailBody = strMailBody.Replace("##ManagerName", ManagerName);
                        strMailBody = strMailBody.Replace("##ProblemDesc", ProblemDesc);
                        strMailBody = strMailBody.Replace("##UserName", UserName);
                        strMailBody = strMailBody.Replace("##QrCodeId", QrCodeId);
                        strMailBody = strMailBody.Replace("##QrcName", QrcName);
                        strMailBody = strMailBody.Replace("##LocationName", LocationName);
                        strMailBody = strMailBody.Replace("##Sign", "<img height='50px' src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">");

                        break;
                    //Added By Bhushan Dod on March-18-2015 for INVENTORYSTOCKCREATE
                    case "INVENTORYSTOCKCREATE":
                        LogoPath = "<img src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">";
                        TemplatePath = ConfigurationManager.AppSettings["InventoryStockCreate"];
                        Subject = "eTrac: Inventory Created at " + LocationName;
                        strMailHeading = "Welcome to eTrac";
                        strMailBody = GetMailBody(TemplatePath);
                        strMailBody = strMailBody.Replace("##Year", Year);
                        strMailBody = strMailBody.Replace("##Logo", LogoPath);
                        strMailBody = strMailBody.Replace("##MailHeading", strMailHeading);
                        strMailBody = strMailBody.Replace("##ManagerName", ManagerName);
                        strMailBody = strMailBody.Replace("##UserName", UserName);
                        strMailBody = strMailBody.Replace("##LocationName", LocationName);
                        strMailBody = strMailBody.Replace("##ItemCode", ItemCode);
                        strMailBody = strMailBody.Replace("##ItemName", ItemName);
                        strMailBody = strMailBody.Replace("##ItemDescription", ItemDescription);
                        strMailBody = strMailBody.Replace("##ItemQuantity", ItemQuantity.ToString());
                        strMailBody = strMailBody.Replace("##Sign", "<img height='50px' src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">");

                        break;
                    //Added By Bhushan Dod on March-18-2015 for INVENTORYSTOCKCREATE
                    case "INVENTORYSTOCKUPDATE":
                        LogoPath = "<img src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">";
                        TemplatePath = ConfigurationManager.AppSettings["InventoryStockCreate"];
                        Subject = "eTrac: Inventory Ppdated at " + LocationName;
                        strMailHeading = "Welcome to eTrac";
                        strMailBody = GetMailBody(TemplatePath);
                        strMailBody = strMailBody.Replace("##Year", Year);
                        strMailBody = strMailBody.Replace("##Logo", LogoPath);
                        strMailBody = strMailBody.Replace("##MailHeading", strMailHeading);
                        strMailBody = strMailBody.Replace("##ManagerName", ManagerName);
                        strMailBody = strMailBody.Replace("##UserName", UserName);
                        strMailBody = strMailBody.Replace("##LocationName", LocationName);
                        strMailBody = strMailBody.Replace("##ItemCode", ItemCode);
                        strMailBody = strMailBody.Replace("##ItemName", ItemName);
                        strMailBody = strMailBody.Replace("##ItemDescription", ItemDescription);
                        strMailBody = strMailBody.Replace("##ItemQuantity", ItemQuantity.ToString());
                        strMailBody = strMailBody.Replace("##Sign", "<img height='50px' src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">");

                        break;
                    //Added By Bhushan Dod on March-18-2015 for INVENTORYSTOCKASSIGNED
                    case "INVENTORYSTOCKASSIGNED":
                        LogoPath = "<img src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">";
                        TemplatePath = ConfigurationManager.AppSettings["InventoryStockAssigned"];
                        Subject = "eTrac: Qunatity was Assigned at " + LocationName;
                        strMailHeading = "Welcome to eTrac";
                        strMailBody = GetMailBody(TemplatePath);
                        strMailBody = strMailBody.Replace("##Year", Year);
                        strMailBody = strMailBody.Replace("##Logo", LogoPath);
                        strMailBody = strMailBody.Replace("##MailHeading", strMailHeading);
                        strMailBody = strMailBody.Replace("##ManagerName", ManagerName);
                        strMailBody = strMailBody.Replace("##UserName", UserName);
                        strMailBody = strMailBody.Replace("##LocationName", LocationName);
                        strMailBody = strMailBody.Replace("##ItemCode", ItemCode);
                        strMailBody = strMailBody.Replace("##AssignedTo", AssignedTo);
                        strMailBody = strMailBody.Replace("##ItemQuantity", ItemQuantity.ToString());
                        strMailBody = strMailBody.Replace("##Sign", "<img height='50px' src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">");

                        break;

                    case "SURVEYFEEDBACK":
                        LogoPath = "<img src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">";
                        TemplatePath = ConfigurationManager.AppSettings["SurveyFeedback"];
                        Subject = "eTrac: Survey Feedback";
                        strMailHeading = "Welcome to eTrac";
                        strMailBody = GetMailBody(TemplatePath);
                        strMailBody = strMailBody.Replace("##Year", Year);
                        strMailBody = strMailBody.Replace("##Logo", LogoPath);
                        strMailBody = strMailBody.Replace("##UserName", UserName);
                        strMailBody = strMailBody.Replace("##RegistrationLink", RegistrationLink);
                        strMailBody = strMailBody.Replace("##WorkID", WorkRequestAssignmentID.ToString());
                        strMailBody = strMailBody.Replace("##UserId", SentBy.ToString());
                        strMailBody = strMailBody.Replace("##Sign", "<img height='50px' src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">");
                        break;
                    //Added By Bhushan Dod on May-27-2015 for employee idle case
                    case "EMPLOYEEIDLE":

                        TemplatePath = ConfigurationManager.AppSettings["EmployeeIdle"];
                        LogoPath = "<img src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">";

                        Subject = "eTrac: Employee is Idle at " + LocationName;
                        strMailHeading = "Welcome to eTrac";
                        strMailBody = GetMailBody(TemplatePath);
                        strMailBody = strMailBody.Replace("##Year", Year);
                        strMailBody = strMailBody.Replace("##Logo", LogoPath);
                        strMailBody = strMailBody.Replace("##MailHeading", strMailHeading);
                        strMailBody = strMailBody.Replace("##ManagerName", ManagerName);
                        strMailBody = strMailBody.Replace("##ProblemDesc", ProblemDesc);
                        strMailBody = strMailBody.Replace("##UserName", UserName);
                        strMailBody = strMailBody.Replace("##LocationName", LocationName);
                        strMailBody = strMailBody.Replace("##Sign", "<img height='50px' src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">");
                        break;
                    //Added By Bhushan Dod on June-08-2015 for Continuous Request
                    case "CONTINIOUSREQUEST":

                        TemplatePath = ConfigurationManager.AppSettings["ContinuousRequest"];
                        LogoPath = "<img src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">";

                        Subject = "eTrac: Continuous Request Assigned to You";
                        strMailBody = GetMailBody(TemplatePath);

                        strMailBody = strMailBody.Replace("##Logo", LogoPath);
                        strMailBody = strMailBody.Replace("##Year", Year);
                        strMailBody = strMailBody.Replace("##Employeename", UserName);
                        strMailBody = strMailBody.Replace("##ProjectDesc", ProjectDesc);
                        strMailBody = strMailBody.Replace("##StartDate", StartDate.ToString());
                        strMailBody = strMailBody.Replace("##EndDate", EndDate.ToString());
                        strMailBody = strMailBody.Replace("##WeekDays", WeekDays);
                        strMailBody = strMailBody.Replace("##StartTime", StartTime);
                        strMailBody = strMailBody.Replace("##AssignedTime", AssignedTime.ToString());
                        strMailBody = strMailBody.Replace("##Sign", "<img height='50px' src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">");
                        break;
                    //Added By Bhushan Dod on July-14-2015 for Checkout
                    case "CHECKOUT":

                        TemplatePath = ConfigurationManager.AppSettings["CheckOut"];
                        LogoPath = "<img src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">";

                        Subject = "eTrac: Check In Not Done at " + LocationName;
                        strMailHeading = "Welcome to eTrac";
                        strMailBody = GetMailBody(TemplatePath);
                        strMailBody = strMailBody.Replace("##Year", Year);
                        strMailBody = strMailBody.Replace("##Logo", LogoPath);
                        strMailBody = strMailBody.Replace("##MailHeading", strMailHeading);
                        strMailBody = strMailBody.Replace("##ManagerName", ManagerName);
                        strMailBody = strMailBody.Replace("##CheckoutUserName", CheckoutUserName);
                        strMailBody = strMailBody.Replace("##NewCheckoutUserName", NewCheckoutUserName);
                        strMailBody = strMailBody.Replace("##QrCodeId", QrCodeId);
                        strMailBody = strMailBody.Replace("##UserName", UserName);
                        strMailBody = strMailBody.Replace("##LocationName", LocationName);
                        strMailBody = strMailBody.Replace("##TimeAttempted", TimeAttempted);
                        strMailBody = strMailBody.Replace("##Sign", "<img height='50px' src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">");
                        break;
                    //Added By Bhushan Dod on July-16-2015 for Elevator Inspection
                    case "QRCELEVATORINSPECTION":

                        TemplatePath = ConfigurationManager.AppSettings["ElevatorInspection"];
                        LogoPath = "<img src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">";

                        Subject = "eTrac: No Elevator Inspection Certificate Posted at " + LocationName;
                        strMailHeading = "Welcome to eTrac";
                        strMailBody = GetMailBody(TemplatePath);
                        strMailBody = strMailBody.Replace("##Year", Year);
                        strMailBody = strMailBody.Replace("##Logo", LogoPath);
                        strMailBody = strMailBody.Replace("##MailHeading", strMailHeading);
                        strMailBody = strMailBody.Replace("##ManagerName", ManagerName);
                        strMailBody = strMailBody.Replace("##QrCodeId", QrCodeId);
                        strMailBody = strMailBody.Replace("##UserName", UserName);
                        strMailBody = strMailBody.Replace("##LocationName", LocationName);
                        break;
                    //Added By Bhushan Dod on July-16-2015 for Elevator Inspection
                    case "QRCELEVATORCAPACITY":

                        TemplatePath = ConfigurationManager.AppSettings["ElevatorCapacity"];
                        LogoPath = "<img src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">";

                        Subject = "eTrac: No Elevator Maximum Capacity Posted at " + LocationName;
                        strMailHeading = "Welcome to eTrac";
                        strMailBody = GetMailBody(TemplatePath);
                        strMailBody = strMailBody.Replace("##Year", Year);
                        strMailBody = strMailBody.Replace("##Logo", LogoPath);
                        strMailBody = strMailBody.Replace("##MailHeading", strMailHeading);
                        strMailBody = strMailBody.Replace("##ManagerName", ManagerName);
                        strMailBody = strMailBody.Replace("##QrCodeId", QrCodeId);
                        strMailBody = strMailBody.Replace("##UserName", UserName);
                        strMailBody = strMailBody.Replace("##LocationName", LocationName);
                        break;
                    //Added By Bhushan Dod on July-22-2015 for Qrc Expiration 
                    case "QRCEXPIRATIONMAIL":

                        TemplatePath = ConfigurationManager.AppSettings["QrcExpiration"];
                        LogoPath = "<img src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">";

                        Subject = "eTrac:  QRC Expiration at  " + LocationName;
                        strMailHeading = "Welcome to eTrac";
                        strMailBody = GetMailBody(TemplatePath);
                        strMailBody = strMailBody.Replace("##Year", Year);
                        strMailBody = strMailBody.Replace("##Logo", LogoPath);
                        strMailBody = strMailBody.Replace("##MailHeading", strMailHeading);
                        strMailBody = strMailBody.Replace("##ManagerName", ManagerName);
                        strMailBody = strMailBody.Replace("##QrCodeId", QrCodeId);
                        strMailBody = strMailBody.Replace("##QrcName", ItemName);
                        strMailBody = strMailBody.Replace("##QrcType", QrcType);
                        strMailBody = strMailBody.Replace("##UserName", UserName);
                        strMailBody = strMailBody.Replace("##LocationName", LocationName);
                        strMailBody = strMailBody.Replace("##Sign", "<img height='50px' src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">");
                        break;



                    //Added By Bhushan Dod on Feb-26-2015 for Vehicle part damaged
                    case "QRCSHUTTLEPARTDAMAGE":
                        LogoPath = "<img src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">";
                        TemplatePath = ConfigurationManager.AppSettings["ShuttlePartDamageTemplate"];
                        Subject = "eTrac: Shuttle Part is Damaged at " + LocationName;
                        strMailHeading = "Welcome to eTrac";
                        strMailBody = GetMailBody(TemplatePath);
                        strMailBody = strMailBody.Replace("##Year", Year);
                        strMailBody = strMailBody.Replace("##Logo", LogoPath);
                        strMailBody = strMailBody.Replace("##MailHeading", strMailHeading);
                        strMailBody = strMailBody.Replace("##ManagerName", ManagerName);
                        strMailBody = strMailBody.Replace("##ProblemDesc", ProblemDesc);
                        strMailBody = strMailBody.Replace("##UserName", UserName);
                        strMailBody = strMailBody.Replace("##LocationName", LocationName);
                        strMailBody = strMailBody.Replace("##TimeAttempted", TimeAttempted);
                        strMailBody = strMailBody.Replace("##QrCodeId", QrCodeId);
                        strMailBody = strMailBody.Replace("##Sign", "<img height='50px' src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">");
                        break;

                    //Added By Bhushan Dod on May-10-2017 for shuttle part damaged
                    case "QRCSHUTTLETIREDAMAGE":
                        LogoPath = "<img src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">";
                        TemplatePath = ConfigurationManager.AppSettings["ShuttleTirePartDamageTemplate"];
                        Subject = "eTrac: Shuttle tire part is damaged at " + LocationName;
                        strMailHeading = "Welcome to eTrac";
                        strMailBody = GetMailBody(TemplatePath);
                        strMailBody = strMailBody.Replace("##Year", Year);
                        strMailBody = strMailBody.Replace("##Logo", LogoPath);
                        strMailBody = strMailBody.Replace("##MailHeading", strMailHeading);
                        strMailBody = strMailBody.Replace("##ManagerName", ManagerName);
                        strMailBody = strMailBody.Replace("##ProblemDesc", ProblemDesc);
                        strMailBody = strMailBody.Replace("##UserName", UserName);
                        strMailBody = strMailBody.Replace("##LocationName", LocationName);
                        strMailBody = strMailBody.Replace("##TimeAttempted", TimeAttempted);
                        strMailBody = strMailBody.Replace("##QrCodeId", QrCodeId);
                        strMailBody = strMailBody.Replace("##Sign", "<img height='50px' src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">");
                        break;

                    //Added By Bhushan Dod on April-27-2015 for vehicle mileage
                    case "QRCSHUTTLEMILEAGECO":

                        TemplatePath = ConfigurationManager.AppSettings["ShuttleMileageTemplate"];
                        LogoPath = "<img src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">";

                        Subject = "eTrac: Shuttle Mileage Change at  " + LocationName;
                        strMailHeading = "Welcome to eTrac";
                        strMailBody = GetMailBody(TemplatePath);
                        strMailBody = strMailBody.Replace("##Year", Year);
                        strMailBody = strMailBody.Replace("##Logo", LogoPath);
                        strMailBody = strMailBody.Replace("##MailHeading", strMailHeading);
                        strMailBody = strMailBody.Replace("##ManagerName", ManagerName);
                        strMailBody = strMailBody.Replace("##Action", Action);
                        strMailBody = strMailBody.Replace("##UserName", UserName);
                        strMailBody = strMailBody.Replace("##LocationName", LocationName);
                        strMailBody = strMailBody.Replace("##PreviousMileage", PreviousMileage);
                        strMailBody = strMailBody.Replace("##CurrentMileage", CurrentMileage);
                        strMailBody = strMailBody.Replace("##QrCodeId", QrCodeId);
                        strMailBody = strMailBody.Replace("##Sign", "<img height='50px' src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">");
                        break;
                    //Added By Bhushan Dod on Sep-22-2017 for eFleet inspection
                    case "EFLEETINSPECTIONREPORT":
                        LogoPath = "<img src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">";
                        TemplatePath = ConfigurationManager.AppSettings["EfleetInspectionPreTripTemplate"];
                        Subject = "eTrac: eFLeet" + InfractionStatus+" Inspection reported at " + LocationName;
                        strMailHeading = "Welcome to eTrac";
                        strMailBody = GetMailBody(TemplatePath);
                        strMailBody = strMailBody.Replace("##Logo", LogoPath);
                        strMailBody = strMailBody.Replace("##MailHeading", strMailHeading);
                        strMailBody = strMailBody.Replace("##ManagerName", ManagerName);
                        strMailBody = strMailBody.Replace("##Make", VehicleMake);
                        strMailBody = strMailBody.Replace("##Model", VehicleModel);
                        strMailBody = strMailBody.Replace("##VehicleNumber", VehicleIdentificationNumber);
                        strMailBody = strMailBody.Replace("##UserName", UserName);
                        strMailBody = strMailBody.Replace("##LocationName", LocationName);
                        strMailBody = strMailBody.Replace("##TimeAttempted", TimeAttempted);
                        strMailBody = strMailBody.Replace("##Status", InfractionStatus);
                        strMailBody = strMailBody.Replace("##QrCodeId", QrCodeId);
                        strMailBody = strMailBody.Replace("##Sign", "<img height='50px' src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">");
                        break;
                    //Added By Bhushan Dod on Sep-22-2017 for eFleet Preventative Maintenance
                    case "PreventativeMaintenance":
                        LogoPath = "<img src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">";
                        TemplatePath = ConfigurationManager.AppSettings["PreventativeMaintenanceTemplate"];
                        Subject = "eTrac: eFLeet Preventative Maintenance reported at " + LocationName;
                        strMailHeading = "Welcome to eTrac";
                        strMailBody = GetMailBody(TemplatePath);
                        strMailBody = strMailBody.Replace("##Logo", LogoPath);
                        strMailBody = strMailBody.Replace("##MailHeading", strMailHeading);
                        strMailBody = strMailBody.Replace("##ManagerName", ManagerName);
                        strMailBody = strMailBody.Replace("##Category", Category);
                        strMailBody = strMailBody.Replace("##VehicleNumber", VehicleNumber);
                        strMailBody = strMailBody.Replace("##Meter", Meter);
                        strMailBody = strMailBody.Replace("##ReminderMeric", RemiderMetric); 
                        strMailBody = strMailBody.Replace("##PMMetric", PMMetric);
                        strMailBody = strMailBody.Replace("##ServiceDueDate",ServiceDueDate);
                        strMailBody = strMailBody.Replace("##UserName", UserName);
                        strMailBody = strMailBody.Replace("##LocationName", LocationName);
                        strMailBody = strMailBody.Replace("##TimeAttempted", TimeAttempted);
                        strMailBody = strMailBody.Replace("##Status", InfractionStatus);
                        strMailBody = strMailBody.Replace("##QrCodeId", QrCodeId);
                        strMailBody = strMailBody.Replace("##Sign", "<img height='50px' src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">");
                        break;

                        //Added by Ashwajit Bansod Date : Oct-04-2017 for Vehicle Incident
                    case "VehicleIncident":
                        LogoPath = "<img src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">";
                        TemplatePath = ConfigurationManager.AppSettings["EfleetVehicleIncidentTemplate"];
                        Subject = "eTrac: eFLeet Vehicle Incident reported at " + LocationName;
                        strMailHeading = "Welcome to eTrac";
                        strMailBody = GetMailBody(TemplatePath);
                        strMailBody = strMailBody.Replace("##Logo", LogoPath);
                        strMailBody = strMailBody.Replace("##MailHeading", strMailHeading);
                        strMailBody = strMailBody.Replace("##ManagerName", ManagerName);
                        strMailBody = strMailBody.Replace("##Prevenatability", Prevenatability);
                        strMailBody = strMailBody.Replace("##City", City);
                        strMailBody = strMailBody.Replace("##AccidentDate", AccidentDate);
                        strMailBody = strMailBody.Replace("##IncidentDescription", IncidentDescription);
                        strMailBody = strMailBody.Replace("##VehicleNumber", VehicleNumber);
                        strMailBody = strMailBody.Replace("##NumberOfInjuries", NumberOfInjuries);
                        strMailBody = strMailBody.Replace("##DriverNameForVehicleIncident", DriverNameForVehicleIncident);
                        strMailBody = strMailBody.Replace("##IncidentDescription", IncidentDescription);
                        strMailBody = strMailBody.Replace("##UserName", UserName);
                        strMailBody = strMailBody.Replace("##LocationName", LocationName);
                        strMailBody = strMailBody.Replace("##TimeAttempted", TimeAttempted);
                        strMailBody = strMailBody.Replace("##Status", InfractionStatus);
                        strMailBody = strMailBody.Replace("##QrCodeId", QrCodeId);
                        strMailBody = strMailBody.Replace("##Sign", "<img height='50px' src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">");
                        break;

                        //Added by Ashwajit Bansod date : Oct-05-2017 for eFleet Fueling
                    case "EfleetFueling":
                        LogoPath = "<img src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">";
                        TemplatePath = ConfigurationManager.AppSettings["EfleetFuelingTemplate"];
                        Subject = "eTrac: eFLeet Fueling reported at " + LocationName;
                        strMailHeading = "Welcome to eTrac";
                        strMailBody = GetMailBody(TemplatePath);
                        strMailBody = strMailBody.Replace("##Logo", LogoPath);
                        strMailBody = strMailBody.Replace("##MailHeading", strMailHeading);
                        strMailBody = strMailBody.Replace("##ManagerName", ManagerName);
                        strMailBody = strMailBody.Replace("##DriverNameforFueling", DriverNameforFueling);
                        strMailBody = strMailBody.Replace("##FuelType", FuelType);
                        strMailBody = strMailBody.Replace("##GasStatioName", GasStatioName);
                        strMailBody = strMailBody.Replace("##Mileage", Mileage);
                        strMailBody = strMailBody.Replace("##VehicleNumber", VehicleNumber);
                        strMailBody = strMailBody.Replace("##CurrentFuel", CurrentFuel);
                        strMailBody = strMailBody.Replace("##Total", Total);
                        strMailBody = strMailBody.Replace("##FuelingDate", FuelingDate);
                        strMailBody = strMailBody.Replace("##UserName", UserName);
                        strMailBody = strMailBody.Replace("##LocationName", LocationName);
                        strMailBody = strMailBody.Replace("##TimeAttempted", TimeAttempted);
                        strMailBody = strMailBody.Replace("##Status", InfractionStatus);
                        strMailBody = strMailBody.Replace("##QrCodeId", QrCodeId);
                        strMailBody = strMailBody.Replace("##Sign", "<img height='50px' src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">");
                        break;

                        //Added by Ashwajit Bansod Date:Oct-05-2017 for Vehicle Incident for Web Service
                    case "EfleetIncidentForService":
                        LogoPath = "<img src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">";
                        TemplatePath = ConfigurationManager.AppSettings["EfleetIncidentForServiceTemplate"];
                        Subject = "eTrac: eFLeet Fueling reported at " + LocationName;
                        strMailHeading = "Welcome to eTrac";
                        strMailBody = GetMailBody(TemplatePath);
                        strMailBody = strMailBody.Replace("##Logo", LogoPath);
                        strMailBody = strMailBody.Replace("##MailHeading", strMailHeading);
                        strMailBody = strMailBody.Replace("##ManagerName", ManagerName);
                        strMailBody = strMailBody.Replace("##IncidentDescription", IncidentDescription);
                        strMailBody = strMailBody.Replace("##Prevenatability", Prevenatability);
                        strMailBody = strMailBody.Replace("##NumberOfInjuries", NumberOfInjuries);
                        strMailBody = strMailBody.Replace("##City", City);
                        strMailBody = strMailBody.Replace("##VehicleNumber", VehicleNumber);
                        //strMailBody = strMailBody.Replace("##CurrentFuel", CurrentFuel);
                        //strMailBody = strMailBody.Replace("##Total", Total);
                        strMailBody = strMailBody.Replace("##AccidentDate", AccidentDate);
                        strMailBody = strMailBody.Replace("##UserName", UserName);
                        strMailBody = strMailBody.Replace("##LocationName", LocationName);
                        strMailBody = strMailBody.Replace("##TimeAttempted", TimeAttempted);
                        strMailBody = strMailBody.Replace("##Status", InfractionStatus);
                        strMailBody = strMailBody.Replace("##QrCodeId", QrCodeId);
                        strMailBody = strMailBody.Replace("##Sign", "<img height='50px' src=" + ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png" + ">");
                        break;

                }
                string body = System.Web.HttpUtility.HtmlDecode(strMailBody);
                List<Attachment> tt = new List<Attachment>();
                if (attachedUrl != null && attachedUrl.Length > 0)
                {
                    foreach (var i in attachedUrl)
                    {
                        if (File.Exists(i))
                        {
                            Attachment ds = new Attachment(i);
                            tt.Add(ds);
                        }
                        else
                        {
                            string path = ConfigurationManager.AppSettings["DefaultImage"];
                            string RootDirectory = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                            RootDirectory = RootDirectory.Substring(0, RootDirectory.Length - 2).Substring(0, RootDirectory.Substring(0, RootDirectory.Length - 2).LastIndexOf("\\")) + path;

                            Attachment ds = new Attachment(RootDirectory);
                            tt.Add(ds);
                        }
                    }
                }

                try
                {
                    SendEmail(emailid, body, Subject, tt);
                }
                catch (Exception ex)
                {

                }
                IsSent = true;
                return IsSent;
            }
            catch (Exception)
            {
                IsSent = true;
                return IsSent;
            }
        }


        /// Sends the email.
        /// </summary>
        /// <param name="emailid">The receiver email id.</param>
        /// <param name="body">The body of the email</param>
        /// <param name="subject">The subject.</param>
        /// <param name="smtpEmailAddress">The SMTP email address.
        ///NOTE : Leave blank if present in the config file</param>
        /// <param name="smtppassword">The SMTP email Password.
        ///NOTE : Leave blank if present in the config file</param>
        /// <param name="host">The SMTP host.
        ///NOTE : Leave blank if present in the config file</param>
        public static void SendEmail(string emailId, string body, string subject, List<Attachment> attachmentObj = null, string smtpEmailAddress = null, string smtpPassword = null, string host = null, bool async = false, long SentBy = 0, long LocationID = 0)
        {

            try
            {
                if (String.IsNullOrEmpty(smtpEmailAddress))
                    smtpEmailAddress = ConfigurationManager.AppSettings["SMTP_DEFAULT_EMAIL"];
                if (String.IsNullOrEmpty(smtpPassword))
                    smtpPassword = ConfigurationManager.AppSettings["SMTP_DEFAULT_PASSWORD"];
                if (String.IsNullOrEmpty(host))
                    host = ConfigurationManager.AppSettings["SMTP_DEFAULT_HOST"];
                var SmtpServer = GetSMTPClient(host);
                SmtpServer.Host = host;
                var mail = new System.Net.Mail.MailMessage();
                SmtpServer.Credentials = new System.Net.NetworkCredential(smtpEmailAddress, smtpPassword);
                SmtpServer.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTP_DEFAULT_PORT"]);
                SmtpServer.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["SMTP_DEFAULT_EnableSsl"]);
                mail.Priority = MailPriority.High;
                mail.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["SMTP_DEFAULT_EMAIL"].ToString(), "eTrac");
                mail.To.Add(emailId.Trim());
                mail.Subject = subject;
                mail.Body = body;

                if (attachmentObj != null && attachmentObj.Count() > 0)
                {
                    foreach (var item in attachmentObj)
                    {
                        mail.Attachments.Add(item);
                    }
                }
                mail.IsBodyHtml = true;
                if (async == true)
                {
                    SmtpServer.SendAsync(mail, null);
                }
                else
                {
                    SmtpServer.Send(mail);
                }

                // bool Log =

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// Sends the email.
        /// </summary>
        /// <param name="emailid">The receiver email id.</param>
        /// <param name="body">The body of the email</param>
        /// <param name="subject">The subject.</param>
        /// <param name="smtpEmailAddress">The SMTP email address.
        ///NOTE : Leave blank if present in the config file</param>
        /// <param name="smtppassword">The SMTP email Password.
        ///NOTE : Leave blank if present in the config file</param>
        /// <param name="host">The SMTP host.
        ///NOTE : Leave blank if present in the config file</param>
        public static void SendEmail(string emailId, string body, string subject, Attachment attachmentObj = null, string smtpEmailAddress = null, string smtpPassword = null, string host = null, bool async = false, long SentBy = 0, long LocationID = 0)
        {

            try
            {
                if (String.IsNullOrEmpty(smtpEmailAddress))
                    smtpEmailAddress = ConfigurationManager.AppSettings["SMTP_DEFAULT_EMAIL"];
                if (String.IsNullOrEmpty(smtpPassword))
                    smtpPassword = ConfigurationManager.AppSettings["SMTP_DEFAULT_PASSWORD"];
                if (String.IsNullOrEmpty(host))
                    host = ConfigurationManager.AppSettings["SMTP_DEFAULT_HOST"];
                var SmtpServer = GetSMTPClient(host);
                SmtpServer.Host = host;
                var mail = new System.Net.Mail.MailMessage();
                SmtpServer.Credentials = new System.Net.NetworkCredential(smtpEmailAddress, smtpPassword);
                SmtpServer.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTP_DEFAULT_PORT"]);
                SmtpServer.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["SMTP_DEFAULT_EnableSsl"]);
                mail.Priority = MailPriority.High;
                mail.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["SMTP_DEFAULT_EMAIL"].ToString(), "eTrac");
                //foreahloop for email id list
                mail.To.Add(emailId.Trim());
                //for each loop close
                mail.Subject = subject;
                mail.Body = body;
                if (attachmentObj != null)
                {

                    mail.Attachments.Add(attachmentObj);

                }
                mail.IsBodyHtml = true;
                if (async == true)
                {
                    SmtpServer.SendAsync(mail, null);
                }
                else
                {
                    SmtpServer.Send(mail);
                }

                // bool Log =

            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Gets the SMTP client.
        /// </summary>
        /// <param name="Host">The host.</param>
        /// <returns>

        /// </returns>
        private static SmtpClient GetSMTPClient(string Host)
        {
            switch (Host)
            {
                case "smtp.gmail.com":
                    return new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false
                    };
                default:
                    return new System.Net.Mail.SmtpClient();
            }
        }
        public static bool IsValidEmail(string emailAddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailAddress);
                return true;
            }
            catch (FormatException)
            { return false; }
        }

        /// <summary>
        /// This method will return the mailbody that is html mail tempalte 
        /// </summary>
        /// <param name="strMailTemplateVirtualPath">virtual path of mail template</param>
        /// <returns>it returns html formated mail body</returns>
        private static string GetMailBody(string strMailTemplateVirtualPath)
        {
            try
            {
                //creating object of stream reader for reading mail template
                string basePath = string.Empty;
                if (HttpContext.Current == null)
                {
                    basePath = HttpRuntime.AppDomainAppPath + strMailTemplateVirtualPath.Remove(0, 2);
                }
                else
                {
                    if (strMailTemplateVirtualPath.Contains("http://"))
                        basePath = strMailTemplateVirtualPath;
                    else
                        basePath = HttpContext.Current.Server.MapPath(strMailTemplateVirtualPath);
                }
                StreamReader objectStreamReader;

                if (strMailTemplateVirtualPath.Contains("http://"))
                {
                    WebRequest request = WebRequest.Create(strMailTemplateVirtualPath);
                    request.Credentials = CredentialCache.DefaultCredentials;
                    WebResponse response = request.GetResponse();
                    Stream dataStream = response.GetResponseStream();
                    objectStreamReader = new StreamReader(dataStream);


                }
                else
                {
                    objectStreamReader = new StreamReader(basePath);
                }

                string strMailBody = objectStreamReader.ReadToEnd();
                objectStreamReader.Close();
                if (strMailBody.Trim().Length > 0)
                {
                    return strMailBody;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}
