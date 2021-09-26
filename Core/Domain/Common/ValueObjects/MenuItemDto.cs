using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ValueObjects
{
    public class MenuItemDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Link { get; set; }
        public int Weight { get; set; } = 100;
        public QueryParamDto[] QueryParams { get; set; }
        public List<MenuItemDto> SubItems { get; set; } = new List<MenuItemDto>();
        public bool IsGroupOnly { get; set; } = false;
    }
    public class QueryParamDto
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
