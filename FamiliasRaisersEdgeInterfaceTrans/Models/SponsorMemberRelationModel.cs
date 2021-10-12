using FamiliasRaisersEdgeInterfaceTrans.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FamiliasRaisersEdgeInterfaceTrans.Models
{
    public class SponsorMemberRelationModel
    {
        public string Project { get; set; }
        public string SponsorId { get; set; }
        public int MemberId { get; set; }
        public string CreationDateTime { get; set; }
        public string RecordStatus { get; set; }
        public string UserId { get; set; }
        public string ExpirationDateTime { get; set; }
        public string Type { get; set; }
        public string Notes { get; set; }
        public string InactiveReason { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }


        public static List<SponsorMemberRelationModel> getSponsorMemberRelationByIds(int MemberId, int SponsorId, string Project, string fc, bool secondRev = false)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[GlobalData.DBCONNECTION].ToString()))
                {
                    connection.Open();

                    var query = "";
                    if (fc == "RS" && !secondRev)
                    {
                        query = string.Format(@"
                        SELECT * 
                        FROM {0} SMR
                        INNER JOIN Member M ON M.Project=SMR.Project
	                        and M.MemberId=SMR.MemberId and M.RecordStatus=SMR.RecordStatus 
                        WHERE SMR.Project='{1}' and SMR.MemberId={2} and SMR.SponsorId={3} and SMR.RecordStatus = ' ' and EndDate is null
                        ORDER BY SMR.CreationDateTime DESC"
                        , GlobalData.TABLE_SPONSOR_MEMBER_REL, Project, MemberId, SponsorId);
                    }
                    else
                    {
                        query = string.Format(@"
                        SELECT * 
                        FROM {0} SMR
                        INNER JOIN Member M ON M.Project=SMR.Project
	                        and M.MemberId=SMR.MemberId and M.RecordStatus=SMR.RecordStatus 
                        WHERE SMR.Project='{1}' and SMR.MemberId={2} and SMR.RecordStatus = ' ' and EndDate is null
                        ORDER BY SMR.CreationDateTime DESC"
                        , GlobalData.TABLE_SPONSOR_MEMBER_REL, Project, MemberId, SponsorId);
                    }

                    SqlCommand command = new SqlCommand(query, connection); command.CommandType = System.Data.CommandType.Text;

                    SqlDataReader queryObject = command.ExecuteReader();

                    List<SponsorMemberRelationModel> relations = new List<SponsorMemberRelationModel>();
                    while(queryObject.Read())
                    {
                        var selectedMember = new SponsorMemberRelationModel
                        {
                            Project = !Convert.IsDBNull(queryObject.GetValue(0)) ? queryObject.GetValue(0).ToString() : null,
                            SponsorId = !Convert.IsDBNull(queryObject.GetValue(1)) ? queryObject.GetValue(1).ToString() : null,
                            MemberId = !Convert.IsDBNull(queryObject.GetValue(2)) ? Convert.ToInt32(queryObject.GetValue(2)) : Convert.ToInt32(null),
                            CreationDateTime = !Convert.IsDBNull(queryObject.GetValue(3)) ? queryObject.GetValue(3).ToString() : null,
                            RecordStatus = !Convert.IsDBNull(queryObject.GetValue(4)) ? queryObject.GetValue(4).ToString() : null,
                            UserId = !Convert.IsDBNull(queryObject.GetValue(5)) ? queryObject.GetValue(5).ToString() : null,
                            ExpirationDateTime = !Convert.IsDBNull(queryObject.GetValue(6)) ? queryObject.GetValue(6).ToString() : null,
                            Type = !Convert.IsDBNull(queryObject.GetValue(7)) ? queryObject.GetValue(7).ToString() : null,
                            Notes = !Convert.IsDBNull(queryObject.GetValue(8)) ? queryObject.GetValue(8).ToString() : null,
                            InactiveReason = !Convert.IsDBNull(queryObject.GetValue(9)) ? queryObject.GetValue(9).ToString() : null,
                            StartDate = !Convert.IsDBNull(queryObject.GetValue(10)) ? queryObject.GetValue(10).ToString() : null,
                            EndDate = !Convert.IsDBNull(queryObject.GetValue(11)) ? queryObject.GetValue(11).ToString() : null
                        };
                        relations.Add(selectedMember);
                    }

                    return relations.Count == 0 ? null : relations;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static SponsorMemberRelationModel getInactiveSponsorMemberRelationByIds(int MemberId, int SponsorId, string Project)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[GlobalData.DBCONNECTION].ToString()))
                {
                    connection.Open();
                    var query = string.Format(@"
                    SELECT TOP 1 *
                    FROM {0} SMR
                    INNER JOIN {1} M ON M.Project=SMR.Project and M.MemberId=SMR.MemberId and M.RecordStatus=SMR.RecordStatus
                    WHERE SMR.EndDate IS NOT NULL AND 
                    M.Project = '{2}' AND SMR.MemberId = {3} AND SMR.SponsorId = {4}
                    ORDER BY SMR.EndDate DESC", GlobalData.TABLE_SPONSOR_MEMBER_REL, GlobalData.TABLE_MEMBER, Project, MemberId, SponsorId);

                    SqlCommand command = new SqlCommand(query, connection); command.CommandType = System.Data.CommandType.Text;

                    SqlDataReader queryObject = command.ExecuteReader();

                    SponsorMemberRelationModel relations = new SponsorMemberRelationModel();
                    if (queryObject.Read())
                    {
                        var selectedMember = new SponsorMemberRelationModel
                        {
                            Project = !Convert.IsDBNull(queryObject.GetValue(0)) ? queryObject.GetValue(0).ToString() : null,
                            SponsorId = !Convert.IsDBNull(queryObject.GetValue(1)) ? queryObject.GetValue(1).ToString() : null,
                            MemberId = !Convert.IsDBNull(queryObject.GetValue(2)) ? Convert.ToInt32(queryObject.GetValue(2)) : Convert.ToInt32(null),
                            CreationDateTime = !Convert.IsDBNull(queryObject.GetValue(3)) ? queryObject.GetValue(3).ToString() : null,
                            RecordStatus = !Convert.IsDBNull(queryObject.GetValue(4)) ? queryObject.GetValue(4).ToString() : null,
                            UserId = !Convert.IsDBNull(queryObject.GetValue(5)) ? queryObject.GetValue(5).ToString() : null,
                            ExpirationDateTime = !Convert.IsDBNull(queryObject.GetValue(6)) ? queryObject.GetValue(6).ToString() : null,
                            Type = !Convert.IsDBNull(queryObject.GetValue(7)) ? queryObject.GetValue(7).ToString() : null,
                            Notes = !Convert.IsDBNull(queryObject.GetValue(8)) ? queryObject.GetValue(8).ToString() : null,
                            InactiveReason = !Convert.IsDBNull(queryObject.GetValue(9)) ? queryObject.GetValue(9).ToString() : null,
                            StartDate = !Convert.IsDBNull(queryObject.GetValue(10)) ? queryObject.GetValue(10).ToString() : null,
                            EndDate = !Convert.IsDBNull(queryObject.GetValue(11)) ? queryObject.GetValue(11).ToString() : null
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

        public static bool isValidInactiveReason(string InactiveReason)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[GlobalData.DBCONNECTION].ToString()))
                {
                    connection.Open();
                    var query = string.Format(@"
                                        SELECT Code FROM {0} WHERE Code = '{1}'",
                                        GlobalData.CD_SPONSOR_MEMBER_RELATION_IR, InactiveReason);

                    SqlCommand command = new SqlCommand(query, connection); command.CommandType = System.Data.CommandType.Text;

                    SqlDataReader queryObject = command.ExecuteReader();

                    if (queryObject.Read())
                    {
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
    }
}