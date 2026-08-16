using AutoMapper;
using IT_asserts_Claim.Dtos.Vendor;
using IT_asserts_Claim.entity;
using IT_asserts_Claim.Repositories.Interface;
using IT_asserts_Claim.Services.Interface;

namespace IT_asserts_Claim.Services.Implementations
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

        public async Task<List<VendorDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<List<VendorDto>>(entities);
        }

        public async Task<VendorDto?> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity != null ? _mapper.Map<VendorDto>(entity) : null;
        }

        public async Task<List<VendorDropdownDto>> GetDropdownAsync()
        {
            var entities = await _repository.GetAllAsync();
            var activeVendors = entities.Where(x => x.IsActive).ToList();
            return _mapper.Map<List<VendorDropdownDto>>(activeVendors);
        }

        public async Task<VendorDto> CreateAsync(CreateVendorDto dto)
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

            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Vendor created: {Name}", entity.VendorName);

            return _mapper.Map<VendorDto>(entity);
        }

        public async Task<VendorDto?> UpdateAsync(Guid id, UpdateVendorDto dto)
        {
            var entity = await _repository.GetByIdAsync(id);
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

            _repository.Update(entity);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Vendor updated: {Id}", id);

            return _mapper.Map<VendorDto>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return false;

            entity.IsDeleted = true;
            entity.IsActive = false;

            _repository.Update(entity);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Vendor soft-deleted: {Id}", id);

            return true;
        }
    }
    }
