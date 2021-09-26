using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IMenuContainer
    {
        string Name { get; }
        IReadOnlyCollection<MenuItemDto> MenuItems { get; }
        MenuItemDto AddMenuItem(MenuItemDto menuItem, MenuItemDto parent = null);
        MenuItemDto AddGroup(MenuItemDto menuItem, MenuItemDto parent = null);
        MenuItemDto GetItem(string name);
        void RemoveEmptyGroups();
        void Sort();
    }
}
