using FamiliasraisersEdgeEntities;
//using FamiliasraisersEdgeEntities.FamiliasTestEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml.Serialization;
using FamiliasRaisersEdgeInterfaceTrans.Models;
using FamiliasRaisersEdgeInterfaceTrans.Helpers;
using System.Threading.Tasks;
using System.Globalization;

namespace FamiliasRaisersEdgeInterfaceTrans.Controllers
{
    public class FamiliasRaisersEdgeInterfaceTransController : ApiController
    {
        /**
         * This class encapsulates functionality that is used to provide the following
         * interfaces between Raiser's Edge and Familias. <p>
         *
         * If a request successfully completes a HttpServletResponse.Status =
         * HttpServletResponse.SC_OK (200) will be returned. <p>
         *
         * In the case of an error a HttpServletResponse.Status with a value of 1 along
         * with a descriptive error message will be returned. <p>
         *
         * Update RE with Familias
         *
         *   Input Parameters:
         *     Name - "FunctionCode"
         *     Value - "EADI" (Extract Affiliate and Disaffiliate Info)
         *
         *   Output:
         *     An XML document that contains one record for each affiliate and
         *     disaffiliate with the following fields:  Project, MemberId, FirstNames,
         *     LastNames, BirthDate (YYYYMMDD), FamilyId, Gender.
         *
         * Update Familias with RE Sponsor Demographic Change
         *
         *   First Input Parameter:
         *     Name - "FunctionCode"
         *     Value - SDC (Sponsor Demo Change)
         *
         *   Plus an input parameter for each of the following fields where the name of
         *   the parameter is the name of the variable and the value of the parameter is
         *   the value of the variable: SponsorId, SponsorNames, OrganizationContactNames,
         *   Gender (see CdGender.DescEnglish), StateOrProvince (see CdStateOrProvince.Code),
         *   Country (see CdCountry.DescEnglish).  All these parameters are required
         *   even if the values haven't changed.
         *
         * Update Familias with RE New Sponsor
         *
         *   First Input Parameter:
         *     Name - "FunctionCode"
         *     Value - "AS" (Add Sponsor)
         *
         *   Plus an input parameter for each of the following fields where the name of
         *   the parameter is the name of the variable and the value of the parameter is
         *   the value of the variable: SponsorId, SponsorNames, OrganizationContactNames,
         *   Gender (see CdGender.DescEnglish), StateOrProvince (see CdStateOrProvince.Code), 
         *   Country (see CdCountry.DescEnglish).  All these parameters are required even if the
         *   values haven't changed.
         *
         * Update Familias with RE New Sponsorship
         *
         *   First Input Parameter:
         *     Name - "FunctionCode"
         *     Value - ASP (Add Sponsorship)
         *
         *   Plus an input parameter for each of the following fields where the name of
         *   the parameter is the name of the variable and the value of the parameter is
         *   the value of the variable: Project(F, S, N), SponsorId, MemberId,
         *   Notes, SponsorshipType (GUID - Guiding or LEAD - Lead), Date (YYYYMMDD).
         *   All these parameters are required even if the values haven't changed.
         *
         * Update Familias with RE Removed Sponsorship
         *
         *   First Input Parameter:
         *     Name - "FunctionCode"
         *     Value - RS (Remove Sponsorship)
         *
         *   Plus an input parameter for each of the following fields where the name of
         *   the parameter is the name of the variable and the value of the parameter is
         *   the value of the variable: Project(F, S, N), SponsorId, MemberId,
         *   InactiveReason (FINA- Financial Reasons, LOP - Lack of Payment, DEAD - Death,
         *   DIVO - Divorce, ILL - Illness, OTHR - Other, UNHA - Unhappy with Program,
         *   UNKN - Unknown), Notes, SponsorshipType, OLD - Old Type, GUID - Guiding or
         *   LEAD - Lead), Date (YYYYMMDD).  All these parameters are required even if
         *   the values haven't changed.
         *
         * Update Familias with RE Sponsor Type Change
         *
         *   First Input Parameter:
         *     Name - "FunctionCode"
         *     Value - "STC" (Sponsorship Type Change)
         *
         *   Plus an input parameter for each of the following fields where the name of
         *   the parameter is the name of the variable and the value of the parameter is
         *   the value of the variable: Project, SponsorId, MemberId, SponsorshipType
         *   (GUID - Guiding or LEAD - Lead) and Notes.  All these parameters are required
         *   even if the values haven't changed.
         *
         * The following is a list of possible errors that might occur:
         * Cannot add a sponsor that already exists.
         *  - FunctionCode parameter not found.
         *  - Invalid FunctionCode parameter specified.
         *  - No changes were specified for this sponsor
         *  - Record Locked: [Information about the DB table MiscMemberSponsorInfo being
         *    used by another process]
         *  - The member id does not correspond to an affiliated member.
         *  - The member specified cannot be sponsored since it has the following
         *    sponsorship restriction:
         *  - The sponsor already actively sponsors this member.
         *  - There currently is no sponsor with this id.
         *  - This member currently doesn't have an active relationship to this sponsor
         *    of the specified type
         *  - Valid Country parameter not found
         *  - Valid Date parameter not found
         *  - Valid Gender parameter not found
         *  - Valid InactiveReason parameter not found
         *  - Valid MemberId parameter not found
         *  - Valid Notes parameter not found
         *  - Valid OrganizationContactNames parameter not found
         *  - Valid Project parameter not found
         *  - Valid SponsorId parameter not found
         *  - Valid SponsorNames parameter not found
         *  - Valid SponsorshipType parameter not found
         *  - Valid StateOrProvince parameter not found
         *
         * Copyright:  Copyright (c) 2002 <p>
         * Company:    Familias de Esperanza <p>
         * @author Scott Heringer
         * @version 1.0
         */

        //FunctionCodes
        private static string ADD_SPONSOR = "AS";
        private static string ADD_SPONSORSHIP = "ASP";
        private static string EXTRACT_AFFILIATE_AND_DISAFFILIATE_INFO = "EADI";
        private static string REMOVE_SPONSORSHIP = "RS";
        private static string SPONSOR_DEMO_CHANGE = "SDC";
        private static string SPONSORSHIP_TYPE_CHANGE = "STC";

		#region GET CALLS
		/*
         * EADI -   Update RE with Familias
         */
		public HttpResponseMessage Get(string FunctionCode)
        {
            if (FunctionCode.Equals(EXTRACT_AFFILIATE_AND_DISAFFILIATE_INFO))
            {
                return processExtractAffiliateAndDisaffilateInfo();
            }
            else
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "An error has occurred, try later.");
        }

        /*
         * AS   -   Update Familias with RE New Sponsor
         * SDC  -   Update Familias with RE Sponsor Demographic Change 
         * - Parameters: FunctionCode, SponsorId, SponsorNames, Gemder, Country, SpeaksSpanish, 
         * - Optional parameters: OrganizationContactNames, StateOrProvince
         */
        public HttpResponseMessage Get(string FunctionCode, int SponsorId, string SponsorNames,
            string Gender, string Country, string SpeaksSpanish, string OrganizationContactNames = "NULL", string StateOrProvince = "NULL")
        {
            /*  AS  */
            if (FunctionCode.Equals(ADD_SPONSOR))
			{
                return processAddSponsor(SponsorId, SponsorNames, OrganizationContactNames, Gender, StateOrProvince, Country, SpeaksSpanish);
			}
            /* SDC  */
            else if (FunctionCode.Equals(SPONSOR_DEMO_CHANGE))
			{
                return processSponsorDemographicChange(SponsorId, SponsorNames, OrganizationContactNames, Gender, StateOrProvince, Country, SpeaksSpanish);
			}
			else
			{
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "An error has occurred, try later.");
			}

        }

        /*
         * STC  -   Update Familias with RE Sponsor Type Change
         * - Parameters: FunctionCode, Project, SponsorId, MemberId, SponsorshipType
         * - Optional Parameters: Notes
         */
        public HttpResponseMessage Get(string FunctionCode, string Project, int SponsorId,
            int MemberId, string SponsorshipType, 
            string Notes = "NULL" )
        {
            /*  STC  */
            if (FunctionCode.Equals(SPONSORSHIP_TYPE_CHANGE))
            {
                var response = processSponsorshipTypeChange(FunctionCode, Project, SponsorId, MemberId, SponsorshipType, Notes);
                return response.Result;
            }
            else
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }

        /*
         * ASP  -   Update Familias with RE New Sponsorship
         * RS   -   Update Familias with RE Removed Sponsorship
         * - Parameters: FunctionCode, Project, SponsorId, MemberId, SponsorshipType, Date
         * - Optional Parameters: InactiveReason, Notes
         */
        public HttpResponseMessage Get(string FunctionCode, string Project, int SponsorId,
            int MemberId, string SponsorshipType, string Date, 
            string InactiveReason = "NULL", string Notes = "NULL")
        {
            /*  ASP  */
            if (FunctionCode.Equals(ADD_SPONSORSHIP))
                return processAddSponsorship(FunctionCode, Project, SponsorId, MemberId, Notes, SponsorshipType, Date).Result;
            /*  RS  */
            else if (FunctionCode.Equals(REMOVE_SPONSORSHIP))
                return processRemoveSponsorship(FunctionCode, Project, SponsorId, MemberId, InactiveReason, Notes, SponsorshipType, Date);
            else
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }
		#endregion
		#region RESPONSES

		/*
         * EADI -   Update RE with Familias
         */
		private HttpResponseMessage processExtractAffiliateAndDisaffilateInfo()
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[GlobalData.DBCONNECTION].ToString()))
            {
                connection.Open();
                    
                var query = String.Format(@"
                SELECT m.Project, m.MemberId, m.FirstNames, m.LastNames, m.BirthDate, 
                    m.LastFamilyId AS FamilyId, m.Gender, m.AffiliationStatus, 
                    csr.DescEnglish AS Restriction 
                    FROM {0} m 
                    INNER JOIN {1} mmsi ON 
                        m.Project = mmsi.Project AND 
                        m.MemberId = mmsi.MemberId AND 
                        mmsi.RecordStatus = ' ' 
                    LEFT OUTER JOIN {2} csr ON 
                        mmsi.Restriction = csr.Code 
                    WHERE m.RecordStatus = ' ' AND 
                        (m.AffiliationStatus = 'AFIL' OR 
                        m.AffiliationStatus = 'DESA' OR 
                        m.AffiliationStatus = 'GRAD' ) 
                    ORDER By m.Project, m.MemberId",
                    GlobalData.TABLE_MEMBER, GlobalData.TABLE_MISC_MEMBER_SPNS_INFO, GlobalData.CD_SPONSORSHIP_RESTR);

                SqlDataAdapter da = new SqlDataAdapter(query, connection);

                DataTable dt = new DataTable { TableName = "AffiliateAndDisaffilateInfo" };
                da.Fill(dt);
                    
                if (dt.Rows.Count > 0)
                    return Request.CreateResponse(HttpStatusCode.OK, dt);
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "No data to send");

            }
        }

        /*
         * AS   -   Update Familias with RE New Sponsor
         */
        private HttpResponseMessage processAddSponsor(int SponsorId, string SponsorNames,
            string OrganizationContactNames, string Gender, string StateOrProvince, string Country, string SpeaksSpanish)
        {
            try
            {
                var sponsorIdVerification = string.Format(@"SELECT SponsorId FROM {0} WHERE SponsorId = {1}",
                            GlobalData.TABLE_SPONSOR, SponsorId);

                if (verifyExistence(sponsorIdVerification))
                    return Request.CreateResponse(HttpStatusCode.Found, "Cannot add a sponsor that already exists");
                else
                {
                    //* Verificación de País
                    var countryVerification = string.Format(@"SELECT Code FROM {0} WHERE Code = '{1}' OR DescEnglish = '{1}'",
                                                GlobalData.CD_COUNTRY, Country);

                    if (!verifyExistence(countryVerification))
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Valid Country parameter not found");

                    //* Verificación de Estado o Provincia

                    StateOrProvince = (StateOrProvince == null) ? "NULL" : StateOrProvince;

                    var stateOrProvinceVerification = string.Format(@"
                                            SELECT SP.Code 
                                            FROM {0} SP
                                            INNER JOIN {3} C ON C.Code = SP.Country OR C.DescEnglish = SP.Code
                                            WHERE SP.Code = '{1}' AND (SP.Country = '{2}' OR C.DescEnglish = '{2}')",
                                            GlobalData.CD_STATE_PROVINCE, StateOrProvince, Country, GlobalData.CD_COUNTRY);

                    if (StateOrProvince != "NULL" && !verifyExistence(stateOrProvinceVerification))
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Valid StateOrProvince parameter not found");

                    //* Verificación de género
                    var genderVerification = string.Format(@"SELECT Code FROM {0} WHERE Code = '{1}' OR DescEnglish = '{1}'",
                                                GlobalData.CD_GENDER, Gender);

                    if (!verifyExistence(genderVerification))
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Valid Gender parameter not found");

                    //* Verificación de hablar español
                    if (isInvalidBool(SpeaksSpanish))
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Valid SpeaksSpanish parameter not found");

                    //* Verificación de OrganizationContactNames en caso sea nulo
                    OrganizationContactNames = OrganizationContactNames == null ? "NULL" : OrganizationContactNames;

                    //* Inserción de valores nuevos

                    var newValues = new List<string>() { 
                        "'" + SponsorId.ToString(), DateTime.Now.ToString(), "", GlobalData.DEFAULT_USERID, 
                        SponsorNames, Gender, OrganizationContactNames, StateOrProvince, 
                        Country, SpeaksSpanish.ToString() + "'" 
                    };

                    var newRegister = string.Format(@"
                                    INSERT INTO {0} ({1}) 
                                    VALUES ({2})",
                                    GlobalData.TABLE_SPONSOR, string.Join(",", GlobalData.FIELDS_SPONSOR), string.Join("','", newValues));

                    return insertData(newRegister);
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        /*
         * SDC  -   Update Familias with RE Sponsor Demographic Change 
         */
        private HttpResponseMessage processSponsorDemographicChange(
            int SponsorId, string SponsorNames, string OrganizationContactNames, 
            string Gender, string StateOrProvince, string Country, string SpeaksSpanish)
        {
            try
            {
                //* OBTIENE DATOS DEL SPONSOR ACTUAL
                var actualSponsor = SponsorModel.getSponsorById(SponsorId);
                if(actualSponsor == null)
                    return Request.CreateResponse(HttpStatusCode.Found, "Valid SponsorId parameter not found");
                
                //* Validación de país
                var countryVerification = string.Format(@"SELECT Code FROM {0} WHERE Code = '{1}' OR DescEnglish = '{1}'",
                                        GlobalData.CD_COUNTRY, Country);
                if (!verifyExistence(countryVerification))
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Valid Country parameter not found");

                //* Validación de género
                var genderVerification = string.Format(@"SELECT Code FROM {0} WHERE Code = '{1}' OR DescEnglish = '{1}'",
                                        GlobalData.CD_GENDER, Gender);
                if (!verifyExistence(genderVerification))
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Valid Gender parameter not found");

                //* Validación de hablar español
                if (isInvalidBool(SpeaksSpanish))
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Valid SpeaksSpanish parameter not found");
                SpeaksSpanish = (SpeaksSpanish.Equals("true") || SpeaksSpanish.Equals("false")) ? SpeaksSpanish : SpeaksSpanish.ToString();

                //* Validación de estado o provincia
                StateOrProvince = (StateOrProvince == null) ? "NULL" : StateOrProvince;
                var stateOrProvinceVerification = string.Format(@"
                                            SELECT SP.Code 
                                            FROM {0} SP
                                            INNER JOIN {3} C ON C.Code = SP.Country OR C.DescEnglish = SP.Code
                                            WHERE SP.Code = '{1}' AND (SP.Country = '{2}' OR C.DescEnglish = '{2}')",
                                            GlobalData.CD_STATE_PROVINCE, StateOrProvince, Country, GlobalData.CD_COUNTRY);

                if (StateOrProvince != "NULL" && !verifyExistence(stateOrProvinceVerification))
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Valid StateOrProvince parameter not found");

                //* Verificación de OrganizationContactNames en caso sea nulo
                OrganizationContactNames = OrganizationContactNames == null ? "NULL" : OrganizationContactNames;

                //* Nuevos datos a ingresar
                var newValues = new List<string>() { "'" + SponsorId.ToString(), DateTime.Now.ToString(), 
                    actualSponsor.RecordStatus, GlobalData.DEFAULT_USERID, SponsorNames, 
                    Gender, OrganizationContactNames, StateOrProvince, Country, SpeaksSpanish + "'" 
                };

                var newRegister = string.Format(@"
                                INSERT INTO {0} ({1}) 
                                VALUES ({2})",
                                GlobalData.TABLE_SPONSOR, string.Join(",", GlobalData.FIELDS_SPONSOR), string.Join("','", newValues));

                return insertData(newRegister);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        /*
         * ASP  -   Update Familias with RE New Sponsorship
         */
        private async Task<HttpResponseMessage> processAddSponsorship(string fc, string Project, int SponsorId, int MemberId, string Notes, string SponsorshipType, string Date)
        {
            try
            {
                //**verificar formatos
                Date = isValidDate(Date);
                if (Date == null)
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Valid date format is not found.");

                //* Verificar memberId y si es afiliado
                var selectedMember = MemberModel.getMemberById(MemberId, Project);
                if (selectedMember == null || !MemberModel.isAffiliated(selectedMember.AffiliationStatus))
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "The member id does not correspond to an affiliated member");

                //* Verifica que no se tenga ninguna restricción para continuar
                var restrictionCode = MiscMemberSponsorInfoModel.getMiscMemberSponsorInfobyId(MemberId, Project).Result.Restriction;
                if (restrictionCode != null)
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "The member specified cannot be sponsored since it has the following sponsorship restriction: " + MiscMemberSponsorInfoModel.getRestrictionDesc(restrictionCode));

                //* Verifica SponsorId
                if (!SponsorModel.isValidSponsor(SponsorId))
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "There currently is no sponsor with this id");

                var keyVerification = string.Format(@"SELECT * FROM {0} 
                                                      WHERE SponsorId = {1} AND MemberId = {2}
                                                        AND Project = '{3}' AND Recordstatus = ' ' AND EndDate IS NULL",
                            GlobalData.TABLE_SPONSOR_MEMBER_REL, SponsorId, MemberId, Project);

                if(verifyExistence(keyVerification))
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "There is actually an existing relation for the selected Member and Sponsor");

                //* Verifica sponsorshipType valido
                if(!isValidSponsorshipType(SponsorshipType))
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "There currently is no valid sponsorship type: " + SponsorshipType);

                //* Obtener cuantos padrinos actuales tiene y depende esto, (según tabla enviada) permite o no la inserción
                var levelSponsorData = await getLevelAndSponsorNumber(Project, MemberId);
                
                if(!hasValidRelations(levelSponsorData.level, SponsorshipType))
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "The member specified cannot be sponsored. The new sponsorship type is not valid for the actual member.");

                //* Proceso de creación de relación
                var addSMRResponse = await processAddSponsorMemberRelation(fc, Project, SponsorId, MemberId, Notes, SponsorshipType, Date);
                if (addSMRResponse.StatusCode != HttpStatusCode.OK)
                    return addSMRResponse;

                //* Actualización de datos en MiscMemberSponsorInfo
                var ADDAMMSIResponse = processAddMiscMemberSponsorInfo(fc, Project, MemberId, SponsorId, SponsorshipType);
                if (ADDAMMSIResponse.Result.StatusCode != HttpStatusCode.OK)
                    return ADDAMMSIResponse.Result;

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        /*
         * STC  -   Update Familias with RE Sponsor Type Change
         */
        private async Task<HttpResponseMessage> processSponsorshipTypeChange(string fc, string Project, int SponsorId,
            int MemberId, string SponsorshipType, string Notes)
        {
            try
            {
                //Verificar MemberId
                var memberIdVerification = string.Format(@"SELECT MemberId FROM {0} WHERE MemberId = {1}",
                                            GlobalData.TABLE_MEMBER, MemberId);

                if (!verifyExistence(memberIdVerification))
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Valid MemberId parameter not found");

                //Verificar SponsorId
                var sponsorIdVerification = string.Format(@"SELECT SponsorId FROM {0} WHERE SponsorId = {1}",
                                            GlobalData.TABLE_SPONSOR, SponsorId);

                if (!verifyExistence(sponsorIdVerification))
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "There currently is no sponsor with this id");

                //Obtener getActiveSponsorMemberRelation y verificar que exista
                if(!MemberModel.getActiveSponsorMemberRelation(Project, MemberId, SponsorId))
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "This member currently doesn't have an active relationship to this sponsor of the specified type.");

                // Verifica que sea correcto el tipo de sponsor
                if(SponsorshipType != "GUID" && SponsorshipType != "LEAD")
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Valid SponsorshipType parameter not found");

                //validar notas
                if(Notes == string.Empty || Notes == null)
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Valid Notes parameter not found");

                //Obtener getActiveSponsorMemberRelation y verificar que exista
                var relations = SponsorMemberRelationModel.getSponsorMemberRelationByIds(MemberId, SponsorId, Project, fc);

                if (relations == null || relations.Count == 0)
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "This member currently doesn't have an active relationship to this sponsor of the specified type");

                if(relations.Count > 1)
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Can not change when there is more than one active relation.");

                if (relations.Exists(x => x.Type == SponsorshipType))
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "No changes were specified for this sponsor");

                //Verificando relaciones correctas y calculando sponsorshiplevel
                var newSponsorshipLevel = string.Empty;
                if (!canCreateRelation(SponsorshipType, MemberId, SponsorId, Project, ref newSponsorshipLevel, true, fc))
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Cannot add invalid relation type");

                var selectedMMSI = await MiscMemberSponsorInfoModel.getMiscMemberSponsorInfobyId(MemberId, Project);
                // Finalizar registro actual
                var addSMRResponse = finishActualMemberRelation(relations[0]);

                if (addSMRResponse.StatusCode != HttpStatusCode.OK)
                    return addSMRResponse;

                //Insertar nuevo
                addSMRResponse = STCNewRelation(fc, Project, SponsorId, MemberId, Notes, SponsorshipType, DateTime.Now.ToString());
                if (addSMRResponse.StatusCode != HttpStatusCode.OK)
                    return addSMRResponse;

                var newValues = new List<string>() { "'" + Project, MemberId.ToString(), DateTime.Now.ToString(), 
                    selectedMMSI.RecordStatus, GlobalData.DEFAULT_USERID, selectedMMSI.ExpirationDateTime, 
                    selectedMMSI.Photo, selectedMMSI.PhotoDate, selectedMMSI.RetakePhotoDate, 
                    selectedMMSI.RetakephotoUserId, selectedMMSI.LastCarnetPrintDate, newSponsorshipLevel, 
                    SponsorshipType, selectedMMSI.Restriction, selectedMMSI.RestrictionDate, 
                    selectedMMSI.ExceptionPhotoDate };

                newValues = newValues.Select(s => s ?? "NULL").ToList();

                newValues[newValues.Count - 1] = newValues[newValues.Count - 1] + "'";

                var newRegister = string.Format(@"
                                    INSERT INTO {0} ({1}) 
                                    VALUES ({2})",
                                GlobalData.TABLE_MISC_MEMBER_SPNS_INFO , string.Join(",", GlobalData.FIELDS_MISC_MEMBER_SPONSOR_INFO), string.Join("','", newValues));

                return insertData(newRegister);

            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        /*
         * RS   -   Update Familias with RE Removed Sponsorship
         */
        private HttpResponseMessage processRemoveSponsorship(string fc, string Project, int SponsorId,
            int MemberId, string InactiveReason, string Notes, string SponsorshipType, string Date)
        {
            try
            {
                //* Valida miembro y verifica que sea afiliado

                var selectedMember = MemberModel.getMemberById(MemberId, Project);

                if (selectedMember == null )
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "The member id does not correspond to an affiliated member");

                //Verificar SponsorId
                
                if (!SponsorModel.isValidSponsor(SponsorId))
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "There currently is no sponsor with this id");

                //* Verifica que no se tenga ninguna restricción para continuar
                var restrictionCode = MiscMemberSponsorInfoModel.getMiscMemberSponsorInfobyId(MemberId, Project).Result.Restriction;

                if (restrictionCode != null)
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "The member specified cannot be sponsored since it has the following sponsorship restriction: " + MiscMemberSponsorInfoModel.getRestrictionDesc(restrictionCode));


                //Obtener getActiveSponsorMemberRelation y verificar que exista
                var relations = SponsorMemberRelationModel.getSponsorMemberRelationByIds(MemberId, SponsorId, Project, fc);
                if(relations == null || relations.Count == 0 || !relations.Exists(x => x.Type == SponsorshipType))
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "This member currently doesn't have an active relationship to this sponsor of the specified type");

                //* Verificando tipo valido
                if (!isValidSponsorshipType(SponsorshipType))
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Valid SponsorshipType parameter not found");

                //**verificar formatos
                Date = isValidDate(Date);
                if(Date == null)
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Valid date format is not found.");

                //validar InactiveReason
                if(!SponsorMemberRelationModel.isValidInactiveReason(InactiveReason))
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Valid InactiveReason parameter not found");

                //validar notas
                Notes = Notes == null ? "NULL" : Notes;

                //! Nueva inserción - Corregir insert-select
                var addSMRResponse = processAddSponsorMemberRelationEnd(fc, Project, SponsorId, MemberId, Notes, SponsorshipType, Date, relations[0], InactiveReason);
                if (addSMRResponse.StatusCode != HttpStatusCode.OK)
                    return addSMRResponse;

                var ADDAMMSIResponse = processAddMiscMemberSponsorInfo(fc, Project, MemberId, SponsorId, SponsorshipType);
                if (ADDAMMSIResponse.Result.StatusCode != HttpStatusCode.OK)
                    return ADDAMMSIResponse.Result;

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        private HttpResponseMessage STCNewRelation(string fc, string Project, int SponsorId, int MemberId, string Notes, string SponsorshipType, string Date)
        {
            try
            {
                //* Inserta nuevos valores de miembro y sponsor a relacionar
                var newValues = new List<string>() { "'" + Project, SponsorId.ToString(), MemberId.ToString(), DateTime.Now.ToString(), " ", GlobalData.DEFAULT_USERID, "NULL", SponsorshipType, Notes, "NULL", Date.ToString(), "NULL" + "'" };

                var newRegister = string.Format(@"
                                    INSERT INTO {0} ({1}) 
                                    SELECT	Project, SponsorId, MemberId, GETDATE(), ' ', 'ReInterface', NULL, '{5}', '{6}', InactiveReason, StartDate, EndDate
                                    FROM	{0}
                                    WHERE	RecordStatus=' ' and Project='{2}' and MemberId={3} and SponsorId={4}",
                                    GlobalData.TABLE_SPONSOR_MEMBER_REL,
                                    string.Join(",", GlobalData.FIELDS_SPONSOR_MEMBER_REL),
                                    Project, MemberId.ToString(), SponsorId.ToString(), SponsorshipType, Notes);

                return insertData(newRegister);

            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        private async Task<Level_SponsorNumberInfo> getLevelAndSponsorNumber(string Project, int MemberId)
        {
            try
            {
                //* Inserta nuevos valores de miembro y sponsor a relacionar
                

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[GlobalData.DBCONNECTION].ToString()))
                {
                    connection.Open();

                    var query = string.Format(@"
                                    DECLARE @level varchar(4)
                                    DECLARE @nPadrinos int
                                    DECLARE @nLevel varchar(4)

                                    SELECT @level = SponsorshipLevel, @nPadrinos = dbo.fn_GEN_Npadrinos(Project, MemberId)
                                    FROM {0}
                                    WHERE RecordStatus = ' ' AND Project = '{1}' AND MemberId = {2};

                                    SELECT @level, @nPadrinos",
                                    GlobalData.TABLE_MISC_MEMBER_SPNS_INFO, Project, MemberId);

                    SqlCommand command = new SqlCommand(query, connection); command.CommandType = System.Data.CommandType.Text;

                    SqlDataReader queryObject = command.ExecuteReader();

                    if (queryObject.Read())
                    {
                        var info = new Level_SponsorNumberInfo
                        {
                            level = (string)queryObject.GetValue(0),
                            n_sponsors = Convert.ToInt32(queryObject.GetValue(1))
                        };
                        return info;
                    }
                    return null;
                }

            }
            catch (Exception e)
            {
                return null;
            }
        }


        private async Task<HttpResponseMessage> processAddSponsorMemberRelation(string fc, string Project, int SponsorId, int MemberId, string Notes, string SponsorshipType, string Date)
        {
            try
            {
                //* Inserta nuevos valores de miembro y sponsor a relacionar
                var newValues = new List<string>() { "'" + Project, SponsorId.ToString(), MemberId.ToString(), DateTime.Now.ToString(), " ", GlobalData.DEFAULT_USERID, "NULL", SponsorshipType, Notes, "NULL", Date.ToString(), "NULL" + "'" };

                var newRegister = string.Format(@"
                                    INSERT INTO {0} ({1}) 
                                    VALUES ({2})",
                                    GlobalData.TABLE_SPONSOR_MEMBER_REL, 
                                    string.Join(",", GlobalData.FIELDS_SPONSOR_MEMBER_REL), 
                                    string.Join("','", newValues));

                return insertData(newRegister);

            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        private HttpResponseMessage processAddSponsorMemberRelationEnd(string fc, string Project, int SponsorId, int MemberId, string Notes, string SponsorshipType, string Date, SponsorMemberRelationModel relation, string InactiveReason)
        {
            try
            {
                
                //* Inserta nuevos valores de miembro y sponsor a relacionar
                var newValues = new List<string>() { "'" + Project, SponsorId.ToString(), MemberId.ToString(), DateTime.Now.ToString(), relation.RecordStatus, GlobalData.DEFAULT_USERID, "NULL", relation.Type, relation.Notes, InactiveReason, relation.StartDate, Date.ToString() + "'" };

                var newRegister = string.Format(@"
                                    INSERT INTO {0} ({1}) 
                                    VALUES ({2})",
                                    GlobalData.TABLE_SPONSOR_MEMBER_REL,
                                    string.Join(",", GlobalData.FIELDS_SPONSOR_MEMBER_REL),
                                    string.Join("','", newValues));

                return insertData(newRegister);

            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        private HttpResponseMessage finishActualMemberRelation(SponsorMemberRelationModel relation)
        {
            try
            {
                var endDate = relation.EndDate != null ? relation.EndDate.ToString() : "NULL";
                //* Inserta nuevos valores de miembro y sponsor a relacionar
                var newValues = new List<string>() { "'" + relation.Project, relation.SponsorId.ToString(), 
                    relation.MemberId.ToString(), DateTime.Now.ToString(), "H", relation.UserId, 
                    DateTime.Now.ToString(), relation.Type, relation.Notes != null ? relation.Notes : "NULL", 
                    relation.InactiveReason != null ? relation.InactiveReason : "NULL", 
                    relation.StartDate, relation.EndDate != null ? relation.EndDate : "NULL" + "'" };

//                var newRegister = string.Format(@"
//                                    INSERT INTO {0} ({1}) 
//                                    VALUES ({2})",
//                                    GlobalData.TABLE_SPONSOR_MEMBER_REL,
//                                    string.Join(",", GlobalData.FIELDS_SPONSOR_MEMBER_REL),
//                                    string.Join("','", newValues));

                var newRegister = string.Format(@"
                    INSERT INTO {0} ({1})
                    SELECT	Project, 
                        SponsorId,
                        MemberId, 
                        GETDATE(), 
                        'H', 
                        'ReInterface', 
                        GETDATE(), 
                        Type, 
                        Notes, 
                        InactiveReason, 
                        StartDate, 
                        EndDate
                    FROM dbo.SponsorMemberRelation
                    WHERE RecordStatus = ' ' AND Project = '{4}' AND MemberId = {2} AND SponsorId = {3}
                    ", GlobalData.TABLE_SPONSOR_MEMBER_REL, 
                     string.Join(",", GlobalData.FIELDS_SPONSOR_MEMBER_REL),
                     relation.MemberId.ToString(), 
                     relation.SponsorId.ToString(), 
                     relation.Project);
                return insertData(newRegister);

            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        private async Task<HttpResponseMessage> processAddMiscMemberSponsorInfo(string fc, string Project, int MemberId, int SponsorId, string SponsorshipType)
        {
            try
            {
                //**Verificacion para poder insertar
                var newSponsorshipLevel = string.Empty;
                if(!canCreateRelation(SponsorshipType, MemberId, SponsorId, Project, ref newSponsorshipLevel, false, fc))
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Cannot add invalid relation type");

                //Obtener datos por Project y MemberId
                var selectedMMSI = MiscMemberSponsorInfoModel.getMiscMemberSponsorInfobyId(MemberId, Project);

                var recordStatus = fc == "ASP" ? " " : selectedMMSI.Result.RecordStatus;
                //Insertar datos - primera inserción 
                var newValues = new List<string>() { Project, MemberId.ToString(), DateTime.Now.ToString(), recordStatus, GlobalData.DEFAULT_USERID, "NULL", selectedMMSI.Result.Photo, selectedMMSI.Result.PhotoDate, selectedMMSI.Result.RetakePhotoDate, selectedMMSI.Result.RetakephotoUserId, selectedMMSI.Result.LastCarnetPrintDate, newSponsorshipLevel, SponsorshipType, selectedMMSI.Result.Restriction, selectedMMSI.Result.RestrictionDate, selectedMMSI.Result.ExceptionPhotoDate };

                newValues = newValues.Select(s => s ?? "NULL").ToList();

                newValues[0] = "'" + newValues[0];
                newValues[newValues.Count - 1] = newValues[newValues.Count - 1] + "'";

                var newRegister = string.Format(@"
                                    INSERT INTO {0} ({1}) 
                                    VALUES ({2})",
                                    GlobalData.TABLE_MISC_MEMBER_SPNS_INFO,
                                    string.Join(",", GlobalData.FIELDS_MISC_MEMBER_SPONSOR_INFO),
                                    string.Join("','", newValues));

                return insertData(newRegister);

            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        
        #endregion
        #region EXTRAS

        private static string isValidDate(string Date)
        {
            DateTime result;
            if (!DateTime.TryParse(Date, out result))
            {
                try
                {
                    return DateTime.ParseExact(Date, "d/M/yyyy", CultureInfo.InvariantCulture)
                        .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                catch
                {
                    //* Specific format: dd-mm-yyyy
                    var separatedDate = Date.Split('-');
                    if (separatedDate.Count() == 3 && Convert.ToInt32(separatedDate[1]) <= 12)
                    {
                        var newdate = separatedDate[2] + '-' + separatedDate[1] + '-' + separatedDate[0];
                        DateTime parsedDate;
                        DateTime.TryParse(newdate, out parsedDate);
                        return parsedDate != null ? newdate : null;
                    }
                    return null;
                }
            }
            return Date;
        }

        private static bool hasValidRelations(string level, string sponsorshipType)
        {
            if ((level == "PARC" && sponsorshipType == "LEAD") || level == "COMP")
            {
                return false;
            }

            return true;
        }

        private static bool isValidSponsorshipType(string sponsorshipType)
        {
            return sponsorshipType.Equals("GUID") || sponsorshipType.Equals("LEAD");
        }

        private static bool canCreateRelation(string SponsorshipType, int MemberId, int SponsorId, string Project, ref string newSponsorshipLevel, bool useActual, string fc)
        {
            
            var relations = SponsorMemberRelationModel.getSponsorMemberRelationByIds(MemberId, SponsorId, Project, fc, true);


            if (relations == null || relations.Count.Equals(0))
            {
                newSponsorshipLevel = "NING";
                return true;
            }
            else if (relations.Count.Equals(1))
            {
                relations[0].Type = useActual ? SponsorshipType : relations[0].Type;

                if (relations[0].Type.Equals("LEAD"))
                {
                    newSponsorshipLevel = "COMP";
                    return true;
                }
                else if (relations[0].Type.Equals("GUID"))
                {
                    newSponsorshipLevel = "PARC";
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (relations.Count.Equals(2))
            {
                if (relations[0].Type.Equals("GUID") && relations[1].Type.Equals("GUID"))
                {
                    newSponsorshipLevel = "COMP";
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            
        }

        private static bool verifyExistence(string query)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[GlobalData.DBCONNECTION].ToString()))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection); command.CommandType = System.Data.CommandType.Text;

                    SqlDataReader queryObject = command.ExecuteReader();

                    return queryObject.Read() ? true : false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private HttpResponseMessage insertData(string query)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[GlobalData.DBCONNECTION].ToString()))
                {
                    query = query.Replace("'NULL'", "NULL");

                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    object objetoConsulta = command.ExecuteScalar();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        public static bool isInvalidBool(string value)
        {
            return value == null || (!value.Equals("0") && !value.Equals("1") && !Convert.ToBoolean(value).Equals(true) && !Convert.ToBoolean(value).Equals(false)) ? true : false;
        }

        public static string ConvertToXML<T>(T dataToSerialize)
        {
            try
            {
                var stringwriter = new System.IO.StringWriter();
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stringwriter, dataToSerialize);
                return stringwriter.ToString();
            }
            catch
            {
                throw;
            }
        }

        #endregion
    }
}

