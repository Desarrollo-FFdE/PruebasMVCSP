//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class CdSponsorshipRestriction
    {
        public CdSponsorshipRestriction()
        {
            this.MiscMemberSponsorInfoes = new HashSet<MiscMemberSponsorInfo>();
        }
    
        public string Code { get; set; }
        public string DescEnglish { get; set; }
        public string DescSpanish { get; set; }
    
        public virtual ICollection<MiscMemberSponsorInfo> MiscMemberSponsorInfoes { get; set; }
    }
}
