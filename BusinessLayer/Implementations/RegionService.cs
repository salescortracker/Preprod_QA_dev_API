using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.DBContext;
using DataAccessLayer.Repositories.GeneralRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusinessLayer.Implementations
{
    public class RegionService : IRegionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<RegionDto>> GetAllRegionsAsync(int userId)
        {
            var regions = await _unitOfWork.Repository<Region>().GetAllAsync();
            return regions.Where(x=>x.UserId==userId).Select(MapToDto);
        }

        public async Task<RegionDto?> GetRegionByIdAsync(int id)
        {
            var region = await _unitOfWork.Repository<Region>().GetByIdAsync(id);
            return region == null ? null : MapToDto(region);
        }

        public async Task<IEnumerable<RegionDto>> SearchRegionsAsync(object filter)
        {
            var props = filter.GetType().GetProperties();
            var allCompanies = await _unitOfWork.Repository<Region>().GetAllAsync();
            var query = allCompanies.AsQueryable();
           
            foreach (var prop in props)
            {
                var name = prop.Name;
                var value = prop.GetValue(filter);
                if (value != null)
                {
                    switch (name)
                    {
                        case nameof(Region.RegionName):
                            query = query.Where(r => r.RegionName.Contains(value.ToString()!));
                            break;
                        case nameof(Region.Country):
                            query = query.Where(r => r.Country.Contains(value.ToString()!));
                            break;
                        case nameof(Region.CompanyId):
                            query = query.Where(r => r.CompanyId == Convert.ToInt32(value));
                            break;
                    }
                }
            }

            var result =  query.ToList();
            return result.Select(MapToDto);
        }

        public async Task<RegionDto> AddRegionAsync(object model)
        {
            var entity = MapFromDynamic(model);
            entity.CreatedDate = DateTime.Now;
            
            await _unitOfWork.Repository<Region>().AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            return MapToDto(entity);
        }

        public async Task<RegionDto> UpdateRegionAsync(int id, object model)
        {
            try
            {
                var existing = await _unitOfWork.Repository<Region>().GetByIdAsync(id);
                if (existing == null)
                    throw new Exception("Region not found");

                var updateData = MapFromDynamic(model);

                existing.CompanyId = updateData.CompanyId;
                existing.RegionName = updateData.RegionName;
                existing.Country = updateData.Country;
                existing.ModifiedAt = DateTime.Now;
                existing.UserId = updateData.UserId;
                existing.IsActive = updateData.IsActive;
                _unitOfWork.Repository<Region>().Update(existing);
                await _unitOfWork.CompleteAsync();

                return MapToDto(existing);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating Region: " + ex.Message);
            }
        }

        public async Task<bool> DeleteRegionAsync(int id)
        {
            var region = await _unitOfWork.Repository<Region>().GetByIdAsync(id);
            if (region == null) return false;

            _unitOfWork.Repository<Region>().Remove(region);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        private Region MapFromDynamic(object model)
        {
            var json = JsonSerializer.Serialize(model);
            var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var region = new Region();

            foreach (var kvp in dict!)
            {
                var prop = typeof(Region).GetProperty(kvp.Key,
                    System.Reflection.BindingFlags.IgnoreCase |
                    System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.Instance);

                if (prop != null && kvp.Value != null)
                {
                    try
                    {
                        object? value = kvp.Value;

                        // 👇 Handle JsonElement conversion
                        if (value is JsonElement element)
                        {
                            switch (element.ValueKind)
                            {
                                case JsonValueKind.String:
                                    value = element.GetString();
                                    break;
                                case JsonValueKind.Number:
                                    if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?))
                                        value = element.GetInt32();
                                    else if (prop.PropertyType == typeof(long) || prop.PropertyType == typeof(long?))
                                        value = element.GetInt64();
                                    else if (prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(decimal?))
                                        value = element.GetDecimal();
                                    else if (prop.PropertyType == typeof(double) || prop.PropertyType == typeof(double?))
                                        value = element.GetDouble();
                                    break;
                                case JsonValueKind.True:
                                case JsonValueKind.False:
                                    value = element.GetBoolean();
                                    break;
                                case JsonValueKind.Null:
                                    value = null;
                                    break;
                            }
                        }

                        var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                        var safeValue = Convert.ChangeType(value, targetType);
                        prop.SetValue(region, safeValue);
                    }
                    catch
                    {
                        // optional: log property conversion failure
                    }
                }
            }

            return region;
        }


        private RegionDto MapToDto(Region region)
        {
            return new RegionDto
            {
                RegionID = region.RegionId,
                CompanyID = region.CompanyId,
                RegionName = region.RegionName,
                Country = region.Country,
                userId=region.UserId,
                isActive=region.IsActive
               
            };
        }
    }
}
