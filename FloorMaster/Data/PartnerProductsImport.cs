//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FloorMaster.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class PartnerProductsImport
    {
        public int Id { get; set; }
        public int IdProduction { get; set; }
        public int IdPartnerName { get; set; }
        public int CountOfProduction { get; set; }
        public Nullable<System.DateTime> DateOfSale { get; set; }
    
        public virtual PartnerName PartnerName { get; set; }
        public virtual Production Production { get; set; }
    }
}
