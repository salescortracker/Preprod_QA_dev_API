using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.DBContext;
using DataAccessLayer.Repositories.GeneralRepository;

namespace BusinessLayer.Implementations
{
    public class MenuMasterService:IMenuMasterService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MenuMasterService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // 🟢 Get all menus
        public async Task<IEnumerable<MenuMasterDto>> GetAllMenusAsync()
        {
            var menus = await _unitOfWork.Repository<MenuMaster>().GetAllAsync();
            return menus.Select(m => MapToDto(m));
        }

        // 🟢 Get menu by ID
        public async Task<MenuMasterDto?> GetMenuByIdAsync(int id)
        {
            var menu = await _unitOfWork.Repository<MenuMaster>().GetByIdAsync(id);
            return menu == null ? null : MapToDto(menu);
        }

        // 🟢 Dynamic search (similar to Company)
        public async Task<IEnumerable<MenuMasterDto>> SearchMenusAsync(object filter)
        {
            var props = filter.GetType().GetProperties();
            var allMenus = await _unitOfWork.Repository<MenuMaster>().GetAllAsync();
            var query = allMenus.AsQueryable();

            foreach (var prop in props)
            {
                var name = prop.Name;
                var value = prop.GetValue(filter);

                if (value != null)
                {
                    switch (name)
                    {
                        case nameof(MenuMaster.MenuName):
                            query = query.Where(m => m.MenuName != null &&
                                m.MenuName.Contains(value.ToString()!, StringComparison.OrdinalIgnoreCase));
                            break;
                        case nameof(MenuMaster.ParentMenuId):
                            if (int.TryParse(value.ToString(), out int parentId))
                                query = query.Where(m => m.ParentMenuId == parentId);
                            break;
                        case nameof(MenuMaster.IsActive):
                            bool isActive = Convert.ToBoolean(value);
                            query = query.Where(m => m.IsActive == isActive);
                            break;
                    }
                }
            }

            var results = query.ToList();
            return results.Select(m => MapToDto(m));
        }

        // 🟢 Add new menu
        public async Task<MenuMasterDto> AddMenuAsync(MenuMasterDto dto, int createdBy)
        {
            var entity = new MenuMaster
            {
                MenuName = dto.MenuName,
                ParentMenuId = dto.ParentMenuID,
                Url = dto.Url,
                Icon = dto.Icon,
                OrderNo = dto.OrderNo,
                IsActive = dto.IsActive ?? true,
                CreatedBy = createdBy,
                CreatedDate = DateTime.Now
            };

            await _unitOfWork.Repository<MenuMaster>().AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            return MapToDto(entity);
        }

        // 🟢 Update existing menu
        public async Task<MenuMasterDto> UpdateMenuAsync(int id, MenuMasterDto dto, int modifiedBy)
        {
            var entity = await _unitOfWork.Repository<MenuMaster>().GetByIdAsync(id);
            if (entity == null)
                throw new Exception("Menu not found");

            entity.MenuName = dto.MenuName;
            entity.ParentMenuId = dto.ParentMenuID;
            entity.Url = dto.Url;
            entity.Icon = dto.Icon;
            entity.OrderNo = dto.OrderNo;
            entity.IsActive = dto.IsActive;
            entity.ModifiedBy = modifiedBy;
            entity.ModifiedAt = DateTime.Now;

            _unitOfWork.Repository<MenuMaster>().Update(entity);
            await _unitOfWork.CompleteAsync();

            return MapToDto(entity);
        }

        // 🟢 Delete menu
        public async Task<bool> DeleteMenuAsync(int id)
        {
            var entity = await _unitOfWork.Repository<MenuMaster>().GetByIdAsync(id);
            if (entity == null) return false;

            _unitOfWork.Repository<MenuMaster>().Remove(entity);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        // 🟢 Get only active menus
        public async Task<IEnumerable<MenuMasterDto>> GetActiveMenusAsync()
        {
            var menus = await _unitOfWork.Repository<MenuMaster>().FindAsync(m => m.IsActive == true);
            return menus.Select(m => MapToDto(m));
        }

        // 🧭 Mapper
        private MenuMasterDto MapToDto(MenuMaster m)
        {
            return new MenuMasterDto
            {
                MenuID = m.MenuId,
                MenuName = m.MenuName,
                ParentMenuID = m.ParentMenuId,
                Url = m.Url,
                Icon = m.Icon,
                OrderNo = m.OrderNo,
                IsActive = m.IsActive
            };
        }
    }

 }
