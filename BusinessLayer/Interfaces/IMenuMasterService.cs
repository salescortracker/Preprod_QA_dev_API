using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IMenuMasterService
    {
        /// <summary>
        /// Get all menu master records.
        /// </summary>
        Task<IEnumerable<MenuMasterDto>> GetAllMenusAsync();

        /// <summary>
        /// Get a single menu master record by ID.
        /// </summary>
        Task<MenuMasterDto?> GetMenuByIdAsync(int id);

        /// <summary>
        /// Search menus dynamically by properties like MenuName, ParentMenuID, IsActive, etc.
        /// </summary>
        Task<IEnumerable<MenuMasterDto>> SearchMenusAsync(object filter);

        /// <summary>
        /// Add a new menu.
        /// </summary>
        Task<MenuMasterDto> AddMenuAsync(MenuMasterDto dto, int createdBy);

        /// <summary>
        /// Update an existing menu.
        /// </summary>
        Task<MenuMasterDto> UpdateMenuAsync(int id, MenuMasterDto dto, int modifiedBy);

        /// <summary>
        /// Delete a menu by ID.
        /// </summary>
        Task<bool> DeleteMenuAsync(int id);

        /// <summary>
        /// Get all active menus.
        /// </summary>
        Task<IEnumerable<MenuMasterDto>> GetActiveMenusAsync();

           }
}
