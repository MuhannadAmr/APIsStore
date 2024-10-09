﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DEMO.Core.Helper
{
    public class PaginationResponse<TEntity>
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int Count { get; set; }
        public IEnumerable<TEntity> Data { get; set; }

        public PaginationResponse(int pageSize, int pageIndex, int count, IEnumerable<TEntity> data)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            Count = count;
            Data = data;
        }
    }
}