using Application.Interfaces;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace Application.Infrastructure
{
    public class MenuContainer : IMenuContainer
    {
        public string Name { get; set; }
        private List<MenuItemDto> allMenuItems = new List<MenuItemDto>();
        private List<MenuItemDto> rootMenuItems = new List<MenuItemDto>();
        public IReadOnlyCollection<MenuItemDto> MenuItems => rootMenuItems.AsReadOnly();
        private int id_ = 0;

        public MenuItemDto AddGroup(MenuItemDto menuItem, MenuItemDto parent = null)
        {
            menuItem = AddMenuItem(menuItem, parent);
            menuItem.IsGroupOnly = true;
            return menuItem;
        }

        public MenuItemDto AddMenuItem(MenuItemDto menuItem, MenuItemDto parent = null)
        {
            Contract.Requires(menuItem != null);
            if (parent != null)
                Contract.Requires(allMenuItems.Exists(s => s == parent));

            MenuItemDto exist = null;
            if (menuItem.Name != null)
            {
                if (parent == null)
                    exist = rootMenuItems.Where(s => s.Name == menuItem.Name).FirstOrDefault();
                else
                    exist = parent.SubItems.Where(s => s.Name == menuItem.Name).FirstOrDefault();
            }

            if (exist != null)
            {
                exist.Weight = menuItem.Weight;
                return exist;
            }
            menuItem.Id = ++id_;
            allMenuItems.Add(menuItem);

            if (parent == null)
                rootMenuItems.Add(menuItem);
            else
                parent.SubItems.Add(menuItem);

            return menuItem;
        }

        public MenuItemDto GetItem(string name)
        {
            return allMenuItems.Where(s => s.Name == name).FirstOrDefault();
        }

        public void RemoveEmptyGroups()
        {
            var items = rootMenuItems.ToList();
            foreach (var item in items)
                removeEmptyGroups(item, null);
        }
        private void removeEmptyGroups(MenuItemDto menuItem, MenuItemDto parent)
        {
            if (menuItem.SubItems.Count > 0)
            {
                var children = menuItem.SubItems.ToList();
                foreach (var child in children)
                    removeEmptyGroups(child, menuItem);
            }
            if (menuItem.IsGroupOnly && menuItem.SubItems.Count == 0)
            {
                allMenuItems.Remove(menuItem);
                if (parent == null)
                    rootMenuItems.Remove(menuItem);
                else
                    parent.SubItems.Remove(menuItem);
            }
        }

        public void Sort()
        {
            //allMenuItems.ForEach(s => s.SubItems = s.SubItems?.OrderBy(o => o.Weight).ThenBy(o => o.Id).ToList());
            //allMenuItems.ForEach(s => s.SubItems?.ForEach(r => r.SubItems = r.SubItems?.OrderBy(o => o.Weight).ThenBy(o => o.Id).ToList()));
            //allMenuItems.ForEach(s => s.SubItems?.ForEach(r => r.SubItems?.ForEach(p =>
            //p.SubItems = p.SubItems?.OrderBy(o => o.Weight).ThenBy(o => o.Id).ToList())));
            //allMenuItems = allMenuItems.OrderBy(s => s.Weight).ThenBy(s => s.Id).ToList();
            foreach (var item in rootMenuItems)
                SortChildren(item);
            rootMenuItems = rootMenuItems.OrderBy(s => s.Weight).ThenBy(s => s.Id).ToList();
        }
        private void SortChildren(MenuItemDto menuItem)
        {
            if (menuItem.SubItems.Count > 0)
            {
                foreach (var child in menuItem.SubItems)
                    SortChildren(child);
                menuItem.SubItems = menuItem.SubItems.OrderBy(s => s.Weight).ThenBy(s => s.Id).ToList();
            }
        }
    }
}
