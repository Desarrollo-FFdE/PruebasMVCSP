using FamiliasRaisersEdgeInterfaceTrans.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FamiliasRaisersEdgeInterfaceTrans.Models
{
    public class SponsorModel
    {
        public int SponsorId { get; set; }
        public string CreationDateTime { get; set; }
        public string RecordStatus { get; set; }
        public string UserId { get; set; }
        public string ExpirationDateTime { get; set; }
        public string SponsorNames { get; set; }
        public string Gender { get; set; }
        public string OrganizationContactNames { get; set; }
        public string StateOrProvince { get; set; }
        public string Country { get; set; }
        public string SpeaksSpanish { get; set; }


        public static SponsorModel getSponsorById(int SponsorId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[GlobalData.DBCONNECTION].ToString()))
                {
                    connection.Open();
                    var query = string.Format(@"
                    SELECT TOP 1 *
                    FROM {0}
                    WHERE SponsorId = {1}
                    ORDER BY CreationDateTime DESC", GlobalData.TABLE_SPONSOR, SponsorId);

                    SqlCommand command = new SqlCommand(query, connection); command.CommandType = System.Data.CommandType.Text;

                    SqlDataReader queryObject = command.ExecuteReader();

                    if (queryObject.Read())
                    {
                        var selectedSponsor = new SponsorModel
                        {
                            SponsorId = !Convert.IsDBNull(queryObject.GetValue(0)) ? Convert.ToInt32(queryObject.GetValue(0)) : Convert.ToInt32(null),
                            CreationDateTime = !Convert.IsDBNull(queryObject.GetValue(1)) ? Convert.ToString(queryObject.GetValue(1)) : Convert.ToString(null),
                            RecordStatus = !Convert.IsDBNull(queryObject.GetValue(2)) ? queryObject.GetValue(2).ToString() : null,
                            UserId = !Convert.IsDBNull(queryObject.GetValue(3)) ? queryObject.GetValue(3).ToString() : null,
                            ExpirationDateTime = !Convert.IsDBNull(queryObject.GetValue(4)) ? Convert.ToString(queryObject.GetValue(4)) : Convert.ToString(null),
                            SponsorNames = !Convert.IsDBNull(queryObject.GetValue(5)) ? queryObject.GetValue(5).ToString() : null,
                            Gender = !Convert.IsDBNull(queryObject.GetValue(6)) ? queryObject.GetValue(6).ToString() : null,
                            OrganizationContactNames = !Convert.IsDBNull(queryObject.GetValue(7)) ? queryObject.GetValue(7).ToString() : null,
                            StateOrProvince = !Convert.IsDBNull(queryObject.GetValue(8)) ? queryObject.GetValue(8).ToString() : null,
                            Country = !Convert.IsDBNull(queryObject.GetValue(9)) ? queryObject.GetValue(9).ToString() : null,
                            SpeaksSpanish = !Convert.IsDBNull(queryObject.GetValue(10)) ? queryObject.GetValue(10).ToString() : null,
                        };
                        return selectedSponsor;
                    }
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool activelySponsorsMember(int SponsorId, int MemberId, string Project)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[GlobalData.DBCONNECTION].ToString()))
                {
                    connection.Open();

                    var query = string.Format(@"
                    SELECT TOP 1 smr.SponsorId
                    FROM {0} smr
                    WHERE smr.MemberId = {2} AND RecordStatus = ' ' AND EndDate is null AND Project = '{3}'
                    ORDER BY smr.CreationDateTime DESC", GlobalData.TABLE_SPONSOR_MEMBER_REL, SponsorId, MemberId, Project);

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

        public static bool isValidSponsor(int SponsorId)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[GlobalData.DBCONNECTION].ToString()))
            {
                connection.Open();

                var query = string.Format(@"SELECT SponsorId FROM {0} WHERE SponsorId = {1}",
                                            GlobalData.TABLE_SPONSOR, SponsorId);

                SqlCommand command = new SqlCommand(query, connection); command.CommandType = System.Data.CommandType.Text;

                SqlDataReader queryObject = command.ExecuteReader();

                if (queryObject.Read())
                {
                    return true;
                }
                return false;
            }
        }
    }
}