﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FamiliasraisersEdgeEntities
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class FamiliasTestEntities : DbContext
    {
        public FamiliasTestEntities()
            : base("name=FamiliasTestEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<CdCountry> CdCountries { get; set; }
        public DbSet<CdGender> CdGenders { get; set; }
        public DbSet<CdSponsorMemberRelationInactiveReason> CdSponsorMemberRelationInactiveReasons { get; set; }
        public DbSet<CdSponsorMemberRelationType> CdSponsorMemberRelationTypes { get; set; }
        public DbSet<CdSponsorshipLevel> CdSponsorshipLevels { get; set; }
        public DbSet<CdSponsorshipRestriction> CdSponsorshipRestrictions { get; set; }
        public DbSet<CdStateOrProvince> CdStateOrProvinces { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MiscMemberSponsorInfo> MiscMemberSponsorInfoes { get; set; }
        public DbSet<Sponsor> Sponsors { get; set; }
        public DbSet<SponsorMemberRelation> SponsorMemberRelations { get; set; }
        public DbSet<v_AMBF_CondicionesVivienda> v_AMBF_CondicionesVivienda { get; set; }
        public DbSet<v_GEN_Apadrinados> v_GEN_Apadrinados { get; set; }
        public DbSet<v_GEN_FamiliasAfiliadas> v_GEN_FamiliasAfiliadas { get; set; }
        public DbSet<v_GEN_PoblaciónConDerechos> v_GEN_PoblaciónConDerechos { get; set; }
        public DbSet<v_GEN_PoblacionPorSitio> v_GEN_PoblacionPorSitio { get; set; }
    }
}
