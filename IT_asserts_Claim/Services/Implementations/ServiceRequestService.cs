using AutoMapper;
using ITAssetManagement.Domain.Entities;
using ITAssetManagement.Domain.Enums;
using ITAssetManagement.Dtos.Service;
using ITAssetManagement.Repositories.Interface;
using ITAssetManagement.Services.Interface;

namespace ITAssetManagement.Services.Implementations
{
    public class ServiceRequestService : IServiceRequestService
    {
        private readonly IServiceRequestRepository _serviceRequestRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly IMapper _mapper;

        public ServiceRequestService(
            IServiceRequestRepository serviceRequestRepository,
            IAssetRepository assetRepository,
            IMapper mapper)
        {
            _serviceRequestRepository = serviceRequestRepository;
            _assetRepository = assetRepository;
            _mapper = mapper;
        }

        // ─── Get All ────────────────────────────────────────────────────────────────

        public async Task<List<ServiceRequestListItem>> GetAllAsync()
        {
            var serviceRequests = await _serviceRequestRepository.GetAllServiceRequestsAsync();
            return _mapper.Map<List<ServiceRequestListItem>>(serviceRequests);
        }

        // ─── Get By Id ──────────────────────────────────────────────────────────────

        public async Task<ServiceRequestDetails?> GetByIdAsync(Guid id)
        {
            var serviceRequest = await _serviceRequestRepository.GetServiceRequestByIdWithAssetAsync(id);
            if (serviceRequest == null) return null;
            return _mapper.Map<ServiceRequestDetails>(serviceRequest);
        }

        // ─── Create ─────────────────────────────────────────────────────────────────

        public async Task<ServiceRequestDetails> CreateAsync(ServiceRequestCreateRequest dto)
        {
            // 1. Get Asset
            var asset = await _assetRepository.GetAssetByIdWithDetailsAsync(dto.AssetId);
            if (asset == null)
                throw new KeyNotFoundException("Asset not found.");

            // 2. Check if asset already InService (duplicate prevention)
            if (asset.Status == AssetStatus.InService || asset.Status == AssetStatus.Lost)
                throw new InvalidOperationException("This asset already has an active service request.");

            // 3. Check warranty
            if (asset.WarrantyEndDate < DateOnly.FromDateTime(DateTime.UtcNow))
                throw new InvalidOperationException("This asset is out of warranty and is not eligible for service request creation.");

            // 4. Store old status for lifecycle
            var oldStatus = asset.Status;

            // 5. Create ServiceRequest
            var serviceRequest = _mapper.Map<ServiceRequest>(dto);
            await _serviceRequestRepository.AddServiceRequestAsync(serviceRequest);

            // 6. Update Asset status to InService
            asset.Status = AssetStatus.InService;
            //await _assetRepository.
            // 7. Add Lifecycle History
            var history = new AssetLifecycleHistory
            {
                AssetId = asset.Id,
                Action = "Sent to Service",
                OldStatus = oldStatus,
                NewStatus = AssetStatus.InService,
                Remarks = $"Service Request created — Issue: {dto.IssueType}, Priority: {dto.Priority}",
                PerformedBy = "System",
                PerformedDate = DateTime.UtcNow
            };

            await _assetRepository.AddAssetLifecycleHistoryAsync(history);

            // 8. Save all in one transaction
            await _serviceRequestRepository.SaveChangesAsync();
          
            // 9. Reload with navigation for response
            var saved = await _serviceRequestRepository.GetServiceRequestByIdWithAssetAsync(serviceRequest.Id);
            return _mapper.Map<ServiceRequestDetails>(saved!);
        }

        // ─── Resolve ────────────────────────────────────────────────────────────────

        public async Task<ServiceRequestDetails> ResolveAsync(Guid id, ServiceRequestResolveRequest dto)
        {
            // 1. Find ServiceRequest
            var serviceRequest = await _serviceRequestRepository.GetServiceRequestByIdWithAssetAsync(id);
            if (serviceRequest == null)
                throw new KeyNotFoundException("Service request not found.");

            // 2. Already resolved?
            if (serviceRequest.Status == "Resolved")
                throw new InvalidOperationException("This service request is already resolved.");

            var asset = serviceRequest.Asset;

            // 3. Determine restored status
            //    Check if asset has active assignment → restore to Assigned
            //    Otherwise → restore to Available
            var hasActiveAssignment = await _assetRepository.HasActiveAssignmentAsync(asset.Id);
            var restoredStatus = hasActiveAssignment ? AssetStatus.Assigned : AssetStatus.Available;

            // 4. Update ServiceRequest
            serviceRequest.Status = "Resolved";
            serviceRequest.ResolvedDate = DateTime.UtcNow;
            serviceRequest.ResolutionNotes = dto.ResolutionNotes;

            // 5. Restore Asset status
            asset.Status = restoredStatus;

            // 6. Lifecycle History
            var history = new AssetLifecycleHistory
            {
                AssetId = asset.Id,
                Action = "Service Completed",
                OldStatus = AssetStatus.InService,
                NewStatus = restoredStatus,
                Remarks = dto.ResolutionNotes ?? "Service request resolved.",
                PerformedBy = "System",
                PerformedDate = DateTime.UtcNow
            };

            await _assetRepository.AddAssetLifecycleHistoryAsync(history);

            // 7. Save
            await _serviceRequestRepository.SaveChangesAsync();

            return _mapper.Map<ServiceRequestDetails>(serviceRequest);
        }
    }
}