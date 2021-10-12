using FamiliasRaisersEdgeInterfaceTrans.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FamiliasRaisersEdgeInterfaceTrans.Models
{
    public class MiscMemberSponsorInfoModel
    {
        public string Project { get; set; }
        public int MemberId { get; set; }
        public string CreationDateTime { get; set; }
        public string RecordStatus { get; set; }
        public string UserId { get; set; }
        public string ExpirationDateTime { get; set; }
        public string Photo { get; set; }
        public string PhotoDate { get; set; }
        public string RetakePhotoDate { get; set; }
        public string RetakephotoUserId { get; set; }
        public string LastCarnetPrintDate { get; set; }
        public string SponsorshipLevel { get; set; }
        public string SponsorshipType { get; set; }
        public string Restriction { get; set; }
        public string RestrictionDate { get; set; }
        public string ExceptionPhotoDate { get; set; }


        public static async Task<MiscMemberSponsorInfoModel> getMiscMemberSponsorInfobyId(int MemberId, string Project)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[GlobalData.DBCONNECTION].ToString()))
                {
                    connection.Open();
                    var query = string.Format(@"
                    SELECT TOP 1 *
                    FROM {0}
                    WHERE MemberId = {1} AND Project = '{2}'
                    ORDER BY CreationDateTime DESC", GlobalData.TABLE_MISC_MEMBER_SPNS_INFO, MemberId, Project);

                    SqlCommand command = new SqlCommand(query, connection); command.CommandType = System.Data.CommandType.Text;

                    SqlDataReader queryObject = command.ExecuteReader();

                    if (queryObject.Read())
                    {
                        var selectedMMSI = new MiscMemberSponsorInfoModel
                        {
                            Project = !Convert.IsDBNull(queryObject.GetValue(0)) ? Convert.ToString(queryObject.GetValue(0)) : null,
                            MemberId = !Convert.IsDBNull(queryObject.GetValue(1)) ? Convert.ToInt32(queryObject.GetValue(1)) : Convert.ToInt32(null),
                            CreationDateTime = !Convert.IsDBNull(queryObject.GetValue(2)) ? Convert.ToString(queryObject.GetValue(2)) : null,
                            RecordStatus = !Convert.IsDBNull(queryObject.GetValue(3)) ? Convert.ToString(queryObject.GetValue(3)) : null,
                            UserId = !Convert.IsDBNull(queryObject.GetValue(4)) ? Convert.ToString(queryObject.GetValue(4)) : null,
                            ExpirationDateTime = !Convert.IsDBNull(queryObject.GetValue(5)) ? Convert.ToString(queryObject.GetValue(5)) : null,
                            Photo = !Convert.IsDBNull(queryObject.GetValue(6)) ? Convert.ToString(queryObject.GetValue(6)) : null,
                            PhotoDate = !Convert.IsDBNull(queryObject.GetValue(7)) ? Convert.ToString(queryObject.GetValue(7)) : null,
                            RetakePhotoDate = !Convert.IsDBNull(queryObject.GetValue(8)) ? Convert.ToString(queryObject.GetValue(8)) : null,
                            RetakephotoUserId = !Convert.IsDBNull(queryObject.GetValue(9)) ? Convert.ToString(queryObject.GetValue(9)) : null,
                            LastCarnetPrintDate = !Convert.IsDBNull(queryObject.GetValue(10)) ? Convert.ToString(queryObject.GetValue(10)) : null,
                            SponsorshipLevel = !Convert.IsDBNull(queryObject.GetValue(11)) ? Convert.ToString(queryObject.GetValue(11)) : null,
                            SponsorshipType = !Convert.IsDBNull(queryObject.GetValue(12)) ? Convert.ToString(queryObject.GetValue(12)) : null,
                            Restriction = !Convert.IsDBNull(queryObject.GetValue(13)) ? Convert.ToString(queryObject.GetValue(13)) : null,
                            RestrictionDate = !Convert.IsDBNull(queryObject.GetValue(14)) ? Convert.ToString(queryObject.GetValue(14)) : null,
                            ExceptionPhotoDate = !Convert.IsDBNull(queryObject.GetValue(15)) ? Convert.ToString(queryObject.GetValue(15)) : null
                            
                        };
                        return selectedMMSI;
                    }
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string getRestrictionDesc(string RestrictionCode)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[GlobalData.DBCONNECTION].ToString()))
                {
                    connection.Open();
                    var query = string.Format(@"
                    SELECT TOP 1 DescEnglish
                    FROM {0}
                    WHERE Code = '{1}'", GlobalData.CD_SPONSORSHIP_RESTR, RestrictionCode);

                    SqlCommand command = new SqlCommand(query, connection); command.CommandType = System.Data.CommandType.Text;

                    SqlDataReader queryObject = command.ExecuteReader();

                    if (queryObject.Read())
                    {
                        return (string)queryObject.GetValue(0);
                    }
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