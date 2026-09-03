using AutoMapper;
using ITAssetManagement.Dtos.Vendor;
using ITAssetManagement.Domain.Entities;
using ITAssetManagement.Repositories.Interface;
using ITAssetManagement.Services.Interface;

namespace ITAssetManagement.Services.Implementations
{
    public class VendorService : IVendorService
    {
        private readonly IVendorRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<VendorService> _logger;

        public VendorService(IVendorRepository repository, IMapper mapper, ILogger<VendorService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<VendorDetails>> GetAllAsync()
        {
            var entities = await _repository.GetAllVendorsAsync();
            return _mapper.Map<List<VendorDetails>>(entities);
        }

        public async Task<VendorDetails?> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetVendorByIdAsync(id);
            return entity != null ? _mapper.Map<VendorDetails>(entity) : null;
        }

        public async Task<List<VendorDropdownItem>> GetDropdownAsync()
        {
            var entities = await _repository.GetAllVendorsAsync();
            var activeVendors = entities.Where(x => x.IsActive).ToList();
            return _mapper.Map<List<VendorDropdownItem>>(activeVendors);
        }

        public async Task<VendorDetails> CreateAsync(VendorCreateRequest dto)
        {
            // Validate vendor name unique
            var nameExists = await _repository.IsNameExistsAsync(dto.VendorName.Trim());
            if (nameExists)
                throw new InvalidOperationException($"Vendor '{dto.VendorName}' already exists.");

            // Validate email unique
            var emailExists = await _repository.IsEmailExistsAsync(dto.Email.Trim());
            if (emailExists)
                throw new InvalidOperationException($"Email '{dto.Email}' is already registered with another vendor.");

            // Validate GST unique (if provided)
            if (!string.IsNullOrWhiteSpace(dto.GSTNumber))
            {
                var gstExists = await _repository.IsGSTExistsAsync(dto.GSTNumber.Trim());
                if (gstExists)
                    throw new InvalidOperationException($"GST Number '{dto.GSTNumber}' is already registered with another vendor.");
            }

            var entity = _mapper.Map<Vendor>(dto);
            entity.VendorName = dto.VendorName.Trim();
            entity.ContactPerson = dto.ContactPerson.Trim();
            entity.Email = dto.Email.Trim().ToLower();
            entity.Phone = dto.Phone.Trim();
            entity.GSTNumber = dto.GSTNumber?.Trim().ToUpper();
            entity.Address = dto.Address?.Trim();

            await _repository.AddVendorAsync(entity);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Vendor created: {Name}", entity.VendorName);

            return _mapper.Map<VendorDetails>(entity);
        }

        public async Task<VendorDetails?> UpdateAsync(Guid id, VendorUpdateRequest dto)
        {
            var entity = await _repository.GetVendorByIdAsync(id);
            if (entity == null) return null;

            // Validate vendor name unique
            var nameExists = await _repository.IsNameExistsAsync(dto.VendorName.Trim(), id);
            if (nameExists)
                throw new InvalidOperationException($"Vendor '{dto.VendorName}' already exists.");

            // Validate email unique
            var emailExists = await _repository.IsEmailExistsAsync(dto.Email.Trim(), id);
            if (emailExists)
                throw new InvalidOperationException($"Email '{dto.Email}' is already registered with another vendor.");

            // Validate GST unique (if provided)
            if (!string.IsNullOrWhiteSpace(dto.GSTNumber))
            {
                var gstExists = await _repository.IsGSTExistsAsync(dto.GSTNumber.Trim(), id);
                if (gstExists)
                    throw new InvalidOperationException($"GST Number '{dto.GSTNumber}' is already registered with another vendor.");
            }

            entity.VendorName = dto.VendorName.Trim();
            entity.ContactPerson = dto.ContactPerson.Trim();
            entity.Email = dto.Email.Trim().ToLower();
            entity.Phone = dto.Phone.Trim();
            entity.GSTNumber = dto.GSTNumber?.Trim().ToUpper();
            entity.Address = dto.Address?.Trim();
            entity.IsActive = dto.IsActive;

            _repository.UpdateVendor(entity);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Vendor updated: {Id}", id);

            return _mapper.Map<VendorDetails>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _repository.GetVendorByIdAsync(id);
            if (entity == null) return false;

            entity.IsDeleted = true;
            entity.IsActive = false;

            _repository.UpdateVendor(entity);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Vendor soft-deleted: {Id}", id);

            return true;
        }
    }
    }
