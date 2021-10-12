using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FamiliasRaisersEdgeInterfaceTrans.Helpers
{
    public class GlobalData
    {
        //Tables
        public static string TABLE_MEMBER = "Member";
        public static string TABLE_MISC_MEMBER_SPNS_INFO = "MiscMemberSponsorInfo";
        public static string TABLE_SPONSOR = "Sponsor";
        public static string TABLE_SPONSOR_MEMBER_REL = "SponsorMemberRelation";
        //Catalogs
        public static string CD_COUNTRY = "CdCountry";
        public static string CD_STATE_PROVINCE = "CdStateOrProvince";
        public static string CD_GENDER = "CdGender";
        public static string CD_SPONSOR_MEMBER_RELATION_IR = "CdSponsorMemberRelationInactiveReason";
        public static string CD_SPONSOR_MEMBER_RELATION_T = "CdSponsorMemberRelationTYpe";
        public static string CD_SPONSORSHIP_LEVEL = "CdsponsorshipLevel";
        public static string CD_SPONSORSHIP_RESTR = "CdsponsorshipRestriction";

        //Fields
        //ExpirationDateTime missing to set default value null
        public static List<string> FIELDS_SPONSOR = new List<string>() { "SponsorId", "CreationDateTime", "RecordStatus", "UserId", "SponsorNames", "Gender", "OrganizationContactNames", "StateOrProvince", "Country", "SpeaksSpanish" };
        public static List<string> FIELDS_MEMBER = new List<string>() { "Project", "MemberId", "CreationDateTime", "RecordStatus", "UserId", "ExpirationDateTime", "LastFamilyId", "FirstNames", "LastNames", "PreferredName", "BirthDate", "Gender", "AffiliationType", "AffiliationStatus", "affilliationStatusDate", "LiveDead", "DeathDate", "BiologicalMotherMemberId", "BiologicalFatherMemberId", "OtherAffiliation", "OfficialId", "HasFaithOfAgeOrOfficialId", "CellularPhoneNumber", "Literacy", "LastGradePassed", "HasHealthCard", "ExceptionInAgePolicy", "LastGradePassedYear", "LasGradePassedStatus" };
        public static List<string> FIELDS_SPONSOR_MEMBER_REL = new List<string>() { "Project", "SponsorId", "MemberId", "CreationDateTime", "RecordStatus", "UserId", "ExpirationDateTime", "Type", "Notes", "InactiveReason", "StartDate", "EndDate" };
        public static List<string> FIELDS_MISC_MEMBER_SPONSOR_INFO = new List<string>() { "Project", "MemberId", "CreationDateTime", "RecordStatus", "UserId", "ExpirationDateTime", "Photo", "PhotoDate", "RetakePhotoDate", "RetakePhotoUserId", "LastCarnetPrintDate", "SponsorshipLevel", "SponsorshipType", "Restriction", "RestrictionDate", "ExceptionPhotoDate" };

        //Extras
        public static string DEFAULT_USERID = "ReInterface";
        public static string DBCONNECTION = "FamiliasTestEntities";
    }
}