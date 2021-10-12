using FamiliasRaisersEdgeInterfaceTrans.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FamiliasRaisersEdgeInterfaceTrans.Models
{
    public class MemberModel
    {
        public string Project { get; set; }
        public int MemberId { get; set; }
        public string CreationDateTime { get; set; }
        public string RecordStatus { get; set; }
        public string UserId { get; set; }
        public string ExpirationDateTime { get; set; }
        public int LastFamilyId { get; set; }
        public string FirstNames { get; set; }
        public string LastNames { get; set; }
        public string PreferredName { get; set; }
        public string BirthDate { get; set; }
        public string Gender { get; set; }
        public string AffiliationType { get; set; }
        public string AffiliationStatus { get; set; }
        public string AffiliationStatusDate { get; set; }
        public string LiveDead { get; set; }
        public string DeathDate { get; set; }
        public int BiologicalMotherMemberId { get; set; }
        public int BiologicalFatherMemberId { get; set; }
        public string OtherAffiliation { get; set; }
        public string OfficialId{ get; set; }
        public bool HasFaithOfAgeOrOfficialId { get; set; }
        public string CellularPhoneNumber { get; set; }
        public string Literacy { get; set; }
        public string LastGradePassed { get; set; }
        public bool HasHealthCard { get; set; }
        public string ExceptionInAgePolicy { get; set; }
        public int LasGradePassedYear { get; set; }
        public string LastGradePassedStatus { get; set; }

        public static MemberModel getMemberById(int MemberId, string Project)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[GlobalData.DBCONNECTION].ToString()))
                {
                    connection.Open();
                    var query = string.Format(@"
                    SELECT TOP 1 *
                    FROM {0}
                    WHERE MemberId = {1} AND RecordStatus = ' ' AND Project = '{2}'
                    ORDER BY CreationDateTime DESC", GlobalData.TABLE_MEMBER, MemberId, Project);

                    SqlCommand command = new SqlCommand(query, connection); command.CommandType = System.Data.CommandType.Text;

                    SqlDataReader queryObject = command.ExecuteReader();

                    if (queryObject.Read())
                    {
                        var selectedMember = new MemberModel
                        {
                            Project = !Convert.IsDBNull(queryObject.GetValue(0)) ? queryObject.GetValue(0).ToString() : null,
                            MemberId = !Convert.IsDBNull(queryObject.GetValue(1)) ? Convert.ToInt32(queryObject.GetValue(1)) : Convert.ToInt32(null),
                            CreationDateTime = !Convert.IsDBNull(queryObject.GetValue(2)) ? Convert.ToString(queryObject.GetValue(2)) : Convert.ToString(null),
                            RecordStatus = !Convert.IsDBNull(queryObject.GetValue(3)) ? queryObject.GetValue(3).ToString() : null,
                            UserId = !Convert.IsDBNull(queryObject.GetValue(4)) ? queryObject.GetValue(4).ToString() : null,
                            ExpirationDateTime = !Convert.IsDBNull(queryObject.GetValue(5)) ? Convert.ToString(queryObject.GetValue(5)) : Convert.ToString(null),
                            LastFamilyId = !Convert.IsDBNull(queryObject.GetValue(6)) ? (int)queryObject.GetValue(6) : Convert.ToInt32(null),
                            FirstNames = !Convert.IsDBNull(queryObject.GetValue(7)) ? queryObject.GetValue(7).ToString() : null,
                            LastNames = !Convert.IsDBNull(queryObject.GetValue(8)) ? queryObject.GetValue(8).ToString() : null,
                            PreferredName = !Convert.IsDBNull(queryObject.GetValue(9)) ? queryObject.GetValue(9).ToString() : null,
                            BirthDate = !Convert.IsDBNull(queryObject.GetValue(10)) ? Convert.ToString(queryObject.GetValue(10)) : Convert.ToString(null),
                            Gender = !Convert.IsDBNull(queryObject.GetValue(11)) ? queryObject.GetValue(11).ToString() : null,
                            AffiliationType = !Convert.IsDBNull(queryObject.GetValue(12)) ? queryObject.GetValue(12).ToString() : null,
                            AffiliationStatus = !Convert.IsDBNull(queryObject.GetValue(13)) ? queryObject.GetValue(13).ToString() : null,
                            AffiliationStatusDate = !Convert.IsDBNull(queryObject.GetValue(14)) ? Convert.ToString(queryObject.GetValue(14)) : Convert.ToString(null),
                            LiveDead = !Convert.IsDBNull(queryObject.GetValue(15)) ? queryObject.GetValue(15).ToString() : null,
                            DeathDate = !Convert.IsDBNull(queryObject.GetValue(16)) ? Convert.ToString(queryObject.GetValue(16)) : Convert.ToString(null),
                            BiologicalMotherMemberId = !Convert.IsDBNull(queryObject.GetValue(17)) ? Convert.ToInt32(queryObject.GetValue(17)) : Convert.ToInt32(null),
                            BiologicalFatherMemberId = !Convert.IsDBNull(queryObject.GetValue(18)) ? Convert.ToInt32(queryObject.GetValue(18)) : Convert.ToInt32(null),
                            OtherAffiliation = !Convert.IsDBNull(queryObject.GetValue(19)) ? queryObject.GetValue(19).ToString() : null,
                            OfficialId = !Convert.IsDBNull(queryObject.GetValue(20)) ? queryObject.GetValue(20).ToString() : null,
                            HasFaithOfAgeOrOfficialId = !Convert.IsDBNull(queryObject.GetValue(21)) ? Convert.ToBoolean(queryObject.GetValue(21)) : Convert.ToBoolean(null),
                            CellularPhoneNumber = !Convert.IsDBNull(queryObject.GetValue(22)) ? queryObject.GetValue(22).ToString() : null,
                            Literacy = !Convert.IsDBNull(queryObject.GetValue(23)) ? queryObject.GetValue(23).ToString() : null,
                            LastGradePassed = !Convert.IsDBNull(queryObject.GetValue(24)) ? queryObject.GetValue(24).ToString() : null,
                            HasHealthCard = !Convert.IsDBNull(queryObject.GetValue(25)) ? Convert.ToBoolean(queryObject.GetValue(25)) :  Convert.ToBoolean(null),
                            ExceptionInAgePolicy = !Convert.IsDBNull(queryObject.GetValue(26)) ? queryObject.GetValue(26).ToString() : null,
                            LasGradePassedYear = !Convert.IsDBNull(queryObject.GetValue(27)) ? Convert.ToInt32(queryObject.GetValue(27)) : Convert.ToInt32(null),
                            LastGradePassedStatus = !Convert.IsDBNull(queryObject.GetValue(28)) ? queryObject.GetValue(28).ToString() : null

                        };
                        return selectedMember;
                    }
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool getActiveSponsorMemberRelation(string Project, int MemberId, int SponsorId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[GlobalData.DBCONNECTION].ToString()))
                {
                    connection.Open();
                    var query = string.Format(@"
                    SELECT *
                    FROM {0} SMR
                    INNER JOIN {1} M ON M.Project=SMR.Project and M.MemberId=SMR.MemberId and M.RecordStatus=SMR.RecordStatus
                    WHERE M.RecordStatus = ' ' AND SMR.EndDate IS NULL AND 
                    M.Project = '{2}' AND SMR.MemberId = {3} AND SMR.SponsorId = {4}
                    ORDER BY SponsorId", GlobalData.TABLE_SPONSOR_MEMBER_REL, GlobalData.TABLE_MEMBER, Project, MemberId, SponsorId);

                    SqlCommand command = new SqlCommand(query, connection); command.CommandType = System.Data.CommandType.Text;

                    SqlDataReader queryObject = command.ExecuteReader();

                    while (queryObject.Read())
                    {
                        //(11) = EndDate, (1) = SponsorId
                        if(queryObject.GetValue(11).ToString() == "" && (int)queryObject.GetValue(1) == SponsorId)
                            return true;
                    }
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool isAffiliated(string AffiliationStatus)
        {
            return AffiliationStatus == null ? false : AffiliationStatus == "AFIL";
        }
    }
}