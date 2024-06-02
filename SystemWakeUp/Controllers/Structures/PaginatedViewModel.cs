using System;
using SystemWakeUp.DBHandler.Entity;

namespace SystemWakeUp.Controllers.Structures
{
    public class PaginatedViewModel
    {
        public List<DBEntity> Data { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
    }
}

