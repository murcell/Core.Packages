﻿using Core.Persistence.Paging;

namespace Core.Application.Response;

public class GetListResponse<T>:BasePagableModel
{
    private IList<T> _items;

    public IList<T> Items
    {
        get => _items ?? new List<T>();
        set => _items = value;
    }
}
