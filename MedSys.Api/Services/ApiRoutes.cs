namespace MedSys.Api.Services
{
    public static class ApiRoutes
    {
        public static class Checkup
        {
            public const string Base = "api/Checkup";
        }             
        public static class Patient
        {
            public const string Base = "api/Patient";
            public const string Search = $"{Base}/search";
            public const string Export = $"{Base}/export";
        }        
        public static class CheckupType
        {
            public const string Base = "api/CheckupType";
        }

        public static class Disease
        {
            public const string Base = "api/Disease";
        }
        public static class Drug
        {
            public const string Base = "api/Drug";
        }         
        public static class Prescription
        {
            public const string Base = "api/Prescription";
        }          
        
        public static class MedicalHistory
        {
            public const string Base = "api/MedicalHistory";
        }
        public static class MedDocument
        {
            public const string Base = "api/MedicalDocument";
            public const string SearchByCheckup = $"{Base}/by_checkup";
            public const string SearchByPatient = $"{Base}/by_patient";
        }
    }
}
